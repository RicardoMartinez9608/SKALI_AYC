using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class agregarcuentacontableabanco : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Cuenta_Contable",
                table: "Banco",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Conf_Detalle_Partida_Contable_Id_Cuenta_Contable",
                table: "Conf_Detalle_Partida_Contable",
                column: "Id_Cuenta_Contable");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Conf_Detalle_Partida_Contable_Id_Cuenta_Contable",
                table: "Conf_Detalle_Partida_Contable");

            migrationBuilder.DropColumn(
                name: "Cuenta_Contable",
                table: "Banco");
        }
    }
}
