using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaManagementSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class screeningchanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Halls_Screenings_ScreeningId",
                table: "Halls");

            migrationBuilder.DropTable(
                name: "MovieScreening");

            migrationBuilder.RenameColumn(
                name: "ScreeningId",
                table: "Halls",
                newName: "MovieId");

            migrationBuilder.RenameIndex(
                name: "IX_Halls_ScreeningId",
                table: "Halls",
                newName: "IX_Halls_MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_Screenings_HallId",
                table: "Screenings",
                column: "HallId");

            migrationBuilder.CreateIndex(
                name: "IX_Screenings_MovieId",
                table: "Screenings",
                column: "MovieId");

            migrationBuilder.AddForeignKey(
                name: "FK_Halls_Movies_MovieId",
                table: "Halls",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "MovieId");

            migrationBuilder.AddForeignKey(
                name: "FK_Screenings_Halls_HallId",
                table: "Screenings",
                column: "HallId",
                principalTable: "Halls",
                principalColumn: "HallId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Screenings_Movies_MovieId",
                table: "Screenings",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "MovieId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Halls_Movies_MovieId",
                table: "Halls");

            migrationBuilder.DropForeignKey(
                name: "FK_Screenings_Halls_HallId",
                table: "Screenings");

            migrationBuilder.DropForeignKey(
                name: "FK_Screenings_Movies_MovieId",
                table: "Screenings");

            migrationBuilder.DropIndex(
                name: "IX_Screenings_HallId",
                table: "Screenings");

            migrationBuilder.DropIndex(
                name: "IX_Screenings_MovieId",
                table: "Screenings");

            migrationBuilder.RenameColumn(
                name: "MovieId",
                table: "Halls",
                newName: "ScreeningId");

            migrationBuilder.RenameIndex(
                name: "IX_Halls_MovieId",
                table: "Halls",
                newName: "IX_Halls_ScreeningId");

            migrationBuilder.CreateTable(
                name: "MovieScreening",
                columns: table => new
                {
                    MoviesMovieId = table.Column<int>(type: "int", nullable: false),
                    ScreeningsScreeningId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieScreening", x => new { x.MoviesMovieId, x.ScreeningsScreeningId });
                    table.ForeignKey(
                        name: "FK_MovieScreening_Movies_MoviesMovieId",
                        column: x => x.MoviesMovieId,
                        principalTable: "Movies",
                        principalColumn: "MovieId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovieScreening_Screenings_ScreeningsScreeningId",
                        column: x => x.ScreeningsScreeningId,
                        principalTable: "Screenings",
                        principalColumn: "ScreeningId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MovieScreening_ScreeningsScreeningId",
                table: "MovieScreening",
                column: "ScreeningsScreeningId");

            migrationBuilder.AddForeignKey(
                name: "FK_Halls_Screenings_ScreeningId",
                table: "Halls",
                column: "ScreeningId",
                principalTable: "Screenings",
                principalColumn: "ScreeningId");
        }
    }
}
