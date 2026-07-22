using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class transaccion_deposito_plazo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Transaccion_Deposito_Plazo",
                columns: table => new
                {
                    Id_Transaccion_Deposito_Plazo = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Id_Deposito_Plazo = table.Column<int>(nullable: false),
                    Id_Tipo_Operacion_Ahorro = table.Column<int>(nullable: false),
                    Valor = table.Column<decimal>(nullable: false),
                    Saldo = table.Column<decimal>(nullable: false),
                    Dias = table.Column<int>(nullable: false),
                    Fecha = table.Column<DateTime>(nullable: false),
                    Interes_Aplicado = table.Column<decimal>(nullable: false),
                    Interes_Acumulado = table.Column<decimal>(nullable: false),
                    Borrado_Logico = table.Column<bool>(nullable: false),
                    Creado_En = table.Column<DateTime>(nullable: false),
                    Eliminado_En = table.Column<DateTime>(nullable: false),
                    Modificado_En = table.Column<DateTime>(nullable: false),
                    Creado_Por = table.Column<string>(nullable: true),
                    Eliminado_Por = table.Column<string>(nullable: true),
                    Modificado_Por = table.Column<string>(nullable: true),
                    Modificado = table.Column<bool>(nullable: false),
                    Id_Libreta = table.Column<int>(nullable: false),
                    No_Renglon = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaccion_Deposito_Plazo", x => x.Id_Transaccion_Deposito_Plazo);
                    table.ForeignKey(
                        name: "FK_Transaccion_Deposito_Plazo_Deposito_Plazo_Id_Deposito_Plazo",
                        column: x => x.Id_Deposito_Plazo,
                        principalTable: "Deposito_Plazo",
                        principalColumn: "Id_Deposito_Plazo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transaccion_Deposito_Plazo_Tipo_Operacion_Ahorro_Id_Tipo_Ope~",
                        column: x => x.Id_Tipo_Operacion_Ahorro,
                        principalTable: "Tipo_Operacion_Ahorro",
                        principalColumn: "Id_Tipo_Operacion_Ahorro",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transaccion_Deposito_Plazo_Id_Deposito_Plazo",
                table: "Transaccion_Deposito_Plazo",
                column: "Id_Deposito_Plazo");

            migrationBuilder.CreateIndex(
                name: "IX_Transaccion_Deposito_Plazo_Id_Tipo_Operacion_Ahorro",
                table: "Transaccion_Deposito_Plazo",
                column: "Id_Tipo_Operacion_Ahorro");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transaccion_Deposito_Plazo");
        }
    }
}
