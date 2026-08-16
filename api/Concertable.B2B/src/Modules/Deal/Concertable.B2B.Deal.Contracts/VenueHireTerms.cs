namespace Concertable.B2B.Deal.Contracts;

public sealed record VenueHireTerms : IDealTerms
{
    public int Id { get; set; }

    public PaymentMethod PaymentMethod { get; set; }
    public DealType DealType => DealType.VenueHire;
    public decimal HireFee { get; set; }
}
