using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class ingreso_de_cheque : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ingreso_Cheque",
                columns: table => new
                {
                    Id_Ingreso_Cheque = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    No_Cheque = table.Column<string>(nullable: true),
                    Id_Banco = table.Column<int>(nullable: false),
                    Monto = table.Column<decimal>(nullable: false),
                    Id_Agencia = table.Column<int>(nullable: false),
                    Fecha = table.Column<DateTime>(nullable: false),
                    Comentario = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingreso_Cheque", x => x.Id_Ingreso_Cheque);
                    table.ForeignKey(
                        name: "FK_Ingreso_Cheque_Agencia_Id_Agencia",
                        column: x => x.Id_Agencia,
                        principalTable: "Agencia",
                        principalColumn: "Id_Agencia",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ingreso_Cheque_Id_Agencia",
                table: "Ingreso_Cheque",
                column: "Id_Agencia");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ingreso_Cheque");
        }
    }
}
