using Concertable.B2B.Concert.Domain.Entities;
using Concertable.DataAccess.Application;
using Concertable.DataAccess.Application.Diffing;

namespace Concertable.B2B.Concert.Infrastructure.Sync;

internal sealed class OpportunitySyncer
    : CollectionSyncer<OpportunityEntity, OpportunityRequest>, IOpportunitySyncer
{
    private readonly IDealTermsModule dealTermsModule;

    public OpportunitySyncer(IWriteRepository<OpportunityEntity> repository, IDealTermsModule dealTermsModule)
        : base(repository)
    {
        this.dealTermsModule = dealTermsModule;
    }

    protected override async Task<OpportunityEntity> CreateAsync(int venueId, OpportunityRequest dto)
    {
        var result = await dealTermsModule.CreateAsync(dto.Terms);
        if (!result.TryGetValue(out var dealTermsId))
            throw new InvalidOperationException("Deal creation failed after successful validation.");
        return OpportunityEntity.Create(
            venueId,
            new DateRange(dto.StartDate, dto.EndDate),
            dealTermsId,
            dto.Genres);
    }

    protected override async Task UpdateAsync(OpportunityEntity entity, OpportunityRequest dto)
    {
        var result = await dealTermsModule.UpdateAsync(entity.DealTermsId, dto.Terms);
        if (result.IsFailure)
            throw new InvalidOperationException("Deal update failed after successful validation.");

        entity.Update(new DateRange(dto.StartDate, dto.EndDate), entity.DealTermsId, dto.Genres);
    }
}
