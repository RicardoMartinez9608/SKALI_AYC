using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace NucleoAyC.Migrations
{
    public partial class cambio_por_recargo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Interes_Acumulado",
                table: "Transaccion_Credito");

            migrationBuilder.DropColumn(
                name: "Interes_Mora",
                table: "Transaccion_Credito");

            migrationBuilder.AddColumn<decimal>(
                name: "Recargo",
                table: "Transaccion_Credito",
                type: "decimal(65, 30)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Recargo",
                table: "Transaccion_Credito");

            migrationBuilder.AddColumn<decimal>(
                name: "Interes_Acumulado",
                table: "Transaccion_Credito",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Interes_Mora",
                table: "Transaccion_Credito",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
