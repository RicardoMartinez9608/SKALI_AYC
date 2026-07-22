using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class columna_id_transaccion_deposito_plazo_en_transaccion_caja : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id_Transaccion_Deposito_Plazo",
                table: "Transaccion_Caja",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id_Transaccion_Deposito_Plazo",
                table: "Transaccion_Caja");
        }
    }
}
