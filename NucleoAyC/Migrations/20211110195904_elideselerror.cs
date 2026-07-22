using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class elideselerror : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Amortizacion",
                table: "Amortizacion");

            migrationBuilder.AlterColumn<int>(
                name: "No",
                table: "Amortizacion",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<int>(
                name: "Id_Amortizacion",
                table: "Amortizacion",
                nullable: false,
                defaultValue: 0)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Amortizacion",
                table: "Amortizacion",
                column: "Id_Amortizacion");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Amortizacion",
                table: "Amortizacion");

            migrationBuilder.DropColumn(
                name: "Id_Amortizacion",
                table: "Amortizacion");

            migrationBuilder.AlterColumn<int>(
                name: "No",
                table: "Amortizacion",
                type: "int",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Amortizacion",
                table: "Amortizacion",
                column: "No");
        }
    }
}
