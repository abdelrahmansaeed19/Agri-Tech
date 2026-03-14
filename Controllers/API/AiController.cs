using AgriculturalTech.API.DTOs;
using AgriculturalTech.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgriculturalTech.API.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AiController : ControllerBase
    {
        private readonly IAiModelService _modelService;
        private readonly IAiCropRecommendationService _cropRecommendationService;
        public AiController(IAiModelService aiModelService, IAiCropRecommendationService cropRecommendationService)
        {
            _modelService = aiModelService;
            _cropRecommendationService = cropRecommendationService;
        }

        [Authorize]
        [HttpPost("upload_img")]
        public async Task<ActionResult<ApiResponse<AIResponse>>> UploadImage(IFormFile image)
        {
            if (image == null || image.Length == 0)
            {
                return BadRequest("No image uploaded.");
            }

            try
            {
                using var stream = image.OpenReadStream();

                AIResponse aIResponse = await _modelService.PredictAsync(stream);

                return Ok(ApiResponse<AIResponse>.SuccessResponse(new AIResponse
                {
                    ClassId = aIResponse.ClassId,
                    ClassName = aIResponse.ClassName,
                    Confidence = aIResponse.Confidence
                },
                "Image processed successfully"));

            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<bool>.ErrorResponse("Error communicating with AI service", new List<string> { ex.Message }));
            }
        }

        [Authorize]
        [HttpPost("crop_recommendation")]
        public async Task<ActionResult<ApiResponse<CropResponseDto>>> CropRecommendation([FromBody] CropRecommendationRequestDto request)
        {
            try
            {
                CropResponseDto response = await _cropRecommendationService.PredictCropAsync(request);

                return Ok(ApiResponse<CropResponseDto>.SuccessResponse(response, "Crop recommendation successful"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<bool>.ErrorResponse("Error communicating with AI service", new List<string> { ex.Message }));
            }
        }
    }
}
