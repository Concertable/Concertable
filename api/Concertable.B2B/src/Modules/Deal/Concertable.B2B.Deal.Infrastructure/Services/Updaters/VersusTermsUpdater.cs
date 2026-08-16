using Concertable.B2B.Deal.Contracts;
using Concertable.B2B.Deal.Application.Interfaces;
using Concertable.B2B.Deal.Domain.Entities;
using Reunion.Errors;
using Reunion;

namespace Concertable.B2B.Deal.Infrastructure.Services.Updaters;

internal sealed class VersusTermsUpdater : IDealTermsUpdater
{
    public UnitResult<ValidationErrors> Apply(DealTermsEntity existing, IDealTerms source)
    {
        var entity = (VersusTermsEntity)existing;
        var deal = (VersusTerms)source;
        return entity.Update(deal.Guarantee, deal.ArtistDoorPercent, deal.PaymentMethod);
    }
}
