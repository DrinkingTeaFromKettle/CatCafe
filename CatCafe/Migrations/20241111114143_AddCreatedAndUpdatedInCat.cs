using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatCafe.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatedAndUpdatedInCat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Cats",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getutcdate()");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfArrival",
                table: "Cats",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdated",
                table: "Cats",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Cats");

            migrationBuilder.DropColumn(
                name: "DateOfArrival",
                table: "Cats");

            migrationBuilder.DropColumn(
                name: "LastUpdated",
                table: "Cats");
        }
    }
}
