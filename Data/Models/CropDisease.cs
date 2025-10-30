using AgriculturalTech.API.Data.Models;
using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
/// Master table for crop diseases
/// </summary>
public class CropDisease
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(150)]
    public string NameEn { get; set; }

    [MaxLength(150)]
    public string NameAr { get; set; }

    public int? CropTypeId { get; set; } // Null if disease affects multiple crops

    [MaxLength(1000)]
    public string DescriptionEn { get; set; }

    [MaxLength(1000)]
    public string DescriptionAr { get; set; }

    [MaxLength(2000)]
    public string SymptomsEn { get; set; }

    [MaxLength(2000)]
    public string SymptomsAr { get; set; }

    [MaxLength(2000)]
    public string TreatmentEn { get; set; }

    [MaxLength(2000)]
    public string TreatmentAr { get; set; }

    [MaxLength(2000)]
    public string PreventionEn { get; set; }

    [MaxLength(2000)]
    public string PreventionAr { get; set; }

    [MaxLength(50)]
    public string Severity { get; set; } // Low, Medium, High, Critical

    public string ImageUrl { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;

    // Foreign Key
    [ForeignKey("CropTypeId")]
    public virtual CropType CropType { get; set; }

    // Navigation Properties
    public virtual ICollection<DiseaseDetectionLog> DiseaseDetectionLogs { get; set; }
}