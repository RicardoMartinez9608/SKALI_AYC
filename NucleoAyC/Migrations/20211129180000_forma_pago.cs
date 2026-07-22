using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class forma_pago : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id_Forma_Pago",
                table: "Deposito_Plazo",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Forma_Pago",
                columns: table => new
                {
                    Id_Forma_Pago = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Descripcion = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Forma_Pago", x => x.Id_Forma_Pago);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Deposito_Plazo_Id_Forma_Pago",
                table: "Deposito_Plazo",
                column: "Id_Forma_Pago");

            migrationBuilder.AddForeignKey(
                name: "FK_Deposito_Plazo_Forma_Pago_Id_Forma_Pago",
                table: "Deposito_Plazo",
                column: "Id_Forma_Pago",
                principalTable: "Forma_Pago",
                principalColumn: "Id_Forma_Pago",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Deposito_Plazo_Forma_Pago_Id_Forma_Pago",
                table: "Deposito_Plazo");

            migrationBuilder.DropTable(
                name: "Forma_Pago");

            migrationBuilder.DropIndex(
                name: "IX_Deposito_Plazo_Id_Forma_Pago",
                table: "Deposito_Plazo");

            migrationBuilder.DropColumn(
                name: "Id_Forma_Pago",
                table: "Deposito_Plazo");
        }
    }
}
