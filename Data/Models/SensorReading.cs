using AgriculturalTech.API.Data.Models;
using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
/// Sensor readings (Nitrogen, Phosphorous, Humidity, pH, Potassium, Temperature, Rainfall)
/// </summary>
public class SensorReading
{
    [Key]
    public int Id { get; set; }

    public int SensorDeviceId { get; set; }

    public int? UserPlantId { get; set; } // Optional: link to specific plant

    // Sensor Values
    [Column(TypeName = "decimal(10,2)")]
    public decimal? Nitrogen { get; set; } // mg/kg or ppm

    [Column(TypeName = "decimal(10,2)")]
    public decimal? Phosphorous { get; set; } // mg/kg or ppm

    [Column(TypeName = "decimal(10,2)")]
    public decimal? Potassium { get; set; } // mg/kg or ppm

    [Column(TypeName = "decimal(5,2)")]
    public decimal? Ph { get; set; } // pH scale 0-14

    [Column(TypeName = "decimal(5,2)")]
    public decimal? Humidity { get; set; } // Percentage 0-100

    [Column(TypeName = "decimal(5,2)")]
    public decimal? Temperature { get; set; } // Celsius

    [Column(TypeName = "decimal(10,2)")]
    public decimal? Rainfall { get; set; } // mm

    [Column(TypeName = "decimal(10,2)")]
    public decimal? SoilMoisture { get; set; } // Percentage

    public DateTime ReadingTime { get; set; } = DateTime.UtcNow;

    [MaxLength(500)]
    public string? Notes { get; set; }

    // Foreign Keys
    [ForeignKey("SensorDeviceId")]
    public virtual SensorDevice SensorDevice { get; set; }

    [ForeignKey("UserPlantId")]
    public virtual UserPlant UserPlant { get; set; }
}