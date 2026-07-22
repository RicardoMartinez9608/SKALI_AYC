using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class condoncionordendo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id_Linea_Deposito_Plazo",
                table: "Deposito_Plazo",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Condonacion",
                columns: table => new
                {
                    Id_Condonacione = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Id_Credito = table.Column<int>(nullable: false),
                    Fecha = table.Column<DateTime>(nullable: false),
                    Id_Usuario = table.Column<string>(nullable: true),
                    Condonar_Penalizacion = table.Column<bool>(nullable: false),
                    Valor_Dinero_Penalizacion = table.Column<decimal>(nullable: false),
                    Condonar_Recargos = table.Column<bool>(nullable: false),
                    Valor_Dinero_Recargos = table.Column<decimal>(nullable: false),
                    Permitir_Gracia = table.Column<bool>(nullable: false),
                    Valor_Dinero_Gracia = table.Column<decimal>(nullable: false),
                    Lapso_Tiempo = table.Column<string>(nullable: true),
                    Permitir_Gracia_Moratoria = table.Column<bool>(nullable: false),
                    Valor_Dinero_Gracia_Moratoria = table.Column<decimal>(nullable: false),
                    Lapso_Tiempo_Moratoria = table.Column<string>(nullable: true),
                    Pago_Antes = table.Column<decimal>(nullable: false),
                    Condonacion_Total = table.Column<decimal>(nullable: false),
                    Pago_Despues = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Condonacion", x => x.Id_Condonacione);
                    table.ForeignKey(
                        name: "FK_Condonacion_Credito_Id_Credito",
                        column: x => x.Id_Credito,
                        principalTable: "Credito",
                        principalColumn: "Id_Credito",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Linea_Deposito_Plazo",
                columns: table => new
                {
                    Id_Linea_Deposito_Plazo = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Descripcíon = table.Column<string>(nullable: true),
                    Interes = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Linea_Deposito_Plazo", x => x.Id_Linea_Deposito_Plazo);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Deposito_Plazo_Id_Linea_Deposito_Plazo",
                table: "Deposito_Plazo",
                column: "Id_Linea_Deposito_Plazo");

            migrationBuilder.CreateIndex(
                name: "IX_Condonacion_Id_Credito",
                table: "Condonacion",
                column: "Id_Credito");

            migrationBuilder.AddForeignKey(
                name: "FK_Deposito_Linea_Deposito_Id_Linea_Deposito",
                table: "Deposito_Plazo",
                column: "Id_Linea_Deposito_Plazo",
                principalTable: "Linea_Deposito_Plazo",
                principalColumn: "Id_Linea_Deposito_Plazo",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Deposito_Plazo_Linea_Deposito_Plazo_Id_Linea_Deposito_Plazo",
                table: "Deposito_Plazo");

            migrationBuilder.DropTable(
                name: "Condonacion");

            migrationBuilder.DropTable(
                name: "Linea_Deposito_Plazo");

            migrationBuilder.DropIndex(
                name: "IX_Deposito_Plazo_Id_Linea_Deposito_Plazo",
                table: "Deposito_Plazo");

            migrationBuilder.DropColumn(
                name: "Id_Linea_Deposito_Plazo",
                table: "Deposito_Plazo");
        }
    }
}
