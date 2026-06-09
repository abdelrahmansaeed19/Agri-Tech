using AgriculturalTech.API.Data;
using AgriculturalTech.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Syncs weather forecasts and alerts every 6 hours
/// </summary>
public class WeatherSyncService : BackgroundService
{
    private readonly ILogger<WeatherSyncService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly TimeSpan _interval = TimeSpan.FromHours(6);

    public WeatherSyncService(
        ILogger<WeatherSyncService> logger,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Weather Sync Service started");

        // Run immediately on startup
        await SyncWeatherDataAsync();

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(_interval, stoppingToken);

            try
            {
                await SyncWeatherDataAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error syncing weather data");
            }
        }

        _logger.LogInformation("Weather Sync Service stopped");
    }

    private async Task SyncWeatherDataAsync()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var weatherService = scope.ServiceProvider.GetRequiredService<IWeatherService>();

        var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

        // Get unique user locations
        var userLocations = await context.Users
            .Where(u => u.IsActive && !string.IsNullOrEmpty(u.FarmLocation))
            .Select(u => new { u.Id, u.FarmLocation, u.FcmToken })
            .Distinct()
            .ToListAsync();

        _logger.LogInformation($"Syncing weather for {userLocations.Count} locations");

        foreach (var user in userLocations)
        {
            try
            {
                // Sync weather forecasts
                var forecasts = await weatherService.GetWeatherForecastAsync(user.FarmLocation, 7);

                // Save forecasts to database
                foreach (var forecast in forecasts)
                {
                    var existingForecast = await context.WeatherForecasts
                        .FirstOrDefaultAsync(w => w.Location == user.FarmLocation
                            && w.ForecastDate.Date == forecast.Date.Date);

                    if (existingForecast == null)
                    {
                        var weatherForecast = new WeatherForecast
                        {
                            Location = user.FarmLocation,
                            ForecastDate = forecast.Date,
                            TemperatureMax = forecast.TemperatureMax,
                            TemperatureMin = forecast.TemperatureMin,
                            RainfallMm = forecast.RainfallMm,
                            WindSpeedKmh = forecast.WindSpeedKmh,
                            WeatherCondition = forecast.WeatherCondition,
                            Source = "WeatherAPI"
                        };

                        await context.WeatherForecasts.AddAsync(weatherForecast);
                    }
                }

                //var todayForecast = forecasts.FirstOrDefault(f => f.Date.Date == DateTime.UtcNow.Date);

                //if(todayForecast != null)
                //{
                //    if (todayForecast.TemperatureMax > 35)
                //    {

                //        try
                //        {
                //            await notificationService.SendNotificationAsync(user.FcmToken, "Heat Alert", $"High temperatures expected in on {todayForecast.Date:MMMM dd}. Stay hydrated and protect your crops!");
                //        }
                //        catch (Exception ex)
                //        {
                //            _logger.LogError(ex, $"Error sending heat alert notification to user {user.Id}");
                //        }
                //    }

                //    if (todayForecast.TemperatureMin <= 35)
                //    {
                //        try
                //        {
                //            await notificationService.SendNotificationAsync(user.FcmToken, "Cold Alert", $"Low temperatures expected in on {todayForecast.Date:MMMM dd}. Consider frost protection for your crops!");
                //        }
                //        catch
                //        {
                //            _logger.LogError($"Error sending cold alert notification to user {user.Id}");
                //        }
                //    }
                //}

                // Sync weather alerts
                await weatherService.SyncWeatherAlertsAsync(user.Id, user.FarmLocation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error syncing weather for user {user.Id}");
            }
        }

        await context.SaveChangesAsync();
        _logger.LogInformation("Weather sync completed");
    }
}