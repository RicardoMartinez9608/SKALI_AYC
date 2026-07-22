using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class quitarelivaamortizacio : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IvaSobreComision",
                table: "Amortizacion");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "IvaSobreComision",
                table: "Amortizacion",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
