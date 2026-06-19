using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AgriculturalTech.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveSomeUndoneWork : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SensorReadings_UserPlants_UserPlantId",
                table: "SensorReadings");

            migrationBuilder.DropTable(
                name: "ActivityLogs");

            migrationBuilder.DropTable(
                name: "CropCalendarTemplates");

            migrationBuilder.DropTable(
                name: "CropReminders");

            migrationBuilder.DropTable(
                name: "Devices");

            migrationBuilder.DropTable(
                name: "DiseaseDetectionLogs");

            migrationBuilder.DropTable(
                name: "FertilizerApplicationLogs");

            migrationBuilder.DropTable(
                name: "MarketPrices");

            migrationBuilder.DropTable(
                name: "PlantHealthLogs");

            migrationBuilder.DropTable(
                name: "CropDiseases");

            migrationBuilder.DropTable(
                name: "FertilizerRecommendations");

            migrationBuilder.DropTable(
                name: "UserPlants");

            migrationBuilder.DropTable(
                name: "CropTypes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActivityLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    EntityId = table.Column<int>(type: "int", nullable: true),
                    EntityType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivityLogs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CropTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DescriptionAr = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DescriptionEn = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    GrowingDurationDays = table.Column<int>(type: "int", nullable: true),
                    IdealHumidityMax = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    IdealHumidityMin = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    IdealPhMax = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    IdealPhMin = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    IdealTemperatureMax = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    IdealTemperatureMin = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ScientificName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CropTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Devices",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CropCalendarTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CropTypeId = table.Column<int>(type: "int", nullable: false),
                    ActivityNameAr = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ActivityNameEn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ActivityType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DaysAfterPlanting = table.Column<int>(type: "int", nullable: false),
                    DescriptionAr = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    DescriptionEn = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsRecurring = table.Column<bool>(type: "bit", nullable: false),
                    RecurringIntervalDays = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CropCalendarTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CropCalendarTemplates_CropTypes_CropTypeId",
                        column: x => x.CropTypeId,
                        principalTable: "CropTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CropDiseases",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CropTypeId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DescriptionAr = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    DescriptionEn = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    PreventionAr = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    PreventionEn = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Severity = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SymptomsAr = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    SymptomsEn = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    TreatmentAr = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    TreatmentEn = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CropDiseases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CropDiseases_CropTypes_CropTypeId",
                        column: x => x.CropTypeId,
                        principalTable: "CropTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FertilizerRecommendations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CropTypeId = table.Column<int>(type: "int", nullable: false),
                    ApplicationMethod = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FertilizerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FertilizerType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FrequencyDays = table.Column<int>(type: "int", nullable: false),
                    GrowthStage = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    InstructionsAr = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    InstructionsEn = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    MaxSoilPh = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    MinSoilPh = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    NitrogenPercentage = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    PhosphorousPercentage = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    PotassiumPercentage = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    RecommendedAmountPerSquareMeter = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FertilizerRecommendations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FertilizerRecommendations_CropTypes_CropTypeId",
                        column: x => x.CropTypeId,
                        principalTable: "CropTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MarketPrices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CropTypeId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    MarketLocation = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MarketName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PriceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PricePerUnit = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Quality = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Source = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarketPrices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MarketPrices_CropTypes_CropTypeId",
                        column: x => x.CropTypeId,
                        principalTable: "CropTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserPlants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CropTypeId = table.Column<int>(type: "int", nullable: false),
                    SensorDeviceId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ActualHarvestDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AreaInSquareMeters = table.Column<double>(type: "float", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CustomName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ExpectedHarvestDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    PlantingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPlants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPlants_CropTypes_CropTypeId",
                        column: x => x.CropTypeId,
                        principalTable: "CropTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserPlants_SensorDevices_SensorDeviceId",
                        column: x => x.SensorDeviceId,
                        principalTable: "SensorDevices",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserPlants_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CropReminders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserPlantId = table.Column<int>(type: "int", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    IsRecurring = table.Column<bool>(type: "bit", nullable: false),
                    NotificationSent = table.Column<bool>(type: "bit", nullable: false),
                    Priority = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    RecurringIntervalDays = table.Column<int>(type: "int", nullable: true),
                    ReminderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReminderType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CropReminders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CropReminders_UserPlants_UserPlantId",
                        column: x => x.UserPlantId,
                        principalTable: "UserPlants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CropReminders_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DiseaseDetectionLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DetectedDiseaseId = table.Column<int>(type: "int", nullable: true),
                    UserPlantId = table.Column<int>(type: "int", nullable: false),
                    AiModelVersion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ConfidenceScore = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    DetectedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DetectionStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsUserConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiseaseDetectionLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiseaseDetectionLogs_CropDiseases_DetectedDiseaseId",
                        column: x => x.DetectedDiseaseId,
                        principalTable: "CropDiseases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_DiseaseDetectionLogs_UserPlants_UserPlantId",
                        column: x => x.UserPlantId,
                        principalTable: "UserPlants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FertilizerApplicationLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FertilizerRecommendationId = table.Column<int>(type: "int", nullable: true),
                    UserPlantId = table.Column<int>(type: "int", nullable: false),
                    AmountApplied = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    ApplicationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FertilizerUsed = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FertilizerApplicationLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FertilizerApplicationLogs_FertilizerRecommendations_FertilizerRecommendationId",
                        column: x => x.FertilizerRecommendationId,
                        principalTable: "FertilizerRecommendations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_FertilizerApplicationLogs_UserPlants_UserPlantId",
                        column: x => x.UserPlantId,
                        principalTable: "UserPlants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlantHealthLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserPlantId = table.Column<int>(type: "int", nullable: false),
                    GrowthHeightCm = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    HealthStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LoggedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Observations = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlantHealthLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlantHealthLogs_UserPlants_UserPlantId",
                        column: x => x.UserPlantId,
                        principalTable: "UserPlants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "CropTypes",
                columns: new[] { "Id", "Category", "CreatedAt", "DescriptionAr", "DescriptionEn", "GrowingDurationDays", "IdealHumidityMax", "IdealHumidityMin", "IdealPhMax", "IdealPhMin", "IdealTemperatureMax", "IdealTemperatureMin", "ImageUrl", "IsActive", "NameAr", "NameEn", "ScientificName" },
                values: new object[,]
                {
                    { 1, "Vegetables", new DateTime(2026, 6, 2, 5, 51, 8, 669, DateTimeKind.Utc).AddTicks(7322), "خضار شائعة في الحدائق", "Popular garden vegetable", 75, 80m, 60m, 6.8m, 6.0m, 27m, 18m, "", true, "طماطم", "Tomato", "Solanum lycopersicum" },
                    { 2, "Grains", new DateTime(2026, 6, 2, 5, 51, 8, 669, DateTimeKind.Utc).AddTicks(7349), "محصول حبوب أساسي", "Staple grain crop", 120, 70m, 50m, 7.5m, 6.0m, 25m, 15m, "", true, "قمح", "Wheat", "Triticum aestivum" },
                    { 3, "Grains", new DateTime(2026, 6, 2, 5, 51, 8, 669, DateTimeKind.Utc).AddTicks(7354), "حبوب غذائية مهمة", "Important cereal grain", 90, 80m, 60m, 7.0m, 5.8m, 30m, 20m, "", true, "ذرة", "Corn (Maize)", "Zea mays" },
                    { 4, "Vegetables", new DateTime(2026, 6, 2, 5, 51, 8, 669, DateTimeKind.Utc).AddTicks(7358), "محصول خضار جذري", "Root vegetable crop", 100, 80m, 70m, 6.5m, 5.0m, 20m, 15m, "", true, "بطاطس", "Potato", "Solanum tuberosum" },
                    { 5, "Grains", new DateTime(2026, 6, 2, 5, 51, 8, 669, DateTimeKind.Utc).AddTicks(7362), "حبوب غذائية أساسية", "Staple food grain", 120, 90m, 70m, 6.5m, 5.5m, 35m, 20m, "", true, "أرز", "Rice", "Oryza sativa" }
                });

            migrationBuilder.InsertData(
                table: "Devices",
                columns: new[] { "Id", "Description", "Name", "Type" },
                values: new object[,]
                {
                    { "ESP32_DHT22_01", "Standard sensor for monitoring ambient air temperature and relative humidity.", "Greenhouse Temp/Humidity Sensor", "Temperature & Humidity" },
                    { "ESP32_LIGHT_SENSOR_01", "Monitors ambient light levels to ensure optimal photosynthesis.", "General Purpose Light Sensor", "Ambient Light (LUX)" },
                    { "ESP32_NPK_01", "Measures Nitrogen, Phosphorus, and Potassium levels in the soil.", "Hydroponics NPK Sensor", "NPK Soil Sensor" },
                    { "ESP32_SOIL_MOISTURE_01", "Soil moisture sensor for monitoring water content in the root zone.", "Tomato Patch Moisture Sensor", "Soil Moisture" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogs_Action",
                table: "ActivityLogs",
                column: "Action");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogs_CreatedAt",
                table: "ActivityLogs",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogs_UserId",
                table: "ActivityLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CropCalendarTemplates_ActivityType",
                table: "CropCalendarTemplates",
                column: "ActivityType");

            migrationBuilder.CreateIndex(
                name: "IX_CropCalendarTemplates_CropTypeId",
                table: "CropCalendarTemplates",
                column: "CropTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CropDiseases_CropTypeId",
                table: "CropDiseases",
                column: "CropTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CropDiseases_NameEn",
                table: "CropDiseases",
                column: "NameEn");

            migrationBuilder.CreateIndex(
                name: "IX_CropReminders_IsCompleted",
                table: "CropReminders",
                column: "IsCompleted");

            migrationBuilder.CreateIndex(
                name: "IX_CropReminders_ReminderDate",
                table: "CropReminders",
                column: "ReminderDate");

            migrationBuilder.CreateIndex(
                name: "IX_CropReminders_UserId",
                table: "CropReminders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CropReminders_UserPlantId",
                table: "CropReminders",
                column: "UserPlantId");

            migrationBuilder.CreateIndex(
                name: "IX_CropTypes_Category",
                table: "CropTypes",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_CropTypes_NameEn",
                table: "CropTypes",
                column: "NameEn",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Devices_Name",
                table: "Devices",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Devices_Type",
                table: "Devices",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_DiseaseDetectionLogs_DetectedAt",
                table: "DiseaseDetectionLogs",
                column: "DetectedAt");

            migrationBuilder.CreateIndex(
                name: "IX_DiseaseDetectionLogs_DetectedDiseaseId",
                table: "DiseaseDetectionLogs",
                column: "DetectedDiseaseId");

            migrationBuilder.CreateIndex(
                name: "IX_DiseaseDetectionLogs_UserPlantId",
                table: "DiseaseDetectionLogs",
                column: "UserPlantId");

            migrationBuilder.CreateIndex(
                name: "IX_FertilizerApplicationLogs_ApplicationDate",
                table: "FertilizerApplicationLogs",
                column: "ApplicationDate");

            migrationBuilder.CreateIndex(
                name: "IX_FertilizerApplicationLogs_FertilizerRecommendationId",
                table: "FertilizerApplicationLogs",
                column: "FertilizerRecommendationId");

            migrationBuilder.CreateIndex(
                name: "IX_FertilizerApplicationLogs_UserPlantId",
                table: "FertilizerApplicationLogs",
                column: "UserPlantId");

            migrationBuilder.CreateIndex(
                name: "IX_FertilizerRecommendations_CropTypeId",
                table: "FertilizerRecommendations",
                column: "CropTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FertilizerRecommendations_GrowthStage",
                table: "FertilizerRecommendations",
                column: "GrowthStage");

            migrationBuilder.CreateIndex(
                name: "IX_MarketPrices_CropTypeId",
                table: "MarketPrices",
                column: "CropTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_MarketPrices_CropTypeId_PriceDate",
                table: "MarketPrices",
                columns: new[] { "CropTypeId", "PriceDate" });

            migrationBuilder.CreateIndex(
                name: "IX_MarketPrices_PriceDate",
                table: "MarketPrices",
                column: "PriceDate");

            migrationBuilder.CreateIndex(
                name: "IX_PlantHealthLogs_LoggedAt",
                table: "PlantHealthLogs",
                column: "LoggedAt");

            migrationBuilder.CreateIndex(
                name: "IX_PlantHealthLogs_UserPlantId",
                table: "PlantHealthLogs",
                column: "UserPlantId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPlants_CropTypeId",
                table: "UserPlants",
                column: "CropTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPlants_PlantingDate",
                table: "UserPlants",
                column: "PlantingDate");

            migrationBuilder.CreateIndex(
                name: "IX_UserPlants_SensorDeviceId",
                table: "UserPlants",
                column: "SensorDeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPlants_Status",
                table: "UserPlants",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_UserPlants_UserId",
                table: "UserPlants",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_SensorReadings_UserPlants_UserPlantId",
                table: "SensorReadings",
                column: "UserPlantId",
                principalTable: "UserPlants",
                principalColumn: "Id");
        }
    }
}
