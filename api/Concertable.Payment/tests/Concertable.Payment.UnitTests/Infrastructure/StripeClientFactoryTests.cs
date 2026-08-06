using Concertable.Payment.Infrastructure.Services;
using Concertable.Payment.Infrastructure.Settings;
using Stripe;

namespace Concertable.Payment.UnitTests.Infrastructure;

public sealed class StripeClientFactoryTests
{
    [Fact]
    public void Create_UsesBoundedTimeoutAndConfiguredRetries()
    {
        var settings = new StripeSettings
        {
            SecretKey = "sk_test_key",
            RequestTimeoutSeconds = 7,
            MaxNetworkRetries = 3,
        };

        var client = StripeClientFactory.Create(settings);

        var stripeClient = Assert.IsType<StripeClient>(client);
        var httpClient = Assert.IsType<SystemNetHttpClient>(stripeClient.HttpClient);
        Assert.Equal(3, httpClient.MaxNetworkRetries);
        Assert.Equal(TimeSpan.FromSeconds(7), StripeClientFactory.CreateHttpClient(settings).Timeout);
    }
}
