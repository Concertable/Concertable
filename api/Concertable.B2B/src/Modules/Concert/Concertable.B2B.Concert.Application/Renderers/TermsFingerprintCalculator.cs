using System.Security.Cryptography;
using System.Text;
using Concertable.B2B.Concert.Application.Interfaces;
using static System.FormattableString;

namespace Concertable.B2B.Concert.Application.Renderers;

internal sealed class TermsFingerprintCalculator : ITermsFingerprintCalculator
{
    private readonly IDealTermsSerializer termsSerializer;

    public TermsFingerprintCalculator(IDealTermsSerializer termsSerializer) => this.termsSerializer = termsSerializer;

    public string Calculate(IDealTerms terms, DateRange period)
    {
        var numbers = termsSerializer.Serialize(terms);
        var payload = Invariant(
            $"{terms.DealType}|{terms.PaymentMethod}|{numbers}|{TermsFingerprintFormat.Instant(period.Start)}|{TermsFingerprintFormat.Instant(period.End)}");
        return Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(payload)));
    }
}
