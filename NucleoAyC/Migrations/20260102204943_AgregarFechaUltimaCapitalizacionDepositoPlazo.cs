using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class AgregarFechaUltimaCapitalizacionDepositoPlazo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
         

            migrationBuilder.AddColumn<DateTime>(
                name: "Fecha_Ultima_Capitalizacion",
                table: "Deposito_Plazo",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Fecha_Ultima_Capitalizacion",
                table: "Deposito_Plazo");

           
        }
    }
}
