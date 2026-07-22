using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class peps_persona : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Asociado_Cargo",
                table: "Persona",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Familiar_Cargo",
                table: "Persona",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Tiene_Cargo",
                table: "Persona",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Asociado_Cargo",
                table: "Persona");

            migrationBuilder.DropColumn(
                name: "Familiar_Cargo",
                table: "Persona");

            migrationBuilder.DropColumn(
                name: "Tiene_Cargo",
                table: "Persona");
        }
    }
}
