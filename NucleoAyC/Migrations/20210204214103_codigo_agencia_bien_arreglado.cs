using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class codigo_agencia_bien_arreglado : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Credito_Agencia");

            migrationBuilder.CreateTable(
                name: "Codigo_Agencia",
                columns: table => new
                {
                    Id_Codigo_Agencia = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Id_Ahorro = table.Column<int>(nullable: false),
                    Id_Credito = table.Column<int>(nullable: false),
                    Id_Agencia = table.Column<int>(nullable: false),
                    Id_Linea = table.Column<int>(nullable: false),
                    Id_Persona = table.Column<int>(nullable: false),
                    Correlativo = table.Column<int>(nullable: false),
                    Correlativo_Producto = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Codigo_Agencia", x => x.Id_Codigo_Agencia);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Codigo_Agencia");

            migrationBuilder.CreateTable(
                name: "Credito_Agencia",
                columns: table => new
                {
                    Id_Credito = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Correlativo = table.Column<int>(type: "int", nullable: false),
                    Id_Agencia = table.Column<int>(type: "int", nullable: false),
                    Id_Linea = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Credito_Agencia", x => x.Id_Credito);
                });
        }
    }
}
