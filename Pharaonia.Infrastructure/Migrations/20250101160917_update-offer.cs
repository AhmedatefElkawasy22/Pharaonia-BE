using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pharaonia.Migrations
{
    /// <inheritdoc />
    public partial class updateoffer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TypeOfTime",
                table: "offers",
                newName: "TypeOfOfferExpirationDate");

            migrationBuilder.RenameColumn(
                name: "NumberOfTime",
                table: "offers",
                newName: "TypeOfOfferDuration");

            migrationBuilder.AddColumn<int>(
                name: "OfferDurationNumber",
                table: "offers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OfferExpirationNumber",
                table: "offers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OfferDurationNumber",
                table: "offers");

            migrationBuilder.DropColumn(
                name: "OfferExpirationNumber",
                table: "offers");

            migrationBuilder.RenameColumn(
                name: "TypeOfOfferExpirationDate",
                table: "offers",
                newName: "TypeOfTime");

            migrationBuilder.RenameColumn(
                name: "TypeOfOfferDuration",
                table: "offers",
                newName: "NumberOfTime");
        }
    }
}
