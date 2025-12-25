using AgriculturalTech.API.Data.Models;
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
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherService _weatherService;
        private readonly IUnitOfWork _unitOfWork;

        public WeatherController(IWeatherService weatherService, IUnitOfWork unitOfWork)
        {
            _weatherService = weatherService;
            _unitOfWork = unitOfWork;
        }

        // GET: api/weather/forecast
        [HttpGet("forecast")]
        public async Task<ActionResult<ApiResponse<List<WeatherForecastDto>>>> GetForecast(
            [FromQuery] string location = null,
            [FromQuery] int days = 7)
        {
            if (string.IsNullOrEmpty(location))
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _unitOfWork.Query<ApplicationUser>()
                    .FirstOrDefaultAsync(u => u.Id == userId);
                location = user?.FarmLocation;
            }

            if (string.IsNullOrEmpty(location))
                return BadRequest(ApiResponse<List<WeatherForecastDto>>.ErrorResponse("Location is required"));

            var forecast = await _weatherService.GetWeatherForecastAsync(location, days);

            return Ok(ApiResponse<List<WeatherForecastDto>>.SuccessResponse(forecast));
        }

        // GET: api/weather/alerts
        [HttpGet("alerts")]
        public async Task<ActionResult<ApiResponse<List<WeatherAlertDto>>>> GetMyAlerts([FromQuery] bool? unreadOnly = true)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var alerts = await _unitOfWork.WeatherAlerts
                .Query()
                .Where(a => a.UserId == userId)
                .Where(a => !unreadOnly.HasValue || !unreadOnly.Value || !a.IsRead)
                .OrderByDescending(a => a.AlertStartDate)
                .ToListAsync();

            var alertDtos = alerts.Select(a => new WeatherAlertDto
            {
                Id = a.Id,
                AlertType = a.AlertType,
                Severity = a.Severity,
                MessageEn = a.MessageEn,
                MessageAr = a.MessageAr,
                AlertStartDate = a.AlertStartDate,
                AlertEndDate = a.AlertEndDate,
                IsRead = a.IsRead
            }).ToList();

            return Ok(ApiResponse<List<WeatherAlertDto>>.SuccessResponse(alertDtos));
        }

        // PUT: api/weather/alerts/{id}/read
        [HttpPut("alerts/{id}/read")]
        public async Task<ActionResult<ApiResponse<bool>>> MarkAlertAsRead(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var alert = await _unitOfWork.WeatherAlerts
                .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);

            if (alert == null)
                return NotFound(ApiResponse<bool>.ErrorResponse("Alert not found"));

            alert.IsRead = true;
            _unitOfWork.WeatherAlerts.Update(alert);
            await _unitOfWork.SaveChangesAsync();

            return Ok(ApiResponse<bool>.SuccessResponse(true, "Alert marked as read"));
        }
    }
}