using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace NucleoAyC.Migrations
{
    public partial class operaciones_boveda : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id_Banco",
                table: "Transaccion_Simple",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Id_Caja",
                table: "Transaccion_Simple",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "No_Cuenta",
                table: "Transaccion_Simple",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nombre_Banco",
                table: "Transaccion_Simple",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Id_Transaccion_Simple",
                table: "Transaccion_Caja",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Banco",
                columns: table => new
                {
                    Id_Banco = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Borrado_Logico = table.Column<bool>(type: "bit", nullable: false),
                    Creado_En = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Creado_Por = table.Column<string>(type: "longtext", nullable: true),
                    Eliminado_En = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Eliminado_Por = table.Column<string>(type: "longtext", nullable: true),
                    Modificado = table.Column<bool>(type: "bit", nullable: false),
                    Modificado_En = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Modificado_Por = table.Column<string>(type: "longtext", nullable: true),
                    No_Cuenta = table.Column<string>(type: "longtext", nullable: true),
                    Nombre = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banco", x => x.Id_Banco);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Banco");

            migrationBuilder.DropColumn(
                name: "Id_Banco",
                table: "Transaccion_Simple");

            migrationBuilder.DropColumn(
                name: "Id_Caja",
                table: "Transaccion_Simple");

            migrationBuilder.DropColumn(
                name: "No_Cuenta",
                table: "Transaccion_Simple");

            migrationBuilder.DropColumn(
                name: "Nombre_Banco",
                table: "Transaccion_Simple");

            migrationBuilder.DropColumn(
                name: "Id_Transaccion_Simple",
                table: "Transaccion_Caja");
        }
    }
}
