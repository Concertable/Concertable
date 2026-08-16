using Concertable.B2B.Deal.Application.Interfaces;
using Concertable.B2B.Deal.Domain.Entities;
using Reunion.Errors;
using Reunion;

namespace Concertable.B2B.Deal.Application.Mappers;

internal sealed class DoorSplitTermsMapper : IDealTermsMapper
{
    public IDealTerms ToTerms(DealTermsEntity entity)
    {
        var e = (DoorSplitTermsEntity)entity;
        return new DoorSplitTerms
        {
            Id = e.Id,
            PaymentMethod = e.PaymentMethod,
            ArtistDoorPercent = e.ArtistDoorPercent
        };
    }

    public Result<DealTermsEntity, ValidationErrors> ToEntity(IDealTerms terms)
    {
        var c = (DoorSplitTerms)terms;
        return DoorSplitTermsEntity.Create(c.ArtistDoorPercent, c.PaymentMethod).Map<DealTermsEntity>(entity => entity);
    }
}
