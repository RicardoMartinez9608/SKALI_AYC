using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class creditos_grupales_agencias : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<int>(
            //    name: "Id_Agencia",
            //    table: "Credito_Grupal",
            //    nullable: false,
            //    defaultValue: 0);

            //migrationBuilder.CreateIndex(
            //    name: "IX_Credito_Grupal_Id_Agencia",
            //    table: "Credito_Grupal",
            //    column: "Id_Agencia");
            
            //migrationBuilder.AddForeignKey(
            //    name: "FK_Credito_Grupal_Agencia_Id_Agencia",
            //    table: "Credito_Grupal",
            //    column: "Id_Agencia",
            //    principalTable: "Agencia",
            //    principalColumn: "Id_Agencia",
            //    onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Credito_Grupal_Agencia_Id_Agencia",
                table: "Credito_Grupal");

            migrationBuilder.DropIndex(
                name: "IX_Credito_Grupal_Id_Agencia",
                table: "Credito_Grupal");

            migrationBuilder.DropColumn(
                name: "Id_Agencia",
                table: "Credito_Grupal");
        }
    }
}
