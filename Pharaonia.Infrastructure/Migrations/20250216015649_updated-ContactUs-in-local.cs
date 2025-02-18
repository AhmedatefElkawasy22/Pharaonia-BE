using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pharaonia.Migrations
{
    /// <inheritdoc />
    public partial class updatedContactUsinlocal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsContacted",
                table: "ContactUS",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsContacted",
                table: "ContactUS");
        }
    }
}
