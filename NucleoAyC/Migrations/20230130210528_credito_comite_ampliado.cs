using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class credito_comite_ampliado : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CreditoComiteAmpliados",
                columns: table => new
                {
                    Id_Credito_Comite_Ampliado = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Id_Comite_Ampliado = table.Column<int>(nullable: false),
                    Codigo_Credito = table.Column<string>(nullable: true),
                    Cuota_Total = table.Column<decimal>(nullable: false),
                    Linea = table.Column<string>(nullable: true),
                    Monto = table.Column<decimal>(nullable: false),
                    Numero_Cuotas = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditoComiteAmpliados", x => x.Id_Credito_Comite_Ampliado);
                    table.ForeignKey(
                        name: "FK_CreditoComiteAmpliados_Comite_Ampliados_Id_Comite_Ampliado",
                        column: x => x.Id_Comite_Ampliado,
                        principalTable: "Comite_Ampliados",
                        principalColumn: "Id_Comite_Ampliado",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CreditoComiteAmpliados_Id_Comite_Ampliado",
                table: "CreditoComiteAmpliados",
                column: "Id_Comite_Ampliado");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CreditoComiteAmpliados");
        }
    }
}
