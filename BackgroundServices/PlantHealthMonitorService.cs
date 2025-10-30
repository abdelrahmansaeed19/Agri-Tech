using AgriculturalTech.API.Data;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Monitors plant health based on sensor data and creates alerts
/// Runs every 30 minutes
/// </summary>
public class PlantHealthMonitorService : BackgroundService
{
    private readonly ILogger<PlantHealthMonitorService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly TimeSpan _interval = TimeSpan.FromMinutes(30);

    public PlantHealthMonitorService(
        ILogger<PlantHealthMonitorService> logger,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Plant Health Monitor Service started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await MonitorPlantHealthAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error monitoring plant health");
            }

            await Task.Delay(_interval, stoppingToken);
        }

        _logger.LogInformation("Plant Health Monitor Service stopped");
    }

    private async Task MonitorPlantHealthAsync()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

        // Get active plants with recent sensor readings
        var plants = await context.UserPlants
            .Include(p => p.CropType)
            .Include(p => p.SensorReadings.OrderByDescending(r => r.ReadingTime).Take(1))
            .Where(p => p.IsActive && p.Status == "Growing")
            .ToListAsync();

        _logger.LogInformation($"Monitoring {plants.Count} plants");

        foreach (var plant in plants)
        {
            try
            {
                var latestReading = plant.SensorReadings?.FirstOrDefault();

                if (latestReading == null)
                    continue;

                var alerts = new List<string>();

                // Check against ideal conditions
                if (plant.CropType != null)
                {
                    // Temperature check
                    if (latestReading.Temperature.HasValue)
                    {
                        if (plant.CropType.IdealTemperatureMin.HasValue &&
                            (latestReading.Temperature) < plant.CropType.IdealTemperatureMin)
                        {
                            alerts.Add($"Temperature too low ({latestReading.Temperature}°C). Ideal: {plant.CropType.IdealTemperatureMin}-{plant.CropType.IdealTemperatureMax}°C");
                        }
                        else if (plant.CropType.IdealTemperatureMax.HasValue &&
                                 (latestReading.Temperature) > plant.CropType.IdealTemperatureMax)
                        {
                            alerts.Add($"Temperature too high ({latestReading.Temperature}°C). Ideal: {plant.CropType.IdealTemperatureMin}-{plant.CropType.IdealTemperatureMax}°C");
                        }
                    }

                    // pH check
                    if (latestReading.Ph.HasValue && plant.CropType.IdealPhMin.HasValue && plant.CropType.IdealPhMax.HasValue)
                    {
                        if ((latestReading.Ph) < plant.CropType.IdealPhMin || (latestReading.Ph) > plant.CropType.IdealPhMax)
                        {
                            alerts.Add($"Soil pH out of range ({latestReading.Ph}). Ideal: {plant.CropType.IdealPhMin}-{plant.CropType.IdealPhMax}");
                        }
                    }

                    // Humidity check
                    if (latestReading.Humidity.HasValue && plant.CropType.IdealHumidityMin.HasValue && plant.CropType.IdealHumidityMax.HasValue)
                    {
                        if (latestReading.Humidity < plant.CropType.IdealHumidityMin || latestReading.Humidity > plant.CropType.IdealHumidityMax)
                        {
                            alerts.Add($"Humidity out of range ({latestReading.Humidity}%). Ideal: {plant.CropType.IdealHumidityMin}-{plant.CropType.IdealHumidityMax}%");
                        }
                    }
                }

                // Send notification if alerts exist
                if (alerts.Any())
                {
                    var plantName = !string.IsNullOrEmpty(plant.CustomName)
                        ? plant.CustomName
                        : plant.CropType?.NameEn ?? "Unknown";

                    await notificationService.SendAlertNotificationAsync(
                        plant.UserId,
                        $"Plant Alert: {plantName}",
                        string.Join("\n", alerts)
                    );

                    _logger.LogInformation($"Sent health alert for plant {plant.Id}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error monitoring plant {plant.Id}");
            }
        }
    }
}