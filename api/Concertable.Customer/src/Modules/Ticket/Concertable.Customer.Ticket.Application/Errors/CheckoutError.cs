using Concertable.Kernel.Errors;
using Dunet;

namespace Concertable.Customer.Ticket.Application.Errors;

[Union]
internal partial record CheckoutError : IError
{
    partial record ConcertNotFound(int ConcertId);
    partial record Validation(IReadOnlyList<string> Messages);

    public ErrorDescriptor Descriptor => Match<ErrorDescriptor>(
        notFound => new ErrorDescriptor(
            "ticket.concert_not_found",
            $"Concert {notFound.ConcertId} was not found.",
            ErrorKind.NotFound),
        validation => new ValidationErrorDescriptor(
            "ticket.checkout_invalid",
            "The ticket checkout is invalid.",
            new Dictionary<string, string[]> { ["checkout"] = validation.Messages.ToArray() }));
}
