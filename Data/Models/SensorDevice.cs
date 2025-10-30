using AgriculturalTech.API.Data.Models;
using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
/// IoT Sensor devices registered by users
/// </summary>
public class SensorDevice
{
    [Key]
    public int Id { get; set; }

    // Foreign key to the master Device table
    [Required]
    public string DeviceId { get; set; }

    [Required]
    public string UserId { get; set; } = null!;

    [Required, MaxLength(200)]
    public string Location { get; set; } = null!;

    [Required, MaxLength(50)]
    public string Status { get; set; } = null!; // Active, Inactive, Maintenance

    public DateTime? LastSyncAt { get; set; }

    public DateTime InstalledAt { get; set; } = DateTime.UtcNow;

    public bool IsActive { get; set; }

    // Foreign Key
    [ForeignKey("UserId")]
    [Required]
    public virtual ApplicationUser User { get; set; }

    [ForeignKey("DeviceId")]
    [Required]
    public virtual Device Device { get; set; }

    // Navigation Properties
    public virtual ICollection<SensorReading> SensorReadings { get; set; } = new List<SensorReading>();

    public virtual ICollection<UserPlant> UserPlant { get; set; } = new List<UserPlant>();
}