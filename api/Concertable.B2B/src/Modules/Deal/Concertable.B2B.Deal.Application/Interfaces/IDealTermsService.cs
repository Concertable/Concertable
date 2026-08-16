using Concertable.B2B.Deal.Application.Errors;
using Concertable.B2B.Deal.Contracts.Errors;
using Reunion.Errors;
using Reunion;

namespace Concertable.B2B.Deal.Application.Interfaces;

internal interface IDealTermsService
{
    Task<Option<IDealTerms>> FindByIdAsync(int dealTermsId, CancellationToken ct = default);
    Task<Result<IDealTerms, DealTermsError>> GetByIdAsync(int dealTermsId, CancellationToken ct = default);
    Task<IReadOnlyList<IDealTerms>> GetByIdsAsync(IEnumerable<int> dealTermsIds, CancellationToken ct = default);
    UnitResult<ValidationErrors> Validate(IDealTerms terms);
    Task<Result<int, CreateDealTermsError>> CreateAsync(IDealTerms terms, CancellationToken ct = default);
    Task<UnitResult<UpdateDealTermsError>> UpdateAsync(int dealTermsId, IDealTerms terms, CancellationToken ct = default);
    Task DeleteAsync(int dealTermsId, CancellationToken ct = default);
}
