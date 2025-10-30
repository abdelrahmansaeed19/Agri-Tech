using AgriculturalTech.API.Data.Models;
using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
/// Weather alerts for users
/// </summary>
public class WeatherAlert
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; } = null!;

    [Required, MaxLength(200)]
    public string Location { get; set; } = null!;

    [Required, MaxLength(50)]
    public string AlertType { get; set; } = null!; // Rain, Storm, Frost, Heatwave, Drought

    [Required, MaxLength(50)]
    public string Severity { get; set; } = null!; // Low, Medium, High, Extreme

    [Required, MaxLength(500)]
    public string MessageEn { get; set; } = null!;

    [Required, MaxLength(500)]
    public string MessageAr { get; set; } = null!;

    [Required]
    public DateTime AlertStartDate { get; set; }

    public DateTime? AlertEndDate { get; set; }
    [Required]
    public bool IsRead { get; set; }

    [Required, MaxLength(200)]
    public string Source { get; set; } = null!; // Weather API source

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Foreign Key
    [ForeignKey("UserId")]
    [Required]
    public virtual ApplicationUser User { get; set; } = null!;
}