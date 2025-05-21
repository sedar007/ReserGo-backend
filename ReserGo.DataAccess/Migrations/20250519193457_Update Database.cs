using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReserGo.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookingHotel_HotelOffer_HotelOfferId",
                table: "BookingHotel");

            migrationBuilder.DropForeignKey(
                name: "FK_BookingHotel_Hotel_HotelId",
                table: "BookingHotel");

            migrationBuilder.AlterColumn<DateTime>(
                name: "BookingDate",
                table: "BookingRestaurant",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<Guid>(
                name: "HotelOfferId",
                table: "BookingHotel",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "HotelId",
                table: "BookingHotel",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "BookingDate",
                table: "BookingHotel",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "BookingHotel",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "RoomId",
                table: "BookingHotel",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "BookingHotel",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_BookingHotel_HotelOffer_HotelOfferId",
                table: "BookingHotel",
                column: "HotelOfferId",
                principalTable: "HotelOffer",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BookingHotel_Hotel_HotelId",
                table: "BookingHotel",
                column: "HotelId",
                principalTable: "Hotel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "BookingHotel");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "BookingHotel");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "BookingHotel");

            migrationBuilder.AlterColumn<DateTime>(
                name: "BookingDate",
                table: "BookingRestaurant",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "HotelOfferId",
                table: "BookingHotel",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "HotelId",
                table: "BookingHotel",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<DateTime>(
                name: "BookingDate",
                table: "BookingHotel",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

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
    }
}
