public class WeatherForecastDto
{
    public DateTime Date { get; set; }
    public decimal? TemperatureMax { get; set; }
    public decimal? TemperatureMin { get; set; }
    public decimal? Humidity { get; set; }
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