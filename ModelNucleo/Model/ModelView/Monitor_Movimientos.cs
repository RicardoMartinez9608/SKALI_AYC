using System;
using System.Collections.Generic;
using System.Text;

namespace ModelNucleo.Model.ModelView
{
    public class Monitor_Movimientos
    {
        public string Comprobante { get; set; }
        public string Tipo { get; set; }
        public string Cuenta { get; set; }
        public string Concepto { get; set; }
        public decimal Monto { get; set; }
    }
}
