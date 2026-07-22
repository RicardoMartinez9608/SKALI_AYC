using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace NucleoAyC.Migrations
{
    public partial class cambios_evaluacion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Antiguedad_Negocio",
                table: "Evaluacion",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Cuota",
                table: "Evaluacion",
                type: "decimal(65, 30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Monto",
                table: "Evaluacion",
                type: "decimal(65, 30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Monto_Credito_Mayor_Historial",
                table: "Evaluacion",
                type: "decimal(65, 30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "Mora",
                table: "Evaluacion",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Plazo",
                table: "Evaluacion",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Referencia_Comprobada",
                table: "Evaluacion",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Reside_Zona_Peligrosa",
                table: "Evaluacion",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Riesgo_Info_Red",
                table: "Evaluacion",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Sector_Economico",
                table: "Evaluacion",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Venta_Diaria_Bueno",
                table: "Evaluacion",
                type: "decimal(65, 30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Venta_Diaria_Mala",
                table: "Evaluacion",
                type: "decimal(65, 30)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Antiguedad_Negocio",
                table: "Evaluacion");

            migrationBuilder.DropColumn(
                name: "Cuota",
                table: "Evaluacion");

            migrationBuilder.DropColumn(
                name: "Monto",
                table: "Evaluacion");

            migrationBuilder.DropColumn(
                name: "Monto_Credito_Mayor_Historial",
                table: "Evaluacion");

            migrationBuilder.DropColumn(
                name: "Mora",
                table: "Evaluacion");

            migrationBuilder.DropColumn(
                name: "Plazo",
                table: "Evaluacion");

            migrationBuilder.DropColumn(
                name: "Referencia_Comprobada",
                table: "Evaluacion");

            migrationBuilder.DropColumn(
                name: "Reside_Zona_Peligrosa",
                table: "Evaluacion");

            migrationBuilder.DropColumn(
                name: "Riesgo_Info_Red",
                table: "Evaluacion");

            migrationBuilder.DropColumn(
                name: "Sector_Economico",
                table: "Evaluacion");

            migrationBuilder.DropColumn(
                name: "Venta_Diaria_Bueno",
                table: "Evaluacion");

            migrationBuilder.DropColumn(
                name: "Venta_Diaria_Mala",
                table: "Evaluacion");
        }
    }
}
