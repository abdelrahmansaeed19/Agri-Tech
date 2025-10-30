using AgriculturalTech.API.Data.Models;
using AgriculturalTech.API.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AgriculturalTech.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserPlantsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserPlantsController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        // GET: api/userplants
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<UserPlantDto>>>> GetMyPlants()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var plants = await _unitOfWork.UserPlants.GetUserPlantsWithDetailsAsync(userId);

            var plantDtos = plants.Select(p => new UserPlantDto
            {
                Id = p.Id,
                CropTypeId = p.CropTypeId,
                CropTypeName = p.CropType?.NameEn,
                CropTypeNameAr = p.CropType?.NameAr,
                CustomName = p.CustomName,
                PlantingDate = p.PlantingDate,
                ExpectedHarvestDate = p.ExpectedHarvestDate,
                ActualHarvestDate = p.ActualHarvestDate,
                Status = p.Status,
                AreaInSquareMeters = p.AreaInSquareMeters,
                Location = p.Location,
                Notes = p.Notes,
                DaysGrowing = (int)(DateTime.UtcNow - p.PlantingDate).TotalDays,
                DaysUntilHarvest = p.ExpectedHarvestDate.HasValue
                    ? (int)(p.ExpectedHarvestDate.Value - DateTime.UtcNow).TotalDays
                    : null,
                HealthStatus = p.PlantHealthLogs?.OrderByDescending(l => l.LoggedAt)
                    .FirstOrDefault()?.HealthStatus
            }).ToList();

            return Ok(ApiResponse<List<UserPlantDto>>.SuccessResponse(plantDtos));
        }

        // GET: api/userplants/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<UserPlantDetailDto>>> GetPlantDetails(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var plant = await _unitOfWork.UserPlants.GetPlantWithSensorReadingsAsync(id);

            if (plant == null || plant.UserId != userId)
                return NotFound(ApiResponse<UserPlantDetailDto>.ErrorResponse("Plant not found"));

            var plantDto = new UserPlantDetailDto
            {
                Id = plant.Id,
                CropTypeId = plant.CropTypeId,
                CropTypeName = plant.CropType?.NameEn,
                CropTypeNameAr = plant.CropType?.NameAr,
                CustomName = plant.CustomName,
                PlantingDate = plant.PlantingDate,
                ExpectedHarvestDate = plant.ExpectedHarvestDate,
                ActualHarvestDate = plant.ActualHarvestDate,
                Status = plant.Status,
                AreaInSquareMeters = plant.AreaInSquareMeters,
                Location = plant.Location,
                Notes = plant.Notes,
                DaysGrowing = (int)(DateTime.UtcNow - plant.PlantingDate).TotalDays,
                DaysUntilHarvest = plant.ExpectedHarvestDate.HasValue
                    ? (int)(plant.ExpectedHarvestDate.Value - DateTime.UtcNow).TotalDays
                    : null,
                CropTypeDetails = new CropTypeDetailDto
                {
                    Id = plant.CropType.Id,
                    NameEn = plant.CropType.NameEn,
                    NameAr = plant.CropType.NameAr,
                    Category = plant.CropType.Category,
                    DescriptionEn = plant.CropType.DescriptionEn,
                    DescriptionAr = plant.CropType.DescriptionAr,
                    IdealTemperatureMin = plant.CropType.IdealTemperatureMin,
                    IdealTemperatureMax = plant.CropType.IdealTemperatureMax,
                    IdealHumidityMin = plant.CropType.IdealHumidityMin,
                    IdealHumidityMax = plant.CropType.IdealHumidityMax,
                    IdealPhMin = plant.CropType.IdealPhMin,
                    IdealPhMax = plant.CropType.IdealPhMax,
                    GrowingDurationDays = plant.CropType.GrowingDurationDays
                },
                RecentHealthLogs = plant.PlantHealthLogs?.Select(l => new PlantHealthLogDto
                {
                    Id = l.Id,
                    UserPlantId = l.UserPlantId,
                    HealthStatus = l.HealthStatus,
                    Observations = l.Observations,
                    ImageUrl = l.ImageUrl,
                    GrowthHeightCm = l.GrowthHeightCm,
                    LoggedAt = l.LoggedAt
                }).ToList(),
                RecentSensorReadings = plant.SensorReadings?.Select(r => new SensorReadingDto
                {
                    Id = r.Id,
                    SensorDeviceId = r.SensorDeviceId,
                    Nitrogen = r.Nitrogen,
                    Phosphorous = r.Phosphorous,
                    Potassium = r.Potassium,
                    Ph = r.Ph,
                    Humidity = r.Humidity,
                    Temperature = r.Temperature,
                    Rainfall = r.Rainfall,
                    SoilMoisture = r.SoilMoisture,
                    ReadingTime = r.ReadingTime
                }).ToList()
            };

            return Ok(ApiResponse<UserPlantDetailDto>.SuccessResponse(plantDto));
        }

        // POST: api/userplants
        [HttpPost]
        public async Task<ActionResult<ApiResponse<UserPlantDto>>> CreatePlant([FromBody] CreateUserPlantDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Verify crop type exists
            var cropType = await _unitOfWork.CropTypes.GetByIdAsync(dto.CropTypeId);
            if (cropType == null)
                return BadRequest(ApiResponse<UserPlantDto>.ErrorResponse("Invalid crop type"));

            var plant = new UserPlant
            {
                UserId = userId,
                CropTypeId = dto.CropTypeId,
                CustomName = dto.CustomName,
                PlantingDate = dto.PlantingDate,
                AreaInSquareMeters = dto.AreaInSquareMeters,
                Location = dto.Location,
                Notes = dto.Notes,
                Status = "Planted",
                ExpectedHarvestDate = cropType.GrowingDurationDays.HasValue
                    ? dto.PlantingDate.AddDays(cropType.GrowingDurationDays.Value)
                    : null
            };

            await _unitOfWork.UserPlants.AddAsync(plant);
            await _unitOfWork.SaveChangesAsync();

            // Create initial reminders based on crop calendar
            await CreateInitialReminders(userId, plant.Id, dto.CropTypeId, dto.PlantingDate);

            var plantDto = new UserPlantDto
            {
                Id = plant.Id,
                CropTypeId = plant.CropTypeId,
                CropTypeName = cropType.NameEn,
                CustomName = plant.CustomName,
                PlantingDate = plant.PlantingDate,
                ExpectedHarvestDate = plant.ExpectedHarvestDate,
                Status = plant.Status,
                AreaInSquareMeters = plant.AreaInSquareMeters,
                Location = plant.Location,
                Notes = plant.Notes,
                DaysGrowing = 0
            };

            return CreatedAtAction(nameof(GetPlantDetails), new { id = plant.Id },
                ApiResponse<UserPlantDto>.SuccessResponse(plantDto, "Plant created successfully"));
        }

        // PUT: api/userplants/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<UserPlantDto>>> UpdatePlant(int id, [FromBody] UpdateUserPlantDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var plant = await _unitOfWork.UserPlants.FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);

            if (plant == null)
                return NotFound(ApiResponse<UserPlantDto>.ErrorResponse("Plant not found"));

            if (!string.IsNullOrEmpty(dto.CustomName))
                plant.CustomName = dto.CustomName;

            if (!string.IsNullOrEmpty(dto.Status))
                plant.Status = dto.Status;

            if (dto.ExpectedHarvestDate.HasValue)
                plant.ExpectedHarvestDate = dto.ExpectedHarvestDate;

            if (dto.ActualHarvestDate.HasValue)
                plant.ActualHarvestDate = dto.ActualHarvestDate;

            if (!string.IsNullOrEmpty(dto.Notes))
                plant.Notes = dto.Notes;

            plant.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.UserPlants.Update(plant);
            await _unitOfWork.SaveChangesAsync();

            return Ok(ApiResponse<UserPlantDto>.SuccessResponse(null, "Plant updated successfully"));
        }

        // DELETE: api/userplants/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeletePlant(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var plant = await _unitOfWork.UserPlants.FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);

            if (plant == null)
                return NotFound(ApiResponse<bool>.ErrorResponse("Plant not found"));

            plant.IsActive = false;
            _unitOfWork.UserPlants.Update(plant);
            await _unitOfWork.SaveChangesAsync();

            return Ok(ApiResponse<bool>.SuccessResponse(true, "Plant deleted successfully"));
        }

        // GET: api/userplants/harvest-ready
        [HttpGet("harvest-ready")]
        public async Task<ActionResult<ApiResponse<List<UserPlantDto>>>> GetPlantsReadyForHarvest()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var plants = await _unitOfWork.UserPlants.GetPlantsNeedingHarvestAsync(userId);

            var plantDtos = plants.Select(p => new UserPlantDto
            {
                Id = p.Id,
                CropTypeName = p.CropType?.NameEn,
                CustomName = p.CustomName,
                PlantingDate = p.PlantingDate,
                ExpectedHarvestDate = p.ExpectedHarvestDate,
                Status = p.Status,
                DaysUntilHarvest = p.ExpectedHarvestDate.HasValue
                    ? (int)(p.ExpectedHarvestDate.Value - DateTime.UtcNow).TotalDays
                    : null
            }).ToList();

            return Ok(ApiResponse<List<UserPlantDto>>.SuccessResponse(plantDtos));
        }

        private async Task CreateInitialReminders(string userId, int plantId, int cropTypeId, DateTime plantingDate)
        {
            var calendarTemplates = await _unitOfWork.CropCalendarTemplates
                .FindAsync(c => c.CropTypeId == cropTypeId && c.IsActive);

            var reminders = new List<CropReminder>();

            foreach (var template in calendarTemplates)
            {
                var reminder = new CropReminder
                {
                    UserId = userId,
                    UserPlantId = plantId,
                    Title = template.ActivityNameEn,
                    Description = template.DescriptionEn,
                    ReminderType = template.ActivityType,
                    ReminderDate = plantingDate.AddDays(template.DaysAfterPlanting),
                    IsRecurring = template.IsRecurring,
                    RecurringIntervalDays = template.RecurringIntervalDays,
                    Priority = "Medium"
                };

                reminders.Add(reminder);
            }

            if (reminders.Any())
            {
                await _unitOfWork.CropReminders.AddRangeAsync(reminders);
                await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}