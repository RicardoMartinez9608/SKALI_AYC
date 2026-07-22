using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class COnfiguración_Calificación : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Buros",
                columns: table => new
                {
                    Id_Buro = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buros", x => x.Id_Buro);
                });

            migrationBuilder.CreateTable(
                name: "Configuracion_Calificacion",
                columns: table => new
                {
                    Id_Configuracion = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Id_Buro = table.Column<int>(nullable: false),
                    Calificacion = table.Column<string>(nullable: true),
                    Dia_Inicio = table.Column<int>(nullable: false),
                    Dia_Fin = table.Column<int>(nullable: false),
                    Saneamiento = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configuracion_Calificacion", x => x.Id_Configuracion);
                    table.ForeignKey(
                        name: "FK_Configuracion_Calificacion_Buros_Id_Buro",
                        column: x => x.Id_Buro,
                        principalTable: "Buros",
                        principalColumn: "Id_Buro",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Calificacion_Crediticias",
                columns: table => new
                {
                    Id_Calificacion_Crediticia = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Id_Persona = table.Column<int>(nullable: false),
                    Id_Credito = table.Column<int>(nullable: false),
                    Id_Configuracion_Calificacion = table.Column<int>(nullable: false),
                    Dias = table.Column<int>(nullable: false),
                    Fecha = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Calificacion_Crediticias", x => x.Id_Calificacion_Crediticia);
                    table.ForeignKey(
                        name: "FK_Calificacion_Crediticias_Configuracion_Calificacion_Id_Confi~",
                        column: x => x.Id_Configuracion_Calificacion,
                        principalTable: "Configuracion_Calificacion",
                        principalColumn: "Id_Configuracion",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Calificacion_Crediticias_Credito_Id_Credito",
                        column: x => x.Id_Credito,
                        principalTable: "Credito",
                        principalColumn: "Id_Credito",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Calificacion_Crediticias_Persona_Id_Persona",
                        column: x => x.Id_Persona,
                        principalTable: "Persona",
                        principalColumn: "Id_Persona",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Calificacion_Crediticias_Id_Configuracion_Calificacion",
                table: "Calificacion_Crediticias",
                column: "Id_Configuracion_Calificacion");

            migrationBuilder.CreateIndex(
                name: "IX_Calificacion_Crediticias_Id_Credito",
                table: "Calificacion_Crediticias",
                column: "Id_Credito");

            migrationBuilder.CreateIndex(
                name: "IX_Calificacion_Crediticias_Id_Persona",
                table: "Calificacion_Crediticias",
                column: "Id_Persona");

            migrationBuilder.CreateIndex(
                name: "IX_Configuracion_Calificacion_Id_Buro",
                table: "Configuracion_Calificacion",
                column: "Id_Buro");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Calificacion_Crediticias");

            migrationBuilder.DropTable(
                name: "Configuracion_Calificacion");

            migrationBuilder.DropTable(
                name: "Buros");
        }
    }
}
