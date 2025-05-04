using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReserGo.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class updateToDateTime1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdated",
                table: "Restaurant",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Picture",
                table: "Restaurant",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdated",
                table: "Occasion",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Picture",
                table: "Occasion",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdated",
                table: "Hotel",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Picture",
                table: "Hotel",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdated",
                table: "BookingRestaurant",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdated",
                table: "BookingOccasion",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdated",
                table: "BookingHotel",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastUpdated",
                table: "Restaurant");

            migrationBuilder.DropColumn(
                name: "Picture",
                table: "Restaurant");

            migrationBuilder.DropColumn(
                name: "LastUpdated",
                table: "Occasion");

            migrationBuilder.DropColumn(
                name: "Picture",
                table: "Occasion");

            migrationBuilder.DropColumn(
                name: "LastUpdated",
                table: "Hotel");

            migrationBuilder.DropColumn(
                name: "Picture",
                table: "Hotel");

            migrationBuilder.DropColumn(
                name: "LastUpdated",
                table: "BookingRestaurant");

            migrationBuilder.DropColumn(
                name: "LastUpdated",
                table: "BookingOccasion");

            migrationBuilder.DropColumn(
                name: "LastUpdated",
                table: "BookingHotel");
        }
    }
}
