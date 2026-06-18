using AgriculturalTech.API.Data.Models;
using AgriculturalTech.API.DTOs;
using AgriculturalTech.API.Repositories.Interfaces;
using AgriculturalTech.API.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using System.Security.Claims;

namespace AgriculturalTech.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SensorDevicesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISensorDevicesRepository _sensorDevicesRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly INotificationService _notificationService;

        public SensorDevicesController(IUnitOfWork unitOfWork, IMapper mapper, INotificationService notificationService, ISensorDevicesRepository sensorDevicesRepository, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _sensorDevicesRepository = sensorDevicesRepository;
            _userManager = userManager;
            _notificationService = notificationService;
        }

        // GET: api/sensordevices
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<ApiResponse<List<SensorDeviceDto>>>> GetMyDevices()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var devices = await _unitOfWork.SensorDevices.FindAsync(d => d.UserId == userId && d.IsActive);

            var deviceDtos = _mapper.Map<List<SensorDeviceDto>>(devices);

            return Ok(ApiResponse<List<SensorDeviceDto>>.SuccessResponse(deviceDtos));
        }

        [HttpGet("user-device-status")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<UserDeviceStatusDto>>> GetUserDeviceStatus()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if(userId == null)
            {
                return Unauthorized("User not authenticated");
            }


            var device = await _sensorDevicesRepository.GetDeviceByUserIdAsync(userId);

            var statusDto = new UserDeviceStatusDto
            {
                IsPurchased = (device != null),
                IsRegistered = (device?.MacAddress != "Pending Registration" && device != null),
                IsActivated = device?.IsActive ?? false,
                LastSyncAt = device?.LastSyncAt,
                InstalledAt = device?.InstalledAt ?? DateTime.MinValue
            };
            return Ok(ApiResponse<UserDeviceStatusDto>.SuccessResponse(statusDto));
        }

        // GET: api/devices
        [HttpGet("devices")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<List<DeviceDto>>>> GetAllDevices()
        {
            var devices = await _unitOfWork.Devices.GetAllAsync();

            var deviceDtos = _mapper.Map<List<DeviceDto>>(devices);

            return Ok(ApiResponse<List<DeviceDto>>.SuccessResponse(deviceDtos));
        }

        // POST: api/sensordevices
        [HttpPost("activate-device")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<SensorDeviceDto>>> ActivateDevice()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Check if user already has that device
            var sensorDevice = await _unitOfWork.SensorDevices
                .FirstOrDefaultAsync(d => d.UserId == userId);

            if (sensorDevice == null)
                return BadRequest(ApiResponse<SensorDeviceDto>.ErrorResponse("Purchase Kit First!"));

            if (sensorDevice.IsActive)
                return BadRequest(ApiResponse<SensorDeviceDto>.ErrorResponse("Already Activated Before"));

            if (sensorDevice.MacAddress == "Pending Registration")
                return BadRequest(ApiResponse<SensorDeviceDto>.ErrorResponse("Please Connect Your Kit to Wifi to register it first!"));


            sensorDevice.Status = "Active";
            sensorDevice.IsActive = true;

            _unitOfWork.SensorDevices.Update(sensorDevice);

            await _unitOfWork.SaveChangesAsync();

            var deviceDto = _mapper.Map<SensorDeviceDto>(sensorDevice);

            return CreatedAtAction(nameof(GetMyDevices), new { id = sensorDevice.Id },
                ApiResponse<SensorDeviceDto>.SuccessResponse(deviceDto, "Device Assigned successfully"));
        }

        [HttpPost("register-device")]
        public async Task<ActionResult<ApiResponse<SensorDeviceDto>>> RegisterDevice([FromBody] RegisterSensorDeviceDto dto)
        {
            // Check if device with the same MAC address already exists
            
            var user = await _userManager.FindByEmailAsync(dto.UserEmail);

            if (user == null)
                return NotFound("User not found");

            //var RegisteredDevice = user.SensorDevices.FirstOrDefault(d => d.MacAddress == dto.MacAddress);

            var RegisteredDevice = await _unitOfWork.SensorDevices
                .FirstOrDefaultAsync(d => d.MacAddress == dto.MacAddress);


            if (RegisteredDevice != null)
                return BadRequest("Mac Address already registered");

            //var pendingDevice = user.SensorDevices.FirstOrDefault(d => d.UserId == user.Id);

            var pendingDevice = await _unitOfWork.SensorDevices
                .FirstOrDefaultAsync(d => d.UserId == user.Id);

            if (pendingDevice == null)
                return BadRequest("No pending device found for this user");

            pendingDevice.MacAddress = dto.MacAddress;
            pendingDevice.IsActive = false;
            pendingDevice.InstalledAt = DateTime.Now;
            pendingDevice.LastSyncAt = DateTime.Now;

            _unitOfWork.SensorDevices.Update(pendingDevice);

            await _unitOfWork.SaveChangesAsync();

            await _notificationService.SendNotificationAsync(user.FcmToken, "Registration Succeeded!", "Your Kit is Successfuly Registered, you can activate it in the app now!");

            var deviceDto = _mapper.Map<SensorDeviceDto>(pendingDevice);

            return CreatedAtAction(nameof(GetAllDevices), new { id = pendingDevice.Id },
                ApiResponse<SensorDeviceDto>.SuccessResponse(deviceDto, "Device registered successfully, pending assignment"));
        }

        // POST: api/sensordevices/readings
        [HttpPost("readings")]

        public async Task<ActionResult<ApiResponse<bool>>> SubmitReading([FromBody] CreateSensorReadingDto dto)
        {

            // Verify device exists and belongs to user
            var sensorDevice = await _unitOfWork.SensorDevices
                .FirstOrDefaultAsync(d => d.MacAddress == dto.MacAddress);

            if (sensorDevice == null)
                return NotFound(ApiResponse<bool>.ErrorResponse("Device not Registerd"));

            if(!sensorDevice.IsActive)
                return BadRequest(ApiResponse<bool>.ErrorResponse("Device not activated"));

            var reading = _mapper.Map<SensorReading>(dto);

            reading.SensorDeviceId = sensorDevice.Id;

            //reading.UserPlantId

            await _unitOfWork.SensorReadings.AddAsync(reading);

            // Update device last sync time
            sensorDevice.LastSyncAt = DateTime.UtcNow;
            _unitOfWork.SensorDevices.Update(sensorDevice);

            await _unitOfWork.SaveChangesAsync();

            // Check for alerts (e.g., pH out of range, low soil moisture)
            await CheckAndCreateAlerts(reading, sensorDevice.UserId);

            return Ok(ApiResponse<bool>.SuccessResponse(true, "Reading recorded successfully"));
        }

        // GET: api/sensordevices/latest-reading
        [HttpGet("latest-reading")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<SensorReadingDto>>> GetDeviceReadings()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var sensorDevice = await _unitOfWork.SensorDevices
                .FirstOrDefaultAsync(d => d.UserId == userId);

            if (sensorDevice == null)
                return NotFound(ApiResponse<SensorReadingDto>.ErrorResponse("Device not found"));

            var readings = await _unitOfWork.SensorReadings.GetLatestReadingAsync(sensorDevice.Id);

            var readingDtos = _mapper.Map<SensorReadingDto>(readings);

            return Ok(ApiResponse<SensorReadingDto>.SuccessResponse(readingDtos));
        }

        // GET: api/sensordevices/{id}/statistics
        [HttpGet("{id}/statistics")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<List<SensorStatisticsDto>>>> GetDeviceStatistics(int id, [FromQuery] int days = 7)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var device = await _unitOfWork.SensorDevices
                .FirstOrDefaultAsync(d => d.Id == id && d.UserId == userId);

            if (device == null)
                return NotFound(ApiResponse<List<SensorStatisticsDto>>.ErrorResponse("Device not found"));

            var averages = await _unitOfWork.SensorReadings.GetAverageReadingsAsync(id, days);
            var latestReading = await _unitOfWork.SensorReadings.GetLatestReadingAsync(id);

            var statistics = new List<SensorStatisticsDto>();

            if (latestReading != null)
            {
                statistics.Add(CreateStatistic("Nitrogen", averages.TryGetValue("Nitrogen", out var nitrogenAvg) ? nitrogenAvg : null, latestReading.Nitrogen));
                statistics.Add(CreateStatistic("Phosphorous", averages.TryGetValue("Phosphorous", out var phosphorousAvg) ? phosphorousAvg : null, latestReading.Phosphorous));
                statistics.Add(CreateStatistic("Potassium", averages.TryGetValue("Potassium", out var potassiumAvg) ? potassiumAvg : null, latestReading.Potassium));
                statistics.Add(CreateStatistic("pH", averages.TryGetValue("Ph", out var phAvg) ? phAvg : null, latestReading.Ph));
                statistics.Add(CreateStatistic("Humidity", averages.TryGetValue("Humidity", out var humidityAvg) ? humidityAvg : null, latestReading.Humidity));
                statistics.Add(CreateStatistic("Temperature", averages.TryGetValue("Temperature", out var temperatureAvg) ? temperatureAvg : null, latestReading.Temperature));
                statistics.Add(CreateStatistic("Soil Moisture", averages.TryGetValue("SoilMoisture", out var soilMoistureAvg) ? soilMoistureAvg : null, latestReading.SoilMoisture));
            }

            return Ok(ApiResponse<List<SensorStatisticsDto>>.SuccessResponse(statistics));
        }

        private SensorStatisticsDto CreateStatistic(string name, decimal? average, decimal? current)
        {
            string trend = "Stable";
            if (average.HasValue && current.HasValue)
            {
                var diff = current.Value - average.Value;
                if (Math.Abs(diff) > average.Value * 0.1m) // 10% threshold
                {
                    trend = diff > 0 ? "Increasing" : "Decreasing";
                }
            }

            return new SensorStatisticsDto
            {
                ParameterName = name,
                Average = average,
                Current = current,
                Trend = trend
            };
        }

        private async Task CheckAndCreateAlerts(SensorReading reading, string userId)
        {
            var alerts = new List<string>();

            // Check pH
            if (reading.Ph.HasValue && (reading.Ph < 5.5m || reading.Ph > 7.5m))
                alerts.Add($"pH level is {(reading.Ph < 5.5m ? "too acidic" : "too alkaline")} ({reading.Ph})");

            // Check soil moisture
            if (reading.SoilMoisture.HasValue && reading.SoilMoisture < 20m)
                alerts.Add($"Low soil moisture detected ({reading.SoilMoisture}%)");

            // Check temperature
            if (reading.Temperature.HasValue && (reading.Temperature < 10m || reading.Temperature > 35m))
                alerts.Add($"Extreme temperature detected ({reading.Temperature}°C)");

            // Create notifications if alerts exist
            if (alerts.Any())
            {
                var notification = new Notification
                {
                    UserId = userId,
                    NotificationType = "Warning",
                    Title = "Sensor Alert",
                    Message = string.Join("; ", alerts)
                };

                await _unitOfWork.Notifications.AddAsync(notification);
                await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}