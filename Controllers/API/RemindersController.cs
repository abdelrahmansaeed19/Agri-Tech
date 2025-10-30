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
    public class RemindersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RemindersController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: api/reminders
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<CropReminderDto>>>> GetMyReminders([FromQuery] bool? completed = null)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var reminders = await _unitOfWork.CropReminders
                .Query()
                .Include(r => r.UserPlant)
                    .ThenInclude(p => p.CropType)
                .Where(r => r.UserId == userId && r.IsActive)
                .Where(r => !completed.HasValue || r.IsCompleted == completed.Value)
                .OrderBy(r => r.ReminderDate)
                .ToListAsync();

            var reminderDtos = _mapper.Map<List<CropReminderDto>>(reminders);
            return Ok(ApiResponse<List<CropReminderDto>>.SuccessResponse(reminderDtos));
        }

        // GET: api/reminders/upcoming
        [HttpGet("upcoming")]
        public async Task<ActionResult<ApiResponse<List<CropReminderDto>>>> GetUpcomingReminders([FromQuery] int days = 7)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var reminders = await _unitOfWork.CropReminders.GetUpcomingRemindersAsync(userId, days);

            var reminderDtos = _mapper.Map<List<CropReminderDto>>(reminders);
            return Ok(ApiResponse<List<CropReminderDto>>.SuccessResponse(reminderDtos));
        }

        // GET: api/reminders/overdue
        [HttpGet("overdue")]
        public async Task<ActionResult<ApiResponse<List<CropReminderDto>>>> GetOverdueReminders()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var reminders = await _unitOfWork.CropReminders.GetOverdueRemindersAsync(userId);

            var reminderDtos = _mapper.Map<List<CropReminderDto>>(reminders);
            return Ok(ApiResponse<List<CropReminderDto>>.SuccessResponse(reminderDtos));
        }

        // POST: api/reminders
        [HttpPost]
        public async Task<ActionResult<ApiResponse<CropReminderDto>>> CreateReminder([FromBody] CreateCropReminderDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var reminder = _mapper.Map<CropReminder>(dto);
            reminder.UserId = userId;

            await _unitOfWork.CropReminders.AddAsync(reminder);
            await _unitOfWork.SaveChangesAsync();

            var reminderDto = _mapper.Map<CropReminderDto>(reminder);
            return CreatedAtAction(nameof(GetMyReminders), new { id = reminder.Id },
                ApiResponse<CropReminderDto>.SuccessResponse(reminderDto, "Reminder created successfully"));
        }

        // PUT: api/reminders/{id}/complete
        [HttpPut("{id}/complete")]
        public async Task<ActionResult<ApiResponse<bool>>> CompleteReminder(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var reminder = await _unitOfWork.CropReminders
                .FirstOrDefaultAsync(r => r.Id == id && r.UserId == userId);

            if (reminder == null)
                return NotFound(ApiResponse<bool>.ErrorResponse("Reminder not found"));

            reminder.IsCompleted = true;
            reminder.CompletedAt = DateTime.UtcNow;

            _unitOfWork.CropReminders.Update(reminder);
            await _unitOfWork.SaveChangesAsync();

            return Ok(ApiResponse<bool>.SuccessResponse(true, "Reminder marked as complete"));
        }

        // DELETE: api/reminders/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteReminder(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var reminder = await _unitOfWork.CropReminders
                .FirstOrDefaultAsync(r => r.Id == id && r.UserId == userId);

            if (reminder == null)
                return NotFound(ApiResponse<bool>.ErrorResponse("Reminder not found"));

            reminder.IsActive = false;
            _unitOfWork.CropReminders.Update(reminder);
            await _unitOfWork.SaveChangesAsync();

            return Ok(ApiResponse<bool>.SuccessResponse(true, "Reminder deleted successfully"));
        }
    }
}