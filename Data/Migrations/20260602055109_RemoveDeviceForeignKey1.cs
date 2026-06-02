using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgriculturalTech.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDeviceForeignKey1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SensorDevices_Devices_DeviceId",
                table: "SensorDevices");

            migrationBuilder.DropIndex(
                name: "IX_SensorDevices_DeviceId",
                table: "SensorDevices");

            migrationBuilder.DropColumn(
                name: "DeviceId",
                table: "SensorDevices");

            migrationBuilder.UpdateData(
                table: "CropTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 2, 5, 51, 8, 669, DateTimeKind.Utc).AddTicks(7322));

            migrationBuilder.UpdateData(
                table: "CropTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 2, 5, 51, 8, 669, DateTimeKind.Utc).AddTicks(7349));

            migrationBuilder.UpdateData(
                table: "CropTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 2, 5, 51, 8, 669, DateTimeKind.Utc).AddTicks(7354));

            migrationBuilder.UpdateData(
                table: "CropTypes",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 2, 5, 51, 8, 669, DateTimeKind.Utc).AddTicks(7358));

            migrationBuilder.UpdateData(
                table: "CropTypes",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 2, 5, 51, 8, 669, DateTimeKind.Utc).AddTicks(7362));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DeviceId",
                table: "SensorDevices",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "CropTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 2, 5, 39, 28, 414, DateTimeKind.Utc).AddTicks(2509));

            migrationBuilder.UpdateData(
                table: "CropTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 2, 5, 39, 28, 414, DateTimeKind.Utc).AddTicks(2525));

            migrationBuilder.UpdateData(
                table: "CropTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 2, 5, 39, 28, 414, DateTimeKind.Utc).AddTicks(2530));

            migrationBuilder.UpdateData(
                table: "CropTypes",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 2, 5, 39, 28, 414, DateTimeKind.Utc).AddTicks(2533));

            migrationBuilder.UpdateData(
                table: "CropTypes",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 2, 5, 39, 28, 414, DateTimeKind.Utc).AddTicks(2537));

            migrationBuilder.CreateIndex(
                name: "IX_SensorDevices_DeviceId",
                table: "SensorDevices",
                column: "DeviceId");

            migrationBuilder.AddForeignKey(
                name: "FK_SensorDevices_Devices_DeviceId",
                table: "SensorDevices",
                column: "DeviceId",
                principalTable: "Devices",
                principalColumn: "Id");
        }
    }
}
