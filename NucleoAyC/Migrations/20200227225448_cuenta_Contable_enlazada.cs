using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class cuenta_Contable_enlazada : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.AddForeignKey(
                name: "FK_Conf_Detalle_Partida_Contable_Cuenta_Contable_Id_Cuenta_Cont",
                //schema: "Products",
                table: "Conf_Detalle_Partida_Contable",
                column: "Id_Cuenta_Contable",
                //principalSchema: "Cuenta_Contable",
                principalTable: "Cuenta_Contable",
                principalColumn: "Id_Cuenta_Contable",
                onDelete: ReferentialAction.Cascade);


            //migrationBuilder.CreateIndex(
            //    name: "IX_Conf_Detalle_Partida_Contable_Id_Cuenta_Contable",
            //    table: "Conf_Detalle_Partida_Contable",
            //    column: "Id_Cuenta_Contable");

            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.CreateIndex(
                name: "IX_Detalle_Partida_Id_Cuenta_Contable",
                table: "Detalle_Partida",
                column: "Id_Cuenta_Contable");

        }
    }
}
