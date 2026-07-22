using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class agencia_configuraciondeCuentas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id_Agencia",
                table: "Configuracion_Contabilidad",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Configuracion_Contabilidad_Id_Agencia",
                table: "Configuracion_Contabilidad",
                column: "Id_Agencia");

            migrationBuilder.AddForeignKey(
                name: "FK_Configuracion_Contabilidad_Agencia_Id_Agencia",
                table: "Configuracion_Contabilidad",
                column: "Id_Agencia",
                principalTable: "Agencia",
                principalColumn: "Id_Agencia",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Configuracion_Contabilidad_Agencia_Id_Agencia",
                table: "Configuracion_Contabilidad");

            migrationBuilder.DropIndex(
                name: "IX_Configuracion_Contabilidad_Id_Agencia",
                table: "Configuracion_Contabilidad");

            migrationBuilder.DropColumn(
                name: "Id_Agencia",
                table: "Configuracion_Contabilidad");
        }
    }
}
