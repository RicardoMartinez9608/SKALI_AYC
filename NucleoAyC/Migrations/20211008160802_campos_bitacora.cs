using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class campos_bitacora : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Cliente",
                table: "Bitacora",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Codigo",
                table: "Bitacora",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Id_Transaccion",
                table: "Bitacora",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cliente",
                table: "Bitacora");

            migrationBuilder.DropColumn(
                name: "Codigo",
                table: "Bitacora");

            migrationBuilder.DropColumn(
                name: "Id_Transaccion",
                table: "Bitacora");
        }
    }
}
