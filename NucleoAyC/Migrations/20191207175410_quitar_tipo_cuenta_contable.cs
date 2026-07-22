using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class quitar_tipo_cuenta_contable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cuenta_Contable_Tipo_Id_Tipo",
                table: "Cuenta_Contable");

            migrationBuilder.DropIndex(
                name: "IX_Cuenta_Contable_Id_Tipo",
                table: "Cuenta_Contable");

            migrationBuilder.DropColumn(
                name: "Id_Tipo",
                table: "Cuenta_Contable");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id_Tipo",
                table: "Cuenta_Contable",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Cuenta_Contable_Id_Tipo",
                table: "Cuenta_Contable",
                column: "Id_Tipo");

            migrationBuilder.AddForeignKey(
                name: "FK_Cuenta_Contable_Tipo_Id_Tipo",
                table: "Cuenta_Contable",
                column: "Id_Tipo",
                principalTable: "Tipo",
                principalColumn: "Id_Tipo",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
