using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class lasiguientepruebaparamigrar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Persona_Relaciones_Ahorro_Id_Ahorro",
            //    table: "Persona_Relaciones");

            migrationBuilder.AddColumn<int>(
                name: "Frecuencia_Deposito",
                table: "Ahorro",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Mes_Capitalizable",
                table: "Ahorro",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Monto_Minimo_Ahorro",
                table: "Ahorro",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "Plazo",
                table: "Ahorro",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Tasa_Interes",
                table: "Ahorro",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_Persona_Relaciones_Id_Ahorro",
                table: "Persona_Relaciones",
                column: "Id_Ahorro");

            migrationBuilder.CreateIndex(
                name: "IX_Persona_Relaciones_Id_Credito",
                table: "Persona_Relaciones",
                column: "Id_Credito");

            migrationBuilder.AddForeignKey(
                name: "FK_Persona_Relaciones_Ahorro_Id_Ahorro",
                table: "Persona_Relaciones",
                column: "Id_Ahorro",
                principalTable: "Ahorro",
                principalColumn: "Id_Ahorro",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Persona_Relaciones_Ahorro_Id_Ahorro",
                table: "Persona_Relaciones");

            migrationBuilder.DropIndex(
                name: "IX_Persona_Relaciones_Id_Ahorro",
                table: "Persona_Relaciones");

            migrationBuilder.DropIndex(
                name: "IX_Persona_Relaciones_Id_Credito",
                table: "Persona_Relaciones");

            migrationBuilder.DropColumn(
                name: "Frecuencia_Deposito",
                table: "Ahorro");

            migrationBuilder.DropColumn(
                name: "Mes_Capitalizable",
                table: "Ahorro");

            migrationBuilder.DropColumn(
                name: "Monto_Minimo_Ahorro",
                table: "Ahorro");

            migrationBuilder.DropColumn(
                name: "Plazo",
                table: "Ahorro");

            migrationBuilder.DropColumn(
                name: "Tasa_Interes",
                table: "Ahorro");

            migrationBuilder.AddForeignKey(
                name: "FK_Persona_Relaciones_Ahorro_Id_Ahorro",
                table: "Persona_Relaciones",
                column: "Id_Ahorro",
                principalTable: "Ahorro",
                principalColumn: "Id_Ahorro",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
