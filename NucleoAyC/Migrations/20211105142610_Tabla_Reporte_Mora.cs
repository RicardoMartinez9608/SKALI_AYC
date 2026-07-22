using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class Tabla_Reporte_Mora : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Reporte_Mora",
                columns: table => new
                {
                    Id_Reporte_Mora = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Id_Cartera = table.Column<int>(nullable: false),
                    Codigo = table.Column<string>(nullable: true),
                    Nombre_Cliente = table.Column<string>(nullable: true),
                    Saldo_Mora = table.Column<decimal>(nullable: false),
                    Fecha_Vencimiento = table.Column<DateTime>(nullable: false),
                    Fecha_Ultimo_Movimiento = table.Column<DateTime>(nullable: false),
                    Dias_Mora = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reporte_Mora", x => x.Id_Reporte_Mora);
                    table.ForeignKey(
                        name: "FK_Reporte_Mora_Cartera_Id_Cartera",
                        column: x => x.Id_Cartera,
                        principalTable: "Cartera",
                        principalColumn: "Id_Cartera",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reporte_Mora_Id_Cartera",
                table: "Reporte_Mora",
                column: "Id_Cartera");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reporte_Mora");
        }
    }
}
