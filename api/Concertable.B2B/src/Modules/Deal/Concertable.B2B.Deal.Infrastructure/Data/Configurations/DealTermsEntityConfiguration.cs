using Concertable.B2B.Deal.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Concertable.B2B.Deal.Infrastructure.Data.Configurations;

internal sealed class DealTermsEntityConfiguration : IEntityTypeConfiguration<DealTermsEntity>
{
    public void Configure(EntityTypeBuilder<DealTermsEntity> builder)
    {
        builder.ToTable(Schema.Tables.DealTerms, Schema.Name);
        builder.UseTptMappingStrategy();
    }
}

internal sealed class FlatFeeDealEntityConfiguration : IEntityTypeConfiguration<FlatFeeTermsEntity>
{
    public void Configure(EntityTypeBuilder<FlatFeeTermsEntity> builder)
        => builder.ToTable(Schema.Tables.FlatFeeTerms, Schema.Name);
}

internal sealed class DoorSplitDealEntityConfiguration : IEntityTypeConfiguration<DoorSplitTermsEntity>
{
    public void Configure(EntityTypeBuilder<DoorSplitTermsEntity> builder)
        => builder.ToTable(Schema.Tables.DoorSplitTerms, Schema.Name);
}

internal sealed class VersusDealEntityConfiguration : IEntityTypeConfiguration<VersusTermsEntity>
{
    public void Configure(EntityTypeBuilder<VersusTermsEntity> builder)
        => builder.ToTable(Schema.Tables.VersusTerms, Schema.Name);
}

internal sealed class VenueHireDealEntityConfiguration : IEntityTypeConfiguration<VenueHireTermsEntity>
{
    public void Configure(EntityTypeBuilder<VenueHireTermsEntity> builder)
        => builder.ToTable(Schema.Tables.VenueHireTerms, Schema.Name);
}
