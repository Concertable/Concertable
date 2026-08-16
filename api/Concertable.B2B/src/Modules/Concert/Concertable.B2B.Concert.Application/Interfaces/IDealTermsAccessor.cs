using Concertable.B2B.Deal.Contracts;

namespace Concertable.B2B.Concert.Application.Interfaces;

internal interface IDealTermsAccessor
{
    IDealTerms Terms { get; }
}
