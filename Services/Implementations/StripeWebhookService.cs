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
        private readonly ILogger<StripeWebhookService> _logger;

        public StripeWebhookService(IUnitOfWork unitOfWork, IConfiguration config, IAiAuthorizationRepository aiAuthorizationRepository, ILogger<StripeWebhookService> logger)
        {
            _unitOfWork = unitOfWork;
            _authorizationRepository = aiAuthorizationRepository;
            _stripeWebhookSecret = config["Stripe:WebhookSecret"];
            _logger = logger;
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

                        _logger.LogInformation("==================================================");
                        _logger.LogInformation($"Received checkout session completed event for session ID: {session.Id}");
                        _logger.LogInformation("==================================================");

                        await HandleCheckoutCompletedAsync(session);

                        _logger.LogInformation("==================================================");
                        _logger.LogInformation($"Processed checkout session completed for session ID: {session.Id}");
                        _logger.LogInformation("==================================================");

                        break;

                    case EventTypes.CustomerSubscriptionUpdated:

                        var subscription = stripeEvent.Data.Object as Subscription;

                        _logger.LogInformation("==================================================");
                        _logger.LogInformation($"Received subscription updated event for subscription ID: {subscription.Id}");
                        _logger.LogInformation("==================================================");

                        await HandleSubscriptionUpdatedAsync(subscription);

                        _logger.LogInformation("==================================================");
                        _logger.LogInformation($"Processed subscription updated for subscription ID: {subscription.Id}");
                        _logger.LogInformation("==================================================");

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

            if (session.Mode == "subscription")
            {
                var newSubscription = new UserSubscription
                {
                    UserId = userId,
                    StripeSubscriptionId = session.SubscriptionId,
                    StripeCustomerId = session.CustomerId,
                    SubscriptionStatus = ParseStripeSubscriptionStatus(enSubscriptionStatus.Active),
                    CurrentPeriodStart = DateTime.UtcNow,
                    CurrentPeriodEnd = DateTime.UtcNow.AddMonths(1), // Assuming a 1-month subscription
                };

                _logger.LogInformation("==================================================");
                _logger.LogInformation($"Creating new subscription for user ID: {userId} with Stripe subscription ID: {newSubscription.StripeSubscriptionId}");
                _logger.LogInformation("==================================================");

                await _unitOfWork.UserSubscriptions.AddAsync(newSubscription);

                await _unitOfWork.SaveChangesAsync();

                await _authorizationRepository.SetUserAsPremium(userId);
            }
            else if (session.Mode == "payment")
            {
                if (session.Metadata.TryGetValue("PurchaseType", out string purchaseType) && purchaseType == "KitDevice")
                {
                    var newDevice = new SensorDevice
                    {
                        UserId = userId,
                        MacAddress = "Pending_Setup",
                    };

                    _logger.LogInformation("==================================================");
                    _logger.LogInformation($"Creating new sensor device for user ID: {userId} due to kit purchase");
                    _logger.LogInformation("==================================================");

                    await _unitOfWork.SensorDevices.AddAsync(newDevice);

                    await _unitOfWork.SaveChangesAsync();
                }
            }
        }

        public async Task HandleSubscriptionUpdatedAsync(Stripe.Subscription stripeSub)
        {
            var subscription = await _unitOfWork.UserSubscriptions
                .FirstOrDefaultAsync(s => s.StripeSubscriptionId == stripeSub.Id);

            _logger.LogInformation("==================================================");
            _logger.LogInformation($"Updating subscription with Stripe ID: {stripeSub.Id} for user subscription ID: {subscription?.Id}");
            _logger.LogInformation("==================================================");

            if (subscription != null)
            {
                subscription.SubscriptionStatus = stripeSub.Status;

                var primaryItem = stripeSub.Items.Data.FirstOrDefault();

                if (primaryItem != null)
                {
                    subscription.CurrentPeriodEnd = primaryItem.CurrentPeriodEnd;
                }

                subscription.CancelAtPeriodEnd = stripeSub.CancelAtPeriodEnd;

                _logger.LogInformation("==================================================");
                _logger.LogInformation($"Subscription status updated to: {subscription.SubscriptionStatus}, Current period end: {subscription.CurrentPeriodEnd}, Cancel at period end: {subscription.CancelAtPeriodEnd}");
                _logger.LogInformation("==================================================");

                _unitOfWork.UserSubscriptions.Update(subscription);

                await _unitOfWork.SaveChangesAsync();
            }
        }

        private string ParseStripeSubscriptionStatus(enSubscriptionStatus enSubscriptionStatus)
        {
            return enSubscriptionStatus switch
            {
                enSubscriptionStatus.Active => "active",
                enSubscriptionStatus.PastDue => "past_due",
                enSubscriptionStatus.Canceled => "canceled",
                enSubscriptionStatus.Unpaid => "unpaid",
                enSubscriptionStatus.Incomplete => "incomplete",
                _ => "canceled" // Fallback
            };
        }
    }
}
