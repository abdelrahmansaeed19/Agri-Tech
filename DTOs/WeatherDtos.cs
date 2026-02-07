using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


public class ForcastRequestDto
{
    [Required]
    public string Location { get; set; }

    public int Days { get; set; } = 7;
}
public class WeatherForecastDto
{
    public DateTime Date { get; set; }
    public decimal? TemperatureMax { get; set; }
    public decimal? TemperatureMin { get; set; }
    public decimal? RainfallMm { get; set; }
    public decimal? WindSpeedKmh { get; set; }
    public string WeatherCondition { get; set; }
    public string FarmingAdvice { get; set; }
}

public class WeatherAlertDto
{
    public int Id { get; set; }
    public string AlertType { get; set; }
    public string Severity { get; set; }
    public string MessageEn { get; set; }
    public string MessageAr { get; set; }
    public DateTime AlertStartDate { get; set; }
    public DateTime? AlertEndDate { get; set; }
    public bool IsRead { get; set; }
    public List<string> RecommendedActions { get; set; }
}

public class OpenMeteoResponse
{
    [JsonPropertyName("daily")]
    public DailyData Daily { get; set; }
}

public class DailyData
{
    [JsonPropertyName("time")]
    public List<string> Time { get; set; }

    [JsonPropertyName("temperature_2m_max")]
    public List<decimal> TemperatureMax { get; set; }

    [JsonPropertyName("temperature_2m_min")]
    public List<decimal> TemperatureMin { get; set; }

    [JsonPropertyName("precipitation_sum")]
    public List<decimal> PrecipitationSum { get; set; }

    [JsonPropertyName("windspeed_10m_max")]
    public List<decimal> WindSpeedMax { get; set; }

    [JsonPropertyName("weathercode")]
    public List<int> WeatherCode { get; set; }
}

public class GeoCodeDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("latitude")]
    public double Lat { get; set; }
    [JsonPropertyName("longitude")]
    public double Lon { get; set; }
    [JsonPropertyName("country")]
    public string Country { get; set; }
    [JsonPropertyName("elevation")]
    public double Elevation { get; set; }
}

public class GeoCodeResponseDto
{
    [JsonPropertyName("results")]
    public List<GeoCodeDto> Results { get; set; }
}