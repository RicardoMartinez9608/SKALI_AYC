using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class manejar_estado : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Estado",
                table: "Llamada_Telefonica_Referencia",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Estado",
                table: "Llamada_Telefonica_Referencia",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
