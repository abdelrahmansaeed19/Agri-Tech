
namespace AgriculturalTech.API.Services.Interfaces
{
    public interface IStripePaymentService
    {
        Task<string> CreateSubscriptionCheckoutSessionAsync(string userId, string email);

        Task<string> CreateKitCheckoutSessionAsync(string userId, string email);

        Task<string> CreateCustomerPortalSessionAsync(string userId);
    }
}
