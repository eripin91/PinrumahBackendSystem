using Microsoft.EntityFrameworkCore.Migrations;

namespace PinBackendSystem.Migrations
{
    public partial class InitialCreate16 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feature_Listing_feature_Listing_featureId",
                table: "Feature");

            migrationBuilder.DropForeignKey(
                name: "FK_Listing_feature_Listings_ListingId",
                table: "Listing_feature");

            migrationBuilder.DropIndex(
                name: "IX_Listing_feature_ListingId",
                table: "Listing_feature");

            migrationBuilder.DropIndex(
                name: "IX_Feature_Listing_featureId",
                table: "Feature");

            migrationBuilder.DropColumn(
                name: "ListingId",
                table: "Listing_feature");

            migrationBuilder.DropColumn(
                name: "Listing_featureId",
                table: "Feature");

            migrationBuilder.AddColumn<int>(
                name: "FeaturesId",
                table: "Listing_feature",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "listingsId",
                table: "Listing_feature",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Listing_feature_FeaturesId",
                table: "Listing_feature",
                column: "FeaturesId");

            migrationBuilder.CreateIndex(
                name: "IX_Listing_feature_listingsId",
                table: "Listing_feature",
                column: "listingsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Listing_feature_Feature_FeaturesId",
                table: "Listing_feature",
                column: "FeaturesId",
                principalTable: "Feature",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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
                name: "FK_Listing_feature_Feature_FeaturesId",
                table: "Listing_feature");

            migrationBuilder.DropForeignKey(
                name: "FK_Listing_feature_Listings_listingsId",
                table: "Listing_feature");

            migrationBuilder.DropIndex(
                name: "IX_Listing_feature_FeaturesId",
                table: "Listing_feature");

            migrationBuilder.DropIndex(
                name: "IX_Listing_feature_listingsId",
                table: "Listing_feature");

            migrationBuilder.DropColumn(
                name: "FeaturesId",
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

            migrationBuilder.AddColumn<int>(
                name: "Listing_featureId",
                table: "Feature",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Listing_feature_ListingId",
                table: "Listing_feature",
                column: "ListingId");

            migrationBuilder.CreateIndex(
                name: "IX_Feature_Listing_featureId",
                table: "Feature",
                column: "Listing_featureId");

            migrationBuilder.AddForeignKey(
                name: "FK_Feature_Listing_feature_Listing_featureId",
                table: "Feature",
                column: "Listing_featureId",
                principalTable: "Listing_feature",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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
