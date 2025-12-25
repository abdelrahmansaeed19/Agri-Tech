using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
/// Weather forecast data
/// </summary>
public class WeatherForecast
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(200)]
    public string Location { get; set; }

    public DateTime ForecastDate { get; set; }

    [Column(TypeName = "decimal(5,2)")]
    public decimal? TemperatureMax { get; set; }

    [Column(TypeName = "decimal(5,2)")]
    public decimal? TemperatureMin { get; set; }

    //[Column(TypeName = "decimal(5,2)")]
    //public decimal? Humidity { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal? RainfallMm { get; set; }

    [Column(TypeName = "decimal(5,2)")]
    public decimal? WindSpeedKmh { get; set; }

    [MaxLength(50)]
    public string WeatherCondition { get; set; } // Sunny, Cloudy, Rainy, etc.

    [MaxLength(200)]
    public string Source { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}