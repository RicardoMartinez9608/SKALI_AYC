using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class tipo_mutuo_con_Linea_Credito : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tipo_Mutuo",
                columns: table => new
                {
                    Id_Tipo_Mutuo = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Id_Linea_Credito = table.Column<int>(nullable: false),
                    Nombre_Extencion_Documento = table.Column<string>(nullable: true),
                    Copia = table.Column<bool>(nullable: false),
                    Huella = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tipo_Mutuo", x => x.Id_Tipo_Mutuo);
                    table.ForeignKey(
                        name: "FK_Tipo_Mutuo_Linea_Credito_Id_Linea_Credito",
                        column: x => x.Id_Linea_Credito,
                        principalTable: "Linea_Credito",
                        principalColumn: "Id_Linea",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tipo_Mutuo_Id_Linea_Credito",
                table: "Tipo_Mutuo",
                column: "Id_Linea_Credito");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tipo_Mutuo");
        }
    }
}
