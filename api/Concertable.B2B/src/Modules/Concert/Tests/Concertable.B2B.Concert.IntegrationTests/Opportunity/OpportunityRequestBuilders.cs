using Concertable.B2B.Concert.Application.Requests;
using Concertable.Testing.Integration;
using Concertable.Contracts.Enums;
using Concertable.B2B.Deal.Contracts;

namespace Concertable.B2B.Concert.IntegrationTests.Opportunity;

internal static class OpportunityRequestBuilders
{
    public static OpportunityRequest BuildRequest(IDealTerms terms, DateTime now) =>
        new()
        {
            StartDate = now.AddMonths(1),
            EndDate = now.AddMonths(1).AddHours(3),
            Genres = [Genre.Rock],
            Terms = deal
        };

    public static OpportunityRequest BuildDefaultRequest(DateTime now) =>
        BuildRequest(new FlatFeeTerms { PaymentMethod = PaymentMethod.Cash, Fee = 500 }, now);
}
