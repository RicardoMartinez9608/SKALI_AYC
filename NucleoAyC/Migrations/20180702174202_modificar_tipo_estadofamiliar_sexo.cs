using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace NucleoAyC.Migrations
{
    public partial class modificar_tipo_estadofamiliar_sexo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EstadoFamiliar",
                table: "Persona");

            migrationBuilder.DropColumn(
                name: "Sexo",
                table: "Persona");

            migrationBuilder.AddColumn<int>(
                name: "Personas_Cargo",
                table: "Persona",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Referencia_Crediticia_Comprobable",
                table: "Persona",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Residencia",
                table: "Persona",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Personas_Cargo",
                table: "Persona");

            migrationBuilder.DropColumn(
                name: "Referencia_Crediticia_Comprobable",
                table: "Persona");

            migrationBuilder.DropColumn(
                name: "Residencia",
                table: "Persona");

            migrationBuilder.AddColumn<string>(
                name: "EstadoFamiliar",
                table: "Persona",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Sexo",
                table: "Persona",
                nullable: true);
        }
    }
}
