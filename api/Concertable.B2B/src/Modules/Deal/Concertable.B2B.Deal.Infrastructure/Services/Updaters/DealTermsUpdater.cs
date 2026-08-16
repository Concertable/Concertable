using Concertable.B2B.Deal.Contracts;
using Concertable.B2B.Deal.Application.Interfaces;
using Concertable.B2B.Deal.Application.Strategies;
using Concertable.B2B.Deal.Domain.Entities;
using Reunion.Errors;
using Reunion;

namespace Concertable.B2B.Deal.Infrastructure.Services.Updaters;

internal sealed class DealTermsUpdater : IDealTermsUpdater
{
    private readonly IDealStrategyFactory<IDealTermsUpdater> strategies;

    public DealTermsUpdater(IDealStrategyFactory<IDealTermsUpdater> strategies)
    {
        this.strategies = strategies;
    }

    public UnitResult<ValidationErrors> Apply(DealTermsEntity existing, IDealTerms source) =>
        strategies.Create(source.DealType).Apply(existing, source);
}
