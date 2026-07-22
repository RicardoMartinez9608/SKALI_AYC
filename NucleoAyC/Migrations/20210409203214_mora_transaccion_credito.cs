using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class mora_transaccion_credito : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Mora",
                table: "Transaccion_Credito",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Mora",
                table: "Transaccion_Credito");
        }
    }
}
