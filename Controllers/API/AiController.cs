using AgriculturalTech.API.Data.Models;
using AgriculturalTech.API.DTOs;
using AgriculturalTech.API.Repositories.Interfaces;
using AgriculturalTech.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AgriculturalTech.API.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AiController : ControllerBase
    {
        private readonly IAiModelService _modelService;
        private readonly IAiCropRecommendationService _cropRecommendationService;
        private readonly IAiAuthorizationRepository _aiAuthorizationRepository;
        public AiController(IAiModelService aiModelService, IAiCropRecommendationService cropRecommendationService, IAiAuthorizationRepository aiAuthorizationRepository)
        {
            _modelService = aiModelService;
            _cropRecommendationService = cropRecommendationService;
            _aiAuthorizationRepository = aiAuthorizationRepository;
        }

        [Authorize]
        [HttpPost("upload_img")]
        public async Task<ActionResult<ApiResponse<AIResponse>>> UploadImage(IFormFile image)
        {
            if (image == null || image.Length == 0)
            {
                return BadRequest("No image uploaded.");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if(userId == null) 
                return Unauthorized("User not Authorized.");

            if(!await _aiAuthorizationRepository.CanUserRunAiScanAsync(userId))
                return BadRequest("User has exceeded the Free number of AI scans. Please Subscribe to Go.");

            try
            {
                using var stream = image.OpenReadStream();

                AIResponse aIResponse = await _modelService.PredictAsync(stream);

                await _aiAuthorizationRepository.RecordSuccessfulScanAsync(userId);

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
