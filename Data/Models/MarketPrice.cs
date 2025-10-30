using AgriculturalTech.API.Data.Models;
using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
/// Live market prices for crops
/// </summary>
public class MarketPrice
{
    [Key]
    public int Id { get; set; }

    public int CropTypeId { get; set; }

    [Required, MaxLength(100)]
    public string MarketName { get; set; }

    [MaxLength(100)]
    public string MarketLocation { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal PricePerUnit { get; set; }

    [MaxLength(20)]
    public string Unit { get; set; } // kg, ton, pound, etc.

    [MaxLength(10)]
    public string Currency { get; set; } = "USD";

    [MaxLength(50)]
    public string Quality { get; set; } // Premium, Grade A, Grade B, etc.

    public DateTime PriceDate { get; set; }

    [MaxLength(200)]
    public string Source { get; set; } // API source or manual entry

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Foreign Key
    [ForeignKey("CropTypeId")]
    public virtual CropType CropType { get; set; }
}