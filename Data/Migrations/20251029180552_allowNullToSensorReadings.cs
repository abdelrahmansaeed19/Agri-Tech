using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgriculturalTech.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class allowNullToSensorReadings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "SensorReadings",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "SensorReadings",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

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
        }
    }
}
