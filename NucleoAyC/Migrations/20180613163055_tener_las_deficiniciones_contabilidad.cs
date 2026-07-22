using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace NucleoAyC.Migrations
{
    public partial class tener_las_deficiniciones_contabilidad : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Configuracion_Contabilidad",
                columns: table => new
                {
                    Id_Configuracion_Contabilidad = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Concepto = table.Column<string>(type: "longtext", nullable: true),
                    Id_Cuenta = table.Column<int>(type: "int", nullable: false),
                    Id_Operacion = table.Column<int>(type: "int", nullable: false),
                    //Valor = table.Column<decimal>(type: "decimal(65, 30)", nullable: false),
                    Identificador = table.Column<string>(type:"longtext",nullable:true),
                    Descripcion = table.Column<string>(type: "longtext", nullable: true),
                    Modo_Almacen_Valor = table.Column<string>(type: "longtext", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configuracion_Contabilidad", x => x.Id_Configuracion_Contabilidad);
                    
                });
            

            migrationBuilder.AddColumn<int>(
                name: "Id_Definicion_Transaccion",
                table: "Configuracion_Contabilidad",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Definicion_Transaccion",
                columns: table => new
                {
                    Id_Definicion_Transaccion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Descripcion = table.Column<string>(type: "longtext", nullable: true),
                    Identificador = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Definicion_Transaccion", x => x.Id_Definicion_Transaccion);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Configuracion_Contabilidad_Id_Definicion_Transaccion",
                table: "Configuracion_Contabilidad",
                column: "Id_Definicion_Transaccion");

            migrationBuilder.CreateIndex(
                name: "IX_Configuracion_Contabilidad_Id_Operacion",
                table: "Configuracion_Contabilidad",
                column: "Id_Operacion");

            migrationBuilder.AddForeignKey(
                name: "FK_Configuracion_Contabilidad_Definicion_Transaccion_Id_Definicion_Transaccion",
                table: "Configuracion_Contabilidad",
                column: "Id_Definicion_Transaccion",
                principalTable: "Definicion_Transaccion",
                principalColumn: "Id_Definicion_Transaccion",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Configuracion_Contabilidad_Operacion_Id_Operacion",
                table: "Configuracion_Contabilidad",
                column: "Id_Operacion",
                principalTable: "Operacion",
                principalColumn: "Id_Operacion",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Configuracion_Contabilidad_Definicion_Transaccion_Id_Definicion_Transaccion",
            //    table: "Configuracion_Contabilidad");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Configuracion_Contabilidad_Operacion_Id_Operacion",
            //    table: "Configuracion_Contabilidad");

            //migrationBuilder.DropTable(
            //    name: "Definicion_Transaccion");

            //migrationBuilder.DropIndex(
            //    name: "IX_Configuracion_Contabilidad_Id_Definicion_Transaccion",
            //    table: "Configuracion_Contabilidad");

            //migrationBuilder.DropIndex(
            //    name: "IX_Configuracion_Contabilidad_Id_Operacion",
            //    table: "Configuracion_Contabilidad");

            //migrationBuilder.DropColumn(
            //    name: "Id_Definicion_Transaccion",
            //    table: "Configuracion_Contabilidad");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Configuracion_Contabilidad_Operacion_Id_Configuracion_Contabilidad",
            //    table: "Configuracion_Contabilidad",
            //    column: "Id_Configuracion_Contabilidad",
            //    principalTable: "Operacion",
            //    principalColumn: "Id_Operacion",
            //    onDelete: ReferentialAction.Cascade);
        }
    }
}
