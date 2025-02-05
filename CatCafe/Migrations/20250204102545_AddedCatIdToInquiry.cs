using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatCafe.Migrations
{
    /// <inheritdoc />
    public partial class AddedCatIdToInquiry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AdoptionInquiry_CatId",
                table: "AdoptionInquiry");

            migrationBuilder.CreateIndex(
                name: "IX_AdoptionInquiry_CatId",
                table: "AdoptionInquiry",
                column: "CatId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AdoptionInquiry_CatId",
                table: "AdoptionInquiry");

            migrationBuilder.CreateIndex(
                name: "IX_AdoptionInquiry_CatId",
                table: "AdoptionInquiry",
                column: "CatId");
        }
    }
}
