using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class porcentaje_ahorro_Credito : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id_Porcentaje_Ahorro_Credito",
                table: "Credito_Con_Ahorros",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Porcentaje_Ahorro_Creditos",
                columns: table => new
                {
                    Id_Porcentaje_Ahorro_Credito = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Porcentaje = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Porcentaje_Ahorro_Creditos", x => x.Id_Porcentaje_Ahorro_Credito);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Credito_Con_Ahorros_Id_Porcentaje_Ahorro_Credito",
                table: "Credito_Con_Ahorros",
                column: "Id_Porcentaje_Ahorro_Credito");

            migrationBuilder.AddForeignKey(
                name: "FK_Credito_Con_Ahorros_Porcentaje_Ahorro_Creditos_Id_Porcentaje~",
                table: "Credito_Con_Ahorros",
                column: "Id_Porcentaje_Ahorro_Credito",
                principalTable: "Porcentaje_Ahorro_Creditos",
                principalColumn: "Id_Porcentaje_Ahorro_Credito",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Credito_Con_Ahorros_Porcentaje_Ahorro_Creditos_Id_Porcentaje~",
                table: "Credito_Con_Ahorros");

            migrationBuilder.DropTable(
                name: "Porcentaje_Ahorro_Creditos");

            migrationBuilder.DropIndex(
                name: "IX_Credito_Con_Ahorros_Id_Porcentaje_Ahorro_Credito",
                table: "Credito_Con_Ahorros");

            migrationBuilder.DropColumn(
                name: "Id_Porcentaje_Ahorro_Credito",
                table: "Credito_Con_Ahorros");
        }
    }
}
