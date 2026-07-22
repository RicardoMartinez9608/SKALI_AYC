using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class TipoGarantia : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "TipoGarantias",
            //    columns: table => new
            //    {
            //        Tipo_Garantia = table.Column<int>(nullable: false)
            //            .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
            //        Descripcion = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_TipoGarantias", x => x.Tipo_Garantia);
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_Linea_Credito_Tipo_Garantia",
            //    table: "Linea_Credito",
            //    column: "Tipo_Garantia");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Linea_Credito_TipoGarantias_Tipo_Garantia",
            //    table: "Linea_Credito",
            //    column: "Tipo_Garantia",
            //    principalTable: "TipoGarantias",
            //    principalColumn: "Tipo_Garantia",
            //    onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Linea_Credito_TipoGarantias_Tipo_Garantia",
            //    table: "Linea_Credito");

            //migrationBuilder.DropTable(
            //    name: "TipoGarantias");

            //migrationBuilder.DropIndex(
            //    name: "IX_Linea_Credito_Tipo_Garantia",
            //    table: "Linea_Credito");
        }
    }
}
