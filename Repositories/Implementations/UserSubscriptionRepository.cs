using AgriculturalTech.API.Repositories.Interfaces;
using AgriculturalTech.API.Data.Models;
using AgriculturalTech.API.Data;
using Microsoft.EntityFrameworkCore;

namespace AgriculturalTech.API.Repositories.Implementations
{
    public class UserSubscriptionRepository : Repository<UserSubscription>, IUserSubscriptionRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserSubscriptionRepository(ApplicationDbContext context, IUnitOfWork unitOfWork) : base(context) 
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<UserSubscription> GetSubscriptionByUserIdAsync(string userId)
        {
            return await _dbSet
                .Where(x => x.UserId == userId)
                .FirstOrDefaultAsync();
        }

        public async Task AddSubscriptionAsync(UserSubscription subscription)
        {
            await _unitOfWork.UserSubscriptions.AddAsync(subscription);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateSubscriptionAsync(UserSubscription subscription)
        {
            _unitOfWork.UserSubscriptions.Update(subscription);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
