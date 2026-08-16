using Concertable.B2B.Deal.Contracts;
using Concertable.B2B.Deal.Domain.Entities;
using Reunion.Errors;
using Reunion;
using static Concertable.Seed.Identity.Extensions.EntityReflectionExtensions;

namespace Concertable.B2B.Seed.Infrastructure.Factories;

public static class FlatFeeTermsFactory
{
    public static FlatFeeTermsEntity Create(int id, decimal fee, PaymentMethod paymentMethod = PaymentMethod.Cash)
        => DealTermsFactory.RequireValid(FlatFeeTermsEntity.Create(fee, paymentMethod), id);
}

public static class VersusTermsFactory
{
    public static VersusTermsEntity Create(int id, decimal guarantee, decimal artistDoorPercent, PaymentMethod paymentMethod = PaymentMethod.Cash)
        => DealTermsFactory.RequireValid(VersusTermsEntity.Create(guarantee, artistDoorPercent, paymentMethod), id);
}

public static class DoorSplitTermsFactory
{
    public static DoorSplitTermsEntity Create(int id, decimal artistDoorPercent, PaymentMethod paymentMethod = PaymentMethod.Cash)
        => DealTermsFactory.RequireValid(DoorSplitTermsEntity.Create(artistDoorPercent, paymentMethod), id);
}

public static class VenueHireTermsFactory
{
    public static VenueHireTermsEntity Create(int id, decimal hireFee, PaymentMethod paymentMethod = PaymentMethod.Cash)
        => DealTermsFactory.RequireValid(VenueHireTermsEntity.Create(hireFee, paymentMethod), id);
}

internal static class DealTermsFactory
{
    internal static TDeal RequireValid<TDeal>(Result<TDeal, ValidationErrors> result, int id)
        where TDeal : DealTermsEntity =>
        result.Match(
            deal => deal.WithId(id),
            _ => throw new InvalidOperationException($"Seed deal {id} is invalid."));
}
