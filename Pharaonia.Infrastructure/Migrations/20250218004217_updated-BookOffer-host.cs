using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pharaonia.Migrations
{
    /// <inheritdoc />
    public partial class updatedBookOfferhost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Requirements",
                table: "BookOffer",
                type: "nvarchar(3000)",
                maxLength: 3000,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(3000)",
                oldMaxLength: 3000,
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsContacted",
                table: "BookOffer",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsContacted",
                table: "BookOffer");

            migrationBuilder.AlterColumn<string>(
                name: "Requirements",
                table: "BookOffer",
                type: "nvarchar(3000)",
                maxLength: 3000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(3000)",
                oldMaxLength: 3000);
        }
    }
}
