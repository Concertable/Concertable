using Concertable.B2B.Deal.Application.Interfaces;
using Concertable.B2B.Deal.Domain.Entities;
using Reunion.Errors;
using Reunion;

namespace Concertable.B2B.Deal.Application.Mappers;

internal sealed class VenueHireTermsMapper : IDealTermsMapper
{
    public IDealTerms ToTerms(DealTermsEntity entity)
    {
        var e = (VenueHireTermsEntity)entity;
        return new VenueHireTerms
        {
            Id = e.Id,
            PaymentMethod = e.PaymentMethod,
            HireFee = e.HireFee
        };
    }

    public Result<DealTermsEntity, ValidationErrors> ToEntity(IDealTerms terms)
    {
        var c = (VenueHireTerms)terms;
        return VenueHireTermsEntity.Create(c.HireFee, c.PaymentMethod).Map<DealTermsEntity>(entity => entity);
    }
}
