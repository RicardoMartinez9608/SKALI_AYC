using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class frma_autoizada : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Firma_Autorizada_Ahorro",
                columns: table => new
                {
                    Id_Firma_Autorizada_Ahorro = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Id_Ahorro = table.Column<int>(nullable: false),
                    DUI = table.Column<string>(nullable: true),
                    Parentesco = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Firma_Autorizada_Ahorro", x => x.Id_Firma_Autorizada_Ahorro);
                    table.ForeignKey(
                        name: "FK_Firma_Autorizada_Ahorro_Ahorro_Id_Ahorro",
                        column: x => x.Id_Ahorro,
                        principalTable: "Ahorro",
                        principalColumn: "Id_Ahorro",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Firma_Autorizada_Ahorro_Id_Ahorro",
                table: "Firma_Autorizada_Ahorro",
                column: "Id_Ahorro");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Firma_Autorizada_Ahorro");
        }
    }
}
