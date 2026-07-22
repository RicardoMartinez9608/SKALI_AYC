using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class solo_identificar_tipo_codigo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id_Ahorro",
                table: "Codigo_Agencia");

            migrationBuilder.DropColumn(
                name: "Id_Credito",
                table: "Codigo_Agencia");

            migrationBuilder.AddColumn<bool>(
                name: "Es_Ahorro",
                table: "Codigo_Agencia",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Es_Credito",
                table: "Codigo_Agencia",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Es_Ahorro",
                table: "Codigo_Agencia");

            migrationBuilder.DropColumn(
                name: "Es_Credito",
                table: "Codigo_Agencia");

            migrationBuilder.AddColumn<int>(
                name: "Id_Ahorro",
                table: "Codigo_Agencia",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Id_Credito",
                table: "Codigo_Agencia",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
