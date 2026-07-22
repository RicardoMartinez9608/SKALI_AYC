using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace NucleoAyC.Migrations
{
    public partial class registro_comprobantes_colecta_diaria : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "No_Comprobante_Final",
                table: "Colecta_Diaria",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "No_Comprobante_Inicio",
                table: "Colecta_Diaria",
                type: "longtext",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "No_Comprobante_Final",
                table: "Colecta_Diaria");

            migrationBuilder.DropColumn(
                name: "No_Comprobante_Inicio",
                table: "Colecta_Diaria");
        }
    }
}
