using Concertable.B2B.Concert.Application.Strategies;
using Concertable.B2B.Concert.Infrastructure.Extensions;
using Concertable.B2B.Tenant.Contracts.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace Concertable.B2B.Concert.UnitTests.Strategies;

public sealed class ConcertTenantStrategyFactoryTests
{
    [Theory]
    [InlineData(TenantType.Venue, typeof(VenueTestStrategy))]
    [InlineData(TenantType.Artist, typeof(ArtistTestStrategy))]
    public void Create_TenantType_ResolvesExpectedStrategyFromRequestScope(
        TenantType tenantType,
        Type expectedType)
    {
        var services = CreateServicesWithStrategies();
        using var provider = services.BuildServiceProvider(new ServiceProviderOptions
        {
            ValidateScopes = true
        });
        using var scope = provider.CreateScope();
        var factory = scope.ServiceProvider
            .GetRequiredService<IConcertTenantStrategyFactory<ITestStrategy>>();

        var strategy = factory.Create(tenantType);

        Assert.IsType(expectedType, strategy);
    }

    [Fact]
    public void Resolve_FactoryLifetime_IsScoped()
    {
        var services = CreateServicesWithStrategies();
        using var provider = services.BuildServiceProvider(new ServiceProviderOptions
        {
            ValidateScopes = true
        });
        using var firstScope = provider.CreateScope();
        using var secondScope = provider.CreateScope();

        var first = firstScope.ServiceProvider
            .GetRequiredService<IConcertTenantStrategyFactory<ITestStrategy>>();
        var sameScope = firstScope.ServiceProvider
            .GetRequiredService<IConcertTenantStrategyFactory<ITestStrategy>>();
        var second = secondScope.ServiceProvider
            .GetRequiredService<IConcertTenantStrategyFactory<ITestStrategy>>();

        Assert.Same(first, sameScope);
        Assert.NotSame(first, second);
        Assert.Throws<InvalidOperationException>(() =>
            provider.GetRequiredService<IConcertTenantStrategyFactory<ITestStrategy>>());
    }

    [Fact]
    public void AddConcertTenantStrategies_MissingCoverage_ThrowsOnBuild()
    {
        var services = new ServiceCollection();

        Assert.Throws<InvalidOperationException>(() =>
            services.AddConcertTenantStrategies(strategies =>
            {
                strategies.For(TenantType.Venue).AddScoped<ITestStrategy, VenueTestStrategy>();
                strategies.RequireAll<ITestStrategy>();
            }));
    }

    private static ServiceCollection CreateServicesWithStrategies()
    {
        var services = new ServiceCollection();
        services.AddConcertTenantStrategies(strategies =>
        {
            strategies.For(TenantType.Venue).AddScoped<ITestStrategy, VenueTestStrategy>();
            strategies.For(TenantType.Artist).AddScoped<ITestStrategy, ArtistTestStrategy>();
            strategies.RequireAll<ITestStrategy>();
        });

        return services;
    }

    private interface ITestStrategy;

    private sealed class VenueTestStrategy : ITestStrategy;

    private sealed class ArtistTestStrategy : ITestStrategy;
}
