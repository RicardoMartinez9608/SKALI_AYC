using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class Credito_Movible : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Movible",
                table: "Linea_Credito",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Movible",
                table: "Linea_Credito");
        }
    }
}
