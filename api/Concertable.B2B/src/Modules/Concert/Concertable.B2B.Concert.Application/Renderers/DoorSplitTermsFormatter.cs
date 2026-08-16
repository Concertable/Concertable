using Concertable.B2B.Concert.Application.Interfaces;
using static Concertable.B2B.Concert.Application.Renderers.DealTermsFormat;

namespace Concertable.B2B.Concert.Application.Renderers;

internal sealed class DoorSplitTermsFormatter : IDealTermsFormatter
{
    public string Render(IDealTerms terms)
    {
        var doorSplitTerms = (DoorSplitTerms)terms;
        return $"The artist receives {Percent(doorSplitTerms.ArtistDoorPercent)} of door revenue.";
    }

    public string Serialize(IDealTerms terms) =>
        $"ArtistDoorPercent={TermsFingerprintFormat.Number(((DoorSplitTerms)terms).ArtistDoorPercent)}";
}
