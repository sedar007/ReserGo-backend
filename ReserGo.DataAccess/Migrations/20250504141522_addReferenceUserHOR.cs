using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReserGo.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addReferenceUserHOR : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Restaurant",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Occasion",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Hotel",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Restaurant_UserId",
                table: "Restaurant",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Occasion_UserId",
                table: "Occasion",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Hotel_UserId",
                table: "Hotel",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Hotel_Users_UserId",
                table: "Hotel",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Occasion_Users_UserId",
                table: "Occasion",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurant_Users_UserId",
                table: "Restaurant",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hotel_Users_UserId",
                table: "Hotel");

            migrationBuilder.DropForeignKey(
                name: "FK_Occasion_Users_UserId",
                table: "Occasion");

            migrationBuilder.DropForeignKey(
                name: "FK_Restaurant_Users_UserId",
                table: "Restaurant");

            migrationBuilder.DropIndex(
                name: "IX_Restaurant_UserId",
                table: "Restaurant");

            migrationBuilder.DropIndex(
                name: "IX_Occasion_UserId",
                table: "Occasion");

            migrationBuilder.DropIndex(
                name: "IX_Hotel_UserId",
                table: "Hotel");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Restaurant");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Occasion");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Hotel");
        }
    }
}
