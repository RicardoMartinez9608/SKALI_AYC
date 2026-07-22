using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class valoracion_estado : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "Telefono_Equivocado",
                table: "Referencia");

            migrationBuilder.AddColumn<int>(
                name: "Valoracion_Referencia",
                table: "Referencia",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Valoracion_Referencia",
                table: "Referencia");

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
                name: "Telefono_Equivocado",
                table: "Referencia",
                nullable: false,
                defaultValue: false);
        }
    }
}
