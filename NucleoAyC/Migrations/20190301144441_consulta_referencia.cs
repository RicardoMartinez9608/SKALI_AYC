using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class consulta_referencia : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Evaluacion_Referencia",
                columns: table => new
                {
                    Id_Evaluacion_Referencia_Catalogo = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nombre_Evaluacion = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Evaluacion_Referencia", x => x.Id_Evaluacion_Referencia_Catalogo);
                });

            migrationBuilder.CreateTable(
                name: "Pregunta",
                columns: table => new
                {
                    Id_Pregunta = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Id_Referencia_Evaluada_Catalogo = table.Column<int>(nullable: false),
                    Contenido = table.Column<string>(nullable: true),
                    Ponderacion = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pregunta", x => x.Id_Pregunta);
                    table.ForeignKey(
                        name: "FK_Pregunta_Evaluacion_Referencia_Id_Referencia_Evaluada_Catalo~",
                        column: x => x.Id_Referencia_Evaluada_Catalogo,
                        principalTable: "Evaluacion_Referencia",
                        principalColumn: "Id_Evaluacion_Referencia_Catalogo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Respuesta",
                columns: table => new
                {
                    Id_Respuesta = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Id_Pregunta = table.Column<int>(nullable: false),
                    Contenido = table.Column<string>(nullable: true),
                    Ponderacion = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Respuesta", x => x.Id_Respuesta);
                    table.ForeignKey(
                        name: "FK_Respuesta_Pregunta_Id_Pregunta",
                        column: x => x.Id_Pregunta,
                        principalTable: "Pregunta",
                        principalColumn: "Id_Pregunta",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Evaluacion_Referencia_Persona",
                columns: table => new
                {
                    Id_Evaluacion_Referencia_Persona = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Id_Persona = table.Column<int>(nullable: false),
                    Id_Referencia = table.Column<int>(nullable: false),
                    Id_Evaluacion_Referencia_Catalogo = table.Column<int>(nullable: false),
                    Id_Pregunta = table.Column<int>(nullable: false),
                    Id_Respuesta = table.Column<int>(nullable: false),
                    Ponderacion = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Evaluacion_Referencia_Persona", x => x.Id_Evaluacion_Referencia_Persona);
                    table.ForeignKey(
                        name: "FK_Evaluacion_Referencia_Persona_Evaluacion_Referencia_Id_Evalu~",
                        column: x => x.Id_Evaluacion_Referencia_Catalogo,
                        principalTable: "Evaluacion_Referencia",
                        principalColumn: "Id_Evaluacion_Referencia_Catalogo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Evaluacion_Referencia_Persona_Persona_Id_Persona",
                        column: x => x.Id_Persona,
                        principalTable: "Persona",
                        principalColumn: "Id_Persona",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Evaluacion_Referencia_Persona_Pregunta_Id_Pregunta",
                        column: x => x.Id_Pregunta,
                        principalTable: "Pregunta",
                        principalColumn: "Id_Pregunta",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Evaluacion_Referencia_Persona_Referencia_Id_Referencia",
                        column: x => x.Id_Referencia,
                        principalTable: "Referencia",
                        principalColumn: "Id_Referencia",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Evaluacion_Referencia_Persona_Respuesta_Id_Respuesta",
                        column: x => x.Id_Respuesta,
                        principalTable: "Respuesta",
                        principalColumn: "Id_Respuesta",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Evaluacion_Referencia_Persona_Id_Evaluacion_Referencia_Catal~",
                table: "Evaluacion_Referencia_Persona",
                column: "Id_Evaluacion_Referencia_Catalogo");

            migrationBuilder.CreateIndex(
                name: "IX_Evaluacion_Referencia_Persona_Id_Persona",
                table: "Evaluacion_Referencia_Persona",
                column: "Id_Persona");

            migrationBuilder.CreateIndex(
                name: "IX_Evaluacion_Referencia_Persona_Id_Pregunta",
                table: "Evaluacion_Referencia_Persona",
                column: "Id_Pregunta");

            migrationBuilder.CreateIndex(
                name: "IX_Evaluacion_Referencia_Persona_Id_Referencia",
                table: "Evaluacion_Referencia_Persona",
                column: "Id_Referencia");

            migrationBuilder.CreateIndex(
                name: "IX_Evaluacion_Referencia_Persona_Id_Respuesta",
                table: "Evaluacion_Referencia_Persona",
                column: "Id_Respuesta");

            migrationBuilder.CreateIndex(
                name: "IX_Pregunta_Id_Referencia_Evaluada_Catalogo",
                table: "Pregunta",
                column: "Id_Referencia_Evaluada_Catalogo");

            migrationBuilder.CreateIndex(
                name: "IX_Respuesta_Id_Pregunta",
                table: "Respuesta",
                column: "Id_Pregunta");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Evaluacion_Referencia_Persona");

            migrationBuilder.DropTable(
                name: "Respuesta");

            migrationBuilder.DropTable(
                name: "Pregunta");

            migrationBuilder.DropTable(
                name: "Evaluacion_Referencia");
        }
    }
}
