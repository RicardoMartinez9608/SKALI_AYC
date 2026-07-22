using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class para_lavado_dnero : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Cargo_Empleo",
                table: "Referencia",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Direccion_Empleo",
                table: "Referencia",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Direccion_Estudio",
                table: "Referencia",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Esta_Estudiando",
                table: "Referencia",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Esta_Trabajando",
                table: "Referencia",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Explicacion_Residencia",
                table: "Referencia",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nombre_Jefe_Inmediato",
                table: "Referencia",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Salario_Empleo",
                table: "Referencia",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Telefono_Empleo",
                table: "Referencia",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Catalogo_Cuestionario",
                columns: table => new
                {
                    Id_Catalogo_Cuestionario = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nombre_Evaluado = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Catalogo_Cuestionario", x => x.Id_Catalogo_Cuestionario);
                });

            migrationBuilder.CreateTable(
                name: "Referencia_Adicional",
                columns: table => new
                {
                    Id_Referencia_Adicional = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Id_Referencia = table.Column<int>(nullable: false),
                    Parentesco = table.Column<string>(nullable: true),
                    Tipo_Referencia = table.Column<string>(nullable: true),
                    Telefono = table.Column<string>(nullable: true),
                    Residencia = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Referencia_Adicional", x => x.Id_Referencia_Adicional);
                    table.ForeignKey(
                        name: "FK_Referencia_Adicional_Referencia_Id_Referencia",
                        column: x => x.Id_Referencia,
                        principalTable: "Referencia",
                        principalColumn: "Id_Referencia",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pregunta_Catalogo",
                columns: table => new
                {
                    Id_Pregunta_Catalogo = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Id_Catalogo_Cuestionario = table.Column<int>(nullable: false),
                    Contenido_Pregunta = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pregunta_Catalogo", x => x.Id_Pregunta_Catalogo);
                    table.ForeignKey(
                        name: "FK_Pregunta_Catalogo_Catalogo_Cuestionario_Id_Catalogo_Cuestion~",
                        column: x => x.Id_Catalogo_Cuestionario,
                        principalTable: "Catalogo_Cuestionario",
                        principalColumn: "Id_Catalogo_Cuestionario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Respuesta_Catalogo",
                columns: table => new
                {
                    Id_Respuesta_Catalogo = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Id_Pregunta_Catalogo = table.Column<int>(nullable: false),
                    Es_Respuesta_Libre = table.Column<bool>(nullable: false),
                    Contenido_Respuesta_Catalogo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Respuesta_Catalogo", x => x.Id_Respuesta_Catalogo);
                    table.ForeignKey(
                        name: "FK_Respuesta_Catalogo_Pregunta_Catalogo_Id_Pregunta_Catalogo",
                        column: x => x.Id_Pregunta_Catalogo,
                        principalTable: "Pregunta_Catalogo",
                        principalColumn: "Id_Pregunta_Catalogo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Catalogo_Socio",
                columns: table => new
                {
                    Id_Catalogo_Socio = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Id_Catalogo_Cuestionario = table.Column<int>(nullable: false),
                    Id_Persona = table.Column<int>(nullable: false),
                    Id_Pregunta_Catalogo = table.Column<int>(nullable: false),
                    Id_Respuesta_Catalogo = table.Column<int>(nullable: false),
                    Respuesta_Libre = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Catalogo_Socio", x => x.Id_Catalogo_Socio);
                    table.ForeignKey(
                        name: "FK_Catalogo_Socio_Catalogo_Cuestionario_Id_Catalogo_Cuestionario",
                        column: x => x.Id_Catalogo_Cuestionario,
                        principalTable: "Catalogo_Cuestionario",
                        principalColumn: "Id_Catalogo_Cuestionario",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Catalogo_Socio_Persona_Id_Persona",
                        column: x => x.Id_Persona,
                        principalTable: "Persona",
                        principalColumn: "Id_Persona",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Catalogo_Socio_Pregunta_Catalogo_Id_Pregunta_Catalogo",
                        column: x => x.Id_Pregunta_Catalogo,
                        principalTable: "Pregunta_Catalogo",
                        principalColumn: "Id_Pregunta_Catalogo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Catalogo_Socio_Respuesta_Catalogo_Id_Respuesta_Catalogo",
                        column: x => x.Id_Respuesta_Catalogo,
                        principalTable: "Respuesta_Catalogo",
                        principalColumn: "Id_Respuesta_Catalogo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Catalogo_Socio_Id_Catalogo_Cuestionario",
                table: "Catalogo_Socio",
                column: "Id_Catalogo_Cuestionario");

            migrationBuilder.CreateIndex(
                name: "IX_Catalogo_Socio_Id_Persona",
                table: "Catalogo_Socio",
                column: "Id_Persona");

            migrationBuilder.CreateIndex(
                name: "IX_Catalogo_Socio_Id_Pregunta_Catalogo",
                table: "Catalogo_Socio",
                column: "Id_Pregunta_Catalogo");

            migrationBuilder.CreateIndex(
                name: "IX_Catalogo_Socio_Id_Respuesta_Catalogo",
                table: "Catalogo_Socio",
                column: "Id_Respuesta_Catalogo");

            migrationBuilder.CreateIndex(
                name: "IX_Pregunta_Catalogo_Id_Catalogo_Cuestionario",
                table: "Pregunta_Catalogo",
                column: "Id_Catalogo_Cuestionario");

            migrationBuilder.CreateIndex(
                name: "IX_Referencia_Adicional_Id_Referencia",
                table: "Referencia_Adicional",
                column: "Id_Referencia");

            migrationBuilder.CreateIndex(
                name: "IX_Respuesta_Catalogo_Id_Pregunta_Catalogo",
                table: "Respuesta_Catalogo",
                column: "Id_Pregunta_Catalogo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Catalogo_Socio");

            migrationBuilder.DropTable(
                name: "Referencia_Adicional");

            migrationBuilder.DropTable(
                name: "Respuesta_Catalogo");

            migrationBuilder.DropTable(
                name: "Pregunta_Catalogo");

            migrationBuilder.DropTable(
                name: "Catalogo_Cuestionario");

            migrationBuilder.DropColumn(
                name: "Cargo_Empleo",
                table: "Referencia");

            migrationBuilder.DropColumn(
                name: "Direccion_Empleo",
                table: "Referencia");

            migrationBuilder.DropColumn(
                name: "Direccion_Estudio",
                table: "Referencia");

            migrationBuilder.DropColumn(
                name: "Esta_Estudiando",
                table: "Referencia");

            migrationBuilder.DropColumn(
                name: "Esta_Trabajando",
                table: "Referencia");

            migrationBuilder.DropColumn(
                name: "Explicacion_Residencia",
                table: "Referencia");

            migrationBuilder.DropColumn(
                name: "Nombre_Jefe_Inmediato",
                table: "Referencia");

            migrationBuilder.DropColumn(
                name: "Salario_Empleo",
                table: "Referencia");

            migrationBuilder.DropColumn(
                name: "Telefono_Empleo",
                table: "Referencia");
        }
    }
}
