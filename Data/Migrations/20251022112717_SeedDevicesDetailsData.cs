using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AgriculturalTech.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedDevicesDetailsData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeviceName",
                table: "SensorDevices");

            migrationBuilder.DropColumn(
                name: "DeviceType",
                table: "SensorDevices");

            migrationBuilder.AlterColumn<string>(
                name: "DeviceId",
                table: "SensorDevices",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.CreateTable(
                name: "Devices",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "CropTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 22, 11, 27, 16, 477, DateTimeKind.Utc).AddTicks(4050));

            migrationBuilder.UpdateData(
                table: "CropTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 22, 11, 27, 16, 477, DateTimeKind.Utc).AddTicks(4070));

            migrationBuilder.UpdateData(
                table: "CropTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 22, 11, 27, 16, 477, DateTimeKind.Utc).AddTicks(4075));

            migrationBuilder.UpdateData(
                table: "CropTypes",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 22, 11, 27, 16, 477, DateTimeKind.Utc).AddTicks(4080));

            migrationBuilder.UpdateData(
                table: "CropTypes",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 22, 11, 27, 16, 477, DateTimeKind.Utc).AddTicks(4084));

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
                name: "IX_Devices_Name",
                table: "Devices",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Devices_Type",
                table: "Devices",
                column: "Type");

            migrationBuilder.AddForeignKey(
                name: "FK_SensorDevices_Devices_DeviceId",
                table: "SensorDevices",
                column: "DeviceId",
                principalTable: "Devices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SensorDevices_Devices_DeviceId",
                table: "SensorDevices");

            migrationBuilder.DropTable(
                name: "Devices");

            migrationBuilder.AlterColumn<string>(
                name: "DeviceId",
                table: "SensorDevices",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "DeviceName",
                table: "SensorDevices",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeviceType",
                table: "SensorDevices",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "CropTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 21, 12, 9, 19, 574, DateTimeKind.Utc).AddTicks(2351));

            migrationBuilder.UpdateData(
                table: "CropTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 21, 12, 9, 19, 574, DateTimeKind.Utc).AddTicks(2370));

            migrationBuilder.UpdateData(
                table: "CropTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 21, 12, 9, 19, 574, DateTimeKind.Utc).AddTicks(2377));

            migrationBuilder.UpdateData(
                table: "CropTypes",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 21, 12, 9, 19, 574, DateTimeKind.Utc).AddTicks(2382));

            migrationBuilder.UpdateData(
                table: "CropTypes",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 21, 12, 9, 19, 574, DateTimeKind.Utc).AddTicks(2387));
        }
    }
}
