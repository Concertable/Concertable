using Concertable.B2B.Tenant.Contracts.Enums;

namespace Concertable.B2B.Concert.Application.Strategies;

internal interface IConcertTenantStrategyFactory<TStrategy>
    where TStrategy : class
{
    TStrategy Create(TenantType tenantType);
}
