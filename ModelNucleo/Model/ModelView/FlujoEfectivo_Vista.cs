using System;
using System.Collections.Generic;
using System.Text;

namespace ModelNucleo.Model.ModelView
{
   public class FlujoEfectivo_Vista
    {
        //Sumas
        public string Nombre_agencia { get; set; }
        public decimal IngresoPagoCreditos { get; set; }
        public decimal FondosdeAdministracion { get; set; }
        public decimal DepositoenCuentasdeAhorro { get; set; }
        public decimal FondosdeDepositoaPlazo { get; set; }
        public decimal DepositosdePrestamosenCHC { get; set; }
        public decimal ColectorsAKI { get; set; }

        //Restas
        public decimal DesembolsoPrestamos { get; set; }
        public decimal PagodeDepositosaPlazoFijo { get; set; }
        public decimal RetirodeAhorros { get; set; }
        public decimal Remesas { get; set; }
        public decimal RemesasAKI { get; set; }

        //Pendientes de Liquidar
        public decimal ImpuestosMunicipales { get; set; }
        public decimal Alquiler { get; set; }
        public decimal Combustible { get; set; }
        public decimal MantenimientoyReparaciones { get; set; }
        public decimal ServiciosTVporCable { get; set; }
        public decimal BilletesPromocionales { get; set; }
        public decimal ServiciosBasicos { get; set; }

    }
}
