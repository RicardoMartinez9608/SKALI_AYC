using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class cambiodeestadodeunsocio : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Esta_Verificado",
                table: "Persona");

            migrationBuilder.AddColumn<int>(
                name: "Estado_Socio",
                table: "Persona",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Estado_Socio",
                table: "Persona");

            migrationBuilder.AddColumn<bool>(
                name: "Esta_Verificado",
                table: "Persona",
                nullable: false,
                defaultValue: false);
        }
    }
}
