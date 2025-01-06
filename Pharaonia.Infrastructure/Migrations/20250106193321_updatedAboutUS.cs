using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pharaonia.Migrations
{
    /// <inheritdoc />
    public partial class updatedAboutUS : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "aboutUs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Text",
                table: "aboutUs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.DropColumn(
                name: "Aboutus",
                table: "aboutUs");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "aboutUs");

            migrationBuilder.DropColumn(
                name: "Text",
                table: "aboutUs");

            migrationBuilder.AddColumn<string>(
                name: "Aboutus",
                table: "aboutUs",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");
        }
    }
}
