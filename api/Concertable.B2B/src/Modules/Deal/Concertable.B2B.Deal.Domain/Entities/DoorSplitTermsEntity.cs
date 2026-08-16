using Reunion.Errors;
using Reunion;

namespace Concertable.B2B.Deal.Domain.Entities;

public sealed class DoorSplitTermsEntity : DealTermsEntity
{
    private DoorSplitTermsEntity() { }

    public override DealType DealType => DealType.DoorSplit;
    public decimal ArtistDoorPercent { get; private set; }

    public static Result<DoorSplitTermsEntity, ValidationErrors> Create(decimal artistDoorPercent, PaymentMethod paymentMethod)
    {
        var validation = ValidateArtistDoorPercent(artistDoorPercent);
        return validation.Bind(() => Result.Success<DoorSplitTermsEntity, ValidationErrors>(
            new DoorSplitTermsEntity { ArtistDoorPercent = artistDoorPercent, PaymentMethod = paymentMethod }));
    }

    public UnitResult<ValidationErrors> Update(decimal artistDoorPercent, PaymentMethod paymentMethod)
    {
        var validation = ValidateArtistDoorPercent(artistDoorPercent);
        if (validation.IsFailure)
            return validation;

        ArtistDoorPercent = artistDoorPercent;
        PaymentMethod = paymentMethod;
        return new Success();
    }

    private static UnitResult<ValidationErrors> ValidateArtistDoorPercent(decimal artistDoorPercent) =>
        artistDoorPercent is >= 0 and <= 100
            ? new Success()
            : new ValidationErrors(
                [new(nameof(ArtistDoorPercent), "Artist door percent must be between 0 and 100.")]);
}
