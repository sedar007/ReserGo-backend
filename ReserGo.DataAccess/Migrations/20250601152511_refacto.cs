using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReserGo.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class refacto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PricePerPerson",
                table: "BookingEvent");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "PricePerPerson",
                table: "BookingEvent",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
