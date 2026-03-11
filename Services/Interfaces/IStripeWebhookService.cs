using Stripe.Checkout;
using Stripe;

namespace AgriculturalTech.API.Services.Interfaces
{
    public interface IStripeWebhookService
    {
        Task ProcessStripeEventAsync(string json, string signatureHeader);

        Task HandleCheckoutCompletedAsync(Session session);

        Task HandleSubscriptionUpdatedAsync(Subscription StripeSub);
    }
}
