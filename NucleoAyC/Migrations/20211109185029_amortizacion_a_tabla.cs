using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class amortizacion_a_tabla : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Amortizacion",
                columns: table => new
                {
                    No = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Id_Credito = table.Column<int>(nullable: false),
                    FechaPago = table.Column<DateTime>(nullable: false),
                    Fecha_Amortizacion = table.Column<DateTime>(nullable: false),
                    Dias = table.Column<int>(nullable: false),
                    Interes = table.Column<decimal>(nullable: false),
                    Comision = table.Column<decimal>(nullable: false),
                    IvaSobreComision = table.Column<decimal>(nullable: false),
                    Otros_Cobros = table.Column<decimal>(nullable: false),
                    Abono_capital = table.Column<decimal>(nullable: false),
                    Valor_Cuota = table.Column<decimal>(nullable: false),
                    Saldo_Capital = table.Column<decimal>(nullable: false),
                    Pago_para_cancelar = table.Column<decimal>(nullable: false),
                    Recargo = table.Column<decimal>(nullable: false),
                    fecha_anterior = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Amortizacion", x => x.No);
                    table.ForeignKey(
                        name: "FK_Amortizacion_Credito_Id_Credito",
                        column: x => x.Id_Credito,
                        principalTable: "Credito",
                        principalColumn: "Id_Credito",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Amortizacion_Id_Credito",
                table: "Amortizacion",
                column: "Id_Credito");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Amortizacion");
        }
    }
}
