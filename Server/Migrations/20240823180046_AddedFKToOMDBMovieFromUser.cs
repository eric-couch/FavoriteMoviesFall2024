using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FavoriteMoviesFall2024.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddedFKToOMDBMovieFromUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "OMDBMovies",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_OMDBMovies_ApplicationUserId",
                table: "OMDBMovies",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_OMDBMovies_AspNetUsers_ApplicationUserId",
                table: "OMDBMovies",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OMDBMovies_AspNetUsers_ApplicationUserId",
                table: "OMDBMovies");

            migrationBuilder.DropIndex(
                name: "IX_OMDBMovies_ApplicationUserId",
                table: "OMDBMovies");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "OMDBMovies",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
