using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class arreglarlosdepositos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Deposito_Plazo_Ahorro_Id_Ahorro",
                table: "Deposito_Plazo");

            migrationBuilder.DropIndex(
                name: "IX_Deposito_Plazo_Id_Ahorro",
                table: "Deposito_Plazo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Deposito_Plazo_Id_Ahorro",
                table: "Deposito_Plazo",
                column: "Id_Ahorro");

            migrationBuilder.AddForeignKey(
                name: "FK_Deposito_Plazo_Ahorro_Id_Ahorro",
                table: "Deposito_Plazo",
                column: "Id_Ahorro",
                principalTable: "Ahorro",
                principalColumn: "Id_Ahorro",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
