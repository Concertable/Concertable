using Concertable.B2B.Concert.Application.Interfaces;
using Concertable.B2B.Concert.Application.Strategies;

namespace Concertable.B2B.Concert.Application.Renderers;

internal sealed class DealTermsRenderer : IDealTermsRenderer
{
    private readonly IConcertDealStrategyFactory<IDealTermsFormatter> formatters;

    public DealTermsRenderer(IConcertDealStrategyFactory<IDealTermsFormatter> formatters)
    {
        this.formatters = formatters;
    }

    public string Render(IDealTerms terms) =>
        formatters.Create(terms.DealType).Render(terms);
}
