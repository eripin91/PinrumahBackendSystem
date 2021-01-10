using Microsoft.EntityFrameworkCore.Migrations;

namespace PinBackendSystem.Migrations
{
    public partial class InitialCreate24 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Electricity",
                schema: "public",
                table: "Listings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Interior",
                schema: "public",
                table: "Listings",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFeatured",
                schema: "public",
                table: "Listings",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPrimary",
                schema: "public",
                table: "Listings",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Orientation",
                schema: "public",
                table: "Listings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Water",
                schema: "public",
                table: "Listings",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Electricity",
                schema: "public",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "Interior",
                schema: "public",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "IsFeatured",
                schema: "public",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "IsPrimary",
                schema: "public",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "Orientation",
                schema: "public",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "Water",
                schema: "public",
                table: "Listings");
        }
    }
}
