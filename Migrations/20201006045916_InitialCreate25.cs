using Microsoft.EntityFrameworkCore.Migrations;

namespace PinBackendSystem.Migrations
{
    public partial class InitialCreate25 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ListingCard",
                schema: "public",
                table: "Listings");

            migrationBuilder.AddColumn<int>(
                name: "Length",
                schema: "public",
                table: "Listings",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Width",
                schema: "public",
                table: "Listings",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Length",
                schema: "public",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "Width",
                schema: "public",
                table: "Listings");

            migrationBuilder.AddColumn<string>(
                name: "ListingCard",
                schema: "public",
                table: "Listings",
                type: "text",
                nullable: true);
        }
    }
}
