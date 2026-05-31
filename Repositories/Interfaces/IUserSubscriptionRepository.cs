using AgriculturalTech.API.Data.Models;

namespace AgriculturalTech.API.Repositories.Interfaces
{
    public interface IUserSubscriptionRepository
    {
        Task<UserSubscription> GetSubscriptionByUserIdAsync(string userId);

        Task AddSubscriptionAsync(UserSubscription subscription);

        Task UpdateSubscriptionAsync(UserSubscription subscription);
    }
}
