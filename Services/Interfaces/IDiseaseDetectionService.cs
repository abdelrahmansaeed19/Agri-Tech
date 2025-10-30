public interface IDiseaseDetectionService
{
    Task<DiseaseDetectionDto> DetectDiseaseAsync(DetectDiseaseDto request, string userId);
    Task<List<CropDiseaseDto>> GetCommonDiseasesForCropAsync(int cropTypeId);
}