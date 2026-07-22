using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class registro_llamada : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Llamada_Telefonica_Referencia",
                columns: table => new
                {
                    Id_Llamada_Telefonica_Referencia = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Id_Referencia = table.Column<int>(nullable: false),
                    Registrado_Para = table.Column<DateTime>(nullable: false),
                    Usuario = table.Column<string>(nullable: true),
                    Efectuado_En = table.Column<DateTime>(nullable: false),
                    Intentos = table.Column<int>(nullable: false),
                    Estado = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Llamada_Telefonica_Referencia", x => x.Id_Llamada_Telefonica_Referencia);
                    table.ForeignKey(
                        name: "FK_Llamada_Telefonica_Referencia_Referencia_Id_Referencia",
                        column: x => x.Id_Referencia,
                        principalTable: "Referencia",
                        principalColumn: "Id_Referencia",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Llamada_Telefonica_Referencia_Id_Referencia",
                table: "Llamada_Telefonica_Referencia",
                column: "Id_Referencia");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Llamada_Telefonica_Referencia");
        }
    }
}
