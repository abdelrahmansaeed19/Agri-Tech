using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgriculturalTech.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDeviceForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SensorDevices_Devices_MacAddress",
                table: "SensorDevices");

            migrationBuilder.AlterColumn<string>(
                name: "DeviceId",
                table: "SensorDevices",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SensorDevices_Devices_DeviceId",
                table: "SensorDevices");

            migrationBuilder.DropIndex(
                name: "IX_SensorDevices_DeviceId",
                table: "SensorDevices");

            migrationBuilder.AlterColumn<string>(
                name: "DeviceId",
                table: "SensorDevices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "CropTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 15, 9, 27, 10, 494, DateTimeKind.Utc).AddTicks(3603));

            migrationBuilder.UpdateData(
                table: "CropTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 15, 9, 27, 10, 494, DateTimeKind.Utc).AddTicks(3617));

            migrationBuilder.UpdateData(
                table: "CropTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 15, 9, 27, 10, 494, DateTimeKind.Utc).AddTicks(3661));

            migrationBuilder.UpdateData(
                table: "CropTypes",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 15, 9, 27, 10, 494, DateTimeKind.Utc).AddTicks(3665));

            migrationBuilder.UpdateData(
                table: "CropTypes",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 15, 9, 27, 10, 494, DateTimeKind.Utc).AddTicks(3669));

            migrationBuilder.AddForeignKey(
                name: "FK_SensorDevices_Devices_MacAddress",
                table: "SensorDevices",
                column: "MacAddress",
                principalTable: "Devices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
