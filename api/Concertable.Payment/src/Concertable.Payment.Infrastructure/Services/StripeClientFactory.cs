using Concertable.Payment.Infrastructure.Settings;
using Stripe;

namespace Concertable.Payment.Infrastructure.Services;

internal static class StripeClientFactory
{
    public static IStripeClient Create(StripeSettings settings)
    {
        var httpClient = CreateHttpClient(settings);
        var stripeHttpClient = new SystemNetHttpClient(
            httpClient,
            settings.MaxNetworkRetries);

        return new StripeClient(settings.SecretKey, httpClient: stripeHttpClient);
    }

    internal static HttpClient CreateHttpClient(StripeSettings settings) =>
        new() { Timeout = TimeSpan.FromSeconds(settings.RequestTimeoutSeconds) };
}
