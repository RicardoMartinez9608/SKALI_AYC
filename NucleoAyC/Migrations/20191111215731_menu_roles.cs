using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class menu_roles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Efectivo_Denominacion_efectivo_id_denominacion",
                table: "Efectivo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Denominacion_efectivo",
                table: "Denominacion_efectivo");

            migrationBuilder.RenameTable(
                name: "Denominacion_efectivo",
                newName: "Denominacion_Efectivo");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Denominacion_Efectivo",
                table: "Denominacion_Efectivo",
                column: "Id_denominacion");

            migrationBuilder.CreateTable(
                name: "MenuCategoriaVista",
                columns: table => new
                {
                    Id_Menu_Categoria_Vista = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Titulo = table.Column<string>(nullable: true),
                    Nivel = table.Column<int>(nullable: false),
                    Icono = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuCategoriaVista", x => x.Id_Menu_Categoria_Vista);
                });

            migrationBuilder.CreateTable(
                name: "MenuContenidoVista",
                columns: table => new
                {
                    Id_Menu_Contenido_Vista = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Id_Menu_Categoria_Vista = table.Column<int>(nullable: false),
                    Titulo = table.Column<string>(nullable: true),
                    Controller = table.Column<string>(nullable: true),
                    Action = table.Column<string>(nullable: true),
                    Icono = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuContenidoVista", x => x.Id_Menu_Contenido_Vista);
                    table.ForeignKey(
                        name: "FK_MenuContenidoVista_MenuCategoriaVista_Id_Menu_Categoria_Vista",
                        column: x => x.Id_Menu_Categoria_Vista,
                        principalTable: "MenuCategoriaVista",
                        principalColumn: "Id_Menu_Categoria_Vista",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MenuRolCategoria",
                columns: table => new
                {
                    Id_Menu_Rol_Categoria = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Id_Menu_Categoria_Vista = table.Column<int>(nullable: false),
                    Rol = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuRolCategoria", x => x.Id_Menu_Rol_Categoria);
                    table.ForeignKey(
                        name: "FK_MenuRolCategoria_MenuCategoriaVista_Id_Menu_Rol_Categoria",
                        column: x => x.Id_Menu_Rol_Categoria,
                        principalTable: "MenuCategoriaVista",
                        principalColumn: "Id_Menu_Categoria_Vista",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MenuRolContenido",
                columns: table => new
                {
                    Id_Menu_Rol_Contenido = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Id_Menu_Contenido_Vista = table.Column<int>(nullable: false),
                    Rol = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuRolContenido", x => x.Id_Menu_Rol_Contenido);
                    table.ForeignKey(
                        name: "FK_MenuRolContenido_MenuContenidoVista_Id_Menu_Rol_Contenido",
                        column: x => x.Id_Menu_Rol_Contenido,
                        principalTable: "MenuContenidoVista",
                        principalColumn: "Id_Menu_Contenido_Vista",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MenuContenidoVista_Id_Menu_Categoria_Vista",
                table: "MenuContenidoVista",
                column: "Id_Menu_Categoria_Vista");

            migrationBuilder.AddForeignKey(
                name: "FK_Efectivo_Denominacion_Efectivo_id_denominacion",
                table: "Efectivo",
                column: "id_denominacion",
                principalTable: "Denominacion_Efectivo",
                principalColumn: "Id_denominacion",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Efectivo_Denominacion_Efectivo_id_denominacion",
                table: "Efectivo");

            migrationBuilder.DropTable(
                name: "MenuRolCategoria");

            migrationBuilder.DropTable(
                name: "MenuRolContenido");

            migrationBuilder.DropTable(
                name: "MenuContenidoVista");

            migrationBuilder.DropTable(
                name: "MenuCategoriaVista");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Denominacion_Efectivo",
                table: "Denominacion_Efectivo");

            migrationBuilder.RenameTable(
                name: "Denominacion_Efectivo",
                newName: "Denominacion_efectivo");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Denominacion_efectivo",
                table: "Denominacion_efectivo",
                column: "Id_denominacion");

            migrationBuilder.AddForeignKey(
                name: "FK_Efectivo_Denominacion_efectivo_id_denominacion",
                table: "Efectivo",
                column: "id_denominacion",
                principalTable: "Denominacion_efectivo",
                principalColumn: "Id_denominacion",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
