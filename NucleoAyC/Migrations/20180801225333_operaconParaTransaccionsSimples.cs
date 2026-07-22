using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace NucleoAyC.Migrations
{
    public partial class operaconParaTransaccionsSimples : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaccion_Ahorro_Caja_Id_Caja",
                table: "Transaccion_Ahorro");

            migrationBuilder.DropIndex(
                name: "IX_Transaccion_Ahorro_Id_Caja",
                table: "Transaccion_Ahorro");

            migrationBuilder.DropColumn(
                name: "Id_Caja",
                table: "Transaccion_Ahorro");

            migrationBuilder.CreateTable(
                name: "Tipo_Operacion_Simple",
                columns: table => new
                {
                    Id_Tipo_Operacion_Simple = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Descripcion = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tipo_Operacion_Simple", x => x.Id_Tipo_Operacion_Simple);
                });

            migrationBuilder.CreateTable(
                name: "Transaccion_Simple",
                columns: table => new
                {
                    Id_Transaccion_Simple = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Borrado_Logico = table.Column<bool>(type: "bit", nullable: false),
                    Creado_En = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Creado_Por = table.Column<string>(type: "longtext", nullable: true),
                    Eliminado_En = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Eliminado_Por = table.Column<string>(type: "longtext", nullable: true),
                    Id_Tipo_Operacion_Simple = table.Column<int>(type: "int", nullable: false),
                    Modificado = table.Column<bool>(type: "bit", nullable: false),
                    Modificado_En = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Modificado_Por = table.Column<string>(type: "longtext", nullable: true),
                    Monto = table.Column<decimal>(type: "decimal(65, 30)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaccion_Simple", x => x.Id_Transaccion_Simple);
                    table.ForeignKey(
                        name: "FK_Transaccion_Simple_Tipo_Operacion_Simple_Id_Tipo_Operacion_Simple",
                        column: x => x.Id_Tipo_Operacion_Simple,
                        principalTable: "Tipo_Operacion_Simple",
                        principalColumn: "Id_Tipo_Operacion_Simple",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transaccion_Simple_Id_Tipo_Operacion_Simple",
                table: "Transaccion_Simple",
                column: "Id_Tipo_Operacion_Simple");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transaccion_Simple");

            migrationBuilder.DropTable(
                name: "Tipo_Operacion_Simple");

            migrationBuilder.AddColumn<int>(
                name: "Id_Caja",
                table: "Transaccion_Ahorro",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Transaccion_Ahorro_Id_Caja",
                table: "Transaccion_Ahorro",
                column: "Id_Caja");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaccion_Ahorro_Caja_Id_Caja",
                table: "Transaccion_Ahorro",
                column: "Id_Caja",
                principalTable: "Caja",
                principalColumn: "Id_Caja",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
