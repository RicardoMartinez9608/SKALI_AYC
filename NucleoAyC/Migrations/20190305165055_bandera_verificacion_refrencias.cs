using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class bandera_verificacion_refrencias : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Esta_Verificado",
                table: "Referencia",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "No_Contesta",
                table: "Referencia",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "Ponderacion",
                table: "Referencia",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "Esta_Verificado",
                table: "Persona",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "Ponderacion",
                table: "Persona",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Esta_Verificado",
                table: "Referencia");

            migrationBuilder.DropColumn(
                name: "No_Contesta",
                table: "Referencia");

            migrationBuilder.DropColumn(
                name: "Ponderacion",
                table: "Referencia");

            migrationBuilder.DropColumn(
                name: "Esta_Verificado",
                table: "Persona");

            migrationBuilder.DropColumn(
                name: "Ponderacion",
                table: "Persona");
        }
    }
}
