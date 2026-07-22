using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class quitar_agencia_cuenta_contable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Cuenta_Contable_Agencia_Id_Agencia",
            //    table: "Cuenta_Contable");

            migrationBuilder.DropIndex(
                name: "IX_Cuenta_Contable_Id_Agencia",
                table: "Cuenta_Contable");

            migrationBuilder.DropColumn(
                name: "Id_Agencia",
                table: "Cuenta_Contable");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id_Agencia",
                table: "Cuenta_Contable",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Cuenta_Contable_Id_Agencia",
                table: "Cuenta_Contable",
                column: "Id_Agencia");

            migrationBuilder.AddForeignKey(
                name: "FK_Cuenta_Contable_Agencia_Id_Agencia",
                table: "Cuenta_Contable",
                column: "Id_Agencia",
                principalTable: "Agencia",
                principalColumn: "Id_Agencia",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
