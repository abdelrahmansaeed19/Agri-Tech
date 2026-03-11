using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgriculturalTech.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class edit : Migration
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
                name: "Location",
                table: "SensorDevices");

            migrationBuilder.AlterColumn<string>(
                name: "DeviceId",
                table: "SensorDevices",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "MacAddress",
                table: "SensorDevices",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "UserSubscription",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StripeCustomerId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StripeSubscriptionId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubscriptionStatus = table.Column<int>(type: "int", nullable: false),
                    CurrentPeriodStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CurrentPeriodEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CancelAtPeriodEnd = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSubscription", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSubscription_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "CropTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 11, 8, 20, 12, 280, DateTimeKind.Utc).AddTicks(7050));

            migrationBuilder.UpdateData(
                table: "CropTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 11, 8, 20, 12, 280, DateTimeKind.Utc).AddTicks(7063));

            migrationBuilder.UpdateData(
                table: "CropTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 11, 8, 20, 12, 280, DateTimeKind.Utc).AddTicks(7068));

            migrationBuilder.UpdateData(
                table: "CropTypes",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 11, 8, 20, 12, 280, DateTimeKind.Utc).AddTicks(7072));

            migrationBuilder.UpdateData(
                table: "CropTypes",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 11, 8, 20, 12, 280, DateTimeKind.Utc).AddTicks(7076));

            migrationBuilder.CreateIndex(
                name: "IX_SensorDevices_MacAddress",
                table: "SensorDevices",
                column: "MacAddress",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserSubscription_UserId",
                table: "UserSubscription",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_SensorDevices_Devices_MacAddress",
                table: "SensorDevices",
                column: "MacAddress",
                principalTable: "Devices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SensorDevices_Devices_MacAddress",
                table: "SensorDevices");

            migrationBuilder.DropTable(
                name: "UserSubscription");

            migrationBuilder.DropIndex(
                name: "IX_SensorDevices_MacAddress",
                table: "SensorDevices");

            migrationBuilder.DropColumn(
                name: "MacAddress",
                table: "SensorDevices");

            migrationBuilder.AlterColumn<string>(
                name: "DeviceId",
                table: "SensorDevices",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "SensorDevices",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "CropTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 3, 11, 6, 29, 974, DateTimeKind.Utc).AddTicks(9238));

            migrationBuilder.UpdateData(
                table: "CropTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 3, 11, 6, 29, 974, DateTimeKind.Utc).AddTicks(9252));

            migrationBuilder.UpdateData(
                table: "CropTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 3, 11, 6, 29, 974, DateTimeKind.Utc).AddTicks(9257));

            migrationBuilder.UpdateData(
                table: "CropTypes",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 3, 11, 6, 29, 974, DateTimeKind.Utc).AddTicks(9261));

            migrationBuilder.UpdateData(
                table: "CropTypes",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 3, 11, 6, 29, 974, DateTimeKind.Utc).AddTicks(9264));

            migrationBuilder.CreateIndex(
                name: "IX_SensorDevices_DeviceId",
                table: "SensorDevices",
                column: "DeviceId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SensorDevices_Devices_DeviceId",
                table: "SensorDevices",
                column: "DeviceId",
                principalTable: "Devices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
