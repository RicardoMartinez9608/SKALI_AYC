using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class resto_campos_flujos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Cantidad_aki",
                table: "SaldoDisponible",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Cantidad_cheque",
                table: "SaldoDisponible",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Efectivo_total",
                table: "SaldoDisponible",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Numero_cheque",
                table: "SaldoDisponible",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Saldo_anterior",
                table: "SaldoDisponible",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cantidad_aki",
                table: "SaldoDisponible");

            migrationBuilder.DropColumn(
                name: "Cantidad_cheque",
                table: "SaldoDisponible");

            migrationBuilder.DropColumn(
                name: "Efectivo_total",
                table: "SaldoDisponible");

            migrationBuilder.DropColumn(
                name: "Numero_cheque",
                table: "SaldoDisponible");

            migrationBuilder.DropColumn(
                name: "Saldo_anterior",
                table: "SaldoDisponible");
        }
    }
}
