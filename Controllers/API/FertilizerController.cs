using AgriculturalTech.API.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AgriculturalTech.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FertilizerController : ControllerBase
    {
        private readonly IFertilizerService _fertilizerService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FertilizerController(
            IFertilizerService fertilizerService,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _fertilizerService = fertilizerService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // POST: api/fertilizer/calculate
        [HttpPost("calculate")]
        public async Task<ActionResult<ApiResponse<FertilizerCalculationResultDto>>> CalculateFertilizer(
            [FromBody] CalculateFertilizerDto dto)
        {
            var result = await _fertilizerService.CalculateFertilizerNeedsAsync(dto);
            return Ok(ApiResponse<FertilizerCalculationResultDto>.SuccessResponse(result));
        }

        // GET: api/fertilizer/recommendations/{cropTypeId}
        [HttpGet("recommendations/{cropTypeId}")]
        public async Task<ActionResult<ApiResponse<List<FertilizerRecommendationDto>>>> GetRecommendations(
            int cropTypeId,
            [FromQuery] string growthStage = null)
        {
            var recommendations = await _fertilizerService.GetRecommendationsForCropAsync(cropTypeId, growthStage);
            return Ok(ApiResponse<List<FertilizerRecommendationDto>>.SuccessResponse(recommendations));
        }

        // POST: api/fertilizer/log
        [HttpPost("log")]
        public async Task<ActionResult<ApiResponse<bool>>> LogApplication([FromBody] LogFertilizerApplicationDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Verify plant belongs to user
            var plant = await _unitOfWork.UserPlants
                .FirstOrDefaultAsync(p => p.Id == dto.UserPlantId && p.UserId == userId);

            if (plant == null)
                return NotFound(ApiResponse<bool>.ErrorResponse("Plant not found"));

            var log = _mapper.Map<FertilizerApplicationLog>(dto);
            await _unitOfWork.FertilizerApplicationLogs.AddAsync(log);
            await _unitOfWork.SaveChangesAsync();

            return Ok(ApiResponse<bool>.SuccessResponse(true, "Fertilizer application logged successfully"));
        }
    }
}