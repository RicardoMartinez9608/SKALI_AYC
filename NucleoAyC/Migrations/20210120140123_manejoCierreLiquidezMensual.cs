using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class manejoCierreLiquidezMensual : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Cierre",
                table: "SaldoDisponible",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "Saldo_Cierre",
                table: "SaldoDisponible",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cierre",
                table: "SaldoDisponible");

            migrationBuilder.DropColumn(
                name: "Saldo_Cierre",
                table: "SaldoDisponible");
        }
    }
}
