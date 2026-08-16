using Concertable.B2B.Deal.Application.Interfaces;
using Concertable.B2B.Deal.Domain.Entities;
using Reunion.Errors;
using Reunion;

namespace Concertable.B2B.Deal.Application.Mappers;

internal sealed class FlatFeeTermsMapper : IDealTermsMapper
{
    public IDealTerms ToTerms(DealTermsEntity entity)
    {
        var e = (FlatFeeTermsEntity)entity;
        return new FlatFeeTerms
        {
            Id = e.Id,
            PaymentMethod = e.PaymentMethod,
            Fee = e.Fee
        };
    }

    public Result<DealTermsEntity, ValidationErrors> ToEntity(IDealTerms terms)
    {
        var c = (FlatFeeTerms)terms;
        return FlatFeeTermsEntity.Create(c.Fee, c.PaymentMethod).Map<DealTermsEntity>(entity => entity);
    }
}
