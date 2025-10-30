using AgriculturalTech.API.Data.Models;
using AutoMapper;

namespace AgriculturalTech.API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // CropType mappings
            CreateMap<CropType, CropTypeDto>();
            CreateMap<CropType, CropTypeDetailDto>()
                // FIX: Map the CommonDiseases from the CropType's navigation property
                .ForMember(dest => dest.CommonDiseases, opt => opt.MapFrom(src => src.CropDiseases));

            // UserPlant mappings
            CreateMap<UserPlant, UserPlantDto>()
                .ForMember(dest => dest.CropTypeName, opt => opt.MapFrom(src => src.CropType.NameEn))
                .ForMember(dest => dest.CropTypeNameAr, opt => opt.MapFrom(src => src.CropType.NameAr))
                .ForMember(dest => dest.DaysGrowing, opt => opt.MapFrom(src => (int)(DateTime.UtcNow - src.PlantingDate).TotalDays))
                .ForMember(dest => dest.DaysUntilHarvest, opt => opt.MapFrom(src =>
                    src.ExpectedHarvestDate.HasValue ? (int)(src.ExpectedHarvestDate.Value - DateTime.UtcNow).TotalDays : (int?)null))
                // FIX: Ignore properties that are calculated in the service layer, not mapped directly
                .ForMember(dest => dest.LatestSensorData, opt => opt.Ignore())
                .ForMember(dest => dest.HealthStatus, opt => opt.Ignore());

            // SensorDevice mappings
            CreateMap<SensorDevice, SensorDeviceDto>();

            // SensorReading mappings
            CreateMap<SensorReading, SensorReadingDto>();

            // Disease mappings
            CreateMap<CropDisease, CropDiseaseDto>();
            CreateMap<DiseaseDetectionLog, DiseaseDetectionDto>()
                .ForMember(dest => dest.DiseaseName, opt => opt.MapFrom(src => src.DetectedDisease.NameEn))
                .ForMember(dest => dest.DiseaseNameAr, opt => opt.MapFrom(src => src.DetectedDisease.NameAr))
                .ForMember(dest => dest.Treatment, opt => opt.MapFrom(src => src.DetectedDisease.TreatmentEn))
                .ForMember(dest => dest.TreatmentAr, opt => opt.MapFrom(src => src.DetectedDisease.TreatmentAr));

            // Reminder mappings
            CreateMap<CropReminder, CropReminderDto>()
                .ForMember(dest => dest.PlantName, opt => opt.MapFrom(src => src.UserPlant.CustomName ?? src.UserPlant.CropType.NameEn))
                .ForMember(dest => dest.IsOverdue, opt => opt.MapFrom(src => src.ReminderDate < DateTime.UtcNow))
                .ForMember(dest => dest.DaysUntilDue, opt => opt.MapFrom(src => (int)(src.ReminderDate - DateTime.UtcNow).TotalDays));

            // Market Price mappings
            CreateMap<MarketPrice, MarketPriceDto>()
                .ForMember(dest => dest.CropName, opt => opt.MapFrom(src => src.CropType.NameEn))
                // FIX: Ignore calculated properties
                .ForMember(dest => dest.PriceChange, opt => opt.Ignore())
                .ForMember(dest => dest.PriceTrend, opt => opt.Ignore());

            // Fertilizer mappings
            CreateMap<FertilizerRecommendation, FertilizerRecommendationDto>();

            // Health Log mappings
            CreateMap<PlantHealthLog, PlantHealthLogDto>();

            // Weather mappings
            CreateMap<WeatherAlert, WeatherAlertDto>()
                // FIX: Ignore calculated property
                .ForMember(dest => dest.RecommendedActions, opt => opt.Ignore());

            CreateMap<WeatherForecast, WeatherForecastDto>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.ForecastDate))
                // FIX: Ignore calculated property
                .ForMember(dest => dest.FarmingAdvice, opt => opt.Ignore());

            // =================================================================
            // DTO -> Model Mappings
            // (Ignoring all server-side and navigation properties)
            // =================================================================

            CreateMap<CreateUserPlantDto, UserPlant>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.ExpectedHarvestDate, opt => opt.Ignore())
                .ForMember(dest => dest.ActualHarvestDate, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.CropType, opt => opt.Ignore())
                .ForMember(dest => dest.PlantHealthLogs, opt => opt.Ignore())
                .ForMember(dest => dest.SensorReadings, opt => opt.Ignore())
                .ForMember(dest => dest.DiseaseDetectionLogs, opt => opt.Ignore());

            CreateMap<UpdateUserPlantDto, UserPlant>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.CropTypeId, opt => opt.Ignore())
                .ForMember(dest => dest.PlantingDate, opt => opt.Ignore())
                .ForMember(dest => dest.AreaInSquareMeters, opt => opt.Ignore())
                .ForMember(dest => dest.Location, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.CropType, opt => opt.Ignore())
                .ForMember(dest => dest.PlantHealthLogs, opt => opt.Ignore())
                .ForMember(dest => dest.SensorReadings, opt => opt.Ignore())
                .ForMember(dest => dest.DiseaseDetectionLogs, opt => opt.Ignore());

            CreateMap<CreateSensorDeviceDto, SensorDevice>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.LastSyncAt, opt => opt.Ignore())
                .ForMember(dest => dest.InstalledAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.SensorReadings, opt => opt.Ignore());

            CreateMap<CreateSensorReadingDto, SensorReading>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Notes, opt => opt.Ignore())
                .ForMember(dest => dest.SensorDevice, opt => opt.Ignore())
                .ForMember(dest => dest.ReadingTime, opt => opt.Ignore())
                .ForMember(dest => dest.UserPlantId, opt => opt.Ignore())
                .ForMember(dest => dest.UserPlant, opt => opt.Ignore());

            CreateMap<CreateCropReminderDto, CropReminder>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.IsCompleted, opt => opt.Ignore())
                .ForMember(dest => dest.CompletedAt, opt => opt.Ignore())
                .ForMember(dest => dest.NotificationSent, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.UserPlant, opt => opt.Ignore());

            CreateMap<LogFertilizerApplicationDto, FertilizerApplicationLog>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ApplicationDate, opt => opt.Ignore())
                .ForMember(dest => dest.UserPlant, opt => opt.Ignore())
                .ForMember(dest => dest.FertilizerRecommendation, opt => opt.Ignore());

            CreateMap<CreatePlantHealthLogDto, PlantHealthLog>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore())
                .ForMember(dest => dest.LoggedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UserPlant, opt => opt.Ignore());
        }

    }
}