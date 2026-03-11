using Stripe;
using Stripe.Checkout;
using AgriculturalTech.API.Services.Interfaces;
using AgriculturalTech.API.Data.Models;
using AgriculturalTech.API.Data.Enums;
using AgriculturalTech.API.Repositories.Interfaces;

namespace AgriculturalTech.API.Services.Implementations
{
    public class StripeWebhookService : IStripeWebhookService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAiAuthorizationRepository _authorizationRepository;
        private readonly string _stripeWebhookSecret;

        public StripeWebhookService(IUnitOfWork unitOfWork, IConfiguration config, IAiAuthorizationRepository aiAuthorizationRepository)
        {
            _unitOfWork = unitOfWork;
            _authorizationRepository = aiAuthorizationRepository;
            _stripeWebhookSecret = config["Stripe:WebhookSecret"];
        }

        public async Task ProcessStripeEventAsync(string json, string signatureHeader)
        {
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json, signatureHeader, _stripeWebhookSecret);

                switch (stripeEvent.Type)
                {

                    case EventTypes.CheckoutSessionCompleted:
                        var session = stripeEvent.Data.Object as Session;
                        await HandleCheckoutCompletedAsync(session);
                        break;

                    case EventTypes.CustomerSubscriptionUpdated:
                        var subscription = stripeEvent.Data.Object as Subscription;
                        await HandleSubscriptionUpdatedAsync(subscription);
                        break;
                    // Handle other event types as needed
                    default:
                        Console.WriteLine($"Unhandled Stripe event type: {stripeEvent.Type}");
                        break;
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error processing Stripe webhook: {ex.Message}");

                throw; // Re-throw to ensure Stripe receives a non-200 response
            }
        }

        public async Task HandleCheckoutCompletedAsync(Session session)
        {
            var userId = session.ClientReferenceId;

            if(session.Mode == "subscription")
            {
                var newSubscription = new UserSubscription
                {
                    UserId = userId,
                    StripeSubscriptionId = session.SubscriptionId,
                    StripeCustomerId = session.CustomerId,
                    SubscriptionStatus = enSubscriptionStatus.Active,
                    CurrentPeriodStart = DateTime.UtcNow,
                    CurrentPeriodEnd = DateTime.UtcNow.AddMonths(1), // Assuming a 1-month subscription
                };

                await _unitOfWork.UserSubscriptions.AddAsync(newSubscription);

                await _unitOfWork.SaveChangesAsync();

                await _authorizationRepository.SetUserAsPremium(userId);
            }
            else if (session.Mode == "payment")
            {
                if(session.Metadata.TryGetValue("PurchaseType", out string purchaseType) && purchaseType == "KitDevice")
                {
                    var newDevice = new SensorDevice
                    {
                        UserId = userId,
                        MacAddress = "Pending_Setup",
                    };

                    await _unitOfWork.SensorDevices.AddAsync(newDevice);

                    await _unitOfWork.SaveChangesAsync();
                }
            }
        }

        public async Task HandleSubscriptionUpdatedAsync(Stripe.Subscription stripeSub)
        {
            var subscription = await _unitOfWork.UserSubscriptions
                .FirstOrDefaultAsync(s => s.StripeSubscriptionId == stripeSub.Id);

            if (subscription != null)
            {
                subscription.SubscriptionStatus = ParseStripeStatus(stripeSub.Status);

                var primaryItem = stripeSub.Items.Data.FirstOrDefault();

                if (primaryItem != null)
                {
                    subscription.CurrentPeriodEnd = primaryItem.CurrentPeriodEnd;
                }

                subscription.CancelAtPeriodEnd = stripeSub.CancelAtPeriodEnd;

                _unitOfWork.UserSubscriptions.Update(subscription);

                await _unitOfWork.SaveChangesAsync();
            }
        }

        private enSubscriptionStatus ParseStripeStatus(string stripeStatus)
        {
            return stripeStatus.ToLower() switch
            {
                "active" => enSubscriptionStatus.Active,
                "past_due" => enSubscriptionStatus.PastDue,
                "canceled" => enSubscriptionStatus.Canceled,
                "unpaid" => enSubscriptionStatus.Unpaid,
                "incomplete" => enSubscriptionStatus.Incomplete,
                _ => enSubscriptionStatus.Canceled // Fallback
            };
        }
    }
}
