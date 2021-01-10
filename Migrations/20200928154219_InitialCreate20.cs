using Microsoft.EntityFrameworkCore.Migrations;

namespace PinBackendSystem.Migrations
{
    public partial class InitialCreate20 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Listing_feature_Listings_listingsId",
                table: "Listing_feature");

            migrationBuilder.RenameColumn(
                name: "listingsId",
                table: "Listing_feature",
                newName: "ListingsId");

            migrationBuilder.RenameIndex(
                name: "IX_Listing_feature_listingsId",
                table: "Listing_feature",
                newName: "IX_Listing_feature_ListingsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Listing_feature_Listings_ListingsId",
                table: "Listing_feature",
                column: "ListingsId",
                principalTable: "Listings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Listing_feature_Listings_ListingsId",
                table: "Listing_feature");

            migrationBuilder.RenameColumn(
                name: "ListingsId",
                table: "Listing_feature",
                newName: "listingsId");

            migrationBuilder.RenameIndex(
                name: "IX_Listing_feature_ListingsId",
                table: "Listing_feature",
                newName: "IX_Listing_feature_listingsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Listing_feature_Listings_listingsId",
                table: "Listing_feature",
                column: "listingsId",
                principalTable: "Listings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
