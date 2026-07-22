using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace NucleoAyC.Migrations
{
    public partial class araizdecapitalizacion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Frecuencia_Deposito",
                table: "Ahorro");

            migrationBuilder.DropColumn(
                name: "Mes_Capitalizable",
                table: "Ahorro");

            migrationBuilder.DropColumn(
                name: "Plazo",
                table: "Ahorro");

            migrationBuilder.AddColumn<int>(
                name: "Frecuencia_Deposito",
                table: "Linea_Ahorro",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Mes_Capitalizable",
                table: "Linea_Ahorro",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Plazo",
                table: "Linea_Ahorro",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Saldo_Actual",
                table: "Ahorro",
                type: "decimal(65, 30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "Ultima_Fecha_Traslado",
                table: "Ahorro",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Frecuencia_Deposito",
                table: "Linea_Ahorro");

            migrationBuilder.DropColumn(
                name: "Mes_Capitalizable",
                table: "Linea_Ahorro");

            migrationBuilder.DropColumn(
                name: "Plazo",
                table: "Linea_Ahorro");

            migrationBuilder.DropColumn(
                name: "Saldo_Actual",
                table: "Ahorro");

            migrationBuilder.DropColumn(
                name: "Ultima_Fecha_Traslado",
                table: "Ahorro");

            migrationBuilder.AddColumn<int>(
                name: "Frecuencia_Deposito",
                table: "Ahorro",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Mes_Capitalizable",
                table: "Ahorro",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Plazo",
                table: "Ahorro",
                nullable: false,
                defaultValue: 0);
        }
    }
}
