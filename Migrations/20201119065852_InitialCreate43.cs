using Microsoft.EntityFrameworkCore.Migrations;

namespace PinBackendSystem.Migrations
{
    public partial class InitialCreate43 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "KecamatanId",
                schema: "public",
                table: "Listings",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "KotaId",
                schema: "public",
                table: "Listings",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Listings_KecamatanId",
                schema: "public",
                table: "Listings",
                column: "KecamatanId");

            migrationBuilder.CreateIndex(
                name: "IX_Listings_KotaId",
                schema: "public",
                table: "Listings",
                column: "KotaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Listings_Kecamatans_KecamatanId",
                schema: "public",
                table: "Listings",
                column: "KecamatanId",
                principalSchema: "public",
                principalTable: "Kecamatans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Listings_Kotas_KotaId",
                schema: "public",
                table: "Listings",
                column: "KotaId",
                principalSchema: "public",
                principalTable: "Kotas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Listings_Kecamatans_KecamatanId",
                schema: "public",
                table: "Listings");

            migrationBuilder.DropForeignKey(
                name: "FK_Listings_Kotas_KotaId",
                schema: "public",
                table: "Listings");

            migrationBuilder.DropIndex(
                name: "IX_Listings_KecamatanId",
                schema: "public",
                table: "Listings");

            migrationBuilder.DropIndex(
                name: "IX_Listings_KotaId",
                schema: "public",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "KecamatanId",
                schema: "public",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "KotaId",
                schema: "public",
                table: "Listings");
        }
    }
}
