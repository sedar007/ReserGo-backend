using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReserGo.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddBookingsToRoomAvailabilit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RoomAvailabilityId",
                table: "BookingHotel",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_BookingHotel_RoomAvailabilityId",
                table: "BookingHotel",
                column: "RoomAvailabilityId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookingHotel_RoomAvailability_RoomAvailabilityId",
                table: "BookingHotel",
                column: "RoomAvailabilityId",
                principalTable: "RoomAvailability",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookingHotel_RoomAvailability_RoomAvailabilityId",
                table: "BookingHotel");

            migrationBuilder.DropIndex(
                name: "IX_BookingHotel_RoomAvailabilityId",
                table: "BookingHotel");

            migrationBuilder.DropColumn(
                name: "RoomAvailabilityId",
                table: "BookingHotel");
        }
    }
}
