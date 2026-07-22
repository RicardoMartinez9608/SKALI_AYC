using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class _arreglos_nuevo_contabilidad : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Evaluacion_Credito_Id_Credito",
            //    table: "Evaluacion");

            //migrationBuilder.DropUniqueConstraint(
            //    name: "AK_Persona_Tipo_Persona_Id_Persona_Tipo_Persona",
            //    table: "Persona_Tipo_Persona");

            //migrationBuilder.DropUniqueConstraint(
            //    name: "AK_Persona_Relaciones_Id_Persona_Relaciones",
            //    table: "Persona_Relaciones");

            //migrationBuilder.DropUniqueConstraint(
            //    name: "AK_Persona_Referencia_Id_Persona_Referencia",
            //    table: "Persona_Referencia");

            //migrationBuilder.DropIndex(
            //    name: "IX_Evaluacion_Id_Credito",
            //    table: "Evaluacion");

            //migrationBuilder.DropColumn(
            //    name: "Id_Linea_Ahorro",
            //    table: "Conf_Detalle_Partida_Contable");

            //migrationBuilder.DropColumn(
            //    name: "Id_Linea_Credito",
            //    table: "Conf_Detalle_Partida_Contable");

            ////migrationBuilder.AddColumn<int>(
            ////    name: "Id_Linea_Ahorro",
            ////    table: "Conf_Partida_Contable",
            ////    nullable: false,
            ////    defaultValue: 0);

            ////migrationBuilder.AddColumn<int>(
            ////    name: "Id_Linea_Credito",
            ////    table: "Conf_Partida_Contable",
            ////    nullable: false,
            ////    defaultValue: 0);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Credito_Evaluacion_Id_Credito",
            //    table: "Credito",
            //    column: "Id_Credito",
            //    principalTable: "Evaluacion",
            //    principalColumn: "Id_Evaluacion",
            //    onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Credito_Evaluacion_Id_Credito",
                table: "Credito");

            migrationBuilder.DropColumn(
                name: "Id_Linea_Ahorro",
                table: "Conf_Partida_Contable");

            migrationBuilder.DropColumn(
                name: "Id_Linea_Credito",
                table: "Conf_Partida_Contable");

            migrationBuilder.AddColumn<int>(
                name: "Id_Linea_Ahorro",
                table: "Conf_Detalle_Partida_Contable",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Id_Linea_Credito",
                table: "Conf_Detalle_Partida_Contable",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Persona_Tipo_Persona_Id_Persona_Tipo_Persona",
                table: "Persona_Tipo_Persona",
                column: "Id_Persona_Tipo_Persona");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Persona_Relaciones_Id_Persona_Relaciones",
                table: "Persona_Relaciones",
                column: "Id_Persona_Relaciones");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Persona_Referencia_Id_Persona_Referencia",
                table: "Persona_Referencia",
                column: "Id_Persona_Referencia");

            migrationBuilder.CreateIndex(
                name: "IX_Evaluacion_Id_Credito",
                table: "Evaluacion",
                column: "Id_Credito",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Evaluacion_Credito_Id_Credito",
                table: "Evaluacion",
                column: "Id_Credito",
                principalTable: "Credito",
                principalColumn: "Id_Credito",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
