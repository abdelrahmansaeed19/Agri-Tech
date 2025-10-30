
using AgriculturalTech.API.Data.Models;
using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
/// Manual health logs and observations by user
/// </summary>
public class PlantHealthLog
{
    [Key]
    public int Id { get; set; }

    public int UserPlantId { get; set; }

    [MaxLength(50)]
    public string HealthStatus { get; set; } // Excellent, Good, Fair, Poor, Critical

    [MaxLength(2000)]
    public string Observations { get; set; }

    public string ImageUrl { get; set; }

    [Column(TypeName = "decimal(5,2)")]
    public decimal? GrowthHeightCm { get; set; }

    public DateTime LoggedAt { get; set; } = DateTime.UtcNow;

    // Foreign Key
    [ForeignKey("UserPlantId")]
    public virtual UserPlant UserPlant { get; set; }
}