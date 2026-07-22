using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class unicosinkey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Dui",
                table: "Persona",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Persona_Dui",
                table: "Persona",
                column: "Dui",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Persona_Dui",
                table: "Persona");

            migrationBuilder.AlterColumn<string>(
                name: "Dui",
                table: "Persona",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
