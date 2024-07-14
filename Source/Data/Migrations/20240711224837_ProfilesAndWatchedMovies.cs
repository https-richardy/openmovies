using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenMovies.WebApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class ProfilesAndWatchedMovies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookmarkedMovies_AspNetUsers_UserId",
                table: "BookmarkedMovies");

            migrationBuilder.DropIndex(
                name: "IX_BookmarkedMovies_UserId",
                table: "BookmarkedMovies");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "BookmarkedMovies");

            migrationBuilder.AddColumn<int>(
                name: "ProfileId",
                table: "BookmarkedMovies",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Profiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Avatar = table.Column<string>(type: "TEXT", nullable: false),
                    IsChild = table.Column<bool>(type: "INTEGER", nullable: false),
                    AccountId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Profiles_AspNetUsers_AccountId",
                        column: x => x.AccountId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WatchedMovies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MovieId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProfileId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WatchedMovies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WatchedMovies_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WatchedMovies_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookmarkedMovies_ProfileId",
                table: "BookmarkedMovies",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_AccountId",
                table: "Profiles",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_WatchedMovies_MovieId",
                table: "WatchedMovies",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_WatchedMovies_ProfileId",
                table: "WatchedMovies",
                column: "ProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookmarkedMovies_Profiles_ProfileId",
                table: "BookmarkedMovies",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookmarkedMovies_Profiles_ProfileId",
                table: "BookmarkedMovies");

            migrationBuilder.DropTable(
                name: "WatchedMovies");

            migrationBuilder.DropTable(
                name: "Profiles");

            migrationBuilder.DropIndex(
                name: "IX_BookmarkedMovies_ProfileId",
                table: "BookmarkedMovies");

            migrationBuilder.DropColumn(
                name: "ProfileId",
                table: "BookmarkedMovies");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "BookmarkedMovies",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookmarkedMovies_UserId",
                table: "BookmarkedMovies",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookmarkedMovies_AspNetUsers_UserId",
                table: "BookmarkedMovies",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
