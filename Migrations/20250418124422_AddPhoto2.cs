using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Elagy.Migrations
{
    /// <inheritdoc />
    public partial class AddPhoto2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PharmacyLicense",
                table: "pharmacies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TaxCard",
                table: "pharmacies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TradeLicense",
                table: "pharmacies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PharmacyLicense",
                table: "pharmacies");

            migrationBuilder.DropColumn(
                name: "TaxCard",
                table: "pharmacies");

            migrationBuilder.DropColumn(
                name: "TradeLicense",
                table: "pharmacies");
        }
    }
}
