using Reunion.Errors;
using Dunet;

namespace Concertable.B2B.Deal.Application.Errors;

[Union(EnableImplicitConversions = false)]
internal abstract partial record DealTermsError : IError
{
    public ErrorDefinition Definition => this switch
    {
        NotFound(var dealTermsId) =>
            ErrorDefinition.NotFound<NotFound>($"Deal {dealTermsId} was not found.")
    };

    [ErrorCode("deal.get.not_found")]
    public partial record NotFound(int DealTermsId);
}
