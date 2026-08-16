using Concertable.B2B.DataAccess.Infrastructure;
using Concertable.DataAccess;
using Concertable.Seed.Shared;
using Concertable.Seed.Shared.Extensions;
using Concertable.B2B.Deal.Application.Interfaces;
using Concertable.B2B.Deal.Application.Mappers;
using Concertable.B2B.Deal.Application.Services;
using Concertable.B2B.Deal.Application.Strategies;
using Concertable.B2B.Deal.Infrastructure.Data;
using Concertable.B2B.Deal.Infrastructure.Data.Seeders;
using Concertable.B2B.Deal.Infrastructure.Repositories;
using Concertable.B2B.Deal.Infrastructure.Services.Strategies;
using Concertable.B2B.Deal.Infrastructure.Services.Updaters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Concertable.DataAccess.Infrastructure.Data;

namespace Concertable.B2B.Deal.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDealModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DealDbContext>((sp, opt) =>
            opt.UseSqlServer(configuration.GetConnectionString(B2BDb.Name))
                .AddInterceptors(
                    sp.GetRequiredService<AuditInterceptor>(),
                    sp.GetRequiredService<TenantInterceptor>(),
                    sp.GetRequiredService<IDomainEventDispatchInterceptor>())
                .UseSeedingSupport(sp));

        services.AddScoped<IDealTermsRepository, DealTermsRepository>();
        services.AddScoped<IDealTermsService, DealTermsService>();
        services.AddScoped<IDealTermsModule, DealTermsModule>();

        services.AddDealStrategies();

        services.AddSingleton<DealConfigurationProvider>();
        services.AddSingleton<IEntityTypeConfigurationProvider>(sp => sp.GetRequiredService<DealConfigurationProvider>());

        return services;
    }

    internal static IServiceCollection AddDealStrategies(this IServiceCollection services)
    {
        services.AddScoped<IDealTermsMapper, DealTermsMapper>();
        services.AddScoped<IDealTermsUpdater, DealTermsUpdater>();

        return services.AddDealStrategies(strategies =>
        {
            strategies.For(DealType.FlatFee)
                .AddSingleton<IDealTermsMapper, FlatFeeTermsMapper>()
                .AddSingleton<IDealTermsUpdater, FlatFeeTermsUpdater>();
            strategies.For(DealType.DoorSplit)
                .AddSingleton<IDealTermsMapper, DoorSplitTermsMapper>()
                .AddSingleton<IDealTermsUpdater, DoorSplitTermsUpdater>();
            strategies.For(DealType.Versus)
                .AddSingleton<IDealTermsMapper, VersusTermsMapper>()
                .AddSingleton<IDealTermsUpdater, VersusTermsUpdater>();
            strategies.For(DealType.VenueHire)
                .AddSingleton<IDealTermsMapper, VenueHireTermsMapper>()
                .AddSingleton<IDealTermsUpdater, VenueHireTermsUpdater>();

            strategies.RequireAll<IDealTermsMapper>();
            strategies.RequireAll<IDealTermsUpdater>();
        });
    }

    internal static IServiceCollection AddDealStrategies(
        this IServiceCollection services,
        Action<DealStrategyBuilder> configure)
    {
        var builder = new DealStrategyBuilder(services);
        configure(builder);
        builder.Build();

        services.TryAddScoped<IKeyedServiceProvider>(sp => (IKeyedServiceProvider)sp);
        services.TryAddScoped(typeof(IDealStrategyFactory<>), typeof(DealStrategyFactory<>));
        return services;
    }

    public static IServiceCollection AddDealDevSeeder(this IServiceCollection services)
    {
        services.AddScoped<IDevSeeder, DealTermsDevSeeder>();
        return services;
    }

    public static IServiceCollection AddDealTestSeeder(this IServiceCollection services)
    {
        services.AddScoped<ITestSeeder, DealTermsTestSeeder>();
        return services;
    }
}
