public interface IWeatherService
{
    Task<List<WeatherForecastDto>> GetWeatherForecastAsync(string location, int days = 7);
    Task SyncWeatherAlertsAsync(string userId, string location);
}