using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class Libreta_Transaccion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Conf_Detalle_Partida_Contable_Cuenta_Contable_Id_Cuenta_Cont~",
            //    table: "Conf_Detalle_Partida_Contable");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Efectivo_SaldoDisponible_id_saldodisponible",
            //    table: "Efectivo");

            //migrationBuilder.DropIndex(
            //    name: "IX_Efectivo_id_saldodisponible",
            //    table: "Efectivo");

            //migrationBuilder.DropIndex(
            //    name: "IX_Conf_Detalle_Partida_Contable_Id_Cuenta_Contable",
            //    table: "Conf_Detalle_Partida_Contable");

            //migrationBuilder.AlterColumn<string>(
            //    name: "No_Comprobante",
            //    table: "Transaccion_Caja",
            //    nullable: false,
            //    oldClrType: typeof(string),
            //    oldType: "longtext CHARACTER SET utf8mb4",
            //    oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Id_Libreta",
                table: "Transaccion_Ahorro",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "No_Renglon",
                table: "Transaccion_Ahorro",
                nullable: false,
                defaultValue: 0);

            //migrationBuilder.AddColumn<int>(
            //    name: "SaldoDisponibleid_saldodisponible",
            //    table: "Efectivo",
            //    nullable: true);

            //migrationBuilder.AddColumn<int>(
            //    name: "Cuenta_ContableId_Cuenta_Contable",
            //    table: "Conf_Detalle_Partida_Contable",
            //    nullable: true);

            //migrationBuilder.AddUniqueConstraint(
            //    name: "AK_Transaccion_Caja_No_Comprobante_Fecha",
            //    table: "Transaccion_Caja",
            //    columns: new[] { "No_Comprobante", "Fecha" });

            migrationBuilder.CreateTable(
                name: "LibretaTransacciones",
                columns: table => new
                {
                    Id_Libreta = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    No_Libreta = table.Column<string>(nullable: true),
                    Id_Ahorro = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibretaTransacciones", x => x.Id_Libreta);
                    table.ForeignKey(
                        name: "FK_LibretaTransacciones_Ahorro_Id_Ahorro",
                        column: x => x.Id_Ahorro,
                        principalTable: "Ahorro",
                        principalColumn: "Id_Ahorro",
                        onDelete: ReferentialAction.Cascade);
                });

            //migrationBuilder.CreateIndex(
            //    name: "IX_Efectivo_SaldoDisponibleid_saldodisponible",
            //    table: "Efectivo",
            //    column: "SaldoDisponibleid_saldodisponible");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Conf_Detalle_Partida_Contable_Cuenta_ContableId_Cuenta_Conta~",
            //    table: "Conf_Detalle_Partida_Contable",
            //    column: "Cuenta_ContableId_Cuenta_Contable");

            migrationBuilder.CreateIndex(
                name: "IX_LibretaTransacciones_Id_Ahorro",
                table: "LibretaTransacciones",
                column: "Id_Ahorro");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Conf_Detalle_Partida_Contable_Cuenta_Contable_Cuenta_Contabl~",
            //    table: "Conf_Detalle_Partida_Contable",
            //    column: "Cuenta_ContableId_Cuenta_Contable",
            //    principalTable: "Cuenta_Contable",
            //    principalColumn: "Id_Cuenta_Contable",
            //    onDelete: ReferentialAction.Restrict);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Efectivo_SaldoDisponible_SaldoDisponibleid_saldodisponible",
            //    table: "Efectivo",
            //    column: "SaldoDisponibleid_saldodisponible",
            //    principalTable: "SaldoDisponible",
            //    principalColumn: "id_saldodisponible",
            //    onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Conf_Detalle_Partida_Contable_Cuenta_Contable_Cuenta_Contabl~",
            //    table: "Conf_Detalle_Partida_Contable");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Efectivo_SaldoDisponible_SaldoDisponibleid_saldodisponible",
            //    table: "Efectivo");

            migrationBuilder.DropTable(
                name: "LibretaTransacciones");

            //migrationBuilder.DropUniqueConstraint(
            //    name: "AK_Transaccion_Caja_No_Comprobante_Fecha",
            //    table: "Transaccion_Caja");

            //migrationBuilder.DropIndex(
            //    name: "IX_Efectivo_SaldoDisponibleid_saldodisponible",
            //    table: "Efectivo");

            //migrationBuilder.DropIndex(
            //    name: "IX_Conf_Detalle_Partida_Contable_Cuenta_ContableId_Cuenta_Conta~",
            //    table: "Conf_Detalle_Partida_Contable");

            migrationBuilder.DropColumn(
                name: "Id_Libreta",
                table: "Transaccion_Ahorro");

            migrationBuilder.DropColumn(
                name: "No_Renglon",
                table: "Transaccion_Ahorro");

            //migrationBuilder.DropColumn(
            //    name: "SaldoDisponibleid_saldodisponible",
            //    table: "Efectivo");

            //migrationBuilder.DropColumn(
            //    name: "Cuenta_ContableId_Cuenta_Contable",
            //    table: "Conf_Detalle_Partida_Contable");

            //migrationBuilder.AlterColumn<string>(
            //    name: "No_Comprobante",
            //    table: "Transaccion_Caja",
            //    type: "longtext CHARACTER SET utf8mb4",
            //    nullable: true,
            //    oldClrType: typeof(string));

            //migrationBuilder.CreateIndex(
            //    name: "IX_Efectivo_id_saldodisponible",
            //    table: "Efectivo",
            //    column: "id_saldodisponible");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Conf_Detalle_Partida_Contable_Id_Cuenta_Contable",
            //    table: "Conf_Detalle_Partida_Contable",
            //    column: "Id_Cuenta_Contable");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Conf_Detalle_Partida_Contable_Cuenta_Contable_Id_Cuenta_Cont~",
            //    table: "Conf_Detalle_Partida_Contable",
            //    column: "Id_Cuenta_Contable",
            //    principalTable: "Cuenta_Contable",
            //    principalColumn: "Id_Cuenta_Contable",
            //    onDelete: ReferentialAction.Cascade);

        //    migrationBuilder.AddForeignKey(
        //        name: "FK_Efectivo_SaldoDisponible_id_saldodisponible",
        //        table: "Efectivo",
        //        column: "id_saldodisponible",
        //        principalTable: "SaldoDisponible",
        //        principalColumn: "id_saldodisponible",
        //        onDelete: ReferentialAction.Cascade);
        }
    }
}
