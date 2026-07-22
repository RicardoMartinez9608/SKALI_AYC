using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class contabilidad_modificada : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cuenta_Contable_Tipo_Id_Tipo",
                table: "Cuenta_Contable");

            migrationBuilder.DropTable(
                name: "Configuracion_Contabilidad");

            migrationBuilder.DropTable(
                name: "Detalle_Partida");

            migrationBuilder.DropTable(
                name: "Grupo");

            migrationBuilder.DropTable(
                name: "Naturaleza");

            migrationBuilder.DropTable(
                name: "Tipo");

            migrationBuilder.DropTable(
                name: "Partida_Contable");

            migrationBuilder.RenameColumn(
                name: "Id_Tipo",
                table: "Cuenta_Contable",
                newName: "Id_Agencia");

            migrationBuilder.RenameIndex(
                name: "IX_Cuenta_Contable_Id_Tipo",
                table: "Cuenta_Contable",
                newName: "IX_Cuenta_Contable_Id_Agencia");

            migrationBuilder.CreateTable(
                name: "Asiento_Contable",
                columns: table => new
                {
                    Id_Asiento_Contable = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Id_Transaccion_Caja = table.Column<int>(nullable: false),
                    Fecha = table.Column<DateTime>(nullable: false),
                    Concepto = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Asiento_Contable", x => x.Id_Asiento_Contable);
                });

            migrationBuilder.CreateTable(
                name: "Conf_Partida_Contable",
                columns: table => new
                {
                    Id_Conf_Partida_Contable = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Identificador = table.Column<string>(nullable: true),
                    Descripcion = table.Column<string>(nullable: true),
                    Id_Agencia = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conf_Partida_Contable", x => x.Id_Conf_Partida_Contable);
                    table.ForeignKey(
                        name: "FK_Conf_Partida_Contable_Agencia_Id_Agencia",
                        column: x => x.Id_Agencia,
                        principalTable: "Agencia",
                        principalColumn: "Id_Agencia",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Conf_Detalle_Partida_Contable",
                columns: table => new
                {
                    Id_Conf_Detalle_Partida_Contable = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Id_Conf_Partida_Contable = table.Column<int>(nullable: false),
                    Id_Cuenta_Contable = table.Column<int>(nullable: false),
                    Id_Linea_Credito = table.Column<int>(nullable: false),
                    Id_Linea_Ahorro = table.Column<int>(nullable: false),
                    Identificador = table.Column<string>(nullable: true),
                    Concepto = table.Column<string>(nullable: true),
                    Cargo = table.Column<bool>(nullable: false),
                    Abono = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conf_Detalle_Partida_Contable", x => x.Id_Conf_Detalle_Partida_Contable);
                    table.ForeignKey(
                        name: "FK_Conf_Detalle_Partida_Contable_Conf_Partida_Contable_Id_Conf_~",
                        column: x => x.Id_Conf_Partida_Contable,
                        principalTable: "Conf_Partida_Contable",
                        principalColumn: "Id_Conf_Partida_Contable",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Conf_Detalle_Partida_Contable_Cuenta_Contable_Id_Cuenta_Cont~",
                        column: x => x.Id_Cuenta_Contable,
                        principalTable: "Cuenta_Contable",
                        principalColumn: "Id_Cuenta_Contable",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Detalle_Asiento_Contable",
                columns: table => new
                {
                    Id_Detalle_Asiento_Contable = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Id_Conf_Detalle_Partida_Contable = table.Column<int>(nullable: false),
                    Id_Asiento_Contable = table.Column<int>(nullable: false),
                    Concepto = table.Column<string>(nullable: true),
                    Cargo = table.Column<decimal>(nullable: false),
                    Abono = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Detalle_Asiento_Contable", x => x.Id_Detalle_Asiento_Contable);
                    table.ForeignKey(
                        name: "FK_Detalle_Asiento_Contable_Asiento_Contable_Id_Asiento_Contable",
                        column: x => x.Id_Asiento_Contable,
                        principalTable: "Asiento_Contable",
                        principalColumn: "Id_Asiento_Contable",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Detalle_Asiento_Contable_Conf_Detalle_Partida_Contable_Id_Co~",
                        column: x => x.Id_Conf_Detalle_Partida_Contable,
                        principalTable: "Conf_Detalle_Partida_Contable",
                        principalColumn: "Id_Conf_Detalle_Partida_Contable",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Conf_Detalle_Partida_Contable_Id_Conf_Partida_Contable",
                table: "Conf_Detalle_Partida_Contable",
                column: "Id_Conf_Partida_Contable");

            migrationBuilder.CreateIndex(
                name: "IX_Conf_Detalle_Partida_Contable_Id_Cuenta_Contable",
                table: "Conf_Detalle_Partida_Contable",
                column: "Id_Cuenta_Contable");

            migrationBuilder.CreateIndex(
                name: "IX_Conf_Partida_Contable_Id_Agencia",
                table: "Conf_Partida_Contable",
                column: "Id_Agencia");

            migrationBuilder.CreateIndex(
                name: "IX_Detalle_Asiento_Contable_Id_Asiento_Contable",
                table: "Detalle_Asiento_Contable",
                column: "Id_Asiento_Contable");

            migrationBuilder.CreateIndex(
                name: "IX_Detalle_Asiento_Contable_Id_Conf_Detalle_Partida_Contable",
                table: "Detalle_Asiento_Contable",
                column: "Id_Conf_Detalle_Partida_Contable");

            migrationBuilder.AddForeignKey(
                name: "FK_Cuenta_Contable_Agencia_Id_Agencia",
                table: "Cuenta_Contable",
                column: "Id_Agencia",
                principalTable: "Agencia",
                principalColumn: "Id_Agencia",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cuenta_Contable_Agencia_Id_Agencia",
                table: "Cuenta_Contable");

            migrationBuilder.DropTable(
                name: "Detalle_Asiento_Contable");

            migrationBuilder.DropTable(
                name: "Asiento_Contable");

            migrationBuilder.DropTable(
                name: "Conf_Detalle_Partida_Contable");

            migrationBuilder.DropTable(
                name: "Conf_Partida_Contable");

            migrationBuilder.RenameColumn(
                name: "Id_Agencia",
                table: "Cuenta_Contable",
                newName: "Id_Tipo");

            migrationBuilder.RenameIndex(
                name: "IX_Cuenta_Contable_Id_Agencia",
                table: "Cuenta_Contable",
                newName: "IX_Cuenta_Contable_Id_Tipo");

            migrationBuilder.CreateTable(
                name: "Configuracion_Contabilidad",
                columns: table => new
                {
                    Id_Configuracion_Contabilidad = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Concepto = table.Column<string>(nullable: true),
                    Descripcion = table.Column<string>(nullable: true),
                    Id_Cuenta = table.Column<int>(nullable: false),
                    Id_Definicion_Transaccion = table.Column<int>(nullable: false),
                    Id_Operacion = table.Column<int>(nullable: false),
                    Identificador = table.Column<string>(nullable: true),
                    Modo_Almacen_Valor = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configuracion_Contabilidad", x => x.Id_Configuracion_Contabilidad);
                    table.ForeignKey(
                        name: "FK_Configuracion_Contabilidad_Definicion_Transaccion_Id_Definic~",
                        column: x => x.Id_Definicion_Transaccion,
                        principalTable: "Definicion_Transaccion",
                        principalColumn: "Id_Definicion_Transaccion",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Configuracion_Contabilidad_Operacion_Id_Operacion",
                        column: x => x.Id_Operacion,
                        principalTable: "Operacion",
                        principalColumn: "Id_Operacion",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Grupo",
                columns: table => new
                {
                    Id_Grupo = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Descripcion = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grupo", x => x.Id_Grupo);
                });

            migrationBuilder.CreateTable(
                name: "Naturaleza",
                columns: table => new
                {
                    Id_Naturaleza = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Descripcion = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Naturaleza", x => x.Id_Naturaleza);
                });

            migrationBuilder.CreateTable(
                name: "Partida_Contable",
                columns: table => new
                {
                    Id_Partida_Contable = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Borrado_Logico = table.Column<bool>(nullable: false),
                    Comprobante = table.Column<string>(nullable: true),
                    Creado_En = table.Column<DateTime>(nullable: false),
                    Creado_Por = table.Column<string>(nullable: true),
                    Eliminado_En = table.Column<DateTime>(nullable: false),
                    Eliminado_Por = table.Column<string>(nullable: true),
                    Fecha = table.Column<DateTime>(nullable: false),
                    Modificado = table.Column<bool>(nullable: false),
                    Modificado_En = table.Column<DateTime>(nullable: false),
                    Modificado_Por = table.Column<string>(nullable: true),
                    Saldo = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Partida_Contable", x => x.Id_Partida_Contable);
                });

            migrationBuilder.CreateTable(
                name: "Tipo",
                columns: table => new
                {
                    Id_Tipo = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Descripcion = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tipo", x => x.Id_Tipo);
                });

            migrationBuilder.CreateTable(
                name: "Detalle_Partida",
                columns: table => new
                {
                    Id_Detalle_Partida = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Abono = table.Column<decimal>(nullable: false),
                    Borrado_Logico = table.Column<bool>(nullable: false),
                    Cargo = table.Column<decimal>(nullable: false),
                    Concepto = table.Column<string>(nullable: true),
                    Creado_En = table.Column<DateTime>(nullable: false),
                    Creado_Por = table.Column<string>(nullable: true),
                    Eliminado_En = table.Column<DateTime>(nullable: false),
                    Eliminado_Por = table.Column<string>(nullable: true),
                    Fecha = table.Column<DateTime>(nullable: false),
                    Id_Cuenta_Contable = table.Column<int>(nullable: false),
                    Id_Partida_Contable = table.Column<int>(nullable: false),
                    Modificado = table.Column<bool>(nullable: false),
                    Modificado_En = table.Column<DateTime>(nullable: false),
                    Modificado_Por = table.Column<string>(nullable: true),
                    Saldo = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Detalle_Partida", x => x.Id_Detalle_Partida);
                    table.ForeignKey(
                        name: "FK_Detalle_Partida_Cuenta_Contable_Id_Cuenta_Contable",
                        column: x => x.Id_Cuenta_Contable,
                        principalTable: "Cuenta_Contable",
                        principalColumn: "Id_Cuenta_Contable",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Detalle_Partida_Partida_Contable_Id_Partida_Contable",
                        column: x => x.Id_Partida_Contable,
                        principalTable: "Partida_Contable",
                        principalColumn: "Id_Partida_Contable",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Configuracion_Contabilidad_Id_Definicion_Transaccion",
                table: "Configuracion_Contabilidad",
                column: "Id_Definicion_Transaccion");

            migrationBuilder.CreateIndex(
                name: "IX_Configuracion_Contabilidad_Id_Operacion",
                table: "Configuracion_Contabilidad",
                column: "Id_Operacion");

            migrationBuilder.CreateIndex(
                name: "IX_Detalle_Partida_Id_Cuenta_Contable",
                table: "Detalle_Partida",
                column: "Id_Cuenta_Contable");

            migrationBuilder.CreateIndex(
                name: "IX_Detalle_Partida_Id_Partida_Contable",
                table: "Detalle_Partida",
                column: "Id_Partida_Contable");

            migrationBuilder.AddForeignKey(
                name: "FK_Cuenta_Contable_Tipo_Id_Tipo",
                table: "Cuenta_Contable",
                column: "Id_Tipo",
                principalTable: "Tipo",
                principalColumn: "Id_Tipo",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
