using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class cambio_Referencia_porcentaje_Edad : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Edad",
                table: "Persona_Referencia",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "Fecha_Nacimiento",
                table: "Persona_Referencia",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Porcentaje",
                table: "Persona_Referencia",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaNacimiento",
                table: "Persona",
                nullable: true,
                oldClrType: typeof(DateTime));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Edad",
                table: "Persona_Referencia");

            migrationBuilder.DropColumn(
                name: "Fecha_Nacimiento",
                table: "Persona_Referencia");

            migrationBuilder.DropColumn(
                name: "Porcentaje",
                table: "Persona_Referencia");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaNacimiento",
                table: "Persona",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);
        }
    }
}
