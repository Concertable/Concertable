using Concertable.B2B.Deal.Application.Interfaces;
using Concertable.B2B.Deal.Application.Mappers;
using Concertable.B2B.Deal.Application.Strategies;
using Concertable.B2B.Deal.Infrastructure.Extensions;
using Concertable.B2B.Deal.Infrastructure.Services.Updaters;
using Microsoft.Extensions.DependencyInjection;

namespace Concertable.B2B.Deal.UnitTests.Strategies;

public sealed class DealStrategyFactoryTests
{
    [Theory]
    [InlineData(DealType.FlatFee, typeof(FlatFeeTermsMapper), typeof(FlatFeeTermsUpdater))]
    [InlineData(DealType.DoorSplit, typeof(DoorSplitTermsMapper), typeof(DoorSplitTermsUpdater))]
    [InlineData(DealType.Versus, typeof(VersusTermsMapper), typeof(VersusTermsUpdater))]
    [InlineData(DealType.VenueHire, typeof(VenueHireTermsMapper), typeof(VenueHireTermsUpdater))]
    public void Create_DealType_ResolvesExpectedStrategiesFromRequestScope(
        DealType dealType,
        Type expectedMapperType,
        Type expectedUpdaterType)
    {
        var services = new ServiceCollection();
        services.AddDealStrategies();
        using var provider = services.BuildServiceProvider(new ServiceProviderOptions
        {
            ValidateScopes = true
        });
        using var scope = provider.CreateScope();

        var mapper = scope.ServiceProvider
            .GetRequiredService<IDealStrategyFactory<IDealTermsMapper>>()
            .Create(dealType);
        var updater = scope.ServiceProvider
            .GetRequiredService<IDealStrategyFactory<IDealTermsUpdater>>()
            .Create(dealType);

        Assert.IsType(expectedMapperType, mapper);
        Assert.IsType(expectedUpdaterType, updater);
    }

    [Theory]
    [InlineData(typeof(IKeyedServiceProvider))]
    [InlineData(typeof(IDealStrategyFactory<>))]
    [InlineData(typeof(IDealTermsMapper))]
    [InlineData(typeof(IDealTermsUpdater))]
    public void AddDealStrategies_ScopeCapturingServices_RegistersScoped(Type serviceType)
    {
        var services = new ServiceCollection();

        services.AddDealStrategies();

        var descriptor = Assert.Single(
            services,
            candidate => candidate.ServiceType == serviceType && !candidate.IsKeyedService);
        Assert.Equal(ServiceLifetime.Scoped, descriptor.Lifetime);
    }

    [Fact]
    public void Create_SingletonStrategies_AreSharedAcrossScopes()
    {
        var services = new ServiceCollection();
        services.AddDealStrategies();
        using var provider = services.BuildServiceProvider(new ServiceProviderOptions
        {
            ValidateScopes = true
        });
        using var firstScope = provider.CreateScope();
        using var secondScope = provider.CreateScope();

        var first = firstScope.ServiceProvider
            .GetRequiredService<IDealStrategyFactory<IDealTermsMapper>>()
            .Create(DealType.FlatFee);
        var second = secondScope.ServiceProvider
            .GetRequiredService<IDealStrategyFactory<IDealTermsMapper>>()
            .Create(DealType.FlatFee);

        Assert.Same(first, second);
    }
}
