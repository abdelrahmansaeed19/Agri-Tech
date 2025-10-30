using AgriculturalTech.API.Data;
using AgriculturalTech.API.Data.Models;
using Microsoft.EntityFrameworkCore;

public class UserPlantRepository : Repository<UserPlant>, IUserPlantRepository
{
    public UserPlantRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<UserPlant>> GetUserPlantsWithDetailsAsync(string userId)
    {
        return await _dbSet
            .Include(p => p.CropType)
            .Include(p => p.PlantHealthLogs)
            .Where(p => p.UserId == userId && p.IsActive)
            .OrderByDescending(p => p.PlantingDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<UserPlant>> GetActivePlantsByUserAsync(string userId)
    {
        return await _dbSet
            .Include(p => p.CropType)
            .Where(p => p.UserId == userId && p.IsActive && p.Status == "Growing")
            .ToListAsync();
    }

    public async Task<UserPlant> GetPlantWithSensorReadingsAsync(int plantId)
    {
        return await _dbSet
            .Include(p => p.CropType)
            .Include(p => p.SensorReadings.OrderByDescending(r => r.ReadingTime).Take(10))
            .Include(p => p.PlantHealthLogs.OrderByDescending(l => l.LoggedAt).Take(5))
            .FirstOrDefaultAsync(p => p.Id == plantId);
    }

    public async Task<IEnumerable<UserPlant>> GetPlantsNeedingHarvestAsync(string userId)
    {
        var today = DateTime.UtcNow.Date;
        return await _dbSet
            .Include(p => p.CropType)
            .Where(p => p.UserId == userId
                && p.IsActive
                && p.Status == "Growing"
                && p.ExpectedHarvestDate.HasValue
                && p.ExpectedHarvestDate.Value <= today.AddDays(7))
            .ToListAsync();
    }
}