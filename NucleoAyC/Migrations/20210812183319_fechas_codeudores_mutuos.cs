using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class fechas_codeudores_mutuos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Dos_Codeudores",
                table: "Tipo_Mutuo",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Dos_Codeudores_Dos_Fechas",
                table: "Tipo_Mutuo",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Dos_Fechas",
                table: "Tipo_Mutuo",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Dos_Codeudores",
                table: "Tipo_Mutuo");

            migrationBuilder.DropColumn(
                name: "Dos_Codeudores_Dos_Fechas",
                table: "Tipo_Mutuo");

            migrationBuilder.DropColumn(
                name: "Dos_Fechas",
                table: "Tipo_Mutuo");
        }
    }
}
