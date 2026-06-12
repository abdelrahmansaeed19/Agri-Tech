using Stripe;
using AgriculturalTech.API.Services.Interfaces;
using Stripe.Checkout;
using AgriculturalTech.API.Repositories.Interfaces;
using AgriculturalTech.API.Data.Enums;

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


        private string _GetPriceId(SubscriptionPlanType enSubscriptionPlanType)
        {
            return enSubscriptionPlanType switch
            {
                SubscriptionPlanType.Monthly => "Stripe:1_month_plan_priceID",
                SubscriptionPlanType.Quarterly => "Stripe:3_month_plan_priceID",
                SubscriptionPlanType.Yearly => "Stripe:1_year_plan_priceID",
                _ => throw new ArgumentException("Invalid subscription plan type")
            };
        }

        public async Task<string> CreateSubscriptionCheckoutSessionAsync(string userId, string email, SubscriptionPlanType enSubscriptionPlanType)
        {
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                CustomerEmail = email,
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        Price = _config[_GetPriceId(enSubscriptionPlanType)],
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
                SuccessUrl = "smartplant://payment/kit-success",
                CancelUrl = "smartplant://payment/kit-cancel",
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
