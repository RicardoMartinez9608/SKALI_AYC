using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class idagenciaemergencia : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id_Conf_Detalle_Partida_Contable",
                table: "Detalle_Asiento_Contable");

            migrationBuilder.AddColumn<int>(
                name: "Id_Agencia",
                table: "Detalle_Asiento_Contable",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id_Agencia",
                table: "Detalle_Asiento_Contable");

            migrationBuilder.AddColumn<int>(
                name: "Id_Conf_Detalle_Partida_Contable",
                table: "Detalle_Asiento_Contable",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
