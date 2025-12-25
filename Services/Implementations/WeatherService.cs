using System.Text.Json;

public class WeatherService : IWeatherService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;

    public WeatherService(IUnitOfWork unitOfWork, IConfiguration configuration, HttpClient httpClient)
    {
        _unitOfWork = unitOfWork;
        _configuration = configuration;
        _httpClient = httpClient;
    }

    public async Task<List<WeatherForecastDto>> GetWeatherForecastAsync(string location, int days = 7)
    {
        var (lat, lon) = await GeocodeLocationAsync(location);

        var url = $"https://api.open-meteo.com/v1/forecast?latitude={lat}&longitude={lon}&daily=temperature_2m_max,temperature_2m_min,precipitation_sum,windspeed_10m_max,weathercode&timezone=auto&forecast_days={days}";

        var response = await _httpClient.GetStringAsync(url);

        var weatherData = JsonSerializer.Deserialize<OpenMeteoResponse>(response);

        var forecasts = new List<WeatherForecastDto>();

        if (weatherData != null && weatherData.Daily != null)
        {
            for (int i = 0; i < weatherData.Daily.Time.Count; i++)
            {
                var weatherCondition = ParseWmoCode(weatherData.Daily.WeatherCode[i]);

                var forecast = new WeatherForecastDto
                {
                    Date = DateTime.Parse(weatherData.Daily.Time[i]),
                    TemperatureMax = weatherData.Daily.TemperatureMax[i],
                    TemperatureMin = weatherData.Daily.TemperatureMin[i],
                    RainfallMm = weatherData.Daily.PrecipitationSum[i],
                    WindSpeedKmh = weatherData.Daily.WindSpeedMax[i],
                    WeatherCondition = weatherCondition,
                    FarmingAdvice = GetFarmingAdvice(weatherCondition)
                };

                forecasts.Add(forecast);
            }
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

    private string GetFarmingAdvice(string condition)
    {
        if (condition == "Rainy" || condition == "Stormy") return "Avoid applying pesticides; ensure drainage systems are clear.";
        if (condition == "Sunny") return "Good day for irrigation and harvesting.";
        return "Monitor humidity levels for sensitive crops.";
    }

    // Helper: Convert "Cairo" to Lat/Lon using geocoding API
    private async Task<(double lat, double lon)> GeocodeLocationAsync(string location)
    {
        try
        {
            string encodedLocation = Uri.EscapeDataString(location);

            string url = $"https://geocoding-api.open-meteo.com/v1/search?name={encodedLocation}&count=1&language=en&format=json";

            var response = await _httpClient.GetStringAsync(url);

            var geoData = JsonSerializer.Deserialize<GeoCodeResponseDto>(response);

            if (geoData != null && geoData.Results != null && geoData.Results.Count > 0)
            {
                var firstResult = geoData.Results[0];
                return (firstResult.Lat, firstResult.Lon);
            }
        }
        catch
        {
            return (0, 0);
        }

        return (0, 0);
    }

    private string ParseWmoCode(int code)
    {
        return code switch
        {
            0 or 1 => "Sunny",
            2 or 3 => "Cloudy",
            45 or 48 => "Foggy",
            >= 51 and <= 67 => "Rainy",
            >= 80 and <= 82 => "Rainy",
            >= 71 and <= 77 => "Snowy",
            >= 95 => "Stormy",
            _ => "Cloudy"
        };
    }
}