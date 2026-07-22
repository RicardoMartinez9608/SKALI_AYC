using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class campos_aprobacion_legal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            ////migrationBuilder.AddColumn<string>(
            ////    name: "Verificacion_Legal",
            ////    table: "Fotografia_Comprobantes",
            ////    nullable: true);

            //migrationBuilder.AddColumn<DateTime>(
            //    name: "Verificado_Legal_En",
            //    table: "Fotografia_Comprobantes",
            //    nullable: false,
                //defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropColumn(
            //    name: "Verificacion_Legal",
            //    table: "Fotografia_Comprobantes");

            //migrationBuilder.DropColumn(
            //    name: "Verificado_Legal_En",
            //    table: "Fotografia_Comprobantes");
        }
    }
}
