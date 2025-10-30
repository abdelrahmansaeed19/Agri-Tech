using AgriculturalTech.API.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AgriculturalTech.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public DashboardController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/dashboard/summary
        [HttpGet("summary")]
        public async Task<ActionResult<ApiResponse<DashboardSummaryDto>>> GetDashboardSummary()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var totalPlants = await _unitOfWork.UserPlants
                .CountAsync(p => p.UserId == userId && p.IsActive);

            var activePlants = await _unitOfWork.UserPlants
                .CountAsync(p => p.UserId == userId && p.IsActive && p.Status == "Growing");

            var upcomingReminders = await _unitOfWork.CropReminders
                .CountAsync(r => r.UserId == userId && !r.IsCompleted && r.ReminderDate >= DateTime.UtcNow);

            var unreadAlerts = await _unitOfWork.WeatherAlerts
                .CountAsync(a => a.UserId == userId && !a.IsRead);

            var todayReminders = await _unitOfWork.CropReminders.GetTodayRemindersAsync(userId);

            var summary = new DashboardSummaryDto
            {
                TotalPlants = totalPlants,
                ActivePlants = activePlants,
                PlantsNeedingAttention = 0, // Calculate based on health status
                UpcomingReminders = upcomingReminders,
                UnreadAlerts = unreadAlerts,
                TodayReminders = todayReminders.Select(r => new CropReminderDto
                {
                    Id = r.Id,
                    Title = r.Title,
                    Description = r.Description,
                    ReminderType = r.ReminderType,
                    ReminderDate = r.ReminderDate,
                    Priority = r.Priority
                }).ToList()
            };

            return Ok(ApiResponse<DashboardSummaryDto>.SuccessResponse(summary));
        }
    }
}