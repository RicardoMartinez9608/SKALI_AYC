using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class fotografia_atm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Verificado_En",
                table: "Fotografia_Comprobantes",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Verificado_Por",
                table: "Fotografia_Comprobantes",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Fotografia_Atms",
                columns: table => new
                {
                    Id_Fotografia_Atm = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Id_Banco = table.Column<int>(nullable: false),
                    fotografia = table.Column<string>(nullable: true),
                    Creado_Por = table.Column<string>(nullable: true),
                    Creado_En = table.Column<DateTime>(nullable: false),
                    Verificado = table.Column<bool>(nullable: false),
                    Verificado_Por = table.Column<string>(nullable: true),
                    Verificado_En = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fotografia_Atms", x => x.Id_Fotografia_Atm);
                    table.ForeignKey(
                        name: "FK_Fotografia_Atms_Banco_Id_Banco",
                        column: x => x.Id_Banco,
                        principalTable: "Banco",
                        principalColumn: "Id_Banco",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Fotografia_Atms_Id_Banco",
                table: "Fotografia_Atms",
                column: "Id_Banco");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Fotografia_Atms");

            migrationBuilder.DropColumn(
                name: "Verificado_En",
                table: "Fotografia_Comprobantes");

            migrationBuilder.DropColumn(
                name: "Verificado_Por",
                table: "Fotografia_Comprobantes");
        }
    }
}
