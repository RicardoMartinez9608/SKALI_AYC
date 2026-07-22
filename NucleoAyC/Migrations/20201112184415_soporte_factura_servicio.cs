using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class soporte_factura_servicio : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cuenta_Contable",
                table: "Factura_Servicios");

            migrationBuilder.AddColumn<int>(
                name: "Id_Factura_Servicio",
                table: "Conf_Partida_Contable",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id_Factura_Servicio",
                table: "Conf_Partida_Contable");

            migrationBuilder.AddColumn<string>(
                name: "Cuenta_Contable",
                table: "Factura_Servicios",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);
        }
    }
}
