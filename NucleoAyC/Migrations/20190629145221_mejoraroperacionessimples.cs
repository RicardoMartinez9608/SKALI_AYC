using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class mejoraroperacionessimples : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ANombreDe",
                table: "Transaccion_Simple",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Id_Cobro_Acciones",
                table: "Transaccion_Simple",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Id_Deduccion",
                table: "Transaccion_Simple",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Id_Persona",
                table: "Transaccion_Simple",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ANombreDe",
                table: "Transaccion_Simple");

            migrationBuilder.DropColumn(
                name: "Id_Cobro_Acciones",
                table: "Transaccion_Simple");

            migrationBuilder.DropColumn(
                name: "Id_Deduccion",
                table: "Transaccion_Simple");

            migrationBuilder.DropColumn(
                name: "Id_Persona",
                table: "Transaccion_Simple");
        }
    }
}
