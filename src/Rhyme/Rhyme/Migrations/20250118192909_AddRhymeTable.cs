using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rhyme.Migrations
{
    /// <inheritdoc />
    public partial class AddRhymeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rhymes",
                columns: table => new
                {
                    RhymeId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Word = table.Column<string>(type: "TEXT", nullable: false),
                    PlainSyllables = table.Column<string>(type: "TEXT", nullable: false),
                    PhonemeSyllables = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rhymes", x => x.RhymeId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rhymes");
        }
    }
}
