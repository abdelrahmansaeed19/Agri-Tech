using System.ComponentModel.DataAnnotations;

public class CreateUserPlantDto
{
    [Required]
    public int CropTypeId { get; set; }

    [MaxLength(100)]
    public string CustomName { get; set; }

    [Required]
    public DateTime PlantingDate { get; set; }

    public double? AreaInSquareMeters { get; set; }

    [MaxLength(200)]
    public string Location { get; set; }

    [MaxLength(500)]
    public string Notes { get; set; }
}

public class UpdateUserPlantDto
{
    [MaxLength(100)]
    public string CustomName { get; set; }

    [MaxLength(50)]
    public string Status { get; set; }

    public DateTime? ExpectedHarvestDate { get; set; }

    public DateTime? ActualHarvestDate { get; set; }

    [MaxLength(500)]
    public string Notes { get; set; }
}

public class UserPlantDto
{
    public int Id { get; set; }
    public int CropTypeId { get; set; }
    public string CropTypeName { get; set; }
    public string CropTypeNameAr { get; set; }
    public string CustomName { get; set; }
    public DateTime PlantingDate { get; set; }
    public DateTime? ExpectedHarvestDate { get; set; }
    public DateTime? ActualHarvestDate { get; set; }
    public string Status { get; set; }
    public double? AreaInSquareMeters { get; set; }
    public string Location { get; set; }
    public string Notes { get; set; }
    public int DaysGrowing { get; set; }
    public int? DaysUntilHarvest { get; set; }
    public LatestSensorDataDto LatestSensorData { get; set; }
    public string HealthStatus { get; set; }
}

public class UserPlantDetailDto : UserPlantDto
{
    public CropTypeDetailDto CropTypeDetails { get; set; }
    public List<PlantHealthLogDto> RecentHealthLogs { get; set; }
    public List<SensorReadingDto> RecentSensorReadings { get; set; }
    public List<DiseaseDetectionDto> DiseaseDetections { get; set; }
}

public class CropTypeDto
{
    public int Id { get; set; }
    public string NameEn { get; set; }
    public string NameAr { get; set; }
    public string Category { get; set; }
    public string ImageUrl { get; set; }
    public int? GrowingDurationDays { get; set; }
}

public class CropTypeDetailDto : CropTypeDto
{
    public string ScientificName { get; set; }
    public string DescriptionEn { get; set; }
    public string DescriptionAr { get; set; }
    public decimal? IdealTemperatureMin { get; set; }
    public decimal? IdealTemperatureMax { get; set; }
    public decimal? IdealHumidityMin { get; set; }
    public decimal? IdealHumidityMax { get; set; }
    public decimal? IdealPhMin { get; set; }
    public decimal? IdealPhMax { get; set; }
    public List<CropDiseaseDto> CommonDiseases { get; set; }
    public List<FertilizerRecommendationDto> FertilizerRecommendations { get; set; }
}