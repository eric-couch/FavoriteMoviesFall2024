using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FavoriteMoviesFall2024.Server.Migrations
{
    /// <inheritdoc />
    public partial class RemovedFKRelationshipBetweenUserAndOMDBMovie : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OMDBMovies_AspNetUsers_ApplicationUserId",
                table: "OMDBMovies");

            migrationBuilder.DropIndex(
                name: "IX_OMDBMovies_ApplicationUserId",
                table: "OMDBMovies");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "OMDBMovies");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "OMDBMovies",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

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
    }
}
