using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class Mantenimiento_Grupales : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Credito_Grupal",
                columns: table => new
                {
                    Id_Credito_Grupal = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nombre_grupo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Credito_Grupal", x => x.Id_Credito_Grupal);
                });


            migrationBuilder.CreateTable(
                name: "Detalle_Credito_Grupal",
                columns: table => new
                {
                    Id_Detalle_Credito_Grupal = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Id_Credito = table.Column<int>(nullable: false),
                    Id_Credito_Grupal = table.Column<int>(nullable: false),
                    Lider = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Detalle_Credito_Grupal", x => x.Id_Detalle_Credito_Grupal);
                    table.UniqueConstraint("AK_Detalle_Credito_Grupal_Id_Credito_Id_Credito_Grupal", x => new { x.Id_Credito, x.Id_Credito_Grupal });
                    table.ForeignKey(
                        name: "FK_Detalle_Credito_Grupal_Credito_Id_Credito_Grupal",
                        column: x => x.Id_Credito_Grupal,
                        principalTable: "Credito",
                        principalColumn: "Id_Credito",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Detalle_Credito_Grupal_Credito_Grupal_Id_Credito_Grupal",
                        column: x => x.Id_Credito_Grupal,
                        principalTable: "Credito_Grupal",
                        principalColumn: "Id_Credito_Grupal",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Detalle_Credito_Grupal_Id_Credito_Grupal",
                table: "Detalle_Credito_Grupal",
                column: "Id_Credito_Grupal");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Detalle_Credito_Grupal");

            migrationBuilder.DropTable(
                name: "Credito_Grupal");
        }
    }
}
