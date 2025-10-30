public class FertilizerService : IFertilizerService
{
    private readonly IUnitOfWork _unitOfWork;

    public FertilizerService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<FertilizerCalculationResultDto> CalculateFertilizerNeedsAsync(CalculateFertilizerDto request)
    {
        var recommendations = await _unitOfWork.FertilizerRecommendations
            .FindAsync(f => f.CropTypeId == request.CropTypeId
                && f.GrowthStage == request.GrowthStage
                && f.IsActive);

        var filteredRecommendations = recommendations.ToList();

        // Filter by soil pH if provided
        if (request.SoilPh.HasValue)
        {
            filteredRecommendations = filteredRecommendations
                .Where(r => (!r.MinSoilPh.HasValue || r.MinSoilPh <= request.SoilPh)
                    && (!r.MaxSoilPh.HasValue || r.MaxSoilPh >= request.SoilPh))
                .ToList();
        }

        var result = new FertilizerCalculationResultDto
        {
            Recommendations = filteredRecommendations.Select(r => new FertilizerRecommendationDto
            {
                Id = r.Id,
                FertilizerName = r.FertilizerName,
                FertilizerType = r.FertilizerType,
                GrowthStage = r.GrowthStage,
                NitrogenPercentage = r.NitrogenPercentage,
                PhosphorousPercentage = r.PhosphorousPercentage,
                PotassiumPercentage = r.PotassiumPercentage,
                RecommendedAmountPerSquareMeter = r.RecommendedAmountPerSquareMeter * (decimal)request.AreaInSquareMeters,
                Unit = r.Unit,
                ApplicationMethod = r.ApplicationMethod,
                FrequencyDays = r.FrequencyDays,
                InstructionsEn = r.InstructionsEn,
                InstructionsAr = r.InstructionsAr
            }).ToList(),
            TotalAreaInSquareMeters = request.AreaInSquareMeters,
            GrowthStage = request.GrowthStage,
            SoilConditionWarnings = new Dictionary<string, string>()
        };

        // Add warnings based on current soil conditions
        if (request.CurrentNitrogen.HasValue && request.CurrentNitrogen < 20)
            result.SoilConditionWarnings.Add("Nitrogen", "Low nitrogen levels detected. Consider nitrogen-rich fertilizers.");

        if (request.CurrentPhosphorous.HasValue && request.CurrentPhosphorous < 15)
            result.SoilConditionWarnings.Add("Phosphorous", "Low phosphorous levels detected. Consider phosphate fertilizers.");

        if (request.CurrentPotassium.HasValue && request.CurrentPotassium < 100)
            result.SoilConditionWarnings.Add("Potassium", "Low potassium levels detected. Consider potash fertilizers.");

        if (request.SoilPh.HasValue)
        {
            if (request.SoilPh < 5.5m)
                result.SoilConditionWarnings.Add("pH", "Soil is too acidic. Consider adding lime to raise pH.");
            else if (request.SoilPh > 7.5m)
                result.SoilConditionWarnings.Add("pH", "Soil is too alkaline. Consider adding sulfur to lower pH.");
        }

        return result;
    }

    public async Task<List<FertilizerRecommendationDto>> GetRecommendationsForCropAsync(int cropTypeId, string growthStage)
    {
        var recommendations = await _unitOfWork.FertilizerRecommendations
            .FindAsync(f => f.CropTypeId == cropTypeId
                && (string.IsNullOrEmpty(growthStage) || f.GrowthStage == growthStage)
                && f.IsActive);

        return recommendations.Select(r => new FertilizerRecommendationDto
        {
            Id = r.Id,
            FertilizerName = r.FertilizerName,
            FertilizerType = r.FertilizerType,
            GrowthStage = r.GrowthStage,
            NitrogenPercentage = r.NitrogenPercentage,
            PhosphorousPercentage = r.PhosphorousPercentage,
            PotassiumPercentage = r.PotassiumPercentage,
            RecommendedAmountPerSquareMeter = r.RecommendedAmountPerSquareMeter,
            Unit = r.Unit,
            ApplicationMethod = r.ApplicationMethod,
            FrequencyDays = r.FrequencyDays,
            InstructionsEn = r.InstructionsEn,
            InstructionsAr = r.InstructionsAr
        }).ToList();
    }
}