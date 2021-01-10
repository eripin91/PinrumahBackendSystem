using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace PinBackendSystem.Migrations
{
    public partial class InitialCreate14 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feature_Listings_listingsId",
                table: "Feature");

            migrationBuilder.DropIndex(
                name: "IX_Feature_listingsId",
                table: "Feature");

            migrationBuilder.DropColumn(
                name: "listingsId",
                table: "Feature");

            migrationBuilder.AddColumn<int>(
                name: "Listing_featureId",
                table: "Feature",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Listing_feature",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ListingId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Listing_feature", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Listing_feature_Listings_ListingId",
                        column: x => x.ListingId,
                        principalTable: "Listings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Feature_Listing_featureId",
                table: "Feature",
                column: "Listing_featureId");

            migrationBuilder.CreateIndex(
                name: "IX_Listing_feature_ListingId",
                table: "Listing_feature",
                column: "ListingId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Feature_Listing_feature_Listing_featureId",
                table: "Feature",
                column: "Listing_featureId",
                principalTable: "Listing_feature",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feature_Listing_feature_Listing_featureId",
                table: "Feature");

            migrationBuilder.DropTable(
                name: "Listing_feature");

            migrationBuilder.DropIndex(
                name: "IX_Feature_Listing_featureId",
                table: "Feature");

            migrationBuilder.DropColumn(
                name: "Listing_featureId",
                table: "Feature");

            migrationBuilder.AddColumn<int>(
                name: "listingsId",
                table: "Feature",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Feature_listingsId",
                table: "Feature",
                column: "listingsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Feature_Listings_listingsId",
                table: "Feature",
                column: "listingsId",
                principalTable: "Listings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
