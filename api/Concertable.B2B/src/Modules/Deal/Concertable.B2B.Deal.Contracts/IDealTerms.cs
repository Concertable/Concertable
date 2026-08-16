using System.Text.Json.Serialization;

namespace Concertable.B2B.Deal.Contracts;

[JsonDerivedType(typeof(FlatFeeTerms), DealTypeNames.FlatFee)]
[JsonDerivedType(typeof(DoorSplitTerms), DealTypeNames.DoorSplit)]
[JsonDerivedType(typeof(VersusTerms), DealTypeNames.Versus)]
[JsonDerivedType(typeof(VenueHireTerms), DealTypeNames.VenueHire)]
public interface IDealTerms
{
    int Id { get; set; }
    PaymentMethod PaymentMethod { get; set; }
    DealType DealType { get; }
}
