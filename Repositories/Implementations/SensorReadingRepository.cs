using AgriculturalTech.API.Data;
using Microsoft.EntityFrameworkCore;

public class SensorReadingRepository : Repository<SensorReading>, ISensorReadingRepository
{
    public SensorReadingRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<SensorReading>> GetLatestReadingsByDeviceAsync(int deviceId, int count = 100)
    {
        return await _dbSet
            .Where(r => r.SensorDeviceId == deviceId)
            .OrderByDescending(r => r.ReadingTime)
            .Take(count)
            .ToListAsync();
    }

    public async Task<IEnumerable<SensorReading>> GetReadingsByDateRangeAsync(int deviceId, DateTime startDate, DateTime endDate)
    {
        return await _dbSet
            .Where(r => r.SensorDeviceId == deviceId
                && r.ReadingTime >= startDate
                && r.ReadingTime <= endDate)
            .OrderBy(r => r.ReadingTime)
            .ToListAsync();
    }

    public async Task<SensorReading> GetLatestReadingAsync(int deviceId)
    {
        return await _dbSet
            .Where(r => r.SensorDeviceId == deviceId)
            .OrderByDescending(r => r.ReadingTime)
            .FirstOrDefaultAsync();
    }

    public async Task<Dictionary<string, decimal?>> GetAverageReadingsAsync(int deviceId, int days = 7)
    {
        var startDate = DateTime.UtcNow.AddDays(-days);

        var readings = await _dbSet
            .Where(r => r.SensorDeviceId == deviceId && r.ReadingTime >= startDate)
            .ToListAsync();

        if (!readings.Any())
            return new Dictionary<string, decimal?>();

        return new Dictionary<string, decimal?>
            {
                { "Nitrogen", readings.Where(r => r.Nitrogen.HasValue).Average(r => r.Nitrogen) },
                { "Phosphorous", readings.Where(r => r.Phosphorous.HasValue).Average(r => r.Phosphorous) },
                { "Potassium", readings.Where(r => r.Potassium.HasValue).Average(r => r.Potassium) },
                { "Ph", readings.Where(r => r.Ph.HasValue).Average(r => r.Ph) },
                { "Humidity", readings.Where(r => r.Humidity.HasValue).Average(r => r.Humidity) },
                { "Temperature", readings.Where(r => r.Temperature.HasValue).Average(r => r.Temperature) },
                { "SoilMoisture", readings.Where(r => r.SoilMoisture.HasValue).Average(r => r.SoilMoisture) }
            };
    }
}