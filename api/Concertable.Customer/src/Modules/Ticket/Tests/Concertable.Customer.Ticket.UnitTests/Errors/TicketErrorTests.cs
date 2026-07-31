using Concertable.Customer.Ticket.Application.Errors;
using Concertable.Kernel.Errors;

namespace Concertable.Customer.Ticket.UnitTests;

public sealed class TicketErrorTests
{
    [Fact]
    public void Descriptor_PurchaseConcertNotFound_ReturnsStableNotFoundError()
    {
        PurchaseError error = new PurchaseError.ConcertNotFound(42);

        var descriptor = error.Descriptor;

        Assert.Equal("ticket.concert_not_found", descriptor.Code);
        Assert.Equal("Concert 42 was not found.", descriptor.Message);
        Assert.Equal(ErrorKind.NotFound, descriptor.Kind);
    }

    [Fact]
    public void Descriptor_PurchaseValidation_ReturnsStructuredValidationError()
    {
        PurchaseError error = new PurchaseError.Validation(["Not enough tickets available."]);

        var descriptor = Assert.IsType<ValidationErrorDescriptor>(error.Descriptor);

        Assert.Equal("ticket.purchase_invalid", descriptor.Code);
        Assert.Equal("The ticket purchase is invalid.", descriptor.Message);
        Assert.Equal(["Not enough tickets available."], descriptor.Errors["purchase"]);
    }

    [Fact]
    public void Descriptor_PaymentRejected_ReturnsStablePaymentRequiredError()
    {
        PurchaseError error = new PurchaseError.PaymentRejected();

        var descriptor = error.Descriptor;

        Assert.Equal("ticket.payment_rejected", descriptor.Code);
        Assert.Equal("The payment was rejected.", descriptor.Message);
        Assert.Equal(ErrorKind.PaymentRequired, descriptor.Kind);
    }

    [Fact]
    public void Descriptor_CheckoutConcertNotFound_ReturnsStableNotFoundError()
    {
        CheckoutError error = new CheckoutError.ConcertNotFound(42);

        var descriptor = error.Descriptor;

        Assert.Equal("ticket.concert_not_found", descriptor.Code);
        Assert.Equal("Concert 42 was not found.", descriptor.Message);
        Assert.Equal(ErrorKind.NotFound, descriptor.Kind);
    }

    [Fact]
    public void Descriptor_CheckoutValidation_ReturnsStructuredValidationError()
    {
        CheckoutError error = new CheckoutError.Validation(["Not enough tickets available."]);

        var descriptor = Assert.IsType<ValidationErrorDescriptor>(error.Descriptor);

        Assert.Equal("ticket.checkout_invalid", descriptor.Code);
        Assert.Equal("The ticket checkout is invalid.", descriptor.Message);
        Assert.Equal(["Not enough tickets available."], descriptor.Errors["checkout"]);
    }
}
