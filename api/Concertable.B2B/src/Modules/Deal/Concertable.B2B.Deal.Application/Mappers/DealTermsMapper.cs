using Concertable.B2B.Deal.Application.Interfaces;
using Concertable.B2B.Deal.Application.Strategies;
using Concertable.B2B.Deal.Domain.Entities;
using Reunion.Errors;
using Reunion;

namespace Concertable.B2B.Deal.Application.Mappers;

internal sealed class DealTermsMapper : IDealTermsMapper
{
    private readonly IDealStrategyFactory<IDealTermsMapper> strategies;

    public DealTermsMapper(IDealStrategyFactory<IDealTermsMapper> strategies)
    {
        this.strategies = strategies;
    }

    public IDealTerms ToTerms(DealTermsEntity entity) =>
        strategies.Create(entity.DealType).ToTerms(entity);

    public Result<DealTermsEntity, ValidationErrors> ToEntity(IDealTerms terms) =>
        strategies.Create(terms.DealType).ToEntity(terms);
}
