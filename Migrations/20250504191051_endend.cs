using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Elagy.Migrations
{
    /// <inheritdoc />
    public partial class endend : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "roshtat",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SpeicalLocation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Pharmacy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roshtat", x => x.Id);
                    table.ForeignKey(
                        name: "FK_roshtat_pharmacies_Pharmacy",
                        column: x => x.Pharmacy,
                        principalTable: "pharmacies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_roshtat_Pharmacy",
                table: "roshtat",
                column: "Pharmacy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "roshtat");
        }
    }
}
