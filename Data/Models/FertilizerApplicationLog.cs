using AgriculturalTech.API.Data.Models;
using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
/// User's fertilizer application logs
/// </summary>
public class FertilizerApplicationLog
{
    [Key]
    public int Id { get; set; }

    public int UserPlantId { get; set; }

    public int? FertilizerRecommendationId { get; set; }

    [MaxLength(100)]
    public string FertilizerUsed { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal AmountApplied { get; set; }

    [MaxLength(20)]
    public string Unit { get; set; }

    [MaxLength(500)]
    public string Notes { get; set; }

    public DateTime ApplicationDate { get; set; } = DateTime.UtcNow;

    // Foreign Keys
    [ForeignKey("UserPlantId")]
    public virtual UserPlant UserPlant { get; set; }

    [ForeignKey("FertilizerRecommendationId")]
    public virtual FertilizerRecommendation FertilizerRecommendation { get; set; }
}