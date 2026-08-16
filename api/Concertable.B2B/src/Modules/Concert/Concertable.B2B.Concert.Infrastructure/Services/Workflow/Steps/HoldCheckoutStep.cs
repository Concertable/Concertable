using Concertable.B2B.Concert.Application.Responses;
using Concertable.B2B.Concert.Application.Workflow.Steps;
using Concertable.B2B.Deal.Contracts;
using Concertable.Kernel.Exceptions;
using Concertable.Kernel.ValueObjects;

namespace Concertable.B2B.Concert.Infrastructure.Services.Workflow.Steps;

internal sealed class HoldCheckoutStep : IAcceptCheckoutStep
{
    private readonly IApplicationRepository applicationRepository;
    private readonly IDealTermsAccessor dealTermsAccessor;
    private readonly IManagerPaymentOperationsClient managerPaymentClient;

    public HoldCheckoutStep(
        IApplicationRepository applicationRepository,
        IDealTermsAccessor dealTermsAccessor,
        IManagerPaymentOperationsClient managerPaymentClient)
    {
        this.applicationRepository = applicationRepository;
        this.dealTermsAccessor = dealTermsAccessor;
        this.managerPaymentClient = managerPaymentClient;
    }

    public async Task<Checkout> ExecuteAsync(int applicationId)
    {
        var artist = await applicationRepository.GetArtistPayeeAsync(applicationId)
            .OrNotFound(DisplayNames.Application);
        var venueTenantId = await applicationRepository.GetVenueTenantIdAsync(applicationId)
            .OrNotFound(DisplayNames.Application);
        var deal = (FlatFeeTerms)dealTermsAccessor.Terms;

        var metadata = new Dictionary<string, string>
        {
            [PaymentMetadataKeys.Type] = TransactionTypes.ApplicationAccept,
            [PaymentMetadataKeys.ApplicationId] = applicationId.ToString()
        };

        var session = await managerPaymentClient.CreateHoldSessionAsync(venueTenantId, Money.Gbp(deal.Fee), metadata);
        return new Checkout(new FlatPayment(deal.Fee), artist, session, CheckoutLabels.Charge);
    }
}
