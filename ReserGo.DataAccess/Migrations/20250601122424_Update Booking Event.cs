using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReserGo.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBookingEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "BookingEvent");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "BookingEvent",
                newName: "Date");

            migrationBuilder.AddColumn<int>(
                name: "GuestNumber",
                table: "EventOffer",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "EventOfferId",
                table: "BookingEvent",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_BookingEvent_EventOfferId",
                table: "BookingEvent",
                column: "EventOfferId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookingEvent_EventOffer_EventOfferId",
                table: "BookingEvent",
                column: "EventOfferId",
                principalTable: "EventOffer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookingEvent_EventOffer_EventOfferId",
                table: "BookingEvent");

            migrationBuilder.DropIndex(
                name: "IX_BookingEvent_EventOfferId",
                table: "BookingEvent");

            migrationBuilder.DropColumn(
                name: "GuestNumber",
                table: "EventOffer");

            migrationBuilder.DropColumn(
                name: "EventOfferId",
                table: "BookingEvent");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "BookingEvent",
                newName: "StartDate");

            migrationBuilder.AddColumn<DateOnly>(
                name: "EndDate",
                table: "BookingEvent",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));
        }
    }
}
