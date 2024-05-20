using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenMovies.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movies_Categories_CategoryId",
                table: "Movies");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Movies_CategoryId",
                table: "Movies");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "Movies",
                newName: "Category");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Category",
                table: "Movies",
                newName: "CategoryId");

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Movies_CategoryId",
                table: "Movies",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_Categories_CategoryId",
                table: "Movies",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
