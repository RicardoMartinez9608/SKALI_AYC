using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class manejo_refinanciao_credito : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Codigo_Credito_Financiado",
                table: "Deduccion_Historial",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Id_Credito_Financiado",
                table: "Deduccion_Historial",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Codigo_Credito_Financiado",
                table: "Deduccion_Historial");

            migrationBuilder.DropColumn(
                name: "Id_Credito_Financiado",
                table: "Deduccion_Historial");
        }
    }
}
