using Concertable.B2B.Concert.Application.Interfaces;
using static Concertable.B2B.Concert.Application.Renderers.DealTermsFormat;

namespace Concertable.B2B.Concert.Application.Renderers;

internal sealed class FlatFeeTermsFormatter : IDealTermsFormatter
{
    public string Render(IDealTerms terms)
    {
        var flatFeeTerms = (FlatFeeTerms)terms;
        return $"The venue pays the artist a flat fee of {Gbp(flatFeeTerms.Fee)}.";
    }

    public string Serialize(IDealTerms terms) =>
        $"Fee={TermsFingerprintFormat.Number(((FlatFeeTerms)terms).Fee)}";
}
