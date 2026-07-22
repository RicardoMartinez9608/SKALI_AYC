using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class porcentaje_resultado : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Ponderacion_Verficada",
                table: "Referencia",
                nullable: false,
                defaultValue: 0m);

            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ponderacion_Verficada",
                table: "Referencia");

            
        }
    }
}
