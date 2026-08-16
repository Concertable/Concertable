using Concertable.B2B.Deal.Contracts;
using Concertable.Contracts;
using System.Text.Json.Serialization;

namespace Concertable.B2B.Concert.Api.Responses;

internal sealed record OpportunityResponse(
    int Id,
    int VenueId,
    [property: JsonPropertyName("deal")] IDealTerms Terms,
    DateTime StartDate,
    DateTime EndDate,
    IEnumerable<Genre> Genres,
    OpportunityActions Actions);

internal sealed record OpportunityActions(ActionLink? Checkout);
