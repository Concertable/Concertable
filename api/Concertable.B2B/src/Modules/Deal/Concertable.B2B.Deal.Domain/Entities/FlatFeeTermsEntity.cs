using Reunion.Errors;
using Reunion;

namespace Concertable.B2B.Deal.Domain.Entities;

public sealed class FlatFeeTermsEntity : DealTermsEntity
{
    private FlatFeeTermsEntity() { }

    public override DealType DealType => DealType.FlatFee;
    public decimal Fee { get; private set; }

    public static Result<FlatFeeTermsEntity, ValidationErrors> Create(decimal fee, PaymentMethod paymentMethod)
    {
        var validation = ValidateFee(fee);
        return validation.Bind(() => Result.Success<FlatFeeTermsEntity, ValidationErrors>(
            new FlatFeeTermsEntity { Fee = fee, PaymentMethod = paymentMethod }));
    }

    public UnitResult<ValidationErrors> Update(decimal fee, PaymentMethod paymentMethod)
    {
        var validation = ValidateFee(fee);
        if (validation.IsFailure)
            return validation;

        Fee = fee;
        PaymentMethod = paymentMethod;
        return new Success();
    }

    private static UnitResult<ValidationErrors> ValidateFee(decimal fee) =>
        fee > 0
            ? new Success()
            : new ValidationErrors([new(nameof(Fee), "Fee must be greater than zero.")]);
}
