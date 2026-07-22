using System;
using System.Collections.Generic;
using System.Text;

namespace ModelNucleo.Model.ModelView
{
    public class Movimientos_Contabilidad
    {
        public int id_asiento_contable { get; set; }
        public string Codigo { get; set; }
        public string Concepto { get; set; }
        public Totales_Contabilidad totales { get; set; }
        public List<Cargo_Abono_Contabilidad> Distribucion{ get; set; }
    }
    public class Cargo_Abono_Contabilidad
    {
        public string Cuenta_Contable { get; set; }
        public string Concepto { get; set; }
        public decimal Cargo { get; set; }
        public decimal Abono { get; set; }
       
    }
    public class Totales_Contabilidad 
    { 
        public decimal Total_Cargo { get; set; }
        public decimal Total_Abono { get; set; }

    }
}
