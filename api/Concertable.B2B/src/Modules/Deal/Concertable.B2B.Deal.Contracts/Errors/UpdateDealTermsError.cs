using Reunion.Errors;
using Dunet;

namespace Concertable.B2B.Deal.Contracts.Errors;

[Union(EnableImplicitConversions = false)]
public abstract partial record UpdateDealTermsError : IError
{
    public ErrorDefinition Definition => this switch
    {
        DealTermsNotFound =>
            ErrorDefinition.NotFound<DealTermsNotFound>("Deal not found."),
        Invalid(var errors) =>
            ErrorDefinition.Validation<Invalid>(
                "The deal is invalid.",
                errors)
    };

    [ErrorCode("deal.update.not_found")]
    public partial record DealTermsNotFound;

    [ErrorCode("deal.update.invalid")]
    public partial record Invalid(ValidationErrors Errors);
}
