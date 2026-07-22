using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace englishCardsAPI.Migrations
{
    /// <inheritdoc />
    public partial class PhoneticsTypoFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Phoneitcs",
                table: "Cards",
                newName: "Phonetics");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Phonetics",
                table: "Cards",
                newName: "Phoneitcs");
        }
    }
}
