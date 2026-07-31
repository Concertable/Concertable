using Concertable.Kernel.Errors;
using Dunet;

namespace Concertable.Customer.Ticket.Application.Errors;

[Union]
internal partial record PurchaseError : IError
{
    partial record ConcertNotFound(int ConcertId);
    partial record Validation(IReadOnlyList<string> Messages);
    partial record PaymentRejected;

    public ErrorDescriptor Descriptor => Match<ErrorDescriptor>(
        notFound => new ErrorDescriptor(
            "ticket.concert_not_found",
            $"Concert {notFound.ConcertId} was not found.",
            ErrorKind.NotFound),
        validation => new ValidationErrorDescriptor(
            "ticket.purchase_invalid",
            "The ticket purchase is invalid.",
            new Dictionary<string, string[]> { ["purchase"] = validation.Messages.ToArray() }),
        paymentRejected => new ErrorDescriptor(
            "ticket.payment_rejected",
            "The payment was rejected.",
            ErrorKind.PaymentRequired));
}
