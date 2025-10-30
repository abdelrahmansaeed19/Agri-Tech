using AgriculturalTech.API.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AgriculturalTech.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DiseaseDetectionController : ControllerBase
    {
        private readonly IDiseaseDetectionService _diseaseDetectionService;
        private readonly IUnitOfWork _unitOfWork;

        public DiseaseDetectionController(
            IDiseaseDetectionService diseaseDetectionService,
            IUnitOfWork unitOfWork)
        {
            _diseaseDetectionService = diseaseDetectionService;
            _unitOfWork = unitOfWork;
        }

        // POST: api/diseasedetection/detect
        [HttpPost("detect")]
        public async Task<ActionResult<ApiResponse<DiseaseDetectionDto>>> DetectDisease([FromBody] DetectDiseaseDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            try
            {
                var result = await _diseaseDetectionService.DetectDiseaseAsync(dto, userId);
                return Ok(ApiResponse<DiseaseDetectionDto>.SuccessResponse(result));
            }
            catch (UnauthorizedAccessException)
            {
                return NotFound(ApiResponse<DiseaseDetectionDto>.ErrorResponse("Plant not found or access denied"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<DiseaseDetectionDto>.ErrorResponse($"Detection failed: {ex.Message}"));
            }
        }

        // GET: api/diseasedetection/crop/{cropTypeId}
        [HttpGet("crop/{cropTypeId}")]
        public async Task<ActionResult<ApiResponse<List<CropDiseaseDto>>>> GetDiseasesForCrop(int cropTypeId)
        {
            var diseases = await _diseaseDetectionService.GetCommonDiseasesForCropAsync(cropTypeId);
            return Ok(ApiResponse<List<CropDiseaseDto>>.SuccessResponse(diseases));
        }

        // GET: api/diseasedetection/history/{plantId}
        [HttpGet("history/{plantId}")]
        public async Task<ActionResult<ApiResponse<List<DiseaseDetectionDto>>>> GetDetectionHistory(int plantId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var plant = await _unitOfWork.UserPlants
                .FirstOrDefaultAsync(p => p.Id == plantId && p.UserId == userId);

            if (plant == null)
                return NotFound(ApiResponse<List<DiseaseDetectionDto>>.ErrorResponse("Plant not found"));

            var detections = await _unitOfWork.DiseaseDetectionLogs
                .Query()
                .Include(d => d.DetectedDisease)
                .Where(d => d.UserPlantId == plantId)
                .OrderByDescending(d => d.DetectedAt)
                .ToListAsync();

            var detectionDtos = detections.Select(d => new DiseaseDetectionDto
            {
                Id = d.Id,
                UserPlantId = d.UserPlantId,
                DetectedDiseaseId = d.DetectedDiseaseId,
                DiseaseName = d.DetectedDisease?.NameEn,
                ImageUrl = d.ImageUrl,
                ConfidenceScore = d.ConfidenceScore,
                DetectionStatus = d.DetectionStatus,
                DetectedAt = d.DetectedAt
            }).ToList();

            return Ok(ApiResponse<List<DiseaseDetectionDto>>.SuccessResponse(detectionDtos));
        }
    }
}