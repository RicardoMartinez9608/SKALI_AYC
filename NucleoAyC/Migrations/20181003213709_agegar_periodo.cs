using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class agegar_periodo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            

            
            

           
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.DropForeignKey(
                name: "FK_Credito_Periodo_Id_Periodo",
                table: "Credito");

            

            migrationBuilder.DropTable(
                name: "Periodo");

            migrationBuilder.DropIndex(
                name: "IX_Credito_Id_Periodo",
                table: "Credito");

            migrationBuilder.DropColumn(
                name: "Id_Periodo",
                table: "Credito");

            migrationBuilder.AddColumn<string>(
                name: "Forma_pago",
                table: "Credito",
                nullable: true);
        }
    }
}
