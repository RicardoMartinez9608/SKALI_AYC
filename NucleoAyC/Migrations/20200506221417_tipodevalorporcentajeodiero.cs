using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class tipodevalorporcentajeodiero : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Tipo_Valor",
                table: "Deduccion_Historial",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Tipo_Valor",
                table: "Deduccion_Desembolso",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tipo_Valor",
                table: "Deduccion_Historial");

            migrationBuilder.DropColumn(
                name: "Tipo_Valor",
                table: "Deduccion_Desembolso");
        }
    }
}
