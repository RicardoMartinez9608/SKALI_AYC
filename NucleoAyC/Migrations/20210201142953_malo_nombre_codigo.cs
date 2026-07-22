using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class malo_nombre_codigo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "CreditoAgencia");

            migrationBuilder.CreateTable(
                name: "Credito_Agencia",
                columns: table => new
                {
                    Id_Credito = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Id_Agencia = table.Column<int>(nullable: false),
                    Correlativo = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Credito_Agencia", x => x.Id_Credito);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Credito_Agencia");

            migrationBuilder.CreateTable(
                name: "CreditoAgencia",
                columns: table => new
                {
                    Id_Credito = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Correlativo = table.Column<int>(type: "int", nullable: false),
                    Id_Agencia = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditoAgencia", x => x.Id_Credito);
                });
        }
    }
}
