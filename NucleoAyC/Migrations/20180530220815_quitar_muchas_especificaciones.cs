using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace NucleoAyC.Migrations
{
    public partial class quitar_muchas_especificaciones : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cuenta_Contable_Grupo_Id_Cuenta_Contable",
                table: "Cuenta_Contable");

            migrationBuilder.DropForeignKey(
                name: "FK_Cuenta_Contable_Naturaleza_Id_Cuenta_Contable",
                table: "Cuenta_Contable");

            migrationBuilder.DropColumn(
                name: "Id_Grupo",
                table: "Cuenta_Contable");

            migrationBuilder.DropColumn(
                name: "Id_Naturaleza",
                table: "Cuenta_Contable");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id_Grupo",
                table: "Cuenta_Contable",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Id_Naturaleza",
                table: "Cuenta_Contable",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Cuenta_Contable_Grupo_Id_Cuenta_Contable",
                table: "Cuenta_Contable",
                column: "Id_Cuenta_Contable",
                principalTable: "Grupo",
                principalColumn: "Id_Grupo",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Cuenta_Contable_Naturaleza_Id_Cuenta_Contable",
                table: "Cuenta_Contable",
                column: "Id_Cuenta_Contable",
                principalTable: "Naturaleza",
                principalColumn: "Id_Naturaleza",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
