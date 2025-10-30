using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AgriculturalTech.API.Data.Models
{
    /// <summary>
    /// Extended Identity User with agricultural-specific properties
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(100)]
        public string FullName { get; set; }

        [MaxLength(200)]
        public string FarmName { get; set; }

        [MaxLength(500)]
        public string FarmLocation { get; set; }

        public double? FarmAreaInAcres { get; set; }

        [MaxLength(10)]
        public string PreferredLanguage { get; set; } = "en";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLoginAt { get; set; }
        public bool IsActive { get; set; } = true;

        // Navigation Properties
        public virtual ICollection<UserPlant> UserPlants { get; set; }
        public virtual ICollection<CropReminder> CropReminders { get; set; }
        public virtual ICollection<WeatherAlert> WeatherAlerts { get; set; }
        public virtual ICollection<SensorDevice> SensorDevices { get; set; }
    }
}