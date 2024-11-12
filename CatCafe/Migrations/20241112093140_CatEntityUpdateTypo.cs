using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatCafe.Migrations
{
    /// <inheritdoc />
    public partial class CatEntityUpdateTypo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateOfAcqusition",
                table: "Cats",
                newName: "DateOfAcquisition");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateOfAcquisition",
                table: "Cats",
                newName: "DateOfAcqusition");
        }
    }
}
