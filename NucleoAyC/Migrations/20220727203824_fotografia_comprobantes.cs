using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class fotografia_comprobantes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Fotografia_Comprobantes",
                columns: table => new
                {
                    Id_Fotografia_Comprobante = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Id_Colecta = table.Column<int>(nullable: false),
                    Id_Cliente = table.Column<int>(nullable: false),
                    Nombre_Completo = table.Column<string>(nullable: true),
                    Foto = table.Column<string>(nullable: true),
                    Verificado = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fotografia_Comprobantes", x => x.Id_Fotografia_Comprobante);
                    table.ForeignKey(
                        name: "FK_Fotografia_Comprobantes_Colecta_Id_Colecta",
                        column: x => x.Id_Colecta,
                        principalTable: "Colecta",
                        principalColumn: "Id_Colecta",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Fotografia_Comprobantes_Id_Colecta",
                table: "Fotografia_Comprobantes",
                column: "Id_Colecta");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Fotografia_Comprobantes");
        }
    }
}
