using Concertable.B2B.Deal.Contracts;

namespace Concertable.B2B.Concert.Application.Interfaces;

internal interface IDealTermsResolver
{
    Task<IDealTerms> ResolveByOpportunityIdAsync(int opportunityId);
    Task<IDealTerms> ResolveByApplicationIdAsync(int applicationId);
    Task<IDealTerms> ResolveByConcertIdAsync(int concertId);
}
