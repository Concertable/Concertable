using Concertable.B2B.Deal.Application.Errors;
using Concertable.B2B.Deal.Application.Interfaces;
using Concertable.B2B.Deal.Contracts.Errors;
using Concertable.B2B.Deal.Domain.Entities;
using Reunion.Errors;
using Reunion;

namespace Concertable.B2B.Deal.Application.Services;

internal sealed class DealTermsService : IDealTermsService
{
    private readonly IDealTermsRepository dealTermsRepository;
    private readonly IDealTermsMapper mapper;
    private readonly IDealTermsUpdater updater;

    public DealTermsService(
        IDealTermsRepository dealTermsRepository,
        IDealTermsMapper mapper,
        IDealTermsUpdater updater)
    {
        this.dealTermsRepository = dealTermsRepository;
        this.mapper = mapper;
        this.updater = updater;
    }

    public Task<Option<IDealTerms>> FindByIdAsync(int dealTermsId, CancellationToken ct = default) =>
        dealTermsRepository.GetByIdAsync(dealTermsId, ct)
            .ToOption()
            .Map(mapper.ToTerms);

    public Task<Result<IDealTerms, DealTermsError>> GetByIdAsync(int dealTermsId, CancellationToken ct = default) =>
        FindByIdAsync(dealTermsId, ct)
            .OrFailure(() => (DealTermsError)new DealTermsError.NotFound(dealTermsId));

    public async Task<IReadOnlyList<IDealTerms>> GetByIdsAsync(IEnumerable<int> dealTermsIds, CancellationToken ct = default)
    {
        var entities = await dealTermsRepository.GetByIdsAsync(dealTermsIds, ct);
        return mapper.ToTerms(entities);
    }

    public UnitResult<ValidationErrors> Validate(IDealTerms terms) =>
        mapper.ToEntity(terms).Match(
            _ => UnitResult.Success<ValidationErrors>(),
            UnitResult.Failure);

    public Task<Result<int, CreateDealTermsError>> CreateAsync(IDealTerms terms, CancellationToken ct = default) =>
        mapper.ToEntity(terms)
            .BindAsync(async (DealTermsEntity entity) =>
            {
                await dealTermsRepository.AddAsync(entity, ct);
                await dealTermsRepository.SaveChangesAsync(ct);
                return Result.Success<int, CreateDealTermsError>(entity.Id);
            }, errors => new CreateDealTermsError.Invalid(errors));

    public async Task<UnitResult<UpdateDealTermsError>> UpdateAsync(int dealTermsId, IDealTerms terms, CancellationToken ct = default)
    {
        var existing = await dealTermsRepository.GetByIdAsync(dealTermsId, ct);
        if (existing is null)
            return new UpdateDealTermsError.DealTermsNotFound();

        var update = updater.Apply(existing, terms)
            .MapError<UpdateDealTermsError>(errors => new UpdateDealTermsError.Invalid(errors));
        if (update.IsFailure)
            return update;

        dealTermsRepository.Update(existing);
        await dealTermsRepository.SaveChangesAsync(ct);
        return new Success();
    }

    public async Task DeleteAsync(int dealTermsId, CancellationToken ct = default)
    {
        var existing = await dealTermsRepository.GetByIdAsync(dealTermsId, ct);
        if (existing is null) return;

        dealTermsRepository.Remove(existing);
        await dealTermsRepository.SaveChangesAsync(ct);
    }
}
