using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgriculturalTech.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class ReadingsDataTypeChangeToDecimal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "IdealTemperatureMin",
                table: "CropTypes",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "IdealTemperatureMax",
                table: "CropTypes",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "IdealPhMin",
                table: "CropTypes",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "IdealPhMax",
                table: "CropTypes",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "IdealHumidityMin",
                table: "CropTypes",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "IdealHumidityMax",
                table: "CropTypes",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "CropTypes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "IdealHumidityMax", "IdealHumidityMin", "IdealPhMax", "IdealPhMin", "IdealTemperatureMax", "IdealTemperatureMin" },
                values: new object[] { new DateTime(2025, 10, 21, 12, 9, 19, 574, DateTimeKind.Utc).AddTicks(2351), 80m, 60m, 6.8m, 6.0m, 27m, 18m });

            migrationBuilder.UpdateData(
                table: "CropTypes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "IdealHumidityMax", "IdealHumidityMin", "IdealPhMax", "IdealPhMin", "IdealTemperatureMax", "IdealTemperatureMin" },
                values: new object[] { new DateTime(2025, 10, 21, 12, 9, 19, 574, DateTimeKind.Utc).AddTicks(2370), 70m, 50m, 7.5m, 6.0m, 25m, 15m });

            migrationBuilder.UpdateData(
                table: "CropTypes",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "IdealHumidityMax", "IdealHumidityMin", "IdealPhMax", "IdealPhMin", "IdealTemperatureMax", "IdealTemperatureMin" },
                values: new object[] { new DateTime(2025, 10, 21, 12, 9, 19, 574, DateTimeKind.Utc).AddTicks(2377), 80m, 60m, 7.0m, 5.8m, 30m, 20m });

            migrationBuilder.UpdateData(
                table: "CropTypes",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "IdealHumidityMax", "IdealHumidityMin", "IdealPhMax", "IdealPhMin", "IdealTemperatureMax", "IdealTemperatureMin" },
                values: new object[] { new DateTime(2025, 10, 21, 12, 9, 19, 574, DateTimeKind.Utc).AddTicks(2382), 80m, 70m, 6.5m, 5.0m, 20m, 15m });

            migrationBuilder.UpdateData(
                table: "CropTypes",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "IdealHumidityMax", "IdealHumidityMin", "IdealPhMax", "IdealPhMin", "IdealTemperatureMax", "IdealTemperatureMin" },
                values: new object[] { new DateTime(2025, 10, 21, 12, 9, 19, 574, DateTimeKind.Utc).AddTicks(2387), 90m, 70m, 6.5m, 5.5m, 35m, 20m });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "IdealTemperatureMin",
                table: "CropTypes",
                type: "float",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)",
                oldPrecision: 5,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "IdealTemperatureMax",
                table: "CropTypes",
                type: "float",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)",
                oldPrecision: 5,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "IdealPhMin",
                table: "CropTypes",
                type: "float",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)",
                oldPrecision: 5,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "IdealPhMax",
                table: "CropTypes",
                type: "float",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)",
                oldPrecision: 5,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "IdealHumidityMin",
                table: "CropTypes",
                type: "float",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)",
                oldPrecision: 5,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "IdealHumidityMax",
                table: "CropTypes",
                type: "float",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)",
                oldPrecision: 5,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "CropTypes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "IdealHumidityMax", "IdealHumidityMin", "IdealPhMax", "IdealPhMin", "IdealTemperatureMax", "IdealTemperatureMin" },
                values: new object[] { new DateTime(2025, 10, 20, 21, 29, 45, 558, DateTimeKind.Utc).AddTicks(7517), 80.0, 60.0, 6.7999999999999998, 6.0, 27.0, 18.0 });

            migrationBuilder.UpdateData(
                table: "CropTypes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "IdealHumidityMax", "IdealHumidityMin", "IdealPhMax", "IdealPhMin", "IdealTemperatureMax", "IdealTemperatureMin" },
                values: new object[] { new DateTime(2025, 10, 20, 21, 29, 45, 558, DateTimeKind.Utc).AddTicks(7534), 70.0, 50.0, 7.5, 6.0, 25.0, 15.0 });

            migrationBuilder.UpdateData(
                table: "CropTypes",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "IdealHumidityMax", "IdealHumidityMin", "IdealPhMax", "IdealPhMin", "IdealTemperatureMax", "IdealTemperatureMin" },
                values: new object[] { new DateTime(2025, 10, 20, 21, 29, 45, 558, DateTimeKind.Utc).AddTicks(7539), 80.0, 60.0, 7.0, 5.7999999999999998, 30.0, 20.0 });

            migrationBuilder.UpdateData(
                table: "CropTypes",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "IdealHumidityMax", "IdealHumidityMin", "IdealPhMax", "IdealPhMin", "IdealTemperatureMax", "IdealTemperatureMin" },
                values: new object[] { new DateTime(2025, 10, 20, 21, 29, 45, 558, DateTimeKind.Utc).AddTicks(7543), 80.0, 70.0, 6.5, 5.0, 20.0, 15.0 });

            migrationBuilder.UpdateData(
                table: "CropTypes",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "IdealHumidityMax", "IdealHumidityMin", "IdealPhMax", "IdealPhMin", "IdealTemperatureMax", "IdealTemperatureMin" },
                values: new object[] { new DateTime(2025, 10, 20, 21, 29, 45, 558, DateTimeKind.Utc).AddTicks(7547), 90.0, 70.0, 6.5, 5.5, 35.0, 20.0 });
        }
    }
}
