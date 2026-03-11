using AgriculturalTech.API.Data;
using AgriculturalTech.API.Data.Models;
using AgriculturalTech.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AgriculturalTech.API.Repositories.Implementations
{
    public class AiAuthorizationRepository : Repository<ApplicationUser>, IAiAuthorizationRepository
    {
        public AiAuthorizationRepository(ApplicationDbContext context) : base(context) { }


        public async Task SetUserAsPremium(string userId)
        {
            var user = await _dbSet.FindAsync(userId);

            if (user != null)
            {
                if(user.IsPremiumUser)
                {
                    throw new Exception("User is already a premium user");
                }

                user.IsPremiumUser = true;

                _dbSet.Update(user);

                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("User not found");
            }
        }
        public async Task<bool> CanUserRunAiScanAsync(string userId)
        {
            // get only isFreescansFinished and IspremiumUser from _dbSet

            var user = await _dbSet
                .Where(u => u.Id == userId)
                .Select(u => new { u.LifetimeFreeScansUsed, u.IsPremiumUser })
                .FirstOrDefaultAsync();

            return user != null && (user.IsPremiumUser || user.LifetimeFreeScansUsed < 5);
        }

        public async Task RecordSuccessfulScanAsync(string userId)
        {
            var user = await _dbSet.FindAsync(userId);

            if (user != null)
            {
                user.LifetimeFreeScansUsed += 1;

                _dbSet.Update(user);

                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("User not found");
            }
        }
    }
}
