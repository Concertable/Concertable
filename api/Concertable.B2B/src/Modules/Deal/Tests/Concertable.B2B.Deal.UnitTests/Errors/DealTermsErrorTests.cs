using Concertable.B2B.Deal.Application.Errors;
using Concertable.B2B.Deal.Contracts.Errors;
using Reunion.Errors;

namespace Concertable.B2B.Deal.UnitTests.Errors;

public sealed class DealTermsErrorTests
{
    [Fact]
    public void Definition_DealTermsNotFound_ReturnsStableDefinition()
    {
        var definition = new DealTermsError.NotFound(42).Definition;

        Assert.Equal("deal.get.not_found", definition.Code);
        Assert.Equal("Deal 42 was not found.", definition.Message);
        Assert.Equal(ErrorKind.NotFound, definition.Kind);
    }

    [Fact]
    public void Definition_CreateValidation_ReturnsStableValidationDefinition()
    {
        var errors = new ValidationErrors([new("Fee", "Fee must be greater than zero.")]);

        var definition = Assert.IsType<ValidationError>(
            new CreateDealTermsError.Invalid(errors).Definition);

        Assert.Equal("deal.create.invalid", definition.Code);
        Assert.Equal("The deal is invalid.", definition.Message);
        Assert.Equal(ErrorKind.Invalid, definition.Kind);
        Assert.Equal(["Fee must be greater than zero."], definition.Errors.Errors["Fee"]);
    }

    [Fact]
    public void Definition_UpdateNotFound_ReturnsStableNotFoundDefinition()
    {
        var definition = new UpdateDealTermsError.DealTermsNotFound().Definition;

        Assert.Equal("deal.update.not_found", definition.Code);
        Assert.Equal("Deal not found.", definition.Message);
        Assert.Equal(ErrorKind.NotFound, definition.Kind);
    }

    [Fact]
    public void Definition_UpdateValidation_ReturnsStableValidationDefinition()
    {
        var errors = new ValidationErrors([new("HireFee", "Hire fee must be greater than zero.")]);

        var definition = Assert.IsType<ValidationError>(
            new UpdateDealTermsError.Invalid(errors).Definition);

        Assert.Equal("deal.update.invalid", definition.Code);
        Assert.Equal("The deal is invalid.", definition.Message);
        Assert.Equal(ErrorKind.Invalid, definition.Kind);
        Assert.Equal(["Hire fee must be greater than zero."], definition.Errors.Errors["HireFee"]);
    }
}
