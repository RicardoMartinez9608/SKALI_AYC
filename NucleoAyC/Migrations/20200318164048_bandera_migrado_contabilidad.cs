using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class bandera_migrado_contabilidad : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Migrada",
                table: "Asiento_Contable",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Referencia_Contabilidad",
                table: "Asiento_Contable",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Migrada",
                table: "Asiento_Contable");

            migrationBuilder.DropColumn(
                name: "Referencia_Contabilidad",
                table: "Asiento_Contable");
        }
    }
}
