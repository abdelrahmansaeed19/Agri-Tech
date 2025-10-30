using AgriculturalTech.API.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AgriculturalTech.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class MarketPricesController : ControllerBase
    {
        private readonly IMarketPriceService _marketPriceService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MarketPricesController(
            IMarketPriceService marketPriceService,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _marketPriceService = marketPriceService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: api/marketprices/crop/{cropTypeId}
        [HttpGet("crop/{cropTypeId}")]
        public async Task<ActionResult<ApiResponse<List<MarketPriceDto>>>> GetPricesForCrop(int cropTypeId)
        {
            var prices = await _unitOfWork.MarketPrices.GetLatestPricesByCropAsync(cropTypeId);
            var priceDtos = _mapper.Map<List<MarketPriceDto>>(prices);

            return Ok(ApiResponse<List<MarketPriceDto>>.SuccessResponse(priceDtos));
        }

        // GET: api/marketprices/statistics/{cropTypeId}
        [HttpGet("statistics/{cropTypeId}")]
        public async Task<ActionResult<ApiResponse<MarketPriceStatisticsDto>>> GetPriceStatistics(
            int cropTypeId,
            [FromQuery] int days = 30)
        {
            var statistics = await _marketPriceService.GetPriceStatisticsAsync(cropTypeId, days);

            if (statistics == null)
                return NotFound(ApiResponse<MarketPriceStatisticsDto>.ErrorResponse("No price data available"));

            return Ok(ApiResponse<MarketPriceStatisticsDto>.SuccessResponse(statistics));
        }

        // GET: api/marketprices/mycrops
        [HttpGet("mycrops")]
        public async Task<ActionResult<ApiResponse<List<MarketPriceDto>>>> GetPricesForMyCrops()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var userCropTypeIds = await _unitOfWork.UserPlants
                .Query()
                .Where(p => p.UserId == userId && p.IsActive)
                .Select(p => p.CropTypeId)
                .Distinct()
                .ToListAsync();

            var allPrices = new List<MarketPrice>();
            foreach (var cropTypeId in userCropTypeIds)
            {
                var prices = await _unitOfWork.MarketPrices.GetLatestPricesByCropAsync(cropTypeId);
                allPrices.AddRange(prices);
            }

            var priceDtos = _mapper.Map<List<MarketPriceDto>>(allPrices);
            return Ok(ApiResponse<List<MarketPriceDto>>.SuccessResponse(priceDtos));
        }
    }
}