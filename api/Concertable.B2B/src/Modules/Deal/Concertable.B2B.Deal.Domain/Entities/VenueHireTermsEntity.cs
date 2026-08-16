using Reunion.Errors;
using Reunion;

namespace Concertable.B2B.Deal.Domain.Entities;

public sealed class VenueHireTermsEntity : DealTermsEntity
{
    private VenueHireTermsEntity() { }

    public override DealType DealType => DealType.VenueHire;
    public decimal HireFee { get; private set; }

    public static Result<VenueHireTermsEntity, ValidationErrors> Create(decimal hireFee, PaymentMethod paymentMethod)
    {
        var validation = ValidateFee(hireFee);
        return validation.Bind(() => Result.Success<VenueHireTermsEntity, ValidationErrors>(
            new VenueHireTermsEntity { HireFee = hireFee, PaymentMethod = paymentMethod }));
    }

    public UnitResult<ValidationErrors> Update(decimal hireFee, PaymentMethod paymentMethod)
    {
        var validation = ValidateFee(hireFee);
        if (validation.IsFailure)
            return validation;

        HireFee = hireFee;
        PaymentMethod = paymentMethod;
        return new Success();
    }

    private static UnitResult<ValidationErrors> ValidateFee(decimal hireFee) =>
        hireFee > 0
            ? new Success()
            : new ValidationErrors([new(nameof(HireFee), "Hire fee must be greater than zero.")]);
}
