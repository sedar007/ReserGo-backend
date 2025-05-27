using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReserGo.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class updatedatabaseeventoffer4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "BookingOccasion");

            migrationBuilder.RenameColumn(
                name: "VipAccess",
                table: "BookingOccasion",
                newName: "IsConfirmed");

            migrationBuilder.RenameColumn(
                name: "LastUpdated",
                table: "BookingOccasion",
                newName: "StartDate");

            migrationBuilder.AlterColumn<DateTime>(
                name: "BookingDate",
                table: "BookingOccasion",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "BookingOccasion",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "BookingOccasion",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "NumberOfGuests",
                table: "BookingOccasion",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "BookingOccasion",
                type: "double precision",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "BookingOccasion");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "BookingOccasion");

            migrationBuilder.DropColumn(
                name: "NumberOfGuests",
                table: "BookingOccasion");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "BookingOccasion");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "BookingOccasion",
                newName: "LastUpdated");

            migrationBuilder.RenameColumn(
                name: "IsConfirmed",
                table: "BookingOccasion",
                newName: "VipAccess");

            migrationBuilder.AlterColumn<DateTime>(
                name: "BookingDate",
                table: "BookingOccasion",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "BookingOccasion",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
