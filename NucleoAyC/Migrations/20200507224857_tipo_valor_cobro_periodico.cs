using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class tipo_valor_cobro_periodico : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Tipo_Valor",
                table: "Cobro_Periodico_Historial",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Tipo_Valor",
                table: "Cobro_Periodico",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tipo_Valor",
                table: "Cobro_Periodico_Historial");

            migrationBuilder.DropColumn(
                name: "Tipo_Valor",
                table: "Cobro_Periodico");
        }
    }
}
