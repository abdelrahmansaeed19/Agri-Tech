using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgriculturalTech.API.Data.Models
{
    /// <summary>
    /// User's actual plants/crops they are growing
    /// </summary>
    public class UserPlant
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = null!;

        public int? SensorDeviceId { get; set; } // Optional link to a sensor device

        [Required]
        public int CropTypeId { get; set; }

        [Required, MaxLength(100)]
        public string CustomName { get; set; } = null!;
        [Required]
        public DateTime PlantingDate { get; set; }

        public DateTime? ExpectedHarvestDate { get; set; }

        public DateTime? ActualHarvestDate { get; set; }

        [Required, MaxLength(50)]
        public string Status { get; set; } = null!; // Planted, Growing, Harvested, Dead

        public double? AreaInSquareMeters { get; set; }

        [Required, MaxLength(200)]
        public string Location { get; set; } = null!;

        [Required, MaxLength(500)]
        public string Notes { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; }

        // Foreign Keys
        [ForeignKey("UserId")]
        [Required]
        public virtual ApplicationUser User { get; set; } = null!;

        [ForeignKey("CropTypeId")]
        [Required]
        public virtual CropType CropType { get; set; } = null!;

        [ForeignKey("SensorDeviceId")]
        public virtual SensorDevice SensorDevice { get; set; }

        // Navigation Properties
        public virtual ICollection<PlantHealthLog> PlantHealthLogs { get; set; } = new List<PlantHealthLog>();
        public virtual ICollection<SensorReading> SensorReadings { get; set; } = new List<SensorReading>();
        public virtual ICollection<DiseaseDetectionLog> DiseaseDetectionLogs { get; set; } = new List<DiseaseDetectionLog>();
    }
}