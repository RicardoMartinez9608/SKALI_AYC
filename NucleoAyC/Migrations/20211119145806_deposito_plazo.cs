using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class deposito_plazo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Deposito_Plazo",
                columns: table => new
                {
                    Id_Deposito_Plazo = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Id_Persona = table.Column<int>(nullable: false),
                    No_Ahorro = table.Column<string>(nullable: true),
                    Fecha_Apertura = table.Column<DateTime>(nullable: false),
                    Fecha_Vencimiento = table.Column<DateTime>(nullable: false),
                    Monto = table.Column<decimal>(nullable: false),
                    Interes = table.Column<decimal>(nullable: false),
                    Plazo = table.Column<int>(nullable: false),
                    Id_Ahorro = table.Column<int>(nullable: false),
                    No_Certificado = table.Column<string>(nullable: true),
                    Pagos_Interes_Mensual = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deposito_Plazo", x => x.Id_Deposito_Plazo);
                    table.ForeignKey(
                        name: "FK_Deposito_Plazo_Ahorro_Id_Ahorro",
                        column: x => x.Id_Ahorro,
                        principalTable: "Ahorro",
                        principalColumn: "Id_Ahorro",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Deposito_Plazo_Persona_Id_Persona",
                        column: x => x.Id_Persona,
                        principalTable: "Persona",
                        principalColumn: "Id_Persona",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Deposito_Plazo_Id_Ahorro",
                table: "Deposito_Plazo",
                column: "Id_Ahorro");

            migrationBuilder.CreateIndex(
                name: "IX_Deposito_Plazo_Id_Persona",
                table: "Deposito_Plazo",
                column: "Id_Persona");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Deposito_Plazo");
        }
    }
}
