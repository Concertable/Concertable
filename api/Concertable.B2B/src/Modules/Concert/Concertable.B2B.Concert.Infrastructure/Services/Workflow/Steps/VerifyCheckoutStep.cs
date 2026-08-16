using Concertable.B2B.Concert.Application.Mappers;
using Concertable.B2B.Concert.Application.Responses;
using Concertable.B2B.Concert.Application.Workflow.Steps;
using Concertable.Kernel.Exceptions;

namespace Concertable.B2B.Concert.Infrastructure.Services.Workflow.Steps;

internal sealed class VerifyCheckoutStep : IAcceptCheckoutStep
{
    private readonly IApplicationRepository applicationRepository;
    private readonly IDealTermsAccessor dealTermsAccessor;
    private readonly IManagerPaymentOperationsClient managerPaymentClient;
    private readonly IPaymentAmountMapper paymentAmountMapper;

    public VerifyCheckoutStep(
        IApplicationRepository applicationRepository,
        IDealTermsAccessor dealTermsAccessor,
        IManagerPaymentOperationsClient managerPaymentClient,
        IPaymentAmountMapper paymentAmountMapper)
    {
        this.applicationRepository = applicationRepository;
        this.dealTermsAccessor = dealTermsAccessor;
        this.managerPaymentClient = managerPaymentClient;
        this.paymentAmountMapper = paymentAmountMapper;
    }

    public async Task<Checkout> ExecuteAsync(int applicationId)
    {
        var artist = await applicationRepository.GetArtistPayeeAsync(applicationId)
            .OrNotFound(DisplayNames.Application);
        /* the user id rides the Stripe metadata so the failure webhook can notify the venue manager */
        var venueManagerId = await applicationRepository.GetVenueManagerIdAsync(applicationId)
            .OrNotFound(DisplayNames.Application);
        var venueTenantId = await applicationRepository.GetVenueTenantIdAsync(applicationId)
            .OrNotFound(DisplayNames.Application);

        var metadata = new Dictionary<string, string>
        {
            [PaymentMetadataKeys.Type] = TransactionTypes.Verify,
            [PaymentMetadataKeys.ApplicationId] = applicationId.ToString(),
            [PaymentMetadataKeys.VenueManagerId] = venueManagerId.ToString()
        };

        var session = await managerPaymentClient.CreateVerifySessionAsync(venueTenantId, metadata);
        var amount = paymentAmountMapper.ToPaymentAmount(dealTermsAccessor.Terms);
        return new Checkout(amount, artist, session, CheckoutLabels.Settlement);
    }
}
