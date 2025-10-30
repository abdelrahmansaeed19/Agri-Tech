using System.ComponentModel.DataAnnotations;

public class CreateSensorDeviceDto
{
    //[Required, MaxLength(100)]
    //public string DeviceName { get; set; }

    [Required, MaxLength(100)]
    public string DeviceId { get; set; }

    //[MaxLength(50)]
    //public string DeviceType { get; set; }

    [MaxLength(200)]
    public string Location { get; set; }
}

public class SensorDeviceDto
{
    public int Id { get; set; }
    public string DeviceId { get; set; }
    public string Location { get; set; }
    public string Status { get; set; }
    public DateTime? LastSyncAt { get; set; }
    public DateTime InstalledAt { get; set; }
    public bool IsActive { get; set; }
}

public class CreateSensorReadingDto
{
    [Required]
    public string DeviceId { get; set; }

    //public int? UserPlantId { get; set; }
    public decimal? Nitrogen { get; set; }
    public decimal? Phosphorous { get; set; }
    public decimal? Potassium { get; set; }
    public decimal? Ph { get; set; }
    public decimal? Humidity { get; set; }
    public decimal? Temperature { get; set; }
    public decimal? Rainfall { get; set; }
    public decimal? SoilMoisture { get; set; }
    //public DateTime? ReadingTime { get; set; }
}

public class SensorReadingDto
{
    public int Id { get; set; }
    public int SensorDeviceId { get; set; }
    public decimal? Nitrogen { get; set; }
    public decimal? Phosphorous { get; set; }
    public decimal? Potassium { get; set; }
    public decimal? Ph { get; set; }
    public decimal? Humidity { get; set; }
    public decimal? Temperature { get; set; }
    public decimal? Rainfall { get; set; }
    public decimal? SoilMoisture { get; set; }
    public DateTime ReadingTime { get; set; }
}

public class LatestSensorDataDto
{
    public decimal? Nitrogen { get; set; }
    public decimal? Phosphorous { get; set; }
    public decimal? Potassium { get; set; }
    public decimal? Ph { get; set; }
    public decimal? Humidity { get; set; }
    public decimal? Temperature { get; set; }
    public decimal? SoilMoisture { get; set; }
    public DateTime? LastReadingTime { get; set; }
    public Dictionary<string, string> Alerts { get; set; } // e.g., "pH": "Too acidic"
}

public class SensorStatisticsDto
{
    public string ParameterName { get; set; }
    public decimal? Average { get; set; }
    public decimal? Min { get; set; }
    public decimal? Max { get; set; }
    public decimal? Current { get; set; }
    public string Trend { get; set; } // Increasing, Decreasing, Stable
}