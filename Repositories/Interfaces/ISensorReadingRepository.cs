public interface ISensorReadingRepository : IRepository<SensorReading>
{
    Task<IEnumerable<SensorReading>> GetLatestReadingsByDeviceAsync(int deviceId, int count = 100);
    Task<IEnumerable<SensorReading>> GetReadingsByDateRangeAsync(int deviceId, DateTime startDate, DateTime endDate);
    Task<SensorReading> GetLatestReadingAsync(int deviceId);
    Task<Dictionary<string, decimal?>> GetAverageReadingsAsync(int deviceId, int days = 7);
}