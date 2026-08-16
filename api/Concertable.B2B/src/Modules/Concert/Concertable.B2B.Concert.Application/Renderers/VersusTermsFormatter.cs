using Concertable.B2B.Concert.Application.Interfaces;
using static Concertable.B2B.Concert.Application.Renderers.DealTermsFormat;

namespace Concertable.B2B.Concert.Application.Renderers;

internal sealed class VersusTermsFormatter : IDealTermsFormatter
{
    public string Render(IDealTerms terms)
    {
        var versusTerms = (VersusTerms)terms;
        return $"The artist receives a guarantee of {Gbp(versusTerms.Guarantee)} plus {Percent(versusTerms.ArtistDoorPercent)} of door revenue.";
    }

    public string Serialize(IDealTerms terms)
    {
        var versusTerms = (VersusTerms)terms;
        return $"Guarantee={TermsFingerprintFormat.Number(versusTerms.Guarantee)};ArtistDoorPercent={TermsFingerprintFormat.Number(versusTerms.ArtistDoorPercent)}";
    }
}
