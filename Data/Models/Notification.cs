using AgriculturalTech.API.Data.Models;
using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
/// User notifications
/// </summary>
public class Notification
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; }

    [MaxLength(50)]
    public string NotificationType { get; set; } // Reminder, Alert, Info, Warning

    [Required, MaxLength(200)]
    public string Title { get; set; }

    [Required, MaxLength(1000)]
    public string Message { get; set; }

    public bool IsRead { get; set; } = false;

    public DateTime? ReadAt { get; set; }

    [MaxLength(200)]
    public string? ActionLink { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Foreign Key
    [ForeignKey("UserId")]
    public virtual ApplicationUser User { get; set; }
}