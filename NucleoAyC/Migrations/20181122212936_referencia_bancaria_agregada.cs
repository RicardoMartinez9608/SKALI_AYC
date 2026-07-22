using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class referencia_bancaria_agregada : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Referencia_Bancaria",
                columns: table => new
                {
                    Id_Referencia_Bancaria = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Id_Persona = table.Column<int>(nullable: false),
                    Tipo_Referencia = table.Column<int>(nullable: false),
                    Institucion = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Referencia_Bancaria", x => x.Id_Referencia_Bancaria);
                    table.ForeignKey(
                        name: "FK_Referencia_Bancaria_Persona_Id_Persona",
                        column: x => x.Id_Persona,
                        principalTable: "Persona",
                        principalColumn: "Id_Persona",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Referencia_Bancaria_Id_Persona",
                table: "Referencia_Bancaria",
                column: "Id_Persona");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Referencia_Bancaria");
        }
    }
}
