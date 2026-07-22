using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace NucleoAyC.Migrations
{
    public partial class cambio_configuracion_contabilidad_rectificando : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Valor",
                table: "Configuracion_Contabilidad");

            migrationBuilder.AddColumn<string>(
                name: "Descripcion",
                table: "Configuracion_Contabilidad",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Identificador",
                table: "Configuracion_Contabilidad",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Modo_Almacen_Valor",
                table: "Configuracion_Contabilidad",
                type: "longtext",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Descripcion",
                table: "Configuracion_Contabilidad");

            migrationBuilder.DropColumn(
                name: "Identificador",
                table: "Configuracion_Contabilidad");

            migrationBuilder.DropColumn(
                name: "Modo_Almacen_Valor",
                table: "Configuracion_Contabilidad");

            migrationBuilder.AddColumn<decimal>(
                name: "Valor",
                table: "Configuracion_Contabilidad",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
