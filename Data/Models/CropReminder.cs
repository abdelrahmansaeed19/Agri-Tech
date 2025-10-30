using AgriculturalTech.API.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
/// User-specific reminders for crop activities
/// </summary>
public class CropReminder
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; } = null!;

    public int? UserPlantId { get; set; }

    [Required, MaxLength(200)]
    public string Title { get; set; } = null!;

    [Required, MaxLength(1000)]
    public string Description { get; set; } = null!;

    [Required, MaxLength(50)]
    public string ReminderType { get; set; } = null!; // Watering, Fertilizing, Inspection, Harvesting, Custom
    [Required]
    public DateTime ReminderDate { get; set; }
    [Required]
    public bool IsRecurring { get; set; }

    public int? RecurringIntervalDays { get; set; }

    [Required, MaxLength(20)]
    public string Priority { get; set; } = null!; // Low, Medium, High
    [Required]
    public bool IsCompleted { get; set; }
    [Required]
    public DateTime? CompletedAt { get; set; }
    [Required]
    public bool NotificationSent { get; set; }
    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public bool IsActive { get; set; }

    // Foreign Keys
    [ForeignKey("UserId")]
    [Required]
    public virtual ApplicationUser User { get; set; }

    [ForeignKey("UserPlantId")]
    [Required]
    public virtual UserPlant UserPlant { get; set; }
}