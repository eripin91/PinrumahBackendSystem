using Microsoft.EntityFrameworkCore.Migrations;

namespace PinBackendSystem.Migrations
{
    public partial class InitialCreate23 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Listing_feature_Feature_FeaturesId",
                table: "Listing_feature");

            migrationBuilder.DropForeignKey(
                name: "FK_Listing_feature_Listings_ListingsId",
                table: "Listing_feature");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Listing_feature",
                table: "Listing_feature");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Feature",
                table: "Feature");

            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.RenameTable(
                name: "Listings",
                newName: "Listings",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "AspNetUserTokens",
                newName: "AspNetUserTokens",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "AspNetUsers",
                newName: "AspNetUsers",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "AspNetUserRoles",
                newName: "AspNetUserRoles",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "AspNetUserLogins",
                newName: "AspNetUserLogins",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "AspNetUserClaims",
                newName: "AspNetUserClaims",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "AspNetRoles",
                newName: "AspNetRoles",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "AspNetRoleClaims",
                newName: "AspNetRoleClaims",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "Listing_feature",
                newName: "Listing_features",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "Feature",
                newName: "Features",
                newSchema: "public");

            migrationBuilder.RenameIndex(
                name: "IX_Listing_feature_ListingsId",
                schema: "public",
                table: "Listing_features",
                newName: "IX_Listing_features_ListingsId");

            migrationBuilder.RenameIndex(
                name: "IX_Listing_feature_FeaturesId",
                schema: "public",
                table: "Listing_features",
                newName: "IX_Listing_features_FeaturesId");

            migrationBuilder.AlterColumn<int>(
                name: "FeaturesId",
                schema: "public",
                table: "Listing_features",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Listing_features",
                schema: "public",
                table: "Listing_features",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Features",
                schema: "public",
                table: "Features",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Listing_features_Features_FeaturesId",
                schema: "public",
                table: "Listing_features",
                column: "FeaturesId",
                principalSchema: "public",
                principalTable: "Features",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Listing_features_Listings_ListingsId",
                schema: "public",
                table: "Listing_features",
                column: "ListingsId",
                principalSchema: "public",
                principalTable: "Listings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Listing_features_Features_FeaturesId",
                schema: "public",
                table: "Listing_features");

            migrationBuilder.DropForeignKey(
                name: "FK_Listing_features_Listings_ListingsId",
                schema: "public",
                table: "Listing_features");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Listing_features",
                schema: "public",
                table: "Listing_features");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Features",
                schema: "public",
                table: "Features");

            migrationBuilder.RenameTable(
                name: "Listings",
                schema: "public",
                newName: "Listings");

            migrationBuilder.RenameTable(
                name: "AspNetUserTokens",
                schema: "public",
                newName: "AspNetUserTokens");

            migrationBuilder.RenameTable(
                name: "AspNetUsers",
                schema: "public",
                newName: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "AspNetUserRoles",
                schema: "public",
                newName: "AspNetUserRoles");

            migrationBuilder.RenameTable(
                name: "AspNetUserLogins",
                schema: "public",
                newName: "AspNetUserLogins");

            migrationBuilder.RenameTable(
                name: "AspNetUserClaims",
                schema: "public",
                newName: "AspNetUserClaims");

            migrationBuilder.RenameTable(
                name: "AspNetRoles",
                schema: "public",
                newName: "AspNetRoles");

            migrationBuilder.RenameTable(
                name: "AspNetRoleClaims",
                schema: "public",
                newName: "AspNetRoleClaims");

            migrationBuilder.RenameTable(
                name: "Listing_features",
                schema: "public",
                newName: "Listing_feature");

            migrationBuilder.RenameTable(
                name: "Features",
                schema: "public",
                newName: "Feature");

            migrationBuilder.RenameIndex(
                name: "IX_Listing_features_ListingsId",
                table: "Listing_feature",
                newName: "IX_Listing_feature_ListingsId");

            migrationBuilder.RenameIndex(
                name: "IX_Listing_features_FeaturesId",
                table: "Listing_feature",
                newName: "IX_Listing_feature_FeaturesId");

            migrationBuilder.AlterColumn<int>(
                name: "FeaturesId",
                table: "Listing_feature",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Listing_feature",
                table: "Listing_feature",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Feature",
                table: "Feature",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Listing_feature_Feature_FeaturesId",
                table: "Listing_feature",
                column: "FeaturesId",
                principalTable: "Feature",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Listing_feature_Listings_ListingsId",
                table: "Listing_feature",
                column: "ListingsId",
                principalTable: "Listings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
