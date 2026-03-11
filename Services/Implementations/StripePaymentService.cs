using Stripe;
using AgriculturalTech.API.Services.Interfaces;
using Stripe.Checkout;
using AgriculturalTech.API.Repositories.Interfaces;

namespace AgriculturalTech.API.Services.Implementations
{
    public class StripePaymentService : IStripePaymentService
    {
        private readonly IConfiguration _config;
        private readonly IUserSubscriptionRepository _userSubRepo;

        public StripePaymentService(IConfiguration config, IUserSubscriptionRepository userSubRepo)
        {
            _config = config;

            _userSubRepo = userSubRepo;

            StripeConfiguration.ApiKey = _config["Stripe:SecretKey"];
        }


        public async Task<string> CreateSubscriptionCheckoutSessionAsync(string userId, string email)
        {
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                CustomerEmail = email,
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        Price = _config["Stripe:PremiumPlanPriceId"],
                        Quantity = 1
                    },
                },
                Mode = "subscription",
                SuccessUrl = "smartplant://payment/success",
                CancelUrl = "smartplant://payment/cancel",
                ClientReferenceId = userId
            };

            var service = new SessionService();

            Session session = await service.CreateAsync(options);

            return session.Url;
        }

        public async Task<string> CreateKitCheckoutSessionAsync(string userId, string email)
        {
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                CustomerEmail = email,
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        Price = _config["Stripe:KitDevicePriceId"],
                        Quantity = 1
                    },
                },
                Mode = "payment",
                SuccessUrl = "smartplant://payment/success",
                CancelUrl = "smartplant://payment/cancel",
                ClientReferenceId = userId,

                Metadata = new Dictionary<string, string>
                {
                    { "PurchaseType", "KitDevice" },
                    { "Item" , "ESP32_Kit" }
                }
            };

            var service = new SessionService();

            Session session = await service.CreateAsync(options);

            return session.Url;
        }

        public async Task<string> CreateCustomerPortalSessionAsync(string userId)
        {
            var userSub = await _userSubRepo.GetSubscriptionByUserIdAsync(userId);

            if (userSub == null || string.IsNullOrEmpty(userSub.StripeCustomerId))
            {
                throw new Exception("User does not have an active subscription.");
            }

            var options = new Stripe.BillingPortal.SessionCreateOptions
            {
                Customer = userSub.StripeCustomerId,
                ReturnUrl = "smartplant://payment/portal-return"
            };

            var service = new Stripe.BillingPortal.SessionService();
            var session = await service.CreateAsync(options);

            return session.Url;
        }
    }
}
