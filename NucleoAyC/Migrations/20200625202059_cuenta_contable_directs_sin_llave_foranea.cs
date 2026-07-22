using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class cuenta_contable_directs_sin_llave_foranea : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Detalle_Asiento_Contable_Conf_Detalle_Partida_Contable_Id_Co~",
                table: "Detalle_Asiento_Contable");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Efectivo_SaldoDisponible_SaldoDisponibleid_saldodisponible",
            //    table: "Efectivo");

            //migrationBuilder.DropIndex(
            //    name: "IX_Efectivo_SaldoDisponibleid_saldodisponible",
            //    table: "Efectivo");

            migrationBuilder.DropIndex(
                name: "IX_Detalle_Asiento_Contable_Id_Conf_Detalle_Partida_Contable",
                table: "Detalle_Asiento_Contable");

            //migrationBuilder.DropColumn(
            //    name: "SaldoDisponibleid_saldodisponible",
            //    table: "Efectivo");

            migrationBuilder.AddColumn<string>(
                name: "Cuenta_Contable",
                table: "Detalle_Asiento_Contable",
                nullable: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_Efectivo_id_saldodisponible",
            //    table: "Efectivo",
            //    column: "id_saldodisponible");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Efectivo_SaldoDisponible_id_saldodisponible",
            //    table: "Efectivo",
            //    column: "id_saldodisponible",
            //    principalTable: "SaldoDisponible",
            //    principalColumn: "id_saldodisponible",
            //    onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Efectivo_SaldoDisponible_id_saldodisponible",
            //    table: "Efectivo");

            //migrationBuilder.DropIndex(
            //    name: "IX_Efectivo_id_saldodisponible",
            //    table: "Efectivo");

            migrationBuilder.DropColumn(
                name: "Cuenta_Contable",
                table: "Detalle_Asiento_Contable");

            //migrationBuilder.AddColumn<int>(
            //    name: "SaldoDisponibleid_saldodisponible",
            //    table: "Efectivo",
            //    type: "int",
            //    nullable: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_Efectivo_SaldoDisponibleid_saldodisponible",
            //    table: "Efectivo",
            //    column: "SaldoDisponibleid_saldodisponible");

            migrationBuilder.CreateIndex(
                name: "IX_Detalle_Asiento_Contable_Id_Conf_Detalle_Partida_Contable",
                table: "Detalle_Asiento_Contable",
                column: "Id_Conf_Detalle_Partida_Contable");

            migrationBuilder.AddForeignKey(
                name: "FK_Detalle_Asiento_Contable_Conf_Detalle_Partida_Contable_Id_Co~",
                table: "Detalle_Asiento_Contable",
                column: "Id_Conf_Detalle_Partida_Contable",
                principalTable: "Conf_Detalle_Partida_Contable",
                principalColumn: "Id_Conf_Detalle_Partida_Contable",
                onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Efectivo_SaldoDisponible_SaldoDisponibleid_saldodisponible",
            //    table: "Efectivo",
            //    column: "SaldoDisponibleid_saldodisponible",
            //    principalTable: "SaldoDisponible",
            //    principalColumn: "id_saldodisponible",
            //    onDelete: ReferentialAction.Restrict);
        }
    }
}
