using Concertable.B2B.Deal.Contracts;
using Concertable.B2B.Deal.Application.Interfaces;
using Concertable.B2B.Deal.Domain.Entities;
using Reunion.Errors;
using Reunion;

namespace Concertable.B2B.Deal.Infrastructure.Services.Updaters;

internal sealed class VenueHireTermsUpdater : IDealTermsUpdater
{
    public UnitResult<ValidationErrors> Apply(DealTermsEntity existing, IDealTerms source)
    {
        var entity = (VenueHireTermsEntity)existing;
        var deal = (VenueHireTerms)source;
        return entity.Update(deal.HireFee, deal.PaymentMethod);
    }
}
