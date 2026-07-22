using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace NucleoAyC.Migrations
{
    public partial class colecta_maestro_detalle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id_Usuario",
                table: "Colecta");

            migrationBuilder.AddColumn<int>(
                name: "Id_Colecta_Diaria",
                table: "Colecta",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Colecta_Diaria",
                columns: table => new
                {
                    Id_Colecta_Diaria = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Borrado_Logico = table.Column<bool>(type: "bit", nullable: false),
                    Creado_En = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Creado_Por = table.Column<string>(type: "longtext", nullable: true),
                    Eliminado_En = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Eliminado_Por = table.Column<string>(type: "longtext", nullable: true),
                    Fecha = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Id_Usuario = table.Column<int>(type: "int", nullable: false),
                    Modificado = table.Column<bool>(type: "bit", nullable: false),
                    Modificado_En = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Modificado_Por = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Colecta_Diaria", x => x.Id_Colecta_Diaria);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Colecta_Id_Colecta_Diaria",
                table: "Colecta",
                column: "Id_Colecta_Diaria");

            migrationBuilder.AddForeignKey(
                name: "FK_Colecta_Colecta_Diaria_Id_Colecta_Diaria",
                table: "Colecta",
                column: "Id_Colecta_Diaria",
                principalTable: "Colecta_Diaria",
                principalColumn: "Id_Colecta_Diaria",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Colecta_Colecta_Diaria_Id_Colecta_Diaria",
                table: "Colecta");

            migrationBuilder.DropTable(
                name: "Colecta_Diaria");

            migrationBuilder.DropIndex(
                name: "IX_Colecta_Id_Colecta_Diaria",
                table: "Colecta");

            migrationBuilder.DropColumn(
                name: "Id_Colecta_Diaria",
                table: "Colecta");

            migrationBuilder.AddColumn<int>(
                name: "Id_Usuario",
                table: "Colecta",
                nullable: false,
                defaultValue: 0);
        }
    }
}
