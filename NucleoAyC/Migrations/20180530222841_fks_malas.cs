using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace NucleoAyC.Migrations
{
    public partial class fks_malas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cuenta_Contable_Tipo_Id_Cuenta_Contable",
                table: "Cuenta_Contable");

            migrationBuilder.DropForeignKey(
                name: "FK_Detalle_Partida_Cuenta_Contable_Id_Detalle_Partida",
                table: "Detalle_Partida");

            migrationBuilder.DropForeignKey(
                name: "FK_Detalle_Partida_Partida_Contable_Id_Detalle_Partida",
                table: "Detalle_Partida");

            migrationBuilder.CreateIndex(
                name: "IX_Detalle_Partida_Id_Cuenta_Contable",
                table: "Detalle_Partida",
                column: "Id_Cuenta_Contable");

            migrationBuilder.CreateIndex(
                name: "IX_Detalle_Partida_Id_Partida_Contable",
                table: "Detalle_Partida",
                column: "Id_Partida_Contable");

            migrationBuilder.CreateIndex(
                name: "IX_Cuenta_Contable_Id_Tipo",
                table: "Cuenta_Contable",
                column: "Id_Tipo");

            migrationBuilder.AddForeignKey(
                name: "FK_Cuenta_Contable_Tipo_Id_Tipo",
                table: "Cuenta_Contable",
                column: "Id_Tipo",
                principalTable: "Tipo",
                principalColumn: "Id_Tipo",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Detalle_Partida_Cuenta_Contable_Id_Cuenta_Contable",
                table: "Detalle_Partida",
                column: "Id_Cuenta_Contable",
                principalTable: "Cuenta_Contable",
                principalColumn: "Id_Cuenta_Contable",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Detalle_Partida_Partida_Contable_Id_Partida_Contable",
                table: "Detalle_Partida",
                column: "Id_Partida_Contable",
                principalTable: "Partida_Contable",
                principalColumn: "Id_Partida_Contable",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cuenta_Contable_Tipo_Id_Tipo",
                table: "Cuenta_Contable");

            migrationBuilder.DropForeignKey(
                name: "FK_Detalle_Partida_Cuenta_Contable_Id_Cuenta_Contable",
                table: "Detalle_Partida");

            migrationBuilder.DropForeignKey(
                name: "FK_Detalle_Partida_Partida_Contable_Id_Partida_Contable",
                table: "Detalle_Partida");

            migrationBuilder.DropIndex(
                name: "IX_Detalle_Partida_Id_Cuenta_Contable",
                table: "Detalle_Partida");

            migrationBuilder.DropIndex(
                name: "IX_Detalle_Partida_Id_Partida_Contable",
                table: "Detalle_Partida");

            migrationBuilder.DropIndex(
                name: "IX_Cuenta_Contable_Id_Tipo",
                table: "Cuenta_Contable");

            migrationBuilder.AddForeignKey(
                name: "FK_Cuenta_Contable_Tipo_Id_Cuenta_Contable",
                table: "Cuenta_Contable",
                column: "Id_Cuenta_Contable",
                principalTable: "Tipo",
                principalColumn: "Id_Tipo",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Detalle_Partida_Cuenta_Contable_Id_Detalle_Partida",
                table: "Detalle_Partida",
                column: "Id_Detalle_Partida",
                principalTable: "Cuenta_Contable",
                principalColumn: "Id_Cuenta_Contable",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Detalle_Partida_Partida_Contable_Id_Detalle_Partida",
                table: "Detalle_Partida",
                column: "Id_Detalle_Partida",
                principalTable: "Partida_Contable",
                principalColumn: "Id_Partida_Contable",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
