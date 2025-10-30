using AgriculturalTech.API.Data;
using AgriculturalTech.API.Data.Models;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction _transaction;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;

        UserPlants = new UserPlantRepository(_context);
        SensorReadings = new SensorReadingRepository(_context);
        CropReminders = new CropReminderRepository(_context);
        MarketPrices = new MarketPriceRepository(_context);
        CropTypes = new Repository<CropType>(_context);
        CropDiseases = new Repository<CropDisease>(_context);
        DiseaseDetectionLogs = new Repository<DiseaseDetectionLog>(_context);
        SensorDevices = new Repository<SensorDevice>(_context);
        PlantHealthLogs = new Repository<PlantHealthLog>(_context);
        CropCalendarTemplates = new Repository<CropCalendarTemplate>(_context);
        FertilizerRecommendations = new Repository<FertilizerRecommendation>(_context);
        FertilizerApplicationLogs = new Repository<FertilizerApplicationLog>(_context);
        WeatherAlerts = new Repository<WeatherAlert>(_context);
        WeatherForecasts = new Repository<WeatherForecast>(_context);
        ActivityLogs = new Repository<ActivityLog>(_context);
        Notifications = new Repository<Notification>(_context);
    }

    public IUserPlantRepository UserPlants { get; private set; }
    public ISensorReadingRepository SensorReadings { get; private set; }
    public ICropReminderRepository CropReminders { get; private set; }
    public IMarketPriceRepository MarketPrices { get; private set; }
    public IRepository<CropType> CropTypes { get; private set; }
    public IRepository<CropDisease> CropDiseases { get; private set; }
    public IRepository<DiseaseDetectionLog> DiseaseDetectionLogs { get; private set; }
    public IRepository<SensorDevice> SensorDevices { get; private set; }
    public IRepository<PlantHealthLog> PlantHealthLogs { get; private set; }
    public IRepository<CropCalendarTemplate> CropCalendarTemplates { get; private set; }
    public IRepository<FertilizerRecommendation> FertilizerRecommendations { get; private set; }
    public IRepository<FertilizerApplicationLog> FertilizerApplicationLogs { get; private set; }
    public IRepository<WeatherAlert> WeatherAlerts { get; private set; }
    public IRepository<WeatherForecast> WeatherForecasts { get; private set; }
    public IRepository<ActivityLog> ActivityLogs { get; private set; }
    public IRepository<Notification> Notifications { get; private set; }

    public IQueryable<T> Query<T>() where T : class
    {
        return _context.Set<T>().AsQueryable();
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
            await _transaction.CommitAsync();
        }
        catch
        {
            await RollbackTransactionAsync();
            throw;
        }
        finally
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}
