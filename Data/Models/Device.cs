using System.ComponentModel.DataAnnotations;

namespace AgriculturalTech.API.Data.Models
{
    /// <summary>
    /// Represents a master record for a type of sensor device.
    /// This table stores general information about a device, not a user-specific instance.
    /// </summary>
    public class Device
    {
        [Key]
        public string Id { get; set; }  // e.g., "ESP32_DHT22_01". This is the unique ID the physical device uses.

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        public string Type { get; set; } // e.g., "Temperature & Humidity"

        public string? Description { get; set; }

        public virtual ICollection<SensorDevice> SensorDevices { get; set; } = new List<SensorDevice>();
    }
}
