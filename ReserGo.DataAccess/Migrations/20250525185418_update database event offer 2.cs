using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReserGo.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class updatedatabaseeventoffer2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price",
                table: "OccasionOffer",
                newName: "PricePerPerson");

            migrationBuilder.RenameColumn(
                name: "NumberOfGuests",
                table: "OccasionOffer",
                newName: "GuestLimit");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "OccasionOffer",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PricePerPerson",
                table: "OccasionOffer",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "GuestLimit",
                table: "OccasionOffer",
                newName: "NumberOfGuests");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "OccasionOffer",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
