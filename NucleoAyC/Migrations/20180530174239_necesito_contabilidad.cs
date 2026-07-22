using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace NucleoAyC.Migrations
{
    public partial class necesito_contabilidad : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Grupo",
                columns: table => new
                {
                    Id_Grupo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Descripcion = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grupo", x => x.Id_Grupo);
                });

            migrationBuilder.CreateTable(
                name: "Naturaleza",
                columns: table => new
                {
                    Id_Naturaleza = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Descripcion = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Naturaleza", x => x.Id_Naturaleza);
                });

            migrationBuilder.CreateTable(
                name: "Operacion",
                columns: table => new
                {
                    Id_Operacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Descripcion = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operacion", x => x.Id_Operacion);
                });

            migrationBuilder.CreateTable(
                name: "Partida_Contable",
                columns: table => new
                {
                    Id_Partida_Contable = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Borrado_Logico = table.Column<bool>(type: "bit", nullable: false),
                    Comprobante = table.Column<string>(type: "longtext", nullable: true),
                    Creado_En = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Creado_Por = table.Column<string>(type: "longtext", nullable: true),
                    Eliminado_En = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Eliminado_Por = table.Column<string>(type: "longtext", nullable: true),
                    Fecha = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Modificado = table.Column<bool>(type: "bit", nullable: false),
                    Modificado_En = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Modificado_Por = table.Column<string>(type: "longtext", nullable: true),
                    Saldo = table.Column<decimal>(type: "decimal(65, 30)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Partida_Contable", x => x.Id_Partida_Contable);
                });

            migrationBuilder.CreateTable(
                name: "Tipo",
                columns: table => new
                {
                    Id_Tipo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Descripcion = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tipo", x => x.Id_Tipo);
                });

            migrationBuilder.CreateTable(
                name: "Configuracion_Contabilidad",
                columns: table => new
                {
                    Id_Configuracion_Contabilidad = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Concepto = table.Column<string>(type: "longtext", nullable: true),
                    Id_Cuenta = table.Column<int>(type: "int", nullable: false),
                    Id_Operacion = table.Column<int>(type: "int", nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(65, 30)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configuracion_Contabilidad", x => x.Id_Configuracion_Contabilidad);
                    table.ForeignKey(
                        name: "FK_Configuracion_Contabilidad_Operacion_Id_Configuracion_Contabilidad",
                        column: x => x.Id_Configuracion_Contabilidad,
                        principalTable: "Operacion",
                        principalColumn: "Id_Operacion",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cuenta_Contable",
                columns: table => new
                {
                    Id_Cuenta_Contable = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Codigo = table.Column<string>(type: "longtext", nullable: true),
                    Descripcion = table.Column<string>(type: "longtext", nullable: true),
                    Id_Grupo = table.Column<int>(type: "int", nullable: false),
                    Id_Naturaleza = table.Column<int>(type: "int", nullable: false),
                    Id_Tipo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cuenta_Contable", x => x.Id_Cuenta_Contable);
                    table.ForeignKey(
                        name: "FK_Cuenta_Contable_Grupo_Id_Cuenta_Contable",
                        column: x => x.Id_Cuenta_Contable,
                        principalTable: "Grupo",
                        principalColumn: "Id_Grupo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cuenta_Contable_Naturaleza_Id_Cuenta_Contable",
                        column: x => x.Id_Cuenta_Contable,
                        principalTable: "Naturaleza",
                        principalColumn: "Id_Naturaleza",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cuenta_Contable_Tipo_Id_Cuenta_Contable",
                        column: x => x.Id_Cuenta_Contable,
                        principalTable: "Tipo",
                        principalColumn: "Id_Tipo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Detalle_Partida",
                columns: table => new
                {
                    Id_Detalle_Partida = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Abono = table.Column<decimal>(type: "decimal(65, 30)", nullable: false),
                    Borrado_Logico = table.Column<bool>(type: "bit", nullable: false),
                    Cargo = table.Column<decimal>(type: "decimal(65, 30)", nullable: false),
                    Concepto = table.Column<string>(type: "longtext", nullable: true),
                    Creado_En = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Creado_Por = table.Column<string>(type: "longtext", nullable: true),
                    Eliminado_En = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Eliminado_Por = table.Column<string>(type: "longtext", nullable: true),
                    Fecha = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Id_Cuenta_Contable = table.Column<int>(type: "int", nullable: false),
                    Id_Partida_Contable = table.Column<int>(type: "int", nullable: false),
                    Modificado = table.Column<bool>(type: "bit", nullable: false),
                    Modificado_En = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Modificado_Por = table.Column<string>(type: "longtext", nullable: true),
                    Saldo = table.Column<decimal>(type: "decimal(65, 30)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Detalle_Partida", x => x.Id_Detalle_Partida);
                    table.ForeignKey(
                        name: "FK_Detalle_Partida_Cuenta_Contable_Id_Detalle_Partida",
                        column: x => x.Id_Detalle_Partida,
                        principalTable: "Cuenta_Contable",
                        principalColumn: "Id_Cuenta_Contable",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Detalle_Partida_Partida_Contable_Id_Detalle_Partida",
                        column: x => x.Id_Detalle_Partida,
                        principalTable: "Partida_Contable",
                        principalColumn: "Id_Partida_Contable",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Configuracion_Contabilidad");

            migrationBuilder.DropTable(
                name: "Detalle_Partida");

            migrationBuilder.DropTable(
                name: "Operacion");

            migrationBuilder.DropTable(
                name: "Cuenta_Contable");

            migrationBuilder.DropTable(
                name: "Partida_Contable");

            migrationBuilder.DropTable(
                name: "Grupo");

            migrationBuilder.DropTable(
                name: "Naturaleza");

            migrationBuilder.DropTable(
                name: "Tipo");
        }
    }
}
