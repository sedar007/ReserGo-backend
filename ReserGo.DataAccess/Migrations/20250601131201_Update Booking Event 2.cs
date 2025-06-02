using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReserGo.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBookingEvent2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "BookingEvent");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "BookingEvent",
                newName: "Date");
        }
    }
}
