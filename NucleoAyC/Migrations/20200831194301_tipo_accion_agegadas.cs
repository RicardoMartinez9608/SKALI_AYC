using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class tipo_accion_agegadas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id_Tipo_Accion",
                table: "Accion",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Tipo_Accion",
                columns: table => new
                {
                    Id_Tipo_Accion = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Tipo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tipo_Accion", x => x.Id_Tipo_Accion);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accion_Id_Tipo_Accion",
                table: "Accion",
                column: "Id_Tipo_Accion");

            migrationBuilder.AddForeignKey(
                name: "FK_Accion_Tipo_Accion_Id_Tipo_Accion",
                table: "Accion",
                column: "Id_Tipo_Accion",
                principalTable: "Tipo_Accion",
                principalColumn: "Id_Tipo_Accion",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accion_Tipo_Accion_Id_Tipo_Accion",
                table: "Accion");

            migrationBuilder.DropTable(
                name: "Tipo_Accion");

            migrationBuilder.DropIndex(
                name: "IX_Accion_Id_Tipo_Accion",
                table: "Accion");

            migrationBuilder.DropColumn(
                name: "Id_Tipo_Accion",
                table: "Accion");
        }
    }
}
