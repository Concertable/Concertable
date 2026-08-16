namespace Concertable.B2B.Deal.Contracts;

public sealed record VersusTerms : IDealTerms
{
    public int Id { get; set; }

    public PaymentMethod PaymentMethod { get; set; }
    public DealType DealType => DealType.Versus;
    public decimal Guarantee { get; set; }
    public decimal ArtistDoorPercent { get; set; }
}
