using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReserGo.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addStayId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "StayId",
                table: "Restaurant",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "StayId",
                table: "Occasion",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "StayId",
                table: "Hotel",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Restaurant_StayId",
                table: "Restaurant",
                column: "StayId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Occasion_StayId",
                table: "Occasion",
                column: "StayId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Hotel_StayId",
                table: "Hotel",
                column: "StayId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Restaurant_StayId",
                table: "Restaurant");

            migrationBuilder.DropIndex(
                name: "IX_Occasion_StayId",
                table: "Occasion");

            migrationBuilder.DropIndex(
                name: "IX_Hotel_StayId",
                table: "Hotel");

            migrationBuilder.DropColumn(
                name: "StayId",
                table: "Restaurant");

            migrationBuilder.DropColumn(
                name: "StayId",
                table: "Occasion");

            migrationBuilder.DropColumn(
                name: "StayId",
                table: "Hotel");
        }
    }
}
