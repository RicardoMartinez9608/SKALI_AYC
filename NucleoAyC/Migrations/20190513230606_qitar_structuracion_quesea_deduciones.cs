using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class qitar_structuracion_quesea_deduciones : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Estructuracion",
                table: "Linea_Credito_Historial");

            migrationBuilder.DropColumn(
                name: "Estructuracion",
                table: "Linea_Credito");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Estructuracion",
                table: "Linea_Credito_Historial",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Estructuracion",
                table: "Linea_Credito",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
