using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReserGo.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDatabase1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_BookingHotel_RoomId",
                table: "BookingHotel",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookingHotel_Room_RoomId",
                table: "BookingHotel",
                column: "RoomId",
                principalTable: "Room",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookingHotel_Room_RoomId",
                table: "BookingHotel");

            migrationBuilder.DropIndex(
                name: "IX_BookingHotel_RoomId",
                table: "BookingHotel");
        }
    }
}
