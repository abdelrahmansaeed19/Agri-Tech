using AgriculturalTech.API.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AgriculturalTech.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SensorDevicesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SensorDevicesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: api/sensordevices
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<ApiResponse<List<SensorDeviceDto>>>> GetMyDevices()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var devices = await _unitOfWork.SensorDevices.FindAsync(d => d.UserId == userId && d.IsActive);

            //var deviceDtos = devices.Select(d => new SensorDeviceDto
            //{
            //    Id = d.Id,
            //    DeviceId = d.DeviceId,
            //    Location = d.Location,
            //    Status = d.Status,
            //    LastSyncAt = d.LastSyncAt,
            //    InstalledAt = d.InstalledAt,
            //    IsActive = d.IsActive
            //}).ToList();

            var deviceDtos = _mapper.Map<List<SensorDeviceDto>>(devices);

            return Ok(ApiResponse<List<SensorDeviceDto>>.SuccessResponse(deviceDtos));
        }

        // POST: api/sensordevices
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ApiResponse<SensorDeviceDto>>> RegisterDevice([FromBody] CreateSensorDeviceDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Check if user already has that device
            var existingDevice = await _unitOfWork.SensorDevices
                .FirstOrDefaultAsync(d => d.DeviceId == dto.DeviceId && d.UserId == userId);

            if (existingDevice != null)
                return BadRequest(ApiResponse<SensorDeviceDto>.ErrorResponse("Device ID already registered to that user"));

            //var device = new SensorDevice
            //{
            //    UserId = userId,
            //    DeviceId = dto.DeviceId,
            //    Location = dto.Location,
            //    Status = "Active",
            //    IsActive = true,
            //};

            var device = _mapper.Map<SensorDevice>(dto);

            device.UserId = userId;
            device.Status = "Active";
            device.IsActive = true;

            await _unitOfWork.SensorDevices.AddAsync(device);
            await _unitOfWork.SaveChangesAsync();

            //var deviceDto = new SensorDeviceDto
            //{
            //    Id = device.Id,
            //    DeviceId = device.DeviceId,
            //    Location = device.Location,
            //    Status = device.Status,
            //    InstalledAt = device.InstalledAt,
            //    IsActive = device.IsActive
            //};

            var deviceDto = _mapper.Map<SensorDeviceDto>(device);

            return CreatedAtAction(nameof(GetMyDevices), new { id = device.Id },
                ApiResponse<SensorDeviceDto>.SuccessResponse(deviceDto, "Device registered successfully"));
        }

        // POST: api/sensordevices/readings
        [HttpPost("readings")]

        public async Task<ActionResult<ApiResponse<bool>>> SubmitReading([FromBody] CreateSensorReadingDto dto)
        {
            // Verify device exists and belongs to user
            var sensorDevice = await _unitOfWork.SensorDevices
                .FirstOrDefaultAsync(d => d.DeviceId == dto.DeviceId && d.IsActive);

            if (sensorDevice == null)
                return NotFound(ApiResponse<bool>.ErrorResponse("Device not found"));

            //// Verify user owns the device
            //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            //if (sensorDevice.UserId != userId)
            //    return Forbid();

            //var reading = new SensorReading
            //{
            //    SensorDeviceId = sensorDevice.Id,
            //    UserPlantId = dto.UserPlantId,
            //    Nitrogen = dto.Nitrogen,
            //    Phosphorous = dto.Phosphorous,
            //    Potassium = dto.Potassium,
            //    Ph = dto.Ph,
            //    Humidity = dto.Humidity,
            //    Temperature = dto.Temperature,
            //    Rainfall = dto.Rainfall,
            //    SoilMoisture = dto.SoilMoisture,
            //    ReadingTime = dto.ReadingTime ?? DateTime.UtcNow
            //};

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

        // GET: api/sensordevices/{id}/readings
        [HttpGet("{id}/readings")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<List<SensorReadingDto>>>> GetDeviceReadings(int id, [FromQuery] int count = 100)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var device = await _unitOfWork.SensorDevices
                .FirstOrDefaultAsync(d => d.Id == id && d.UserId == userId);

            if (device == null)
                return NotFound(ApiResponse<List<SensorReadingDto>>.ErrorResponse("Device not found"));

            var readings = await _unitOfWork.SensorReadings.GetLatestReadingsByDeviceAsync(id, count);

            //var readingDtos = readings.Select(r => new SensorReadingDto
            //{
            //    Id = r.Id,
            //    SensorDeviceId = r.SensorDeviceId,
            //    Nitrogen = r.Nitrogen,
            //    Phosphorous = r.Phosphorous,
            //    Potassium = r.Potassium,
            //    Ph = r.Ph,
            //    Humidity = r.Humidity,
            //    Temperature = r.Temperature,
            //    Rainfall = r.Rainfall,
            //    SoilMoisture = r.SoilMoisture,
            //    ReadingTime = r.ReadingTime
            //}).ToList();

            var readingDtos = _mapper.Map<List<SensorReadingDto>>(readings);

            return Ok(ApiResponse<List<SensorReadingDto>>.SuccessResponse(readingDtos));
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

            //if (latestReading != null)
            //{
            //    statistics.Add(CreateStatistic("Nitrogen", averages.GetValueOrDefault("Nitrogen"), latestReading.Nitrogen));
            //    statistics.Add(CreateStatistic("Phosphorous", averages.GetValueOrDefault("Phosphorous"), latestReading.Phosphorous));
            //    statistics.Add(CreateStatistic("Potassium", averages.GetValueOrDefault("Potassium"), latestReading.Potassium));
            //    statistics.Add(CreateStatistic("pH", averages.GetValueOrDefault("Ph"), latestReading.Ph));
            //    statistics.Add(CreateStatistic("Humidity", averages.GetValueOrDefault("Humidity"), latestReading.Humidity));
            //    statistics.Add(CreateStatistic("Temperature", averages.GetValueOrDefault("Temperature"), latestReading.Temperature));
            //    statistics.Add(CreateStatistic("Soil Moisture", averages.GetValueOrDefault("SoilMoisture"), latestReading.SoilMoisture));
            //}

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