using Concertable.Kernel;

namespace Concertable.B2B.Deal.Domain.Entities;

public abstract class DealTermsEntity : IIdEntity, ITenantScoped
{
    protected DealTermsEntity() { }

    public int Id { get; private set; }
    public Guid TenantId { get; set; }
    public PaymentMethod PaymentMethod { get; protected set; }
    public abstract DealType DealType { get; }
}
