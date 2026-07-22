using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class cambio_catalogoevaluacion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Evaluacion_Referencia_Persona_Evaluacion_Referencia_Id_Evalu~",
                table: "Evaluacion_Referencia_Persona");

            migrationBuilder.DropForeignKey(
                name: "FK_Pregunta_Evaluacion_Referencia_Id_Referencia_Evaluada_Catalo~",
                table: "Pregunta");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Evaluacion_Referencia",
                table: "Evaluacion_Referencia");

            migrationBuilder.RenameTable(
                name: "Evaluacion_Referencia",
                newName: "Evaluacion_Referencia_Catalogo");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Evaluacion_Referencia_Catalogo",
                table: "Evaluacion_Referencia_Catalogo",
                column: "Id_Evaluacion_Referencia_Catalogo");

            migrationBuilder.AddForeignKey(
                name: "FK_Evaluacion_Referencia_Persona_Evaluacion_Referencia_Catalogo~",
                table: "Evaluacion_Referencia_Persona",
                column: "Id_Evaluacion_Referencia_Catalogo",
                principalTable: "Evaluacion_Referencia_Catalogo",
                principalColumn: "Id_Evaluacion_Referencia_Catalogo",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pregunta_Evaluacion_Referencia_Catalogo_Id_Referencia_Evalua~",
                table: "Pregunta",
                column: "Id_Referencia_Evaluada_Catalogo",
                principalTable: "Evaluacion_Referencia_Catalogo",
                principalColumn: "Id_Evaluacion_Referencia_Catalogo",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Evaluacion_Referencia_Persona_Evaluacion_Referencia_Catalogo~",
                table: "Evaluacion_Referencia_Persona");

            migrationBuilder.DropForeignKey(
                name: "FK_Pregunta_Evaluacion_Referencia_Catalogo_Id_Referencia_Evalua~",
                table: "Pregunta");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Evaluacion_Referencia_Catalogo",
                table: "Evaluacion_Referencia_Catalogo");

            migrationBuilder.RenameTable(
                name: "Evaluacion_Referencia_Catalogo",
                newName: "Evaluacion_Referencia");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Evaluacion_Referencia",
                table: "Evaluacion_Referencia",
                column: "Id_Evaluacion_Referencia_Catalogo");

            migrationBuilder.AddForeignKey(
                name: "FK_Evaluacion_Referencia_Persona_Evaluacion_Referencia_Id_Evalu~",
                table: "Evaluacion_Referencia_Persona",
                column: "Id_Evaluacion_Referencia_Catalogo",
                principalTable: "Evaluacion_Referencia",
                principalColumn: "Id_Evaluacion_Referencia_Catalogo",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pregunta_Evaluacion_Referencia_Id_Referencia_Evaluada_Catalo~",
                table: "Pregunta",
                column: "Id_Referencia_Evaluada_Catalogo",
                principalTable: "Evaluacion_Referencia",
                principalColumn: "Id_Evaluacion_Referencia_Catalogo",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
