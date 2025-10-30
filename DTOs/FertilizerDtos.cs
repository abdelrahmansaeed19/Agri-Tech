using System.ComponentModel.DataAnnotations;

public class FertilizerRecommendationDto
{
    public int Id { get; set; }
    public string FertilizerName { get; set; }
    public string FertilizerType { get; set; }
    public string GrowthStage { get; set; }
    public decimal NitrogenPercentage { get; set; }
    public decimal PhosphorousPercentage { get; set; }
    public decimal PotassiumPercentage { get; set; }
    public string NPKRatio => $"{NitrogenPercentage}-{PhosphorousPercentage}-{PotassiumPercentage}";
    public decimal RecommendedAmountPerSquareMeter { get; set; }
    public string Unit { get; set; }
    public string ApplicationMethod { get; set; }
    public int FrequencyDays { get; set; }
    public string InstructionsEn { get; set; }
    public string InstructionsAr { get; set; }
}

public class CalculateFertilizerDto
{
    [Required]
    public int CropTypeId { get; set; }

    [Required]
    public double AreaInSquareMeters { get; set; }

    [Required]
    public string GrowthStage { get; set; }

    public decimal? CurrentNitrogen { get; set; }
    public decimal? CurrentPhosphorous { get; set; }
    public decimal? CurrentPotassium { get; set; }
    public decimal? SoilPh { get; set; }
}

public class FertilizerCalculationResultDto
{
    public List<FertilizerRecommendationDto> Recommendations { get; set; }
    public double TotalAreaInSquareMeters { get; set; }
    public string GrowthStage { get; set; }
    public Dictionary<string, string> SoilConditionWarnings { get; set; }
}

public class LogFertilizerApplicationDto
{
    [Required]
    public int UserPlantId { get; set; }

    public int? FertilizerRecommendationId { get; set; }

    [Required, MaxLength(100)]
    public string FertilizerUsed { get; set; }

    [Required]
    public decimal AmountApplied { get; set; }

    [Required, MaxLength(20)]
    public string Unit { get; set; }

    [MaxLength(500)]
    public string Notes { get; set; }
}