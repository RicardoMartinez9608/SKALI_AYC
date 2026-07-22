using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class Tabla_Refinanciamientos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id_Manejo_Refinanciamiento",
                table: "Credito",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Manejo_Refinanciamiento",
                columns: table => new
                {
                    Id_Manejo_Refinanciamiento = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Id_Credito_Refinanciado = table.Column<int>(nullable: false),
                    Id_Credito_Nuevo = table.Column<int>(nullable: false),
                    Fecha_Refinanciamiento = table.Column<DateTime>(nullable: false),
                    Usuario = table.Column<string>(nullable: true),
                    Descuento = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manejo_Refinanciamiento", x => x.Id_Manejo_Refinanciamiento);
                    table.ForeignKey(
                        name: "FK_Manejo_Refinanciamiento_Credito_Id_Credito_Nuevo",
                        column: x => x.Id_Credito_Nuevo,
                        principalTable: "Credito",
                        principalColumn: "Id_Credito",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Manejo_Refinanciamiento_Id_Credito_Nuevo",
                table: "Manejo_Refinanciamiento",
                column: "Id_Credito_Nuevo",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Manejo_Refinanciamiento");

            migrationBuilder.DropColumn(
                name: "Id_Manejo_Refinanciamiento",
                table: "Credito");
        }
    }
}
