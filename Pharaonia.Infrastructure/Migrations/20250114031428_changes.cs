using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pharaonia.Migrations
{
    /// <inheritdoc />
    public partial class changes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_destinationIamges_destinations_DestinationId",
                table: "destinationIamges");

            migrationBuilder.DropForeignKey(
                name: "FK_getOffer_offers_OfferId",
                table: "getOffer");

            migrationBuilder.DropForeignKey(
                name: "FK_OfferImages_offers_OfferId",
                table: "OfferImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_offers",
                table: "offers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_gallery",
                table: "gallery");

            migrationBuilder.DropPrimaryKey(
                name: "PK_destinations",
                table: "destinations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_destinationIamges",
                table: "destinationIamges");

            migrationBuilder.DropPrimaryKey(
                name: "PK_contactUS",
                table: "contactUS");

            migrationBuilder.DropPrimaryKey(
                name: "PK_aboutUs",
                table: "aboutUs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_getOffer",
                table: "getOffer");

            migrationBuilder.RenameTable(
                name: "offers",
                newName: "Offers");

            migrationBuilder.RenameTable(
                name: "gallery",
                newName: "Gallery");

            migrationBuilder.RenameTable(
                name: "destinations",
                newName: "Destinations");

            migrationBuilder.RenameTable(
                name: "destinationIamges",
                newName: "DestinationIamges");

            migrationBuilder.RenameTable(
                name: "contactUS",
                newName: "ContactUS");

            migrationBuilder.RenameTable(
                name: "aboutUs",
                newName: "AboutUs");

            migrationBuilder.RenameTable(
                name: "getOffer",
                newName: "BookOffer");

            migrationBuilder.RenameIndex(
                name: "IX_destinationIamges_DestinationId",
                table: "DestinationIamges",
                newName: "IX_DestinationIamges_DestinationId");

            migrationBuilder.RenameIndex(
                name: "IX_getOffer_OfferId",
                table: "BookOffer",
                newName: "IX_BookOffer_OfferId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Offers",
                table: "Offers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Gallery",
                table: "Gallery",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Destinations",
                table: "Destinations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DestinationIamges",
                table: "DestinationIamges",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContactUS",
                table: "ContactUS",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AboutUs",
                table: "AboutUs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookOffer",
                table: "BookOffer",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BookOffer_Offers_OfferId",
                table: "BookOffer",
                column: "OfferId",
                principalTable: "Offers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DestinationIamges_Destinations_DestinationId",
                table: "DestinationIamges",
                column: "DestinationId",
                principalTable: "Destinations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OfferImages_Offers_OfferId",
                table: "OfferImages",
                column: "OfferId",
                principalTable: "Offers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookOffer_Offers_OfferId",
                table: "BookOffer");

            migrationBuilder.DropForeignKey(
                name: "FK_DestinationIamges_Destinations_DestinationId",
                table: "DestinationIamges");

            migrationBuilder.DropForeignKey(
                name: "FK_OfferImages_Offers_OfferId",
                table: "OfferImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Offers",
                table: "Offers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Gallery",
                table: "Gallery");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Destinations",
                table: "Destinations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DestinationIamges",
                table: "DestinationIamges");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContactUS",
                table: "ContactUS");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AboutUs",
                table: "AboutUs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookOffer",
                table: "BookOffer");

            migrationBuilder.RenameTable(
                name: "Offers",
                newName: "offers");

            migrationBuilder.RenameTable(
                name: "Gallery",
                newName: "gallery");

            migrationBuilder.RenameTable(
                name: "Destinations",
                newName: "destinations");

            migrationBuilder.RenameTable(
                name: "DestinationIamges",
                newName: "destinationIamges");

            migrationBuilder.RenameTable(
                name: "ContactUS",
                newName: "contactUS");

            migrationBuilder.RenameTable(
                name: "AboutUs",
                newName: "aboutUs");

            migrationBuilder.RenameTable(
                name: "BookOffer",
                newName: "getOffer");

            migrationBuilder.RenameIndex(
                name: "IX_DestinationIamges_DestinationId",
                table: "destinationIamges",
                newName: "IX_destinationIamges_DestinationId");

            migrationBuilder.RenameIndex(
                name: "IX_BookOffer_OfferId",
                table: "getOffer",
                newName: "IX_getOffer_OfferId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_offers",
                table: "offers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_gallery",
                table: "gallery",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_destinations",
                table: "destinations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_destinationIamges",
                table: "destinationIamges",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_contactUS",
                table: "contactUS",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_aboutUs",
                table: "aboutUs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_getOffer",
                table: "getOffer",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_destinationIamges_destinations_DestinationId",
                table: "destinationIamges",
                column: "DestinationId",
                principalTable: "destinations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_getOffer_offers_OfferId",
                table: "getOffer",
                column: "OfferId",
                principalTable: "offers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OfferImages_offers_OfferId",
                table: "OfferImages",
                column: "OfferId",
                principalTable: "offers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
