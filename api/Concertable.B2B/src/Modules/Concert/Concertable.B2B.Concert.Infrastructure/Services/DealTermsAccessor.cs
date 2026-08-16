using Concertable.B2B.Deal.Contracts;
using Concertable.Kernel.Exceptions;

namespace Concertable.B2B.Concert.Infrastructure.Services;

internal sealed class DealTermsAccessor : IDealTermsAccessor, IDealTermsResolver
{
    private readonly IApplicationRepository applicationRepository;
    private readonly IOpportunityRepository opportunityRepository;
    private readonly IConcertRepository concertRepository;
    private readonly IDealTermsModule dealTermsModule;

    private IDealTerms? terms;

    public DealTermsAccessor(
        IApplicationRepository applicationRepository,
        IOpportunityRepository opportunityRepository,
        IConcertRepository concertRepository,
        IDealTermsModule dealTermsModule)
    {
        this.applicationRepository = applicationRepository;
        this.opportunityRepository = opportunityRepository;
        this.concertRepository = concertRepository;
        this.dealTermsModule = dealTermsModule;
    }

    public IDealTerms Terms => terms
        ?? throw new InvalidOperationException(
            "No deal terms resolved this scope — the operation's orchestrator must resolve them before a step reads them.");

    public Task<IDealTerms> ResolveByOpportunityIdAsync(int opportunityId) =>
        ResolveAsync(() => opportunityRepository.GetDealTermsIdByIdAsync(opportunityId));

    public Task<IDealTerms> ResolveByApplicationIdAsync(int applicationId) =>
        ResolveAsync(() => applicationRepository.GetDealTermsIdByIdAsync(applicationId));

    public Task<IDealTerms> ResolveByConcertIdAsync(int concertId) =>
        ResolveAsync(() => concertRepository.GetDealTermsIdByIdAsync(concertId));

    private async Task<IDealTerms> ResolveAsync(Func<Task<int?>> resolveDealTermsId)
    {
        if (terms is not null)
            return terms;

        var dealTermsId = await resolveDealTermsId()
            ?? throw new NotFoundException("Deal not found for this entity");

        var termsOption = await dealTermsModule.GetByIdAsync(dealTermsId);
        return terms = termsOption.Match(
            value => value,
            () => throw new InvalidOperationException($"Entity references missing deal terms {dealTermsId}."));
    }
}
