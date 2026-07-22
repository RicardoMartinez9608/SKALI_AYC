using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class activo_firma_autoricacion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Activo",
                table: "Firma_Autorizada_Ahorro",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Nombre_Completo",
                table: "Firma_Autorizada_Ahorro",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Activo",
                table: "Firma_Autorizada_Ahorro");

            migrationBuilder.DropColumn(
                name: "Nombre_Completo",
                table: "Firma_Autorizada_Ahorro");
        }
    }
}
