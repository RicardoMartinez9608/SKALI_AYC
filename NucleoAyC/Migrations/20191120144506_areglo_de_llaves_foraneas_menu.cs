using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class areglo_de_llaves_foraneas_menu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MenuRolCategoria_MenuCategoriaVista_Id_Menu_Rol_Categoria",
                table: "MenuRolCategoria");

            migrationBuilder.DropForeignKey(
                name: "FK_MenuRolContenido_MenuContenidoVista_Id_Menu_Rol_Contenido",
                table: "MenuRolContenido");

            migrationBuilder.CreateIndex(
                name: "IX_MenuRolContenido_Id_Menu_Contenido_Vista",
                table: "MenuRolContenido",
                column: "Id_Menu_Contenido_Vista");

            migrationBuilder.CreateIndex(
                name: "IX_MenuRolCategoria_Id_Menu_Categoria_Vista",
                table: "MenuRolCategoria",
                column: "Id_Menu_Categoria_Vista");

            migrationBuilder.AddForeignKey(
                name: "FK_MenuRolCategoria_MenuCategoriaVista_Id_Menu_Categoria_Vista",
                table: "MenuRolCategoria",
                column: "Id_Menu_Categoria_Vista",
                principalTable: "MenuCategoriaVista",
                principalColumn: "Id_Menu_Categoria_Vista",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MenuRolContenido_MenuContenidoVista_Id_Menu_Contenido_Vista",
                table: "MenuRolContenido",
                column: "Id_Menu_Contenido_Vista",
                principalTable: "MenuContenidoVista",
                principalColumn: "Id_Menu_Contenido_Vista",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MenuRolCategoria_MenuCategoriaVista_Id_Menu_Categoria_Vista",
                table: "MenuRolCategoria");

            migrationBuilder.DropForeignKey(
                name: "FK_MenuRolContenido_MenuContenidoVista_Id_Menu_Contenido_Vista",
                table: "MenuRolContenido");

            migrationBuilder.DropIndex(
                name: "IX_MenuRolContenido_Id_Menu_Contenido_Vista",
                table: "MenuRolContenido");

            migrationBuilder.DropIndex(
                name: "IX_MenuRolCategoria_Id_Menu_Categoria_Vista",
                table: "MenuRolCategoria");

            migrationBuilder.AddForeignKey(
                name: "FK_MenuRolCategoria_MenuCategoriaVista_Id_Menu_Rol_Categoria",
                table: "MenuRolCategoria",
                column: "Id_Menu_Rol_Categoria",
                principalTable: "MenuCategoriaVista",
                principalColumn: "Id_Menu_Categoria_Vista",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MenuRolContenido_MenuContenidoVista_Id_Menu_Rol_Contenido",
                table: "MenuRolContenido",
                column: "Id_Menu_Rol_Contenido",
                principalTable: "MenuContenidoVista",
                principalColumn: "Id_Menu_Contenido_Vista",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
