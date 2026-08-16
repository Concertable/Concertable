using Concertable.B2B.Deal.Application.Interfaces;
using Concertable.B2B.Deal.Domain.Entities;
using Reunion.Errors;
using Reunion;

namespace Concertable.B2B.Deal.Application.Mappers;

internal sealed class VersusTermsMapper : IDealTermsMapper
{
    public IDealTerms ToTerms(DealTermsEntity entity)
    {
        var e = (VersusTermsEntity)entity;
        return new VersusTerms
        {
            Id = e.Id,
            PaymentMethod = e.PaymentMethod,
            Guarantee = e.Guarantee,
            ArtistDoorPercent = e.ArtistDoorPercent
        };
    }

    public Result<DealTermsEntity, ValidationErrors> ToEntity(IDealTerms terms)
    {
        var c = (VersusTerms)terms;
        return VersusTermsEntity.Create(c.Guarantee, c.ArtistDoorPercent, c.PaymentMethod).Map<DealTermsEntity>(entity => entity);
    }
}
