using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace NucleoAyC.Migrations
{
    public partial class estado_maquina_transacciones : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Anticipado",
                table: "Transaccion_Credito",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Puntual",
                table: "Transaccion_Credito",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Vencido",
                table: "Transaccion_Credito",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Anticipado",
                table: "Transaccion_Credito");

            migrationBuilder.DropColumn(
                name: "Puntual",
                table: "Transaccion_Credito");

            migrationBuilder.DropColumn(
                name: "Vencido",
                table: "Transaccion_Credito");
        }
    }
}
