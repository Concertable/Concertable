using Concertable.B2B.Concert.Application.Responses;
using Concertable.B2B.Deal.Contracts;

namespace Concertable.B2B.Concert.Application.Mappers;

internal sealed class FlatFeePaymentAmountMapper : IPaymentAmountMapper
{
    public IPaymentAmount ToPaymentAmount(IDealTerms terms)
    {
        var c = (FlatFeeTerms)terms;
        return new FlatPayment(c.Fee);
    }
}
