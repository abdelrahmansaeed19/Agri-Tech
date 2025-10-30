using AgriculturalTech.API.Data;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Cleans up old data (logs, old sensor readings, etc.)
/// Runs daily at 2 AM
/// </summary>
public class DataCleanupService : BackgroundService
{
    private readonly ILogger<DataCleanupService> _logger;
    private readonly IServiceProvider _serviceProvider;

    public DataCleanupService(
        ILogger<DataCleanupService> logger,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Data Cleanup Service started");

        while (!stoppingToken.IsCancellationRequested)
        {
            // Calculate time until next 2 AM
            var now = DateTime.UtcNow;
            var next2AM = now.Date.AddDays(1).AddHours(2);
            if (now.Hour < 2)
            {
                next2AM = now.Date.AddHours(2);
            }

            var delay = next2AM - now;
            await Task.Delay(delay, stoppingToken);

            try
            {
                await CleanupDataAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during data cleanup");
            }
        }

        _logger.LogInformation("Data Cleanup Service stopped");
    }

    private async Task CleanupDataAsync()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        _logger.LogInformation("Starting data cleanup");

        // Delete sensor readings older than 90 days
        var sensorReadingCutoff = DateTime.UtcNow.AddDays(-90);
        var oldSensorReadings = await context.SensorReadings
            .Where(r => r.ReadingTime < sensorReadingCutoff)
            .CountAsync();

        if (oldSensorReadings > 0)
        {
            await context.Database.ExecuteSqlRawAsync(
                "DELETE FROM SensorReadings WHERE ReadingTime < {0}", sensorReadingCutoff);
            _logger.LogInformation($"Deleted {oldSensorReadings} old sensor readings");
        }

        // Delete activity logs older than 180 days
        var activityLogCutoff = DateTime.UtcNow.AddDays(-180);
        var oldActivityLogs = await context.ActivityLogs
            .Where(l => l.CreatedAt < activityLogCutoff)
            .CountAsync();

        if (oldActivityLogs > 0)
        {
            await context.Database.ExecuteSqlRawAsync(
                "DELETE FROM ActivityLogs WHERE CreatedAt < {0}", activityLogCutoff);
            _logger.LogInformation($"Deleted {oldActivityLogs} old activity logs");
        }

        // Delete read notifications older than 30 days
        var notificationCutoff = DateTime.UtcNow.AddDays(-30);
        var oldNotifications = await context.Notifications
            .Where(n => n.IsRead && n.ReadAt < notificationCutoff)
            .CountAsync();

        if (oldNotifications > 0)
        {
            await context.Database.ExecuteSqlRawAsync(
                "DELETE FROM Notifications WHERE IsRead = 1 AND ReadAt < {0}", notificationCutoff);
            _logger.LogInformation($"Deleted {oldNotifications} old notifications");
        }

        // Delete old weather forecasts (older than 30 days)
        var weatherCutoff = DateTime.UtcNow.AddDays(-30);
        var oldForecasts = await context.WeatherForecasts
            .Where(w => w.ForecastDate < weatherCutoff)
            .CountAsync();

        if (oldForecasts > 0)
        {
            await context.Database.ExecuteSqlRawAsync(
                "DELETE FROM WeatherForecasts WHERE ForecastDate < {0}", weatherCutoff);
            _logger.LogInformation($"Deleted {oldForecasts} old weather forecasts");
        }

        _logger.LogInformation("Data cleanup completed");
    }
}