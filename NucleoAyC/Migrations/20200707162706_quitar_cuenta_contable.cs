using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class quitar_cuenta_contable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Conf_Detalle_Partida_Contable_Cuenta_Contable_Id_Cuenta_Cont",
                table: "Conf_Detalle_Partida_Contable");

            migrationBuilder.DropTable(
                name: "Cuenta_Contable");

            migrationBuilder.DropIndex(
                name: "IX_Conf_Detalle_Partida_Contable_Id_Cuenta_Contable",
                table: "Conf_Detalle_Partida_Contable");

            migrationBuilder.DropColumn(
                name: "Id_Cuenta_Contable",
                table: "Conf_Detalle_Partida_Contable");

            migrationBuilder.AddColumn<string>(
                name: "Cuenta_Contable",
                table: "Conf_Detalle_Partida_Contable",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cuenta_Contable",
                table: "Conf_Detalle_Partida_Contable");

            migrationBuilder.AddColumn<int>(
                name: "Id_Cuenta_Contable",
                table: "Conf_Detalle_Partida_Contable",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Cuenta_Contable",
                columns: table => new
                {
                    Id_Cuenta_Contable = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Codigo = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Descripcion = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cuenta_Contable", x => x.Id_Cuenta_Contable);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Conf_Detalle_Partida_Contable_Id_Cuenta_Contable",
                table: "Conf_Detalle_Partida_Contable",
                column: "Id_Cuenta_Contable");

            migrationBuilder.AddForeignKey(
                name: "FK_Conf_Detalle_Partida_Contable_Cuenta_Contable_Id_Cuenta_Cont~",
                table: "Conf_Detalle_Partida_Contable",
                column: "Id_Cuenta_Contable",
                principalTable: "Cuenta_Contable",
                principalColumn: "Id_Cuenta_Contable",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
