using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rhyme.Migrations
{
    /// <inheritdoc />
    public partial class AddPlainSyllablesModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WordsWithPlainSyllables",
                columns: table => new
                {
                    WordWithPlainSyllablesId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Word = table.Column<string>(type: "TEXT", nullable: false),
                    Syllables = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WordsWithPlainSyllables", x => x.WordWithPlainSyllablesId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WordsWithPlainSyllables");
        }
    }
}
