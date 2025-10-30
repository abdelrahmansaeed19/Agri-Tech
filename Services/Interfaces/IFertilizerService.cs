public interface IFertilizerService
{
    Task<FertilizerCalculationResultDto> CalculateFertilizerNeedsAsync(CalculateFertilizerDto request);
    Task<List<FertilizerRecommendationDto>> GetRecommendationsForCropAsync(int cropTypeId, string growthStage);
}