using Microsoft.EntityFrameworkCore.Migrations;

namespace PinBackendSystem.Migrations
{
    public partial class InitialCreate15 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Listing_feature_ListingId",
                table: "Listing_feature");

            migrationBuilder.CreateIndex(
                name: "IX_Listing_feature_ListingId",
                table: "Listing_feature",
                column: "ListingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Listing_feature_ListingId",
                table: "Listing_feature");

            migrationBuilder.CreateIndex(
                name: "IX_Listing_feature_ListingId",
                table: "Listing_feature",
                column: "ListingId",
                unique: true);
        }
    }
}
