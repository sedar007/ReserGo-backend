using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReserGo.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBookingEventPricePerDay : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PricePerPerson",
                table: "EventOffer",
                newName: "PricePerDay");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PricePerDay",
                table: "EventOffer",
                newName: "PricePerPerson");
        }
    }
}
