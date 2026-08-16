using Concertable.Seed.Shared;
using Concertable.Seed.Shared.Extensions;
using Concertable.B2B.Seed.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Concertable.B2B.Deal.Infrastructure.Data.Seeders;

internal sealed class DealTermsDevSeeder : IDevSeeder
{
    public int Order => 3;

    private readonly DealDbContext context;
    private readonly SeedState seed;

    public DealTermsDevSeeder(DealDbContext context, SeedState seed)
    {
        this.context = context;
        this.seed = seed;
    }

    public Task MigrateAsync(CancellationToken ct = default) => context.Database.MigrateAsync(ct);

    public async Task SeedAsync(CancellationToken ct = default) =>
        await context.DealTerms.SeedIfEmptyAsync(async () =>
        {
            context.DealTerms.AddRange(seed.DealTerms);
            await context.SaveChangesAsync(ct);
        });
}
