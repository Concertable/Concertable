using Concertable.B2B.Concert.Application.Errors;
using Concertable.B2B.Concert.Domain.Entities;
using Concertable.B2B.Deal.Contracts;
using Concertable.Contracts;
using Reunion;
using Concertable.Kernel.Identity;

namespace Concertable.B2B.Concert.Infrastructure.Services;

internal sealed class OpportunityService : IOpportunityService
{
    private readonly IOpportunityRepository repository;
    private readonly IPublicOpportunityRepository publicRepository;
    private readonly IVenueModule venueModule;
    private readonly IDealTermsModule dealTermsModule;
    private readonly IOpportunitySyncer syncer;
    private readonly IOpportunityMapper mapper;
    private readonly ITenantContext tenantContext;
    private readonly IUnitOfWorkBehavior uowBehavior;

    public OpportunityService(
        IOpportunityRepository repository,
        IPublicOpportunityRepository publicRepository,
        IVenueModule venueModule,
        IDealTermsModule dealTermsModule,
        IOpportunitySyncer syncer,
        IOpportunityMapper mapper,
        ITenantContext tenantContext,
        IUnitOfWorkBehavior uowBehavior)
    {
        this.repository = repository;
        this.publicRepository = publicRepository;
        this.venueModule = venueModule;
        this.dealTermsModule = dealTermsModule;
        this.syncer = syncer;
        this.mapper = mapper;
        this.tenantContext = tenantContext;
        this.uowBehavior = uowBehavior;
    }

    public async Task<Result<OpportunityDto, OpportunityMutationError>> CreateAsync(OpportunityRequest request)
    {
        var venue = (await venueModule.GetVenueIdForCurrentTenantAsync())
            .OrFailure(() => (OpportunityMutationError)new OpportunityMutationError.VenueNotFound());
        if (venue.TryGetError(out var venueError))
            return venueError;
        venue.TryGetValue(out var venueId);

        var creation = await uowBehavior.ExecuteAsync(async () =>
        {
            var terms = await CreateTermsAsync(request.Terms);
            return await terms.BindAsync(async dealTermsId =>
            {
                var entity = OpportunityEntity.Create(
                    venueId,
                    new DateRange(request.StartDate, request.EndDate),
                    dealTermsId,
                    request.Genres);
                await repository.AddAsync(entity);
                return Result.Success<OpportunityEntity, OpportunityMutationError>(entity);
            });
        });

        return await creation.MapAsync(mapper.ToDtoAsync);
    }

    public async Task<UnitResult<OpportunityMutationError>> CreateMultipleAsync(IEnumerable<OpportunityRequest> requests)
    {
        var requestList = requests.ToList();
        var venue = (await venueModule.GetVenueIdForCurrentTenantAsync())
            .OrFailure(() => (OpportunityMutationError)new OpportunityMutationError.VenueNotFound());
        if (venue.TryGetError(out var venueError))
            return venueError;
        venue.TryGetValue(out var venueId);

        var validation = ValidateTerms(requestList.Select(request => request.Terms));
        if (validation.IsFailure)
            return validation;

        await uowBehavior.ExecuteAsync(async () =>
        {
            foreach (var request in requestList)
            {
                var dealTermsId = await CreatePrevalidatedTermsAsync(request.Terms);
                var opportunity = OpportunityEntity.Create(
                    venueId,
                    new DateRange(request.StartDate, request.EndDate),
                    dealTermsId,
                    request.Genres);
                await repository.AddAsync(opportunity);
            }
        });

        return new Success();
    }

    public async Task<IPagination<OpportunityDto>> GetActiveByVenueIdAsync(int id, IPageParams pageParams)
    {
        var opportunities = await publicRepository.GetActiveByVenueIdAsync(id, pageParams);
        return await mapper.ToDtosAsync(opportunities);
    }

    public async Task<IReadOnlyList<OpportunityDto>> GetActiveByVenueIdAsync(int venueId)
    {
        var opportunities = await publicRepository.GetActiveByVenueIdAsync(venueId);
        return await mapper.ToDtosAsync(opportunities);
    }

    public async Task<Result<IReadOnlyList<OpportunityDto>, OpportunityMutationError>> UpdateAsync(
        int venueId,
        IEnumerable<OpportunityRequest> desired)
    {
        var venue = (await venueModule.GetVenueIdForCurrentTenantAsync())
            .OrFailure(() => (OpportunityMutationError)new OpportunityMutationError.VenueNotFound());
        if (venue.TryGetError(out var venueError))
            return venueError;
        venue.TryGetValue(out var ownedVenueId);

        if (ownedVenueId != venueId)
            return new OpportunityMutationError.VenueForbidden();

        var desiredList = desired.ToList();
        var validation = ValidateTerms(desiredList.Select(request => request.Terms));
        if (validation.TryGetError(out var error))
            return error;

        /* Read tracked through the writing context: the syncer mutates these entities, and the
           read-only public projection's no-tracking context would silently drop those updates. */
        var current = await repository.GetActiveByVenueIdAsync(venueId);

        await uowBehavior.ExecuteAsync(() => syncer.SyncAsync(venueId, current, desiredList));

        var updated = await publicRepository.GetActiveByVenueIdAsync(venueId);
        return new Success<IReadOnlyList<OpportunityDto>>(
            await mapper.ToDtosAsync(updated));
    }

    public Task<Result<OpportunityDto, OpportunityError>> GetByIdAsync(int id) =>
        repository.GetByIdAsync(id)
            .ToOption()
            .OrFailure(() => (OpportunityError)new OpportunityError.NotFound(id))
            .MapAsync(mapper.ToDtoAsync);

    public async Task<Option<Guid>> GetOwnerByIdAsync(int id) =>
        (await repository.GetOwnerByIdAsync(id)).ToOption();

    public async Task<bool> OwnsOpportunityAsync(int opportunityId)
    {
        if (tenantContext.TenantId is not { } tenant)
            return false;

        var ownerTenantId = await repository.GetTenantIdByIdAsync(opportunityId);
        return ownerTenantId == tenant;
    }

    public async Task<bool> OwnsOpportunityByApplicationIdAsync(int applicationId)
    {
        if (tenantContext.TenantId is not { } tenant)
            return false;

        var opportunity = await repository.GetByApplicationIdAsync(applicationId);
        return opportunity?.TenantId == tenant;
    }

    private UnitResult<OpportunityMutationError> ValidateTerms(IEnumerable<IDealTerms> termsCollection)
    {
        foreach (var terms in termsCollection)
        {
            var validation = dealTermsModule.Validate(terms)
                .MapError<OpportunityMutationError>(
                    errors => new OpportunityMutationError.InvalidDeal(errors));
            if (validation.IsFailure)
                return validation;
        }

        return new Success();
    }

    private async Task<Result<int, OpportunityMutationError>> CreateTermsAsync(IDealTerms terms) =>
        (await dealTermsModule.CreateAsync(terms))
            .MapError<OpportunityMutationError>(
                error => error.Match<OpportunityMutationError>(
                    invalid => new OpportunityMutationError.InvalidDeal(invalid.Errors)));

    private async Task<int> CreatePrevalidatedTermsAsync(IDealTerms terms)
    {
        var result = await dealTermsModule.CreateAsync(terms);
        if (result.TryGetValue(out var dealTermsId))
            return dealTermsId;

        throw new InvalidOperationException("Deal creation failed after successful validation.");
    }
}
