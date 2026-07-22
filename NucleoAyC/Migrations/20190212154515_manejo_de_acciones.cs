using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class manejo_de_acciones : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accion",
                columns: table => new
                {
                    Id_Accion = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Id_Persona = table.Column<int>(nullable: false),
                    Codigo = table.Column<string>(nullable: true),
                    No_Certificado = table.Column<int>(nullable: false),
                    Cantidad_Accion = table.Column<int>(nullable: false),
                    Valor_Unitario = table.Column<decimal>(nullable: false),
                    Valor_Total = table.Column<decimal>(nullable: false),
                    Cancelado = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accion", x => x.Id_Accion);
                    table.ForeignKey(
                        name: "FK_Accion_Persona_Id_Persona",
                        column: x => x.Id_Persona,
                        principalTable: "Persona",
                        principalColumn: "Id_Persona",
                        onDelete: ReferentialAction.Cascade);
                });
            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accion");
            
        }
    }
}
