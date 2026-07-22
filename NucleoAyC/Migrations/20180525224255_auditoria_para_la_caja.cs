using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace NucleoAyC.Migrations
{
    public partial class auditoria_para_la_caja : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Borrado_Logico",
                table: "Transaccion_Credito",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "Creado_En",
                table: "Transaccion_Credito",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Creado_Por",
                table: "Transaccion_Credito",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Eliminado_En",
                table: "Transaccion_Credito",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Eliminado_Por",
                table: "Transaccion_Credito",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Modificado",
                table: "Transaccion_Credito",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "Modificado_En",
                table: "Transaccion_Credito",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Modificado_Por",
                table: "Transaccion_Credito",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Creado_En",
                table: "Transaccion_Caja",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Creado_Por",
                table: "Transaccion_Caja",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Eliminado_En",
                table: "Transaccion_Caja",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Eliminado_Por",
                table: "Transaccion_Caja",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Modificado",
                table: "Transaccion_Caja",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "Modificado_En",
                table: "Transaccion_Caja",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Modificado_Por",
                table: "Transaccion_Caja",
                type: "longtext",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Borrado_Logico",
                table: "Transaccion_Credito");

            migrationBuilder.DropColumn(
                name: "Creado_En",
                table: "Transaccion_Credito");

            migrationBuilder.DropColumn(
                name: "Creado_Por",
                table: "Transaccion_Credito");

            migrationBuilder.DropColumn(
                name: "Eliminado_En",
                table: "Transaccion_Credito");

            migrationBuilder.DropColumn(
                name: "Eliminado_Por",
                table: "Transaccion_Credito");

            migrationBuilder.DropColumn(
                name: "Modificado",
                table: "Transaccion_Credito");

            migrationBuilder.DropColumn(
                name: "Modificado_En",
                table: "Transaccion_Credito");

            migrationBuilder.DropColumn(
                name: "Modificado_Por",
                table: "Transaccion_Credito");

            migrationBuilder.DropColumn(
                name: "Creado_En",
                table: "Transaccion_Caja");

            migrationBuilder.DropColumn(
                name: "Creado_Por",
                table: "Transaccion_Caja");

            migrationBuilder.DropColumn(
                name: "Eliminado_En",
                table: "Transaccion_Caja");

            migrationBuilder.DropColumn(
                name: "Eliminado_Por",
                table: "Transaccion_Caja");

            migrationBuilder.DropColumn(
                name: "Modificado",
                table: "Transaccion_Caja");

            migrationBuilder.DropColumn(
                name: "Modificado_En",
                table: "Transaccion_Caja");

            migrationBuilder.DropColumn(
                name: "Modificado_Por",
                table: "Transaccion_Caja");
        }
    }
}
