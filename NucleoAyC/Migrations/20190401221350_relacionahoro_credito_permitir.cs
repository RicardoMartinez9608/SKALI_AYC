using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class relacionahoro_credito_permitir : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Persona_Relaciones_Ahorro_Id_Ahorro",
            //    table: "Persona_Relaciones");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Persona_Relaciones_Credito_Id_Credito",
            //    table: "Persona_Relaciones");

            //migrationBuilder.DropIndex(
            //    name: "IX_Persona_Relaciones_Id_Ahorro",
            //    table: "Persona_Relaciones");

            //migrationBuilder.DropIndex(
            //    name: "IX_Persona_Relaciones_Id_Credito",
            //    table: "Persona_Relaciones");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AddForeignKey(
                name: "FK_Persona_Relaciones_Credito_Id_Credito",
                table: "Persona_Relaciones",
                column: "Id_Credito",
                principalTable: "Credito",
                principalColumn: "Id_Credito",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
