using AgriculturalTech.API.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
/// Activity logs for audit trail
/// </summary>
public class ActivityLog
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; } = null!;

    [Required, MaxLength(100)]
    public string Action { get; set; } = null!;

    [Required, MaxLength(100)]
    public string EntityType { get; set; } = null!; // UserPlant, SensorReading, etc.

    public int? EntityId { get; set; }

    [Required, MaxLength(1000)]
    public string Details { get; set; } = null!;

    [Required, MaxLength(50)]
    public string IpAddress { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Foreign Key
    [ForeignKey("UserId")]
    [Required]
    public virtual ApplicationUser User { get; set; }
}