using AgriculturalTech.API.DTOs;

namespace AgriculturalTech.API.Services.Interfaces
{
    public interface IAiCropRecommendationService
    {
        public Task<CropResponseDto> PredictCropAsync(CropRecommendationRequestDto request);
    }
}
