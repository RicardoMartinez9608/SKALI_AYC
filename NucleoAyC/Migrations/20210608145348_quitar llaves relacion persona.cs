using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class quitarllavesrelacionpersona : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Persona_Relaciones",
                table: "Persona_Relaciones");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Persona_Relaciones",
                table: "Persona_Relaciones",
                column: "Id_Persona_Relaciones");

            migrationBuilder.CreateIndex(
                name: "IX_Persona_Relaciones_Id_Sujeto",
                table: "Persona_Relaciones",
                column: "Id_Sujeto");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Persona_Relaciones",
                table: "Persona_Relaciones");

            migrationBuilder.DropIndex(
                name: "IX_Persona_Relaciones_Id_Sujeto",
                table: "Persona_Relaciones");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Persona_Relaciones",
                table: "Persona_Relaciones",
                columns: new[] { "Id_Sujeto", "Id_Predicado" });
        }
    }
}
