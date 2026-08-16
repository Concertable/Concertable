using Concertable.B2B.Deal.Domain.Entities;
using Reunion.Errors;
using Reunion;

namespace Concertable.B2B.Deal.Application.Interfaces;

internal interface IDealTermsMapper
{
    IDealTerms ToTerms(DealTermsEntity entity);
    Result<DealTermsEntity, ValidationErrors> ToEntity(IDealTerms terms);

    IReadOnlyList<IDealTerms> ToTerms(IEnumerable<DealTermsEntity> entities) => entities.Select(ToTerms).ToList();
}
