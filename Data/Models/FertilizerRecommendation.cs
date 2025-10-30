using AgriculturalTech.API.Data.Models;
using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
/// Fertilizer recommendations based on crop and soil conditions
/// </summary>
public class FertilizerRecommendation
{
    [Key]
    public int Id { get; set; }

    public int CropTypeId { get; set; }

    [MaxLength(100)]
    public string FertilizerName { get; set; }

    [MaxLength(50)]
    public string FertilizerType { get; set; } // Organic, Synthetic, NPK

    [MaxLength(50)]
    public string GrowthStage { get; set; } // Seedling, Vegetative, Flowering, Fruiting

    // NPK ratio
    [Column(TypeName = "decimal(5,2)")]
    public decimal NitrogenPercentage { get; set; }

    [Column(TypeName = "decimal(5,2)")]
    public decimal PhosphorousPercentage { get; set; }

    [Column(TypeName = "decimal(5,2)")]
    public decimal PotassiumPercentage { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal RecommendedAmountPerSquareMeter { get; set; }

    [MaxLength(20)]
    public string Unit { get; set; } // grams, kg

    [MaxLength(50)]
    public string ApplicationMethod { get; set; } // Broadcast, Drill, Foliar

    public int FrequencyDays { get; set; }

    [MaxLength(1000)]
    public string InstructionsEn { get; set; }

    [MaxLength(1000)]
    public string InstructionsAr { get; set; }

    // Conditions for recommendation
    [Column(TypeName = "decimal(5,2)")]
    public decimal? MinSoilPh { get; set; }

    [Column(TypeName = "decimal(5,2)")]
    public decimal? MaxSoilPh { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;

    // Foreign Key
    [ForeignKey("CropTypeId")]
    public virtual CropType CropType { get; set; }
}