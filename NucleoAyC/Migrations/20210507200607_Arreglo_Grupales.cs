using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class Arreglo_Grupales : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Detalle_Credito_Grupal_Credito_Id_Credito_Grupal",
                table: "Detalle_Credito_Grupal");

            migrationBuilder.AddForeignKey(
                name: "FK_Detalle_Credito_Grupal_Credito_Id_Credito",
                table: "Detalle_Credito_Grupal",
                column: "Id_Credito",
                principalTable: "Credito",
                principalColumn: "Id_Credito",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Detalle_Credito_Grupal_Credito_Id_Credito",
                table: "Detalle_Credito_Grupal");

            migrationBuilder.AddForeignKey(
                name: "FK_Detalle_Credito_Grupal_Credito_Id_Credito_Grupal",
                table: "Detalle_Credito_Grupal",
                column: "Id_Credito_Grupal",
                principalTable: "Credito",
                principalColumn: "Id_Credito",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
