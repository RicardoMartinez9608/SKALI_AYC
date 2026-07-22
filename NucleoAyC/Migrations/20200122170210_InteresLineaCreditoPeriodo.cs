using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoAyC.Migrations
{
    public partial class InteresLineaCreditoPeriodo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Efectivo_SaldoDisponible_id_saldodisponible",
            //    table: "Efectivo");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Evaluacion_Credito_Id_Credito",
            //    table: "Evaluacion");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Linea_Credito_Historial_Credito_Id_Credito",
            //    table: "Linea_Credito_Historial");

            //migrationBuilder.DropUniqueConstraint(
            //    name: "AK_Persona_Tipo_Persona_Id_Persona_Tipo_Persona",
            //    table: "Persona_Tipo_Persona");

            //migrationBuilder.DropUniqueConstraint(
            //    name: "AK_Persona_Relaciones_Id_Persona_Relaciones",
            //    table: "Persona_Relaciones");

            //migrationBuilder.DropUniqueConstraint(
            //    name: "AK_Persona_Referencia_Id_Persona_Referencia",
            //    table: "Persona_Referencia");

            //migrationBuilder.DropIndex(
            //    name: "IX_Linea_Credito_Historial_Id_Credito",
            //    table: "Linea_Credito_Historial");

            //migrationBuilder.DropIndex(
            //    name: "IX_Evaluacion_Id_Credito",
            //    table: "Evaluacion");

            //migrationBuilder.DropIndex(
            //    name: "IX_Efectivo_id_saldodisponible",
            //    table: "Efectivo");

            //migrationBuilder.AddColumn<int>(
            //    name: "saldoid_saldodisponible",
            //    table: "Efectivo",
            //    nullable: true);

            //migrationBuilder.CreateTable(
            //    name: "InteresPeriodoLineaCreditos",
            //    columns: table => new
            //    {
            //        Id_Interes_Periodo_Linea_Credito = table.Column<int>(nullable: false)
            //            .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
            //        Id_Linea_Credito = table.Column<int>(nullable: false),
            //        Id_Periodo = table.Column<int>(nullable: false),
            //        Comision = table.Column<decimal>(nullable: false),
            //        Interes = table.Column<decimal>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_InteresPeriodoLineaCreditos", x => x.Id_Interes_Periodo_Linea_Credito);
            //        table.ForeignKey(
            //            name: "FK_InteresPeriodoLineaCreditos_Linea_Credito_Id_Linea_Credito",
            //            column: x => x.Id_Linea_Credito,
            //            principalTable: "Linea_Credito",
            //            principalColumn: "Id_Linea",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_InteresPeriodoLineaCreditos_Periodo_Id_Periodo",
            //            column: x => x.Id_Periodo,
            //            principalTable: "Periodo",
            //            principalColumn: "Id_Periodo",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_Efectivo_saldoid_saldodisponible",
            //    table: "Efectivo",
            //    column: "saldoid_saldodisponible");

            //migrationBuilder.CreateIndex(
            //    name: "IX_InteresPeriodoLineaCreditos_Id_Linea_Credito",
            //    table: "InteresPeriodoLineaCreditos",
            //    column: "Id_Linea_Credito");

            //migrationBuilder.CreateIndex(
            //    name: "IX_InteresPeriodoLineaCreditos_Id_Periodo",
            //    table: "InteresPeriodoLineaCreditos",
            //    column: "Id_Periodo");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Credito_Evaluacion_Id_Credito",
            //    table: "Credito",
            //    column: "Id_Credito",
            //    principalTable: "Evaluacion",
            //    principalColumn: "Id_Evaluacion",
            //    onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Efectivo_SaldoDisponible_saldoid_saldodisponible",
                table: "Efectivo",
                column: "id_saldodisponible",
                principalTable: "SaldoDisponible",
                principalColumn: "id_saldodisponible",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Linea_Credito_Historial_Credito_Id_Linea_Credito_Historial",
                table: "Linea_Credito_Historial",
                column: "Id_Linea_Credito_Historial",
                principalTable: "Credito",
                principalColumn: "Id_Credito",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Credito_Evaluacion_Id_Credito",
                table: "Credito");

            migrationBuilder.DropForeignKey(
                name: "FK_Efectivo_SaldoDisponible_saldoid_saldodisponible",
                table: "Efectivo");

            migrationBuilder.DropForeignKey(
                name: "FK_Linea_Credito_Historial_Credito_Id_Linea_Credito_Historial",
                table: "Linea_Credito_Historial");

            migrationBuilder.DropTable(
                name: "InteresPeriodoLineaCreditos");

            migrationBuilder.DropIndex(
                name: "IX_Efectivo_saldoid_saldodisponible",
                table: "Efectivo");

            migrationBuilder.DropColumn(
                name: "saldoid_saldodisponible",
                table: "Efectivo");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Persona_Tipo_Persona_Id_Persona_Tipo_Persona",
                table: "Persona_Tipo_Persona",
                column: "Id_Persona_Tipo_Persona");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Persona_Relaciones_Id_Persona_Relaciones",
                table: "Persona_Relaciones",
                column: "Id_Persona_Relaciones");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Persona_Referencia_Id_Persona_Referencia",
                table: "Persona_Referencia",
                column: "Id_Persona_Referencia");

            migrationBuilder.CreateIndex(
                name: "IX_Linea_Credito_Historial_Id_Credito",
                table: "Linea_Credito_Historial",
                column: "Id_Credito",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Evaluacion_Id_Credito",
                table: "Evaluacion",
                column: "Id_Credito",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Efectivo_id_saldodisponible",
                table: "Efectivo",
                column: "id_saldodisponible");

            migrationBuilder.AddForeignKey(
                name: "FK_Efectivo_SaldoDisponible_id_saldodisponible",
                table: "Efectivo",
                column: "id_saldodisponible",
                principalTable: "SaldoDisponible",
                principalColumn: "id_saldodisponible",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Evaluacion_Credito_Id_Credito",
                table: "Evaluacion",
                column: "Id_Credito",
                principalTable: "Credito",
                principalColumn: "Id_Credito",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Linea_Credito_Historial_Credito_Id_Credito",
                table: "Linea_Credito_Historial",
                column: "Id_Credito",
                principalTable: "Credito",
                principalColumn: "Id_Credito",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
