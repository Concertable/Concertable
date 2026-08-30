using Concertable.B2B.Concert.Application.Strategies;
using Concertable.B2B.Tenant.Contracts.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace Concertable.B2B.Concert.Infrastructure.Services.Strategies;

internal sealed class ConcertTenantStrategyFactory<TStrategy> : IConcertTenantStrategyFactory<TStrategy>
    where TStrategy : class
{
    private readonly IKeyedServiceProvider services;

    public ConcertTenantStrategyFactory(IKeyedServiceProvider services)
    {
        this.services = services;
    }

    public TStrategy Create(TenantType tenantType) =>
        services.GetRequiredKeyedService<TStrategy>(tenantType);
}
