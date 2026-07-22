using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class accesosdirectos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categoria_Acceso_Directo",
                columns: table => new
                {
                    Id_Categoria_Acceso_Directo = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Id_Usuario = table.Column<string>(nullable: true),
                    Nombre_Categoría = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categoria_Acceso_Directo", x => x.Id_Categoria_Acceso_Directo);
                });

            migrationBuilder.CreateTable(
                name: "Acceso_Directo",
                columns: table => new
                {
                    Id_Acceso_Directo = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Id_Categoria_Acceso_Directo = table.Column<int>(nullable: false),
                    Nombre = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Acceso_Directo", x => x.Id_Acceso_Directo);
                    table.ForeignKey(
                        name: "FK_Acceso_Directo_Categoria_Acceso_Directo_Id_Categoria_Acceso_~",
                        column: x => x.Id_Categoria_Acceso_Directo,
                        principalTable: "Categoria_Acceso_Directo",
                        principalColumn: "Id_Categoria_Acceso_Directo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Acceso_Directo_Id_Categoria_Acceso_Directo",
                table: "Acceso_Directo",
                column: "Id_Categoria_Acceso_Directo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Acceso_Directo");

            migrationBuilder.DropTable(
                name: "Categoria_Acceso_Directo");
        }
    }
}
