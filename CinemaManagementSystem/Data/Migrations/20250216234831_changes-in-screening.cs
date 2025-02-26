using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaManagementSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class changesinscreening : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ScreeningId",
                table: "Halls",
                type: "int",
                nullable: true);

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
                name: "IX_Halls_ScreeningId",
                table: "Halls",
                column: "ScreeningId");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Halls_Screenings_ScreeningId",
                table: "Halls");

            migrationBuilder.DropTable(
                name: "MovieScreening");

            migrationBuilder.DropIndex(
                name: "IX_Halls_ScreeningId",
                table: "Halls");

            migrationBuilder.DropColumn(
                name: "ScreeningId",
                table: "Halls");
        }
    }
}
