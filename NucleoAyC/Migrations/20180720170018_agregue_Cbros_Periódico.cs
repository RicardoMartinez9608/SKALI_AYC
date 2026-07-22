using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace NucleoAyC.Migrations
{
    public partial class agregue_Cbros_Periódico : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "PlanAyC_Cobros_Periodico_Dinero",
                table: "Linea_Credito",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Cobro_Periodico",
                columns: table => new
                {
                    Id_Cobro_Periodico = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Descripcion = table.Column<string>(type: "longtext", nullable: true),
                    Id_Linea_Credito = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "longtext", nullable: true),
                    Valor = table.Column<decimal>(type: "decimal(65, 30)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cobro_Periodico", x => x.Id_Cobro_Periodico);
                    table.ForeignKey(
                        name: "FK_Cobro_Periodico_Linea_Credito_Id_Linea_Credito",
                        column: x => x.Id_Linea_Credito,
                        principalTable: "Linea_Credito",
                        principalColumn: "Id_Linea",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cobro_Periodico_Id_Linea_Credito",
                table: "Cobro_Periodico",
                column: "Id_Linea_Credito");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cobro_Periodico");

            migrationBuilder.DropColumn(
                name: "PlanAyC_Cobros_Periodico_Dinero",
                table: "Linea_Credito");
        }
    }
}
