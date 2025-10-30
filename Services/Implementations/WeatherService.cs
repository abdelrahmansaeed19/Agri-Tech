public class WeatherService : IWeatherService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;
    // In real implementation, inject HttpClient for weather API calls

    public WeatherService(IUnitOfWork unitOfWork, IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _configuration = configuration;
    }

    public async Task<List<WeatherForecastDto>> GetWeatherForecastAsync(string location, int days = 7)
    {
        // TODO: Call external weather API (OpenWeatherMap, WeatherAPI, etc.)
        // For now, returning placeholder data

        var forecasts = new List<WeatherForecastDto>();
        var random = new Random();

        for (int i = 0; i < days; i++)
        {
            forecasts.Add(new WeatherForecastDto
            {
                Date = DateTime.UtcNow.AddDays(i),
                TemperatureMax = 25 + random.Next(-5, 10),
                TemperatureMin = 15 + random.Next(-5, 5),
                Humidity = 60 + random.Next(-10, 20),
                RainfallMm = random.Next(0, 20),
                WindSpeedKmh = 10 + random.Next(0, 15),
                WeatherCondition = i % 3 == 0 ? "Rainy" : (i % 2 == 0 ? "Cloudy" : "Sunny"),
                FarmingAdvice = GetFarmingAdvice(i % 3)
            });
        }

        return forecasts;
    }

    public async Task SyncWeatherAlertsAsync(string userId, string location)
    {
        // TODO: Fetch alerts from weather API
        // For now, creating sample alert

        var existingAlerts = await _unitOfWork.WeatherAlerts
            .CountAsync(a => a.UserId == userId && !a.IsRead);

        if (existingAlerts == 0)
        {
            var alert = new WeatherAlert
            {
                UserId = userId,
                Location = location,
                AlertType = "Storm",
                Severity = "Medium",
                MessageEn = "Moderate rain expected in the next 48 hours. Ensure proper drainage.",
                MessageAr = "من المتوقع هطول أمطار معتدلة في الـ 48 ساعة القادمة. تأكد من الصرف المناسب.",
                AlertStartDate = DateTime.UtcNow.AddHours(12),
                AlertEndDate = DateTime.UtcNow.AddHours(60),
                Source = "WeatherAPI"
            };

            await _unitOfWork.WeatherAlerts.AddAsync(alert);
            await _unitOfWork.SaveChangesAsync();
        }
    }

    private string GetFarmingAdvice(int weatherType)
    {
        return weatherType switch
        {
            0 => "Rainy day - good for root development. Avoid fertilizer application.",
            1 => "Cloudy conditions - ideal for transplanting and outdoor work.",
            _ => "Sunny weather - ensure adequate irrigation. Good for harvesting."
        };
    }
}