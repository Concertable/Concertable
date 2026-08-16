using Concertable.B2B.Concert.Application.DTOs;
using Concertable.B2B.Concert.Application.Interfaces;
using Concertable.B2B.Concert.Domain.Entities;
using Concertable.B2B.Deal.Contracts;
using Concertable.Contracts;

namespace Concertable.B2B.Concert.Application.Mappers;

internal sealed class OpportunityMapper : IOpportunityMapper
{
    private readonly IDealTermsModule dealTermsModule;

    public OpportunityMapper(IDealTermsModule dealTermsModule)
    {
        this.dealTermsModule = dealTermsModule;
    }

    public async Task<OpportunityDto> ToDtoAsync(OpportunityEntity opportunity)
    {
        var termsOption = await dealTermsModule.GetByIdAsync(opportunity.DealTermsId);
        if (!termsOption.TryGetValue(out var terms))
            throw new InvalidOperationException($"Opportunity {opportunity.Id} references missing deal terms {opportunity.DealTermsId}.");

        return opportunity.ToDto(terms);
    }

    public async Task<IReadOnlyList<OpportunityDto>> ToDtosAsync(IEnumerable<OpportunityEntity> opportunities)
    {
        var opportunityList = opportunities.ToList();
        var termsMap = (await dealTermsModule.GetByIdsAsync(opportunityList.Select(o => o.DealTermsId).Distinct()))
            .ToDictionary(c => c.Id);

        return opportunityList.Select(o =>
        {
            if (!termsMap.TryGetValue(o.DealTermsId, out var terms))
                throw new InvalidOperationException($"Opportunity {o.Id} references missing deal terms {o.DealTermsId}.");
            return o.ToDto(terms);
        }).ToList();
    }

    public async Task<IPagination<OpportunityDto>> ToDtosAsync(IPagination<OpportunityEntity> opportunities)
    {
        var dtos = await ToDtosAsync(opportunities.Data);
        return new Pagination<OpportunityDto>(dtos.ToList(), opportunities.TotalCount, opportunities.PageNumber, opportunities.PageSize);
    }
}

internal static class OpportunityMappers
{
    public static OpportunityDto ToDto(this OpportunityEntity opportunity, IDealTerms terms) => new()
    {
        Id = opportunity.Id,
        VenueId = opportunity.VenueId,
        DealTermsId = opportunity.DealTermsId,
        Terms = terms,
        StartDate = opportunity.Period.Start,
        EndDate = opportunity.Period.End,
        Genres = opportunity.Genres
    };
}
