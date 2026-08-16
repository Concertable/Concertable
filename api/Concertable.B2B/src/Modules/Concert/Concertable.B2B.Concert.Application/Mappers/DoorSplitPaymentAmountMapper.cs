using Concertable.B2B.Concert.Application.Responses;
using Concertable.B2B.Deal.Contracts;

namespace Concertable.B2B.Concert.Application.Mappers;

internal sealed class DoorSplitPaymentAmountMapper : IPaymentAmountMapper
{
    public IPaymentAmount ToPaymentAmount(IDealTerms terms)
    {
        var c = (DoorSplitTerms)terms;
        return new DoorSharePayment(c.ArtistDoorPercent);
    }
}
