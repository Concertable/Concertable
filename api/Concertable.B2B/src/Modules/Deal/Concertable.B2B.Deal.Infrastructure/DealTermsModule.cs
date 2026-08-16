using Concertable.B2B.Deal.Application.Interfaces;
using Concertable.B2B.Deal.Contracts.Errors;
using Reunion.Errors;
using Reunion;

namespace Concertable.B2B.Deal.Infrastructure;

internal sealed class DealTermsModule : IDealTermsModule
{
    private readonly IDealTermsService dealTermsService;

    public DealTermsModule(IDealTermsService dealTermsService)
    {
        this.dealTermsService = dealTermsService;
    }

    public Task<Option<IDealTerms>> GetByIdAsync(int dealTermsId, CancellationToken ct = default)
        => dealTermsService.FindByIdAsync(dealTermsId, ct);

    public Task<IReadOnlyList<IDealTerms>> GetByIdsAsync(IEnumerable<int> dealTermsIds, CancellationToken ct = default)
        => dealTermsService.GetByIdsAsync(dealTermsIds, ct);

    public UnitResult<ValidationErrors> Validate(IDealTerms terms)
        => dealTermsService.Validate(terms);

    public Task<Result<int, CreateDealTermsError>> CreateAsync(IDealTerms terms, CancellationToken ct = default)
        => dealTermsService.CreateAsync(terms, ct);

    public Task<UnitResult<UpdateDealTermsError>> UpdateAsync(int dealTermsId, IDealTerms terms, CancellationToken ct = default)
        => dealTermsService.UpdateAsync(dealTermsId, terms, ct);

    public Task DeleteAsync(int dealTermsId, CancellationToken ct = default)
        => dealTermsService.DeleteAsync(dealTermsId, ct);
}
