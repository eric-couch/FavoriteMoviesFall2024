﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FavoriteMoviesFall2024.Server.Migrations
{
    /// <inheritdoc />
    public partial class ReAddedRatingWithIdentityField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ratings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Source = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OMDBMovieId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ratings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ratings_OMDBMovies_OMDBMovieId",
                        column: x => x.OMDBMovieId,
                        principalTable: "OMDBMovies",
                        principalColumn: "imdbID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_OMDBMovieId",
                table: "Ratings",
                column: "OMDBMovieId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ratings");
        }
    }
}
