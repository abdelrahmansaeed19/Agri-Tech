using AgriculturalTech.API.Data.Models;
using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
/// Template for crop calendar activities (planting, fertilizing, watering, harvesting)
/// </summary>
public class CropCalendarTemplate
{
    [Key]
    public int Id { get; set; }

    public int CropTypeId { get; set; }

    [Required, MaxLength(100)]
    public string ActivityNameEn { get; set; }

    [MaxLength(100)]
    public string ActivityNameAr { get; set; }

    [MaxLength(50)]
    public string ActivityType { get; set; } // Planting, Watering, Fertilizing, Pruning, Harvesting

    public int DaysAfterPlanting { get; set; } // When to perform this activity

    [MaxLength(1000)]
    public string DescriptionEn { get; set; }

    [MaxLength(1000)]
    public string DescriptionAr { get; set; }

    public bool IsRecurring { get; set; } = false;
    public int? RecurringIntervalDays { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;

    // Foreign Key
    [ForeignKey("CropTypeId")]
    public virtual CropType CropType { get; set; }
}