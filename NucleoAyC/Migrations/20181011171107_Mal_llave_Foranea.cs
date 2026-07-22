using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class Mal_llave_Foranea : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Linea_Ahorro_Tipo_Linea_Ahorro_Id_Linea_Ahorro",
                table: "Linea_Ahorro");

            migrationBuilder.CreateIndex(
                name: "IX_Linea_Ahorro_Id_Tipo_Linea_Ahorro",
                table: "Linea_Ahorro",
                column: "Id_Tipo_Linea_Ahorro");

            migrationBuilder.AddForeignKey(
                name: "FK_Linea_Ahorro_Tipo_Linea_Ahorro_Id_Tipo_Linea_Ahorro",
                table: "Linea_Ahorro",
                column: "Id_Tipo_Linea_Ahorro",
                principalTable: "Tipo_Linea_Ahorro",
                principalColumn: "Id_Tipo_Linea_Ahorro",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Linea_Ahorro_Tipo_Linea_Ahorro_Id_Tipo_Linea_Ahorro",
                table: "Linea_Ahorro");

            migrationBuilder.DropIndex(
                name: "IX_Linea_Ahorro_Id_Tipo_Linea_Ahorro",
                table: "Linea_Ahorro");

            migrationBuilder.AddForeignKey(
                name: "FK_Linea_Ahorro_Tipo_Linea_Ahorro_Id_Linea_Ahorro",
                table: "Linea_Ahorro",
                column: "Id_Linea_Ahorro",
                principalTable: "Tipo_Linea_Ahorro",
                principalColumn: "Id_Tipo_Linea_Ahorro",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
