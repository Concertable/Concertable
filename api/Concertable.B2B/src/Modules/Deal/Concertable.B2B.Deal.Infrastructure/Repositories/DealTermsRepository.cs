using Concertable.B2B.Deal.Application.Interfaces;
using Concertable.B2B.Deal.Domain.Entities;
using Concertable.B2B.Deal.Infrastructure.Data;
using Concertable.Kernel.Identity;
using Microsoft.EntityFrameworkCore;

namespace Concertable.B2B.Deal.Infrastructure.Repositories;

internal sealed class DealTermsRepository
    : TenantScopedRepository<DealTermsEntity>, IDealTermsRepository
{
    public DealTermsRepository(DealDbContext context, ITenantContext tenant)
        : base(context, tenant) { }

    public async Task<IReadOnlyList<DealTermsEntity>> GetByIdsAsync(IEnumerable<int> ids, CancellationToken ct = default) =>
        await context.DealTerms
            .Where(c => ids.Contains(c.Id))
            .ToListAsync(ct);
}
