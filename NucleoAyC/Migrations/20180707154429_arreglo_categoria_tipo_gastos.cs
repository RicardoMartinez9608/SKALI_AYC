using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace NucleoAyC.Migrations
{
    public partial class arreglo_categoria_tipo_gastos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gastos_Categoria_Gasto_Id_Categoria_Gasto",
                table: "Gastos");

            migrationBuilder.DropIndex(
                name: "IX_Gastos_Id_Categoria_Gasto",
                table: "Gastos");

            migrationBuilder.DropColumn(
                name: "Id_Categoria_Gasto",
                table: "Gastos");

            migrationBuilder.AddColumn<int>(
                name: "Id_Categoria_Gasto",
                table: "Tipo_Gasto",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Tipo_Gasto_Id_Categoria_Gasto",
                table: "Tipo_Gasto",
                column: "Id_Categoria_Gasto");

            migrationBuilder.AddForeignKey(
                name: "FK_Tipo_Gasto_Categoria_Gasto_Id_Categoria_Gasto",
                table: "Tipo_Gasto",
                column: "Id_Categoria_Gasto",
                principalTable: "Categoria_Gasto",
                principalColumn: "Id_Categoria_Gasto",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tipo_Gasto_Categoria_Gasto_Id_Categoria_Gasto",
                table: "Tipo_Gasto");

            migrationBuilder.DropIndex(
                name: "IX_Tipo_Gasto_Id_Categoria_Gasto",
                table: "Tipo_Gasto");

            migrationBuilder.DropColumn(
                name: "Id_Categoria_Gasto",
                table: "Tipo_Gasto");

            migrationBuilder.AddColumn<int>(
                name: "Id_Categoria_Gasto",
                table: "Gastos",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Gastos_Id_Categoria_Gasto",
                table: "Gastos",
                column: "Id_Categoria_Gasto");

            migrationBuilder.AddForeignKey(
                name: "FK_Gastos_Categoria_Gasto_Id_Categoria_Gasto",
                table: "Gastos",
                column: "Id_Categoria_Gasto",
                principalTable: "Categoria_Gasto",
                principalColumn: "Id_Categoria_Gasto",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
