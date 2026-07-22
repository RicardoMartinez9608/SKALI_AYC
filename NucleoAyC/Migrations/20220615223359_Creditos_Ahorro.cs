using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class Creditos_Ahorro : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Credito_Con_Ahorro",
                table: "Credito",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Credito_Con_Ahorros",
                columns: table => new
                {
                    Id_Credito_Con_Ahorro = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Id_Credito = table.Column<int>(nullable: false),
                    Monto_Ahorro = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Credito_Con_Ahorros", x => x.Id_Credito_Con_Ahorro);
                    table.ForeignKey(
                        name: "FK_Credito_Con_Ahorros_Credito_Id_Credito",
                        column: x => x.Id_Credito,
                        principalTable: "Credito",
                        principalColumn: "Id_Credito",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Credito_Con_Ahorros_Id_Credito",
                table: "Credito_Con_Ahorros",
                column: "Id_Credito");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Credito_Con_Ahorros");

            migrationBuilder.DropColumn(
                name: "Credito_Con_Ahorro",
                table: "Credito");
        }
    }
}
