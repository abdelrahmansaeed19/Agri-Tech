using System.ComponentModel.DataAnnotations;

namespace AgriculturalTech.API.Data.Models
{
    /// <summary>
    /// Master crop types (Tomato, Wheat, Rice, etc.)
    /// </summary>
    public class CropType
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string NameEn { get; set; } = null!;

        [MaxLength(100)]
        public string NameAr { get; set; } = string.Empty;

        [MaxLength(50)]
        public string ScientificName { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Category { get; set; } = string.Empty;

        [MaxLength(500)]
        public string DescriptionEn { get; set; } = string.Empty;

        [MaxLength(500)]
        public string DescriptionAr { get; set; } = string.Empty;

        [Required, MaxLength(500)]
        public string ImageUrl { get; set; } = string.Empty;

        // Ideal Growing Conditions
        public decimal? IdealTemperatureMin { get; set; }
        public decimal? IdealTemperatureMax { get; set; }
        public decimal? IdealHumidityMin { get; set; }
        public decimal? IdealHumidityMax { get; set; }
        public decimal? IdealPhMin { get; set; }
        public decimal? IdealPhMax { get; set; }
        public int? GrowingDurationDays { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;

        // Navigation Properties
        public virtual ICollection<UserPlant> UserPlants { get; set; } = new List<UserPlant>();
        public virtual ICollection<CropDisease> CropDiseases { get; set; } = new List<CropDisease>();
        public virtual ICollection<CropCalendarTemplate> CropCalendarTemplates { get; set; } = new List<CropCalendarTemplate>();
        public virtual ICollection<FertilizerRecommendation> FertilizerRecommendations { get; set; } = new List<FertilizerRecommendation>();
    }
}