using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReserGo.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class updatedatabaseeventoffer3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookingRestaurant_Restaurant_RestaurantId",
                table: "BookingRestaurant");

            migrationBuilder.AlterColumn<Guid>(
                name: "RestaurantId",
                table: "BookingRestaurant",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "BookingRestaurant",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "BookingRestaurant",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_BookingRestaurant_Restaurant_RestaurantId",
                table: "BookingRestaurant",
                column: "RestaurantId",
                principalTable: "Restaurant",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookingRestaurant_Restaurant_RestaurantId",
                table: "BookingRestaurant");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "BookingRestaurant");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "BookingRestaurant");

            migrationBuilder.AlterColumn<Guid>(
                name: "RestaurantId",
                table: "BookingRestaurant",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_BookingRestaurant_Restaurant_RestaurantId",
                table: "BookingRestaurant",
                column: "RestaurantId",
                principalTable: "Restaurant",
                principalColumn: "Id");
        }
    }
}
