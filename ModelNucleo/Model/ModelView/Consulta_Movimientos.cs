using System;
using System.Collections.Generic;
using System.Text;

namespace ModelNucleo.Model.ModelView
{
    public class Consulta_Movimientos
    {
        public string Cuenta_Contable { get; set; }
        public string Descripcion { get; set; }
        public decimal Cargo { get; set; }
        public decimal abono { get; set; }
        public int  No_Comprobante { get; set; }
    }
}
