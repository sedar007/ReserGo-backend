using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReserGo.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBooking3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "BookingRestaurant");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "BookingHotel");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "BookingEvent");

            migrationBuilder.AddColumn<double>(
                name: "PricePerPerson",
                table: "BookingRestaurant",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PriceTotal",
                table: "BookingRestaurant",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PricePerPerson",
                table: "BookingHotel",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PriceTotal",
                table: "BookingHotel",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PricePerPerson",
                table: "BookingEvent",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PriceTotal",
                table: "BookingEvent",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PricePerPerson",
                table: "BookingRestaurant");

            migrationBuilder.DropColumn(
                name: "PriceTotal",
                table: "BookingRestaurant");

            migrationBuilder.DropColumn(
                name: "PricePerPerson",
                table: "BookingHotel");

            migrationBuilder.DropColumn(
                name: "PriceTotal",
                table: "BookingHotel");

            migrationBuilder.DropColumn(
                name: "PricePerPerson",
                table: "BookingEvent");

            migrationBuilder.DropColumn(
                name: "PriceTotal",
                table: "BookingEvent");

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "BookingRestaurant",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "BookingHotel",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "BookingEvent",
                type: "double precision",
                nullable: true);
        }
    }
}
