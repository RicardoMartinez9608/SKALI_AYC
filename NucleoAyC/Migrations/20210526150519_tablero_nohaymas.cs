using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class tablero_nohaymas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cosecha",
                columns: table => new
                {
                    Id_Cosecha = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Id_Cartera = table.Column<int>(nullable: false),
                    Saldo = table.Column<decimal>(nullable: false),
                    Fecha = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cosecha", x => x.Id_Cosecha);
                    table.ForeignKey(
                        name: "FK_Cosecha_Cartera_Id_Cartera",
                        column: x => x.Id_Cartera,
                        principalTable: "Cartera",
                        principalColumn: "Id_Cartera",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Meta",
                columns: table => new
                {
                    Id_Meta = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Id_Cartera = table.Column<int>(nullable: false),
                    Colecta = table.Column<decimal>(nullable: false),
                    Capital = table.Column<decimal>(nullable: false),
                    Cosecha = table.Column<decimal>(nullable: false),
                    No_Clientes = table.Column<int>(nullable: false),
                    Colocacion = table.Column<decimal>(nullable: false),
                    Ingresos = table.Column<decimal>(nullable: false),
                    Mora = table.Column<decimal>(nullable: false),
                    Mes = table.Column<int>(nullable: false),
                    Anyo = table.Column<int>(nullable: false),
                    Creado_En = table.Column<DateTime>(nullable: false),
                    Creado_Por = table.Column<string>(nullable: true),
                    Modificado_En = table.Column<DateTime>(nullable: false),
                    Modificado_Por = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meta", x => x.Id_Meta);
                    table.ForeignKey(
                        name: "FK_Meta_Cartera_Id_Cartera",
                        column: x => x.Id_Cartera,
                        principalTable: "Cartera",
                        principalColumn: "Id_Cartera",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rancking",
                columns: table => new
                {
                    Id_Rancking = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Id_Cartera = table.Column<int>(nullable: false),
                    Saldo_Cartera = table.Column<decimal>(nullable: false),
                    Valor_Pagado = table.Column<decimal>(nullable: false),
                    Refinanciado = table.Column<decimal>(nullable: false),
                    Abono_Capital = table.Column<decimal>(nullable: false),
                    Capital_Mora = table.Column<decimal>(nullable: false),
                    Ingresos = table.Column<decimal>(nullable: false),
                    Desembolso = table.Column<decimal>(nullable: false),
                    No_Clientes_Activos = table.Column<int>(nullable: false),
                    No_Creditos_Activos = table.Column<int>(nullable: false),
                    Fecha = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rancking", x => x.Id_Rancking);
                    table.ForeignKey(
                        name: "FK_Rancking_Cartera_Id_Cartera",
                        column: x => x.Id_Cartera,
                        principalTable: "Cartera",
                        principalColumn: "Id_Cartera",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cosecha_Id_Cartera",
                table: "Cosecha",
                column: "Id_Cartera");

            migrationBuilder.CreateIndex(
                name: "IX_Meta_Id_Cartera",
                table: "Meta",
                column: "Id_Cartera");

            migrationBuilder.CreateIndex(
                name: "IX_Rancking_Id_Cartera",
                table: "Rancking",
                column: "Id_Cartera");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cosecha");

            migrationBuilder.DropTable(
                name: "Meta");

            migrationBuilder.DropTable(
                name: "Rancking");
        }
    }
}
