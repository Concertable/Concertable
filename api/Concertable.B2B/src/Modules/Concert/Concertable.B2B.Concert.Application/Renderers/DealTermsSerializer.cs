using Concertable.B2B.Concert.Application.Interfaces;
using Concertable.B2B.Concert.Application.Strategies;

namespace Concertable.B2B.Concert.Application.Renderers;

internal sealed class DealTermsSerializer : IDealTermsSerializer
{
    private readonly IConcertDealStrategyFactory<IDealTermsFormatter> formatters;

    public DealTermsSerializer(IConcertDealStrategyFactory<IDealTermsFormatter> formatters)
    {
        this.formatters = formatters;
    }

    public string Serialize(IDealTerms terms) =>
        formatters.Create(terms.DealType).Serialize(terms);
}
