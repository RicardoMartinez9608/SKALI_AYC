using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class parentesco_autrizad : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Autorizado",
                columns: table => new
                {
                    Id_Autorizado = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Id_Deposito_Plazo = table.Column<int>(nullable: false),
                    Nombre = table.Column<string>(nullable: true),
                    Dui = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Autorizado", x => x.Id_Autorizado);
                    table.ForeignKey(
                        name: "FK_Autorizado_Deposito_Plazo_Id_Deposito_Plazo",
                        column: x => x.Id_Deposito_Plazo,
                        principalTable: "Deposito_Plazo",
                        principalColumn: "Id_Deposito_Plazo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Parentesco",
                columns: table => new
                {
                    Id_Parentesco = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Descripcion = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parentesco", x => x.Id_Parentesco);
                });

            migrationBuilder.CreateTable(
                name: "Beneficiarios_Deposito_Plazo",
                columns: table => new
                {
                    Id_Beneficiario_Deposito_Plazo = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Id_Deposito_Plazo = table.Column<int>(nullable: false),
                    Id_Parentesco = table.Column<int>(nullable: false),
                    Nombre = table.Column<string>(nullable: true),
                    Edad = table.Column<int>(nullable: false),
                    Sexo = table.Column<string>(nullable: true),
                    Fecha_Nacimiento = table.Column<DateTime>(nullable: false),
                    Residencia = table.Column<string>(nullable: true),
                    Telefono = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Beneficiarios_Deposito_Plazo", x => x.Id_Beneficiario_Deposito_Plazo);
                    table.ForeignKey(
                        name: "FK_Beneficiarios_Deposito_Plazo_Deposito_Plazo_Id_Deposito_Plazo",
                        column: x => x.Id_Deposito_Plazo,
                        principalTable: "Deposito_Plazo",
                        principalColumn: "Id_Deposito_Plazo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Beneficiarios_Deposito_Plazo_Parentesco_Id_Parentesco",
                        column: x => x.Id_Parentesco,
                        principalTable: "Parentesco",
                        principalColumn: "Id_Parentesco",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Autorizado_Id_Deposito_Plazo",
                table: "Autorizado",
                column: "Id_Deposito_Plazo");

            migrationBuilder.CreateIndex(
                name: "IX_Beneficiarios_Deposito_Plazo_Id_Deposito_Plazo",
                table: "Beneficiarios_Deposito_Plazo",
                column: "Id_Deposito_Plazo");

            migrationBuilder.CreateIndex(
                name: "IX_Beneficiarios_Deposito_Plazo_Id_Parentesco",
                table: "Beneficiarios_Deposito_Plazo",
                column: "Id_Parentesco");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Autorizado");

            migrationBuilder.DropTable(
                name: "Beneficiarios_Deposito_Plazo");

            migrationBuilder.DropTable(
                name: "Parentesco");
        }
    }
}
