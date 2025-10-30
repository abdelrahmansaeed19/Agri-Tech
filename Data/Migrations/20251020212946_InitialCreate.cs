using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AgriculturalTech.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CropTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameEn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ScientificName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DescriptionEn = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DescriptionAr = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IdealTemperatureMin = table.Column<double>(type: "float", nullable: true),
                    IdealTemperatureMax = table.Column<double>(type: "float", nullable: true),
                    IdealHumidityMin = table.Column<double>(type: "float", nullable: true),
                    IdealHumidityMax = table.Column<double>(type: "float", nullable: true),
                    IdealPhMin = table.Column<double>(type: "float", nullable: true),
                    IdealPhMax = table.Column<double>(type: "float", nullable: true),
                    GrowingDurationDays = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CropTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FarmName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FarmLocation = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FarmAreaInAcres = table.Column<double>(type: "float", nullable: true),
                    PreferredLanguage = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastLoginAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WeatherForecasts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Location = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ForecastDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TemperatureMax = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    TemperatureMin = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    Humidity = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    RainfallMm = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    WindSpeedKmh = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    WeatherCondition = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Source = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeatherForecasts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CropCalendarTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CropTypeId = table.Column<int>(type: "int", nullable: false),
                    ActivityNameEn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ActivityNameAr = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ActivityType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DaysAfterPlanting = table.Column<int>(type: "int", nullable: false),
                    DescriptionEn = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    DescriptionAr = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    IsRecurring = table.Column<bool>(type: "bit", nullable: false),
                    RecurringIntervalDays = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
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
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CropTypeId = table.Column<int>(type: "int", nullable: true),
                    DescriptionEn = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    DescriptionAr = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    SymptomsEn = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    SymptomsAr = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    TreatmentEn = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    TreatmentAr = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    PreventionEn = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    PreventionAr = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Severity = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
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
                    FertilizerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FertilizerType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    GrowthStage = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NitrogenPercentage = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    PhosphorousPercentage = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    PotassiumPercentage = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    RecommendedAmountPerSquareMeter = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ApplicationMethod = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FrequencyDays = table.Column<int>(type: "int", nullable: false),
                    InstructionsEn = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    InstructionsAr = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    MinSoilPh = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    MaxSoilPh = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
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
                    MarketName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MarketLocation = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PricePerUnit = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Quality = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PriceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Source = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                name: "RoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleClaims_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActivityLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EntityType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EntityId = table.Column<int>(type: "int", nullable: true),
                    Details = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NotificationType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    ReadAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActionLink = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SensorDevices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DeviceName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DeviceId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DeviceType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Location = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastSyncAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InstalledAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SensorDevices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SensorDevices_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaims_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_UserLogins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserPlants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CropTypeId = table.Column<int>(type: "int", nullable: false),
                    CustomName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PlantingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpectedHarvestDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActualHarvestDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AreaInSquareMeters = table.Column<double>(type: "float", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
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
                        name: "FK_UserPlants_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_UserTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WeatherAlerts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    AlertType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Severity = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MessageEn = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    MessageAr = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    AlertStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AlertEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    Source = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeatherAlerts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeatherAlerts_Users_UserId",
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
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ReminderType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ReminderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRecurring = table.Column<bool>(type: "bit", nullable: false),
                    RecurringIntervalDays = table.Column<int>(type: "int", nullable: true),
                    Priority = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NotificationSent = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
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
                    UserPlantId = table.Column<int>(type: "int", nullable: false),
                    DetectedDiseaseId = table.Column<int>(type: "int", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConfidenceScore = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    DetectionStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AiModelVersion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    IsUserConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    DetectedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                    UserPlantId = table.Column<int>(type: "int", nullable: false),
                    FertilizerRecommendationId = table.Column<int>(type: "int", nullable: true),
                    FertilizerUsed = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AmountApplied = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ApplicationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                    HealthStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Observations = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GrowthHeightCm = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    LoggedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "SensorReadings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SensorDeviceId = table.Column<int>(type: "int", nullable: false),
                    UserPlantId = table.Column<int>(type: "int", nullable: true),
                    Nitrogen = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    Phosphorous = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    Potassium = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    Ph = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    Humidity = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    Temperature = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    Rainfall = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    SoilMoisture = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    ReadingTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SensorReadings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SensorReadings_SensorDevices_SensorDeviceId",
                        column: x => x.SensorDeviceId,
                        principalTable: "SensorDevices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SensorReadings_UserPlants_UserPlantId",
                        column: x => x.UserPlantId,
                        principalTable: "UserPlants",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "CropTypes",
                columns: new[] { "Id", "Category", "CreatedAt", "DescriptionAr", "DescriptionEn", "GrowingDurationDays", "IdealHumidityMax", "IdealHumidityMin", "IdealPhMax", "IdealPhMin", "IdealTemperatureMax", "IdealTemperatureMin", "ImageUrl", "IsActive", "NameAr", "NameEn", "ScientificName" },
                values: new object[,]
                {
                    { 1, "Vegetables", new DateTime(2025, 10, 20, 21, 29, 45, 558, DateTimeKind.Utc).AddTicks(7517), "خضار شائعة في الحدائق", "Popular garden vegetable", 75, 80.0, 60.0, 6.7999999999999998, 6.0, 27.0, 18.0, "", true, "طماطم", "Tomato", "Solanum lycopersicum" },
                    { 2, "Grains", new DateTime(2025, 10, 20, 21, 29, 45, 558, DateTimeKind.Utc).AddTicks(7534), "محصول حبوب أساسي", "Staple grain crop", 120, 70.0, 50.0, 7.5, 6.0, 25.0, 15.0, "", true, "قمح", "Wheat", "Triticum aestivum" },
                    { 3, "Grains", new DateTime(2025, 10, 20, 21, 29, 45, 558, DateTimeKind.Utc).AddTicks(7539), "حبوب غذائية مهمة", "Important cereal grain", 90, 80.0, 60.0, 7.0, 5.7999999999999998, 30.0, 20.0, "", true, "ذرة", "Corn (Maize)", "Zea mays" },
                    { 4, "Vegetables", new DateTime(2025, 10, 20, 21, 29, 45, 558, DateTimeKind.Utc).AddTicks(7543), "محصول خضار جذري", "Root vegetable crop", 100, 80.0, 70.0, 6.5, 5.0, 20.0, 15.0, "", true, "بطاطس", "Potato", "Solanum tuberosum" },
                    { 5, "Grains", new DateTime(2025, 10, 20, 21, 29, 45, 558, DateTimeKind.Utc).AddTicks(7547), "حبوب غذائية أساسية", "Staple food grain", 120, 90.0, 70.0, 6.5, 5.5, 35.0, 20.0, "", true, "أرز", "Rice", "Oryza sativa" }
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
                name: "IX_Notifications_CreatedAt",
                table: "Notifications",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_IsRead",
                table: "Notifications",
                column: "IsRead");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PlantHealthLogs_LoggedAt",
                table: "PlantHealthLogs",
                column: "LoggedAt");

            migrationBuilder.CreateIndex(
                name: "IX_PlantHealthLogs_UserPlantId",
                table: "PlantHealthLogs",
                column: "UserPlantId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_RoleId",
                table: "RoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "Roles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_SensorDevices_DeviceId",
                table: "SensorDevices",
                column: "DeviceId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SensorDevices_Status",
                table: "SensorDevices",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_SensorDevices_UserId",
                table: "SensorDevices",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SensorReadings_ReadingTime",
                table: "SensorReadings",
                column: "ReadingTime");

            migrationBuilder.CreateIndex(
                name: "IX_SensorReadings_SensorDeviceId",
                table: "SensorReadings",
                column: "SensorDeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_SensorReadings_UserPlantId",
                table: "SensorReadings",
                column: "UserPlantId");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_UserId",
                table: "UserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_UserId",
                table: "UserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPlants_CropTypeId",
                table: "UserPlants",
                column: "CropTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPlants_PlantingDate",
                table: "UserPlants",
                column: "PlantingDate");

            migrationBuilder.CreateIndex(
                name: "IX_UserPlants_Status",
                table: "UserPlants",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_UserPlants_UserId",
                table: "UserPlants",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "Users",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "Users",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_WeatherAlerts_AlertStartDate",
                table: "WeatherAlerts",
                column: "AlertStartDate");

            migrationBuilder.CreateIndex(
                name: "IX_WeatherAlerts_IsRead",
                table: "WeatherAlerts",
                column: "IsRead");

            migrationBuilder.CreateIndex(
                name: "IX_WeatherAlerts_UserId",
                table: "WeatherAlerts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WeatherForecasts_ForecastDate",
                table: "WeatherForecasts",
                column: "ForecastDate");

            migrationBuilder.CreateIndex(
                name: "IX_WeatherForecasts_Location",
                table: "WeatherForecasts",
                column: "Location");

            migrationBuilder.CreateIndex(
                name: "IX_WeatherForecasts_Location_ForecastDate",
                table: "WeatherForecasts",
                columns: new[] { "Location", "ForecastDate" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityLogs");

            migrationBuilder.DropTable(
                name: "CropCalendarTemplates");

            migrationBuilder.DropTable(
                name: "CropReminders");

            migrationBuilder.DropTable(
                name: "DiseaseDetectionLogs");

            migrationBuilder.DropTable(
                name: "FertilizerApplicationLogs");

            migrationBuilder.DropTable(
                name: "MarketPrices");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "PlantHealthLogs");

            migrationBuilder.DropTable(
                name: "RoleClaims");

            migrationBuilder.DropTable(
                name: "SensorReadings");

            migrationBuilder.DropTable(
                name: "UserClaims");

            migrationBuilder.DropTable(
                name: "UserLogins");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropTable(
                name: "WeatherAlerts");

            migrationBuilder.DropTable(
                name: "WeatherForecasts");

            migrationBuilder.DropTable(
                name: "CropDiseases");

            migrationBuilder.DropTable(
                name: "FertilizerRecommendations");

            migrationBuilder.DropTable(
                name: "SensorDevices");

            migrationBuilder.DropTable(
                name: "UserPlants");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "CropTypes");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
