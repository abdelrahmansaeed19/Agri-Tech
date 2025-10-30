using AgriculturalTech.API.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AgriculturalTech.API.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // ===================== DBSETS =====================
        public DbSet<CropType> CropTypes { get; set; }
        public DbSet<UserPlant> UserPlants { get; set; }
        public DbSet<CropDisease> CropDiseases { get; set; }
        public DbSet<DiseaseDetectionLog> DiseaseDetectionLogs { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<SensorDevice> SensorDevices { get; set; }
        public DbSet<SensorReading> SensorReadings { get; set; }
        public DbSet<PlantHealthLog> PlantHealthLogs { get; set; }
        public DbSet<MarketPrice> MarketPrices { get; set; }
        public DbSet<CropCalendarTemplate> CropCalendarTemplates { get; set; }
        public DbSet<CropReminder> CropReminders { get; set; }
        public DbSet<FertilizerRecommendation> FertilizerRecommendations { get; set; }
        public DbSet<FertilizerApplicationLog> FertilizerApplicationLogs { get; set; }
        public DbSet<WeatherAlert> WeatherAlerts { get; set; }
        public DbSet<WeatherForecast> WeatherForecasts { get; set; }
        public DbSet<ActivityLog> ActivityLogs { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // ===================== IDENTITY TABLES CONFIGURATION =====================
            builder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable(name: "Users");
            });

            builder.Entity<Microsoft.AspNetCore.Identity.IdentityRole>(entity =>
            {
                entity.ToTable(name: "Roles");
            });

            builder.Entity<Microsoft.AspNetCore.Identity.IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("UserRoles");
            });

            builder.Entity<Microsoft.AspNetCore.Identity.IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("UserClaims");
            });

            builder.Entity<Microsoft.AspNetCore.Identity.IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("UserLogins");
            });

            builder.Entity<Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("RoleClaims");
            });

            builder.Entity<Microsoft.AspNetCore.Identity.IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("UserTokens");
            });

            // ===================== CROP TYPE CONFIGURATION =====================
            builder.Entity<CropType>(entity =>
            {
                entity.HasIndex(e => e.NameEn).IsUnique();
                entity.HasIndex(e => e.Category);
                entity.Property(e => e.IdealTemperatureMin).HasPrecision(5, 2);
                entity.Property(e => e.IdealTemperatureMax).HasPrecision(5, 2);
                entity.Property(e => e.IdealPhMax).HasPrecision(5, 2);
                entity.Property(e => e.IdealPhMin).HasPrecision(5, 2);
                entity.Property(e => e.IdealHumidityMin).HasPrecision(5, 2);
                entity.Property(e => e.IdealHumidityMax).HasPrecision(5, 2);
            });

            // ===================== USER PLANT CONFIGURATION =====================
            builder.Entity<UserPlant>(entity =>
            {
                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserPlants)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.CropType)
                    .WithMany(p => p.UserPlants)
                    .HasForeignKey(d => d.CropTypeId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.SensorDevice)
                    .WithMany(p => p.UserPlant)
                    .HasForeignKey(d => d.SensorDeviceId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasIndex(e => e.UserId);
                entity.HasIndex(e => e.CropTypeId);
                entity.HasIndex(e => e.Status);
                entity.HasIndex(e => e.PlantingDate);
            });

            // ===================== CROP DISEASE CONFIGURATION =====================
            builder.Entity<CropDisease>(entity =>
            {
                entity.HasOne(d => d.CropType)
                    .WithMany(p => p.CropDiseases)
                    .HasForeignKey(d => d.CropTypeId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(e => e.NameEn);
                entity.HasIndex(e => e.CropTypeId);
            });

            // ===================== DISEASE DETECTION LOG CONFIGURATION =====================
            builder.Entity<DiseaseDetectionLog>(entity =>
            {
                entity.HasOne(d => d.UserPlant)
                    .WithMany(p => p.DiseaseDetectionLogs)
                    .HasForeignKey(d => d.UserPlantId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.DetectedDisease)
                    .WithMany(p => p.DiseaseDetectionLogs)
                    .HasForeignKey(d => d.DetectedDiseaseId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasIndex(e => e.UserPlantId);
                entity.HasIndex(e => e.DetectedDiseaseId);
                entity.HasIndex(e => e.DetectedAt);
            });

            // ===================== DEVICE CONFIGURATION =====================
            builder.Entity<Device>(entity =>
            {
                entity.HasIndex(e => e.Name).IsUnique();
                entity.HasIndex(e => e.Type);
            });

            // ===================== SENSOR DEVICE CONFIGURATION =====================
            builder.Entity<SensorDevice>(entity =>
            {
                entity.HasOne(d => d.User)
                    .WithMany(p => p.SensorDevices)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.Device)
                    .WithMany(p => p.SensorDevices)
                    .HasForeignKey(d => d.DeviceId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(e => e.DeviceId).IsUnique();
                entity.HasIndex(e => e.UserId);
                entity.HasIndex(e => e.Status);
            });

            // ===================== SENSOR READING CONFIGURATION =====================
            builder.Entity<SensorReading>(entity =>
            {
                entity.HasOne(d => d.SensorDevice)
                    .WithMany(p => p.SensorReadings)
                    .HasForeignKey(d => d.SensorDeviceId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.UserPlant)
                    .WithMany(p => p.SensorReadings)
                    .HasForeignKey(d => d.UserPlantId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasIndex(e => e.SensorDeviceId);
                entity.HasIndex(e => e.UserPlantId);
                entity.HasIndex(e => e.ReadingTime);
            });

            // ===================== PLANT HEALTH LOG CONFIGURATION =====================
            builder.Entity<PlantHealthLog>(entity =>
            {
                entity.HasOne(d => d.UserPlant)
                    .WithMany(p => p.PlantHealthLogs)
                    .HasForeignKey(d => d.UserPlantId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => e.UserPlantId);
                entity.HasIndex(e => e.LoggedAt);
            });

            // ===================== MARKET PRICE CONFIGURATION =====================
            builder.Entity<MarketPrice>(entity =>
            {
                entity.HasOne(d => d.CropType)
                    .WithMany()
                    .HasForeignKey(d => d.CropTypeId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(e => e.CropTypeId);
                entity.HasIndex(e => e.PriceDate);
                entity.HasIndex(e => new { e.CropTypeId, e.PriceDate });
            });

            // ===================== CROP CALENDAR TEMPLATE CONFIGURATION =====================
            builder.Entity<CropCalendarTemplate>(entity =>
            {
                entity.HasOne(d => d.CropType)
                    .WithMany(p => p.CropCalendarTemplates)
                    .HasForeignKey(d => d.CropTypeId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => e.CropTypeId);
                entity.HasIndex(e => e.ActivityType);
            });

            // ===================== CROP REMINDER CONFIGURATION =====================
            builder.Entity<CropReminder>(entity =>
            {
                entity.HasOne(d => d.User)
                    .WithMany(p => p.CropReminders)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(d => d.UserPlant)
                    .WithMany()
                    .HasForeignKey(d => d.UserPlantId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => e.UserId);
                entity.HasIndex(e => e.UserPlantId);
                entity.HasIndex(e => e.ReminderDate);
                entity.HasIndex(e => e.IsCompleted);
            });

            // ===================== FERTILIZER RECOMMENDATION CONFIGURATION =====================
            builder.Entity<FertilizerRecommendation>(entity =>
            {
                entity.HasOne(d => d.CropType)
                    .WithMany(p => p.FertilizerRecommendations)
                    .HasForeignKey(d => d.CropTypeId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(e => e.CropTypeId);
                entity.HasIndex(e => e.GrowthStage);
            });

            // ===================== FERTILIZER APPLICATION LOG CONFIGURATION =====================
            builder.Entity<FertilizerApplicationLog>(entity =>
            {
                entity.HasOne(d => d.UserPlant)
                    .WithMany()
                    .HasForeignKey(d => d.UserPlantId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.FertilizerRecommendation)
                    .WithMany()
                    .HasForeignKey(d => d.FertilizerRecommendationId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasIndex(e => e.UserPlantId);
                entity.HasIndex(e => e.ApplicationDate);
            });

            // ===================== WEATHER ALERT CONFIGURATION =====================
            builder.Entity<WeatherAlert>(entity =>
            {
                entity.HasOne(d => d.User)
                    .WithMany(p => p.WeatherAlerts)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => e.UserId);
                entity.HasIndex(e => e.AlertStartDate);
                entity.HasIndex(e => e.IsRead);
            });

            // ===================== WEATHER FORECAST CONFIGURATION =====================
            builder.Entity<WeatherForecast>(entity =>
            {
                entity.HasIndex(e => e.Location);
                entity.HasIndex(e => e.ForecastDate);
                entity.HasIndex(e => new { e.Location, e.ForecastDate });
            });

            // ===================== ACTIVITY LOG CONFIGURATION =====================
            builder.Entity<ActivityLog>(entity =>
            {
                entity.HasOne(d => d.User)
                    .WithMany()
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => e.UserId);
                entity.HasIndex(e => e.Action);
                entity.HasIndex(e => e.CreatedAt);
            });

            // ===================== NOTIFICATION CONFIGURATION =====================
            builder.Entity<Notification>(entity =>
            {
                entity.HasOne(d => d.User)
                    .WithMany()
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => e.UserId);
                entity.HasIndex(e => e.IsRead);
                entity.HasIndex(e => e.CreatedAt);
            });

            // ===================== SEED DATA =====================
            SeedData(builder);
        }

        private void SeedData(ModelBuilder builder)
        {
            // Seed crop types
            builder.Entity<CropType>().HasData(
                new CropType
                {
                    Id = 1,
                    NameEn = "Tomato",
                    NameAr = "طماطم",
                    ScientificName = "Solanum lycopersicum",
                    Category = "Vegetables",
                    DescriptionEn = "Popular garden vegetable",
                    DescriptionAr = "خضار شائعة في الحدائق",
                    IdealTemperatureMin = 18,
                    IdealTemperatureMax = 27,
                    IdealHumidityMin = 60,
                    IdealHumidityMax = 80,
                    IdealPhMin = 6.0m,
                    IdealPhMax = 6.8m,
                    GrowingDurationDays = 75
                },
                new CropType
                {
                    Id = 2,
                    NameEn = "Wheat",
                    NameAr = "قمح",
                    ScientificName = "Triticum aestivum",
                    Category = "Grains",
                    DescriptionEn = "Staple grain crop",
                    DescriptionAr = "محصول حبوب أساسي",
                    IdealTemperatureMin = 15,
                    IdealTemperatureMax = 25,
                    IdealHumidityMin = 50,
                    IdealHumidityMax = 70,
                    IdealPhMin = 6.0m,
                    IdealPhMax = 7.5m,
                    GrowingDurationDays = 120
                },
                new CropType
                {
                    Id = 3,
                    NameEn = "Corn (Maize)",
                    NameAr = "ذرة",
                    ScientificName = "Zea mays",
                    Category = "Grains",
                    DescriptionEn = "Important cereal grain",
                    DescriptionAr = "حبوب غذائية مهمة",
                    IdealTemperatureMin = 20,
                    IdealTemperatureMax = 30,
                    IdealHumidityMin = 60,
                    IdealHumidityMax = 80,
                    IdealPhMin = 5.8m,
                    IdealPhMax = 7.0m,
                    GrowingDurationDays = 90
                },
                new CropType
                {
                    Id = 4,
                    NameEn = "Potato",
                    NameAr = "بطاطس",
                    ScientificName = "Solanum tuberosum",
                    Category = "Vegetables",
                    DescriptionEn = "Root vegetable crop",
                    DescriptionAr = "محصول خضار جذري",
                    IdealTemperatureMin = 15,
                    IdealTemperatureMax = 20,
                    IdealHumidityMin = 70,
                    IdealHumidityMax = 80,
                    IdealPhMin = 5.0m,
                    IdealPhMax = 6.5m,
                    GrowingDurationDays = 100
                },
                new CropType
                {
                    Id = 5,
                    NameEn = "Rice",
                    NameAr = "أرز",
                    ScientificName = "Oryza sativa",
                    Category = "Grains",
                    DescriptionEn = "Staple food grain",
                    DescriptionAr = "حبوب غذائية أساسية",
                    IdealTemperatureMin = 20,
                    IdealTemperatureMax = 35,
                    IdealHumidityMin = 70,
                    IdealHumidityMax = 90,
                    IdealPhMin = 5.5m,
                    IdealPhMax = 6.5m,
                    GrowingDurationDays = 120
                }
            );

            builder.Entity<Device>().HasData(
                new Device
                {
                    Id = "ESP32_DHT22_01", // The unique ID the ESP32 sends
                    Name = "Greenhouse Temp/Humidity Sensor",
                    Type = "Temperature & Humidity",
                    Description = "Standard sensor for monitoring ambient air temperature and relative humidity."
                },
                new Device
                {
                    Id = "ESP32_SOIL_MOISTURE_01",
                    Name = "Tomato Patch Moisture Sensor",
                    Type = "Soil Moisture",
                    Description = "Soil moisture sensor for monitoring water content in the root zone."
                },
                new Device
                {
                    Id = "ESP32_NPK_01",
                    Name = "Hydroponics NPK Sensor",
                    Type = "NPK Soil Sensor",
                    Description = "Measures Nitrogen, Phosphorus, and Potassium levels in the soil."
                },
                new Device
                {
                    Id = "ESP32_LIGHT_SENSOR_01",
                    Name = "General Purpose Light Sensor",
                    Type = "Ambient Light (LUX)",
                    Description = "Monitors ambient light levels to ensure optimal photosynthesis."
                }
            );

        }
    }
}