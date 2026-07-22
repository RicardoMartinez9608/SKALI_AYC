using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace NucleoAyC.Migrations
{
    public partial class catgoria_gastos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id_Categoria_Gasto",
                table: "Gastos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Venta_Mensual_Malo",
                table: "Evaluacion",
                type: "decimal(65, 30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "Categoria_Gasto",
                columns: table => new
                {
                    Id_Categoria_Gasto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Descripcion = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categoria_Gasto", x => x.Id_Categoria_Gasto);
                });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gastos_Categoria_Gasto_Id_Categoria_Gasto",
                table: "Gastos");

            migrationBuilder.DropTable(
                name: "Categoria_Gasto");

            migrationBuilder.DropIndex(
                name: "IX_Gastos_Id_Categoria_Gasto",
                table: "Gastos");

            migrationBuilder.DropColumn(
                name: "Id_Categoria_Gasto",
                table: "Gastos");

            migrationBuilder.DropColumn(
                name: "Venta_Mensual_Malo",
                table: "Evaluacion");
        }
    }
}
