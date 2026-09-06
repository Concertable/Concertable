using Concertable.B2B.Concert.Api.Controllers;
using Concertable.B2B.Concert.Api.Mappers;
using Concertable.B2B.Concert.Api.Validators;
using Concertable.B2B.Concert.Infrastructure.Extensions;
using Concertable.Shared.Api.Extensions;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Concertable.B2B.Concert.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConcertApi(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddConcertModule(configuration);
        services.AddSingleton<IApplicationMapper, ApplicationMapper>();
        services.AddSingleton<IOpportunityMapper, OpportunityMapper>();
        services.AddConcertTenantStrategies(strategies =>
        {
            strategies.For(TenantType.Venue)
                .AddScoped<IApplicationResponseMapper, VenueApplicationResponseMapper>();
            strategies.For(TenantType.Artist)
                .AddScoped<IApplicationResponseMapper, ArtistApplicationResponseMapper>();

            strategies.RequireAll<IApplicationResponseMapper>();
        });
        services.AddValidatorsFromAssemblyContaining<ApplyRequestValidator>(includeInternalTypes: true);
        services.AddControllers()
            .AddInternalControllers(typeof(ConcertController).Assembly);
        return services;
    }
}
