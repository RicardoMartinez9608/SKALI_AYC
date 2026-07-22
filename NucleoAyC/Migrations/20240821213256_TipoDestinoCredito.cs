using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class TipoDestinoCredito : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TipoDestinoCreditos",
                columns: table => new
                {
                    Destino_Credito = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Descripcion = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoDestinoCreditos", x => x.Destino_Credito);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Linea_Credito_Destino_Credito",
                table: "Linea_Credito",
                column: "Destino_Credito");

            migrationBuilder.AddForeignKey(
                name: "FK_Linea_Credito_TipoDestinoCreditos_Destino_Credito",
                table: "Linea_Credito",
                column: "Destino_Credito",
                principalTable: "TipoDestinoCreditos",
                principalColumn: "Destino_Credito",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Linea_Credito_TipoDestinoCreditos_Destino_Credito",
                table: "Linea_Credito");

            migrationBuilder.DropTable(
                name: "TipoDestinoCreditos");

            migrationBuilder.DropIndex(
                name: "IX_Linea_Credito_Destino_Credito",
                table: "Linea_Credito");
        }
    }
}
