using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReserGo.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class updateHotelOffer1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HotelOffer_Hotel_HotelStayId",
                table: "HotelOffer");

            migrationBuilder.RenameColumn(
                name: "HotelStayId",
                table: "HotelOffer",
                newName: "HotelId");

            migrationBuilder.RenameIndex(
                name: "IX_HotelOffer_HotelStayId",
                table: "HotelOffer",
                newName: "IX_HotelOffer_HotelId");

            migrationBuilder.AddForeignKey(
                name: "FK_HotelOffer_Hotel_HotelId",
                table: "HotelOffer",
                column: "HotelId",
                principalTable: "Hotel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HotelOffer_Hotel_HotelId",
                table: "HotelOffer");

            migrationBuilder.RenameColumn(
                name: "HotelId",
                table: "HotelOffer",
                newName: "HotelStayId");

            migrationBuilder.RenameIndex(
                name: "IX_HotelOffer_HotelId",
                table: "HotelOffer",
                newName: "IX_HotelOffer_HotelStayId");

            migrationBuilder.AddForeignKey(
                name: "FK_HotelOffer_Hotel_HotelStayId",
                table: "HotelOffer",
                column: "HotelStayId",
                principalTable: "Hotel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
