using Concertable.B2B.Deal.Domain.Entities;

namespace Concertable.B2B.Deal.UnitTests.Entities;

public sealed class DealTermsEntityValidationTests
{
    [Fact]
    public void Create_NonPositiveFlatFee_ReturnsValidationFailure()
    {
        var result = FlatFeeTermsEntity.Create(0, PaymentMethod.Cash);

        Assert.True(result.TryGetError(out var errors));
        Assert.Contains(nameof(FlatFeeTermsEntity.Fee), errors.Errors.Keys);
    }

    [Fact]
    public void Create_NonPositiveHireFee_ReturnsValidationFailure()
    {
        var result = VenueHireTermsEntity.Create(0, PaymentMethod.Cash);

        Assert.True(result.TryGetError(out var errors));
        Assert.Contains(nameof(VenueHireTermsEntity.HireFee), errors.Errors.Keys);
    }

    [Fact]
    public void Create_OutOfRangeDoorPercent_ReturnsValidationFailure()
    {
        var result = DoorSplitTermsEntity.Create(101, PaymentMethod.Cash);

        Assert.True(result.TryGetError(out var errors));
        Assert.Contains(nameof(DoorSplitTermsEntity.ArtistDoorPercent), errors.Errors.Keys);
    }

    [Fact]
    public void Create_InvalidVersusTerms_AccumulatesValidationFailures()
    {
        var result = VersusTermsEntity.Create(-1, 101, PaymentMethod.Cash);

        Assert.True(result.TryGetError(out var errors));
        Assert.Contains(nameof(VersusTermsEntity.Guarantee), errors.Errors.Keys);
        Assert.Contains(nameof(VersusTermsEntity.ArtistDoorPercent), errors.Errors.Keys);
    }

    [Fact]
    public void Update_InvalidTerms_ReturnsFailureWithoutMutatingDeal()
    {
        var creation = VersusTermsEntity.Create(100, 50, PaymentMethod.Cash);
        Assert.True(creation.TryGetValue(out var deal));

        var result = deal.Update(-1, 101, PaymentMethod.Transfer);

        Assert.True(result.IsFailure);
        Assert.Equal(100, deal.Guarantee);
        Assert.Equal(50, deal.ArtistDoorPercent);
        Assert.Equal(PaymentMethod.Cash, deal.PaymentMethod);
    }

    [Fact]
    public void Update_NonPositiveFlatFee_ReturnsFailureWithoutMutatingDeal()
    {
        var creation = FlatFeeTermsEntity.Create(100, PaymentMethod.Cash);
        Assert.True(creation.TryGetValue(out var deal));

        var result = deal.Update(0, PaymentMethod.Transfer);

        Assert.True(result.IsFailure);
        Assert.Equal(100, deal.Fee);
        Assert.Equal(PaymentMethod.Cash, deal.PaymentMethod);
    }

    [Fact]
    public void Update_NonPositiveHireFee_ReturnsFailureWithoutMutatingDeal()
    {
        var creation = VenueHireTermsEntity.Create(100, PaymentMethod.Cash);
        Assert.True(creation.TryGetValue(out var deal));

        var result = deal.Update(0, PaymentMethod.Transfer);

        Assert.True(result.IsFailure);
        Assert.Equal(100, deal.HireFee);
        Assert.Equal(PaymentMethod.Cash, deal.PaymentMethod);
    }

    [Fact]
    public void Update_OutOfRangeDoorPercent_ReturnsFailureWithoutMutatingDeal()
    {
        var creation = DoorSplitTermsEntity.Create(50, PaymentMethod.Cash);
        Assert.True(creation.TryGetValue(out var deal));

        var result = deal.Update(101, PaymentMethod.Transfer);

        Assert.True(result.IsFailure);
        Assert.Equal(50, deal.ArtistDoorPercent);
        Assert.Equal(PaymentMethod.Cash, deal.PaymentMethod);
    }
}
