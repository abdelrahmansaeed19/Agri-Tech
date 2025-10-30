using System.ComponentModel.DataAnnotations;

public class CreatePlantHealthLogDto
{
    [Required]
    public int UserPlantId { get; set; }

    [Required, MaxLength(50)]
    public string HealthStatus { get; set; }

    [MaxLength(2000)]
    public string Observations { get; set; }

    public string ImageBase64 { get; set; }
    public decimal? GrowthHeightCm { get; set; }
}

public class PlantHealthLogDto
{
    public int Id { get; set; }
    public int UserPlantId { get; set; }
    public string HealthStatus { get; set; }
    public string Observations { get; set; }
    public string ImageUrl { get; set; }
    public decimal? GrowthHeightCm { get; set; }
    public DateTime LoggedAt { get; set; }
}