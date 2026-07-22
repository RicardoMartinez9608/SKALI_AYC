using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class cambionombreColumnaMalos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IvaSobrePlanAyC",
                table: "Transaccion_Credito");

            migrationBuilder.DropColumn(
                name: "PlanAyC",
                table: "Transaccion_Credito");

            migrationBuilder.AddColumn<decimal>(
                name: "Interes_Comision",
                table: "Transaccion_Credito",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "IvaSobreComision",
                table: "Transaccion_Credito",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Interes_Comision",
                table: "Transaccion_Credito");

            migrationBuilder.DropColumn(
                name: "IvaSobreComision",
                table: "Transaccion_Credito");

            migrationBuilder.AddColumn<decimal>(
                name: "IvaSobrePlanAyC",
                table: "Transaccion_Credito",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PlanAyC",
                table: "Transaccion_Credito",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
