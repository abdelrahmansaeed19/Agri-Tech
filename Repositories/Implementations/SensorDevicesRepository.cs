using AgriculturalTech.API.Repositories.Interfaces;
using AgriculturalTech.API.Data.Models;
using AgriculturalTech.API.Data;
using Microsoft.EntityFrameworkCore;

namespace AgriculturalTech.API.Repositories.Implementations
{
    public class SensorDevicesRepository : Repository<SensorDevice>, ISensorDevicesRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public SensorDevicesRepository(ApplicationDbContext context, IUnitOfWork unitOfWork) : base(context) 
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<SensorDevice> GetDeviceByUserIdAsync(string userid)
        {
            return await _dbSet
                .FirstOrDefaultAsync(x => x.UserId == userid);
        }

        public async Task<bool> IsDevicePurchasedByUserIdAsync(string userid)
        {
            return await _dbSet.AnyAsync(x => x.UserId == userid);
        }

        public async Task<List<SensorDevice>> GetDevicesByUserIdAsync(string userId)
        {
            return await _dbSet
                .Where(x => x.UserId == userId)
                .ToListAsync();
        }

    }
}
