using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class fujo_efectivo_agregado : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Denominacion_efectivo",
                columns: table => new
                {
                    Id_denominacion = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    denominacion = table.Column<int>(nullable: false),
                    tipo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Denominacion_efectivo", x => x.Id_denominacion);
                });

            migrationBuilder.CreateTable(
                name: "SaldoDisponible",
                columns: table => new
                {
                    id_saldodisponible = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    fecha = table.Column<DateTime>(nullable: false),
                    saldo = table.Column<decimal>(nullable: false),
                    id_caja = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaldoDisponible", x => x.id_saldodisponible);
                    table.ForeignKey(
                        name: "FK_SaldoDisponible_Caja_id_caja",
                        column: x => x.id_caja,
                        principalTable: "Caja",
                        principalColumn: "Id_Caja",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Efectivo",
                columns: table => new
                {
                    id_flujo = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    id_denominacion = table.Column<int>(nullable: false),
                    id_saldodisponible = table.Column<int>(nullable: false),
                    monto = table.Column<decimal>(nullable: false),
                    cantidad = table.Column<decimal>(nullable: false),
                    id_caja = table.Column<int>(nullable: false),
                    fechaEjercicio = table.Column<DateTime>(nullable: false),
                    fechaCreacion = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Efectivo", x => x.id_flujo);
                    table.ForeignKey(
                        name: "FK_Efectivo_Caja_id_caja",
                        column: x => x.id_caja,
                        principalTable: "Caja",
                        principalColumn: "Id_Caja",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Efectivo_Denominacion_efectivo_id_denominacion",
                        column: x => x.id_denominacion,
                        principalTable: "Denominacion_efectivo",
                        principalColumn: "Id_denominacion",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Efectivo_SaldoDisponible_id_saldodisponible",
                        column: x => x.id_saldodisponible,
                        principalTable: "SaldoDisponible",
                        principalColumn: "id_saldodisponible",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Efectivo_id_caja",
                table: "Efectivo",
                column: "id_caja");

            migrationBuilder.CreateIndex(
                name: "IX_Efectivo_id_denominacion",
                table: "Efectivo",
                column: "id_denominacion");

            migrationBuilder.CreateIndex(
                name: "IX_Efectivo_id_saldodisponible",
                table: "Efectivo",
                column: "id_saldodisponible");

            migrationBuilder.CreateIndex(
                name: "IX_SaldoDisponible_id_caja",
                table: "SaldoDisponible",
                column: "id_caja");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Efectivo");

            migrationBuilder.DropTable(
                name: "Denominacion_efectivo");

            migrationBuilder.DropTable(
                name: "SaldoDisponible");
        }
    }
}
