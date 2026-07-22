using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class Comite_Ampliado : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Comite_Ampliados",
                columns: table => new
                {
                    Id_Comite_Ampliado = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Id_Credito = table.Column<int>(nullable: false),
                    Creado_En = table.Column<DateTime>(nullable: false),
                    Creado_Por = table.Column<string>(nullable: true),
                    Aprobado_En = table.Column<DateTime>(nullable: false),
                    Aprobado_Por = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comite_Ampliados", x => x.Id_Comite_Ampliado);
                    table.ForeignKey(
                        name: "FK_Comite_Ampliados_Credito_Id_Credito",
                        column: x => x.Id_Credito,
                        principalTable: "Credito",
                        principalColumn: "Id_Credito",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Fotografia_Comites",
                columns: table => new
                {
                    Id_Fotografia_Comite = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Id_Comite_Ampliado = table.Column<int>(nullable: false),
                    Foto = table.Column<string>(nullable: true),
                    Descripcion = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fotografia_Comites", x => x.Id_Fotografia_Comite);
                    table.ForeignKey(
                        name: "FK_Fotografia_Comites_Comite_Ampliados_Id_Comite_Ampliado",
                        column: x => x.Id_Comite_Ampliado,
                        principalTable: "Comite_Ampliados",
                        principalColumn: "Id_Comite_Ampliado",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comite_Ampliados_Id_Credito",
                table: "Comite_Ampliados",
                column: "Id_Credito");

            migrationBuilder.CreateIndex(
                name: "IX_Fotografia_Comites_Id_Comite_Ampliado",
                table: "Fotografia_Comites",
                column: "Id_Comite_Ampliado");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Fotografia_Comites");

            migrationBuilder.DropTable(
                name: "Comite_Ampliados");
        }
    }
}
