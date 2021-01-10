using Microsoft.EntityFrameworkCore.Migrations;

namespace PinBackendSystem.Migrations
{
    public partial class InitialCreate18 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Listing_feature_Listings_ListingId",
                table: "Listing_feature");

            migrationBuilder.DropIndex(
                name: "IX_Listing_feature_ListingId",
                table: "Listing_feature");

            migrationBuilder.DropColumn(
                name: "ListingId",
                table: "Listing_feature");

            migrationBuilder.AddColumn<int>(
                name: "listingsId",
                table: "Listing_feature",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Listing_feature_listingsId",
                table: "Listing_feature",
                column: "listingsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Listing_feature_Listings_listingsId",
                table: "Listing_feature",
                column: "listingsId",
                principalTable: "Listings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Listing_feature_Listings_listingsId",
                table: "Listing_feature");

            migrationBuilder.DropIndex(
                name: "IX_Listing_feature_listingsId",
                table: "Listing_feature");

            migrationBuilder.DropColumn(
                name: "listingsId",
                table: "Listing_feature");

            migrationBuilder.AddColumn<int>(
                name: "ListingId",
                table: "Listing_feature",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Listing_feature_ListingId",
                table: "Listing_feature",
                column: "ListingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Listing_feature_Listings_ListingId",
                table: "Listing_feature",
                column: "ListingId",
                principalTable: "Listings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
