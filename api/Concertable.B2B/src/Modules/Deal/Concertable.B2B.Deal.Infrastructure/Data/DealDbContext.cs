using Concertable.B2B.Deal.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Concertable.B2B.Deal.Infrastructure.Data;

internal sealed class DealDbContext(
    DbContextOptions<DealDbContext> options,
    DealConfigurationProvider provider)
    : DbContextBase(options)
{
    public DbSet<DealTermsEntity> DealTerms => Set<DealTermsEntity>();
    public DbSet<FlatFeeTermsEntity> FlatFeeTerms => Set<FlatFeeTermsEntity>();
    public DbSet<DoorSplitTermsEntity> DoorSplitTerms => Set<DoorSplitTermsEntity>();
    public DbSet<VersusTermsEntity> VersusTerms => Set<VersusTermsEntity>();
    public DbSet<VenueHireTermsEntity> VenueHireTerms => Set<VenueHireTermsEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema(Schema.Name);
        provider.Configure(modelBuilder);
    }
}
