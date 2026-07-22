using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class quitar_tldes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Nombre_Categoría",
                table: "Categoria_Acceso_Directo");

            migrationBuilder.AddColumn<string>(
                name: "Nombre_Categoria",
                table: "Categoria_Acceso_Directo",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Nombre_Categoria",
                table: "Categoria_Acceso_Directo");

            migrationBuilder.AddColumn<string>(
                name: "Nombre_Categoría",
                table: "Categoria_Acceso_Directo",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);
        }
    }
}
