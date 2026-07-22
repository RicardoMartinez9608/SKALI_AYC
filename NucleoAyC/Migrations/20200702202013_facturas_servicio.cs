using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class facturas_servicio : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comprobante_Gasto",
                table: "Transaccion_Simple");

            migrationBuilder.AddColumn<int>(
                name: "Id_Factura_Servicio",
                table: "Transaccion_Simple",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Iva_Monto",
                table: "Transaccion_Simple",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id_Factura_Servicio",
                table: "Transaccion_Simple");

            migrationBuilder.DropColumn(
                name: "Iva_Monto",
                table: "Transaccion_Simple");

            migrationBuilder.AddColumn<string>(
                name: "Comprobante_Gasto",
                table: "Transaccion_Simple",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);
        }
    }
}
