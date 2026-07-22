using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace NucleoAyC.Migrations
{
    public partial class devastaciontotal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<int>(
            //    name: "Id_Linea_Credito_Historial",
            //    table: "Credito",
            //    type: "int",
            //    nullable: true);

            migrationBuilder.CreateTable(
                name: "Linea_Credito_Historial",
                columns: table => new
                {
                    Id_Linea_Credito_Historial = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Activar_Deducciones = table.Column<bool>(type: "bit", nullable: false),
                    Codigo = table.Column<string>(type: "longtext", nullable: true),
                    Descripcion_linea = table.Column<string>(type: "longtext", nullable: true),
                    Destino_Credito = table.Column<int>(type: "int", nullable: false),
                    Dia_Gracia = table.Column<int>(type: "int", nullable: false),
                    Estructuracion = table.Column<decimal>(type: "decimal(65, 30)", nullable: false),
                    Id_Credito = table.Column<int>(type: "int", nullable: true),
                    Monto_Rango_Final = table.Column<decimal>(type: "decimal(65, 30)", nullable: false),
                    Monto_Rango_Inicial = table.Column<decimal>(type: "decimal(65, 30)", nullable: false),
                    PlanAyC = table.Column<decimal>(type: "decimal(65, 30)", nullable: false),
                    PlanAyC_Cobros_Periodico_Dinero = table.Column<bool>(type: "bit", nullable: false),
                    Plazo_Rango_Final = table.Column<int>(type: "int", nullable: false),
                    Plazo_Rango_Inicial = table.Column<int>(type: "int", nullable: false),
                    Recargo = table.Column<decimal>(type: "decimal(65, 30)", nullable: false),
                    Tasa = table.Column<decimal>(type: "decimal(65, 30)", nullable: false),
                    Tasa_Mora = table.Column<decimal>(type: "decimal(65, 30)", nullable: false),
                    Tipo_Garantia = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Linea_Credito_Historial", x => x.Id_Linea_Credito_Historial);
                    table.ForeignKey(
                        name: "FK_Linea_Credito_Historial_Credito_Id_Credito",
                        column: x => x.Id_Credito,
                        principalTable: "Credito",
                        principalColumn: "Id_Credito",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cobro_Periodico_Historial",
                columns: table => new
                {
                    Id_Cobro_Periodico_Historial = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Descripcion = table.Column<string>(type: "longtext", nullable: true),
                    Id_Linea_Credito_Historial = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "longtext", nullable: true),
                    Valor = table.Column<decimal>(type: "decimal(65, 30)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cobro_Periodico_Historial", x => x.Id_Cobro_Periodico_Historial);
                    table.ForeignKey(
                        name: "FK_Cobro_Periodico_Historial_Linea_Credito_Historial_Id_Linea_Credito_Historial",
                        column: x => x.Id_Linea_Credito_Historial,
                        principalTable: "Linea_Credito_Historial",
                        principalColumn: "Id_Linea_Credito_Historial",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Deduccion_Historial",
                columns: table => new
                {
                    Id_Deduccion_Historial = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Descripcion = table.Column<string>(type: "longtext", nullable: true),
                    Id_Linea_Credito_Historial = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "longtext", nullable: true),
                    Valor = table.Column<decimal>(type: "decimal(65, 30)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deduccion_Historial", x => x.Id_Deduccion_Historial);
                    table.ForeignKey(
                        name: "FK_Deduccion_Historial_Linea_Credito_Historial_Id_Linea_Credito_Historial",
                        column: x => x.Id_Linea_Credito_Historial,
                        principalTable: "Linea_Credito_Historial",
                        principalColumn: "Id_Linea_Credito_Historial",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cobro_Periodico_Historial_Id_Linea_Credito_Historial",
                table: "Cobro_Periodico_Historial",
                column: "Id_Linea_Credito_Historial");

            migrationBuilder.CreateIndex(
                name: "IX_Deduccion_Historial_Id_Linea_Credito_Historial",
                table: "Deduccion_Historial",
                column: "Id_Linea_Credito_Historial");

            migrationBuilder.CreateIndex(
                name: "IX_Linea_Credito_Historial_Id_Credito",
                table: "Linea_Credito_Historial",
                column: "Id_Credito",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cobro_Periodico_Historial");

            migrationBuilder.DropTable(
                name: "Deduccion_Historial");

            migrationBuilder.DropTable(
                name: "Linea_Credito_Historial");

            migrationBuilder.DropColumn(
                name: "Id_Linea_Credito_Historial",
                table: "Credito");
        }
    }
}
