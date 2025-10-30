using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgriculturalTech.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class allowNullToActionLink : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ActionLink",
                table: "Notifications",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.UpdateData(
                table: "CropTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 19, 0, 38, 413, DateTimeKind.Utc).AddTicks(6221));

            migrationBuilder.UpdateData(
                table: "CropTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 19, 0, 38, 413, DateTimeKind.Utc).AddTicks(6239));

            migrationBuilder.UpdateData(
                table: "CropTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 19, 0, 38, 413, DateTimeKind.Utc).AddTicks(6244));

            migrationBuilder.UpdateData(
                table: "CropTypes",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 19, 0, 38, 413, DateTimeKind.Utc).AddTicks(6249));

            migrationBuilder.UpdateData(
                table: "CropTypes",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 19, 0, 38, 413, DateTimeKind.Utc).AddTicks(6253));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ActionLink",
                table: "Notifications",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

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
        }
    }
}
