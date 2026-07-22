using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class dato_equivocado_fecha_nacimiento_porcentaje : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                table: "Referencia",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AddColumn<int>(
                name: "Edad",
                table: "Referencia",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Porcentaje",
                table: "Referencia",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Edad",
                table: "Referencia");

            migrationBuilder.DropColumn(
                name: "Porcentaje",
                table: "Referencia");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaNacimiento",
                table: "Referencia",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

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
        }
    }
}
