using AgriculturalTech.API.Data.Models;
using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
/// AI-based disease detection logs
/// </summary>
public class DiseaseDetectionLog
{
    [Key]
    public int Id { get; set; }

    public int UserPlantId { get; set; }

    public int? DetectedDiseaseId { get; set; }

    [Required]
    public string ImageUrl { get; set; } // Uploaded image for detection

    [Column(TypeName = "decimal(5,2)")]
    public decimal? ConfidenceScore { get; set; } // AI confidence 0-100%

    [MaxLength(50)]
    public string DetectionStatus { get; set; } // Detected, NotDetected, Uncertain

    [MaxLength(500)]
    public string AiModelVersion { get; set; }

    [MaxLength(1000)]
    public string Notes { get; set; }

    public bool IsUserConfirmed { get; set; } = false;

    public DateTime DetectedAt { get; set; } = DateTime.UtcNow;

    // Foreign Keys
    [ForeignKey("UserPlantId")]
    public virtual UserPlant UserPlant { get; set; }

    [ForeignKey("DetectedDiseaseId")]
    public virtual CropDisease DetectedDisease { get; set; }
}
