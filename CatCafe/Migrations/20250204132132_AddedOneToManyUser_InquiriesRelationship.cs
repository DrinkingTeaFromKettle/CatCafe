using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatCafe.Migrations
{
    /// <inheritdoc />
    public partial class AddedOneToManyUser_InquiriesRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AdoptionInquiry_UserId",
                table: "AdoptionInquiry");

            migrationBuilder.CreateIndex(
                name: "IX_AdoptionInquiry_UserId",
                table: "AdoptionInquiry",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AdoptionInquiry_UserId",
                table: "AdoptionInquiry");

            migrationBuilder.CreateIndex(
                name: "IX_AdoptionInquiry_UserId",
                table: "AdoptionInquiry",
                column: "UserId",
                unique: true);
        }
    }
}
