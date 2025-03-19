using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReserGo.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class change_user_users : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookingHotel_User_UserId",
                table: "BookingHotel");

            migrationBuilder.DropForeignKey(
                name: "FK_BookingOccasion_User_UserId",
                table: "BookingOccasion");

            migrationBuilder.DropForeignKey(
                name: "FK_BookingRestaurant_User_UserId",
                table: "BookingRestaurant");

            migrationBuilder.DropForeignKey(
                name: "FK_Login_User_UserId",
                table: "Login");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "Users");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BookingHotel_Users_UserId",
                table: "BookingHotel",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookingOccasion_Users_UserId",
                table: "BookingOccasion",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookingRestaurant_Users_UserId",
                table: "BookingRestaurant",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Login_Users_UserId",
                table: "Login",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookingHotel_Users_UserId",
                table: "BookingHotel");

            migrationBuilder.DropForeignKey(
                name: "FK_BookingOccasion_Users_UserId",
                table: "BookingOccasion");

            migrationBuilder.DropForeignKey(
                name: "FK_BookingRestaurant_Users_UserId",
                table: "BookingRestaurant");

            migrationBuilder.DropForeignKey(
                name: "FK_Login_Users_UserId",
                table: "Login");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "User");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BookingHotel_User_UserId",
                table: "BookingHotel",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookingOccasion_User_UserId",
                table: "BookingOccasion",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookingRestaurant_User_UserId",
                table: "BookingRestaurant",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Login_User_UserId",
                table: "Login",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
