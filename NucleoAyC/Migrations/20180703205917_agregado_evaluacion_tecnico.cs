using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace NucleoAyC.Migrations
{
    public partial class agregado_evaluacion_tecnico : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documento_Persona_Id_Documento",
                table: "Documento");

            migrationBuilder.CreateTable(
                name: "Evaluacion",
                columns: table => new
                {
                    Id_Evaluacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Borrado_Logico = table.Column<bool>(type: "bit", nullable: false),
                    Creado_En = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Creado_Por = table.Column<string>(type: "longtext", nullable: true),
                    Eliminado_En = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Eliminado_Por = table.Column<string>(type: "longtext", nullable: true),
                    Id_Asesor = table.Column<string>(type: "longtext", nullable: true),
                    Id_Persona = table.Column<int>(type: "int", nullable: false),
                    Modificado = table.Column<bool>(type: "bit", nullable: false),
                    Modificado_En = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Modificado_Por = table.Column<string>(type: "longtext", nullable: true),
                    No_Dias_Buenos = table.Column<int>(type: "int", nullable: false),
                    No_Dias_Descanso = table.Column<int>(type: "int", nullable: false),
                    No_Dias_Malos = table.Column<int>(type: "int", nullable: false),
                    Venta_Mensual_Bueno = table.Column<decimal>(type: "decimal(65, 30)", nullable: false),
                    Venta_Semanal_Bueno = table.Column<decimal>(type: "decimal(65, 30)", nullable: false),
                    Venta_Semanal_Malo = table.Column<decimal>(type: "decimal(65, 30)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Evaluacion", x => x.Id_Evaluacion);
                    table.ForeignKey(
                        name: "FK_Evaluacion_Persona_Id_Persona",
                        column: x => x.Id_Persona,
                        principalTable: "Persona",
                        principalColumn: "Id_Persona",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tipo_Gasto",
                columns: table => new
                {
                    Id_Tipo_Gasto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Descripcion = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tipo_Gasto", x => x.Id_Tipo_Gasto);
                });

            migrationBuilder.CreateTable(
                name: "Tipo_Ingreso",
                columns: table => new
                {
                    Id_Tipo_Ingreso = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Descripcion = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tipo_Ingreso", x => x.Id_Tipo_Ingreso);
                });

            migrationBuilder.CreateTable(
                name: "Activo_Negocio",
                columns: table => new
                {
                    Id_Activo_Negocio = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Descripcion = table.Column<string>(type: "longtext", nullable: true),
                    Estado = table.Column<string>(type: "longtext", nullable: true),
                    Id_Evaluacion = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "longtext", nullable: true),
                    Precio_Compra = table.Column<decimal>(type: "decimal(65, 30)", nullable: false),
                    Precio_Peritado = table.Column<decimal>(type: "decimal(65, 30)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activo_Negocio", x => x.Id_Activo_Negocio);
                    table.ForeignKey(
                        name: "FK_Activo_Negocio_Evaluacion_Id_Evaluacion",
                        column: x => x.Id_Evaluacion,
                        principalTable: "Evaluacion",
                        principalColumn: "Id_Evaluacion",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Costo_Venta",
                columns: table => new
                {
                    Id_Costo_Venta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Id_Evaluacion = table.Column<int>(type: "int", nullable: false),
                    Margen_Bruto = table.Column<decimal>(type: "decimal(65, 30)", nullable: false),
                    Margen_Bruto_Porcentaje = table.Column<decimal>(type: "decimal(65, 30)", nullable: false),
                    Precio_Compra = table.Column<decimal>(type: "decimal(65, 30)", nullable: false),
                    Precio_Venta = table.Column<decimal>(type: "decimal(65, 30)", nullable: false),
                    Unidad_Medida = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Costo_Venta", x => x.Id_Costo_Venta);
                    table.ForeignKey(
                        name: "FK_Costo_Venta_Evaluacion_Id_Evaluacion",
                        column: x => x.Id_Evaluacion,
                        principalTable: "Evaluacion",
                        principalColumn: "Id_Evaluacion",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cuenta_Cobrar",
                columns: table => new
                {
                    Id_Cuenta_Cobrar = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Completo = table.Column<decimal>(type: "decimal(65, 30)", nullable: false),
                    Id_Evaluacion = table.Column<int>(type: "int", nullable: false),
                    Recuperado = table.Column<decimal>(type: "decimal(65, 30)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cuenta_Cobrar", x => x.Id_Cuenta_Cobrar);
                    table.ForeignKey(
                        name: "FK_Cuenta_Cobrar_Evaluacion_Id_Evaluacion",
                        column: x => x.Id_Evaluacion,
                        principalTable: "Evaluacion",
                        principalColumn: "Id_Evaluacion",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Inventario_Negocio",
                columns: table => new
                {
                    Id_Inventario_Negocio = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Cantidad = table.Column<decimal>(type: "decimal(65, 30)", nullable: false),
                    Detalle = table.Column<string>(type: "longtext", nullable: true),
                    Id_Evaluacion = table.Column<int>(type: "int", nullable: false),
                    Precio_Compra = table.Column<decimal>(type: "decimal(65, 30)", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(65, 30)", nullable: false),
                    Unidad_Compra = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventario_Negocio", x => x.Id_Inventario_Negocio);
                    table.ForeignKey(
                        name: "FK_Inventario_Negocio_Evaluacion_Id_Evaluacion",
                        column: x => x.Id_Evaluacion,
                        principalTable: "Evaluacion",
                        principalColumn: "Id_Evaluacion",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Prestamo",
                columns: table => new
                {
                    Id_Prestamo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Cuota = table.Column<decimal>(type: "decimal(65, 30)", nullable: false),
                    Frecuencia = table.Column<string>(type: "longtext", nullable: true),
                    Id_Evaluacion = table.Column<int>(type: "int", nullable: false),
                    Monto_Otorgado = table.Column<decimal>(type: "decimal(65, 30)", nullable: false),
                    Saldo = table.Column<decimal>(type: "decimal(65, 30)", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(65, 30)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prestamo", x => x.Id_Prestamo);
                    table.ForeignKey(
                        name: "FK_Prestamo_Evaluacion_Id_Evaluacion",
                        column: x => x.Id_Evaluacion,
                        principalTable: "Evaluacion",
                        principalColumn: "Id_Evaluacion",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Gastos",
                columns: table => new
                {
                    Id_Gasto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Descripcion = table.Column<string>(type: "longtext", nullable: true),
                    Id_Evaluacion = table.Column<int>(type: "int", nullable: false),
                    Id_Tipo_Gasto = table.Column<int>(type: "int", nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(65, 30)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gastos", x => x.Id_Gasto);
                    table.ForeignKey(
                        name: "FK_Gastos_Evaluacion_Id_Evaluacion",
                        column: x => x.Id_Evaluacion,
                        principalTable: "Evaluacion",
                        principalColumn: "Id_Evaluacion",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Gastos_Tipo_Gasto_Id_Tipo_Gasto",
                        column: x => x.Id_Tipo_Gasto,
                        principalTable: "Tipo_Gasto",
                        principalColumn: "Id_Tipo_Gasto",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Otro_Ingreso",
                columns: table => new
                {
                    Id_Otro_Ingreso = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Frecuencia = table.Column<string>(type: "longtext", nullable: true),
                    Id_Evaluacion = table.Column<int>(type: "int", nullable: false),
                    Id_Tipo_Ingreso = table.Column<int>(type: "int", nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(65, 30)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Otro_Ingreso", x => x.Id_Otro_Ingreso);
                    table.ForeignKey(
                        name: "FK_Otro_Ingreso_Evaluacion_Id_Evaluacion",
                        column: x => x.Id_Evaluacion,
                        principalTable: "Evaluacion",
                        principalColumn: "Id_Evaluacion",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Otro_Ingreso_Tipo_Ingreso_Id_Tipo_Ingreso",
                        column: x => x.Id_Tipo_Ingreso,
                        principalTable: "Tipo_Ingreso",
                        principalColumn: "Id_Tipo_Ingreso",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Documento_Id_Persona",
                table: "Documento",
                column: "Id_Persona");

            migrationBuilder.CreateIndex(
                name: "IX_Activo_Negocio_Id_Evaluacion",
                table: "Activo_Negocio",
                column: "Id_Evaluacion");

            migrationBuilder.CreateIndex(
                name: "IX_Costo_Venta_Id_Evaluacion",
                table: "Costo_Venta",
                column: "Id_Evaluacion");

            migrationBuilder.CreateIndex(
                name: "IX_Cuenta_Cobrar_Id_Evaluacion",
                table: "Cuenta_Cobrar",
                column: "Id_Evaluacion");

            migrationBuilder.CreateIndex(
                name: "IX_Evaluacion_Id_Persona",
                table: "Evaluacion",
                column: "Id_Persona");

            migrationBuilder.CreateIndex(
                name: "IX_Gastos_Id_Evaluacion",
                table: "Gastos",
                column: "Id_Evaluacion");

            migrationBuilder.CreateIndex(
                name: "IX_Gastos_Id_Tipo_Gasto",
                table: "Gastos",
                column: "Id_Tipo_Gasto");

            migrationBuilder.CreateIndex(
                name: "IX_Inventario_Negocio_Id_Evaluacion",
                table: "Inventario_Negocio",
                column: "Id_Evaluacion");

            migrationBuilder.CreateIndex(
                name: "IX_Otro_Ingreso_Id_Evaluacion",
                table: "Otro_Ingreso",
                column: "Id_Evaluacion");

            migrationBuilder.CreateIndex(
                name: "IX_Otro_Ingreso_Id_Tipo_Ingreso",
                table: "Otro_Ingreso",
                column: "Id_Tipo_Ingreso");

            migrationBuilder.CreateIndex(
                name: "IX_Prestamo_Id_Evaluacion",
                table: "Prestamo",
                column: "Id_Evaluacion");

            migrationBuilder.AddForeignKey(
                name: "FK_Documento_Persona_Id_Persona",
                table: "Documento",
                column: "Id_Persona",
                principalTable: "Persona",
                principalColumn: "Id_Persona",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documento_Persona_Id_Persona",
                table: "Documento");

            migrationBuilder.DropTable(
                name: "Activo_Negocio");

            migrationBuilder.DropTable(
                name: "Costo_Venta");

            migrationBuilder.DropTable(
                name: "Cuenta_Cobrar");

            migrationBuilder.DropTable(
                name: "Gastos");

            migrationBuilder.DropTable(
                name: "Inventario_Negocio");

            migrationBuilder.DropTable(
                name: "Otro_Ingreso");

            migrationBuilder.DropTable(
                name: "Prestamo");

            migrationBuilder.DropTable(
                name: "Tipo_Gasto");

            migrationBuilder.DropTable(
                name: "Tipo_Ingreso");

            migrationBuilder.DropTable(
                name: "Evaluacion");

            migrationBuilder.DropIndex(
                name: "IX_Documento_Id_Persona",
                table: "Documento");

            migrationBuilder.AddForeignKey(
                name: "FK_Documento_Persona_Id_Documento",
                table: "Documento",
                column: "Id_Documento",
                principalTable: "Persona",
                principalColumn: "Id_Persona",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
