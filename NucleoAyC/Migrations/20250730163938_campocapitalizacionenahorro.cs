using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class campocapitalizacionenahorro : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CapitalizacionAhorro",
                table: "Ahorro",
                nullable: false,
                defaultValue: new DateTime(1900, 1, 1));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CapitalizacionAhorro",
                table: "Ahorro");
        }
    }
}
