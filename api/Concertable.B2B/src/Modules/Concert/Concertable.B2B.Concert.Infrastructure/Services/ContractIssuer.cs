using Concertable.B2B.Concert.Domain.Entities;
using Concertable.Kernel.Exceptions;
using Concertable.Kernel.Identity;
using Microsoft.Extensions.Options;

namespace Concertable.B2B.Concert.Infrastructure.Services;

internal sealed class ContractIssuer : IContractIssuer
{
    private readonly IDealTermsAccessor dealTermsAccessor;
    private readonly IApplicationRepository applicationRepository;
    private readonly IContractRepository contractRepository;
    private readonly IDealTermsRenderer termsRenderer;
    private readonly ICurrentUser currentUser;
    private readonly IClientContext clientContext;
    private readonly LegalSettings legal;
    private readonly TimeProvider timeProvider;

    public ContractIssuer(
        IDealTermsAccessor dealTermsAccessor,
        IApplicationRepository applicationRepository,
        IContractRepository contractRepository,
        IDealTermsRenderer termsRenderer,
        ICurrentUser currentUser,
        IClientContext clientContext,
        IOptions<LegalSettings> legal,
        TimeProvider timeProvider)
    {
        this.dealTermsAccessor = dealTermsAccessor;
        this.applicationRepository = applicationRepository;
        this.contractRepository = contractRepository;
        this.termsRenderer = termsRenderer;
        this.currentUser = currentUser;
        this.clientContext = clientContext;
        this.legal = legal.Value;
        this.timeProvider = timeProvider;
    }

    public async Task IssueAsync(
        ApplicationEntity application,
        BookingEntity booking,
        ESignatureRequest venueESignature)
    {
        var deal = dealTermsAccessor.Terms;
        var (artist, venue) = await applicationRepository.GetArtistAndVenueByIdAsync(application.Id)
            .OrNotFound(DisplayNames.Application);

        var contract = ContractEntity.Create(
            booking,
            venue.Id,
            venue.Name,
            artist.Id,
            artist.Name,
            application.Opportunity.Period,
            deal,
            termsRenderer.Render(deal),
            legal.PlatformTermsVersion,
            application.ArtistESignature,
            new ESignature(
                currentUser.Id ?? throw new ForbiddenException("No user for current request"),
                timeProvider.GetUtcNow().UtcDateTime,
                clientContext.IpAddress,
                clientContext.UserAgent,
                venueESignature.SignatoryName,
                venueESignature.DrawnSignatureImage),
            timeProvider.GetUtcNow().UtcDateTime);

        await contractRepository.AddAsync(contract);
    }
}
