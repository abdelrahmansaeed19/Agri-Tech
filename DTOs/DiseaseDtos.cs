using System.ComponentModel.DataAnnotations;

public class DetectDiseaseDto
{
    [Required]
    public int UserPlantId { get; set; }

    [Required]
    public string ImageBase64 { get; set; } // Base64 encoded image

    public string Notes { get; set; }
}

public class DiseaseDetectionDto
{
    public int Id { get; set; }
    public int UserPlantId { get; set; }
    public int? DetectedDiseaseId { get; set; }
    public string DiseaseName { get; set; }
    public string DiseaseNameAr { get; set; }
    public string ImageUrl { get; set; }
    public decimal? ConfidenceScore { get; set; }
    public string DetectionStatus { get; set; }
    public string Treatment { get; set; }
    public string TreatmentAr { get; set; }
    public DateTime DetectedAt { get; set; }
}

public class CropDiseaseDto
{
    public int Id { get; set; }
    public string NameEn { get; set; }
    public string NameAr { get; set; }
    public string DescriptionEn { get; set; }
    public string DescriptionAr { get; set; }
    public string SymptomsEn { get; set; }
    public string SymptomsAr { get; set; }
    public string TreatmentEn { get; set; }
    public string TreatmentAr { get; set; }
    public string Severity { get; set; }
    public string ImageUrl { get; set; }
}