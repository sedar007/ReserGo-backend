using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReserGo.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class updateBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookingHotel_Hotel_HotelId",
                table: "BookingHotel");

            migrationBuilder.DropColumn(
                name: "CheckIn",
                table: "BookingHotel");

            migrationBuilder.DropColumn(
                name: "LastUpdated",
                table: "BookingHotel");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "BookingHotel");

            migrationBuilder.RenameColumn(
                name: "CheckOut",
                table: "BookingHotel",
                newName: "CreatedAt");

            migrationBuilder.AlterColumn<Guid>(
                name: "HotelId",
                table: "BookingHotel",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "HotelOfferId",
                table: "BookingHotel",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "IsConfirmed",
                table: "BookingHotel",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_BookingHotel_HotelOfferId",
                table: "BookingHotel",
                column: "HotelOfferId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookingHotel_HotelOffer_HotelOfferId",
                table: "BookingHotel",
                column: "HotelOfferId",
                principalTable: "HotelOffer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookingHotel_Hotel_HotelId",
                table: "BookingHotel",
                column: "HotelId",
                principalTable: "Hotel",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookingHotel_HotelOffer_HotelOfferId",
                table: "BookingHotel");

            migrationBuilder.DropForeignKey(
                name: "FK_BookingHotel_Hotel_HotelId",
                table: "BookingHotel");

            migrationBuilder.DropIndex(
                name: "IX_BookingHotel_HotelOfferId",
                table: "BookingHotel");

            migrationBuilder.DropColumn(
                name: "HotelOfferId",
                table: "BookingHotel");

            migrationBuilder.DropColumn(
                name: "IsConfirmed",
                table: "BookingHotel");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "BookingHotel",
                newName: "CheckOut");

            migrationBuilder.AlterColumn<Guid>(
                name: "HotelId",
                table: "BookingHotel",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CheckIn",
                table: "BookingHotel",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdated",
                table: "BookingHotel",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "BookingHotel",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_BookingHotel_Hotel_HotelId",
                table: "BookingHotel",
                column: "HotelId",
                principalTable: "Hotel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
