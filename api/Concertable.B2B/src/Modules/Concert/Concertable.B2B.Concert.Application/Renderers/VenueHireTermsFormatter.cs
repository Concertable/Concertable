using Concertable.B2B.Concert.Application.Interfaces;
using static Concertable.B2B.Concert.Application.Renderers.DealTermsFormat;

namespace Concertable.B2B.Concert.Application.Renderers;

internal sealed class VenueHireTermsFormatter : IDealTermsFormatter
{
    public string Render(IDealTerms terms)
    {
        var venueHireTerms = (VenueHireTerms)terms;
        return $"The artist pays the venue a hire fee of {Gbp(venueHireTerms.HireFee)}.";
    }

    public string Serialize(IDealTerms terms) =>
        $"HireFee={TermsFingerprintFormat.Number(((VenueHireTerms)terms).HireFee)}";
}
