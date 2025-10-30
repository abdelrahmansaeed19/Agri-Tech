public class DiseaseDetectionService : IDiseaseDetectionService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IImageStorageService _imageStorage;
    // In real implementation, inject ML.NET model or Azure Custom Vision service

    public DiseaseDetectionService(IUnitOfWork unitOfWork, IImageStorageService imageStorage)
    {
        _unitOfWork = unitOfWork;
        _imageStorage = imageStorage;
    }

    public async Task<DiseaseDetectionDto> DetectDiseaseAsync(DetectDiseaseDto request, string userId)
    {
        // Verify plant belongs to user
        var plant = await _unitOfWork.UserPlants
            .FirstOrDefaultAsync(p => p.Id == request.UserPlantId && p.UserId == userId);

        if (plant == null)
            throw new UnauthorizedAccessException("Plant not found or access denied");

        // Save image
        var imageUrl = await _imageStorage.SaveImageAsync(request.ImageBase64, "disease-detection");

        // TODO: Call ML model for disease detection
        // For now, using placeholder logic
        var detectedDiseaseId = await SimulateDiseaseDetection(plant.CropTypeId, imageUrl);
        var confidence = detectedDiseaseId.HasValue ? 85.5m : 0m;

        var detectionLog = new DiseaseDetectionLog
        {
            UserPlantId = request.UserPlantId,
            DetectedDiseaseId = detectedDiseaseId,
            ImageUrl = imageUrl,
            ConfidenceScore = confidence,
            DetectionStatus = detectedDiseaseId.HasValue ? "Detected" : "NotDetected",
            AiModelVersion = "v1.0.0",
            Notes = request.Notes,
            DetectedAt = DateTime.UtcNow
        };

        await _unitOfWork.DiseaseDetectionLogs.AddAsync(detectionLog);
        await _unitOfWork.SaveChangesAsync();

        CropDisease disease = null;
        if (detectedDiseaseId.HasValue)
        {
            disease = await _unitOfWork.CropDiseases.GetByIdAsync(detectedDiseaseId.Value);
        }

        return new DiseaseDetectionDto
        {
            Id = detectionLog.Id,
            UserPlantId = detectionLog.UserPlantId,
            DetectedDiseaseId = detectedDiseaseId,
            DiseaseName = disease?.NameEn,
            DiseaseNameAr = disease?.NameAr,
            ImageUrl = imageUrl,
            ConfidenceScore = confidence,
            DetectionStatus = detectionLog.DetectionStatus,
            Treatment = disease?.TreatmentEn,
            TreatmentAr = disease?.TreatmentAr,
            DetectedAt = detectionLog.DetectedAt
        };
    }

    public async Task<List<CropDiseaseDto>> GetCommonDiseasesForCropAsync(int cropTypeId)
    {
        var diseases = await _unitOfWork.CropDiseases
            .FindAsync(d => d.CropTypeId == cropTypeId && d.IsActive);

        return diseases.Select(d => new CropDiseaseDto
        {
            Id = d.Id,
            NameEn = d.NameEn,
            NameAr = d.NameAr,
            DescriptionEn = d.DescriptionEn,
            DescriptionAr = d.DescriptionAr,
            SymptomsEn = d.SymptomsEn,
            SymptomsAr = d.SymptomsAr,
            TreatmentEn = d.TreatmentEn,
            TreatmentAr = d.TreatmentAr,
            Severity = d.Severity,
            ImageUrl = d.ImageUrl
        }).ToList();
    }

    private async Task<int?> SimulateDiseaseDetection(int cropTypeId, string imageUrl)
    {
        // Placeholder: In real implementation, call ML model
        var diseases = await _unitOfWork.CropDiseases
            .FindAsync(d => d.CropTypeId == cropTypeId);

        // Return first disease as simulation
        return diseases.FirstOrDefault()?.Id;
    }
}