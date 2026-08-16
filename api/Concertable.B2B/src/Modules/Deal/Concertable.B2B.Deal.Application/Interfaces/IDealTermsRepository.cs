using Concertable.B2B.Deal.Domain.Entities;
using Concertable.B2B.DataAccess.Application;

namespace Concertable.B2B.Deal.Application.Interfaces;

internal interface IDealTermsRepository : ITenantScopedRepository<DealTermsEntity>
{
    Task<IReadOnlyList<DealTermsEntity>> GetByIdsAsync(IEnumerable<int> ids, CancellationToken ct = default);
}
