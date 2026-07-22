using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class comportamientodelcrédito : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id_Comportamiento_Credito",
                table: "Credito",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Comportamiento_Credito",
                columns: table => new
                {
                    Id_Comportamiento_Credito = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Descripcion_Comportamiento_Credito = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comportamiento_Credito", x => x.Id_Comportamiento_Credito);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Credito_Id_Comportamiento_Credito",
                table: "Credito",
                column: "Id_Comportamiento_Credito");

            migrationBuilder.AddForeignKey(
                name: "FK_Credito_Comportamiento_Credito_Id_Comportamiento_Credito",
                table: "Credito",
                column: "Id_Comportamiento_Credito",
                principalTable: "Comportamiento_Credito",
                principalColumn: "Id_Comportamiento_Credito",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Credito_Comportamiento_Credito_Id_Comportamiento_Credito",
                table: "Credito");

            migrationBuilder.DropTable(
                name: "Comportamiento_Credito");

            migrationBuilder.DropIndex(
                name: "IX_Credito_Id_Comportamiento_Credito",
                table: "Credito");

            migrationBuilder.DropColumn(
                name: "Id_Comportamiento_Credito",
                table: "Credito");
        }
    }
}
