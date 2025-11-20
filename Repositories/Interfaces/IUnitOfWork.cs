using AgriculturalTech.API.Data.Models;

public interface IUnitOfWork : IDisposable
{
    IUserPlantRepository UserPlants { get; }
    ISensorReadingRepository SensorReadings { get; }
    ICropReminderRepository CropReminders { get; }
    IMarketPriceRepository MarketPrices { get; }
    IRepository<CropType> CropTypes { get; }
    IRepository<CropDisease> CropDiseases { get; }
    IRepository<DiseaseDetectionLog> DiseaseDetectionLogs { get; }
    IRepository<SensorDevice> SensorDevices { get; }
    IRepository<Device> Devices { get; }
    IRepository<PlantHealthLog> PlantHealthLogs { get; }
    IRepository<CropCalendarTemplate> CropCalendarTemplates { get; }
    IRepository<FertilizerRecommendation> FertilizerRecommendations { get; }
    IRepository<FertilizerApplicationLog> FertilizerApplicationLogs { get; }
    IRepository<WeatherAlert> WeatherAlerts { get; }
    IRepository<WeatherForecast> WeatherForecasts { get; }
    IRepository<ActivityLog> ActivityLogs { get; }
    IRepository<Notification> Notifications { get; }

    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
    //object Query<T>();

    IQueryable<T> Query<T>() where T : class;
}