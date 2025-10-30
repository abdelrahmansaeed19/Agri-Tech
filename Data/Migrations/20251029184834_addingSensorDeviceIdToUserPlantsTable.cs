using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgriculturalTech.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class addingSensorDeviceIdToUserPlantsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SensorDeviceId",
                table: "UserPlants",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "CropTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 18, 48, 33, 994, DateTimeKind.Utc).AddTicks(2585));

            migrationBuilder.UpdateData(
                table: "CropTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 18, 48, 33, 994, DateTimeKind.Utc).AddTicks(2623));

            migrationBuilder.UpdateData(
                table: "CropTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 18, 48, 33, 994, DateTimeKind.Utc).AddTicks(2630));

            migrationBuilder.UpdateData(
                table: "CropTypes",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 18, 48, 33, 994, DateTimeKind.Utc).AddTicks(2637));

            migrationBuilder.UpdateData(
                table: "CropTypes",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 18, 48, 33, 994, DateTimeKind.Utc).AddTicks(2644));

            migrationBuilder.CreateIndex(
                name: "IX_UserPlants_SensorDeviceId",
                table: "UserPlants",
                column: "SensorDeviceId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPlants_SensorDevices_SensorDeviceId",
                table: "UserPlants",
                column: "SensorDeviceId",
                principalTable: "SensorDevices",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPlants_SensorDevices_SensorDeviceId",
                table: "UserPlants");

            migrationBuilder.DropIndex(
                name: "IX_UserPlants_SensorDeviceId",
                table: "UserPlants");

            migrationBuilder.DropColumn(
                name: "SensorDeviceId",
                table: "UserPlants");

            migrationBuilder.UpdateData(
                table: "CropTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 18, 5, 51, 298, DateTimeKind.Utc).AddTicks(7940));

            migrationBuilder.UpdateData(
                table: "CropTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 18, 5, 51, 298, DateTimeKind.Utc).AddTicks(7957));

            migrationBuilder.UpdateData(
                table: "CropTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 18, 5, 51, 298, DateTimeKind.Utc).AddTicks(7963));

            migrationBuilder.UpdateData(
                table: "CropTypes",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 18, 5, 51, 298, DateTimeKind.Utc).AddTicks(7969));

            migrationBuilder.UpdateData(
                table: "CropTypes",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 18, 5, 51, 298, DateTimeKind.Utc).AddTicks(7974));
        }
    }
}
