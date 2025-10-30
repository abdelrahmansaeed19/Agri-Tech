using AgriculturalTech.API.Data.Models;

public interface IUserPlantRepository : IRepository<UserPlant>
{
    Task<IEnumerable<UserPlant>> GetUserPlantsWithDetailsAsync(string userId);
    Task<IEnumerable<UserPlant>> GetActivePlantsByUserAsync(string userId);
    Task<UserPlant> GetPlantWithSensorReadingsAsync(int plantId);
    Task<IEnumerable<UserPlant>> GetPlantsNeedingHarvestAsync(string userId);
}