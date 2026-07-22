using System;
using System.Collections.Generic;
using System.Text;

namespace ModelNucleo.Model.ModelView
{
    public class Reversion_Credito
    {
        public DateTime Emision { get; set; }
        public string Usuario { get; set; }

        public string Codigo_Credito { get; set; }
        public string Cliente { get; set; }
        public decimal Monto { get; set; }
        public DateTime Revertido_En { get; set; }
        public DateTime Aperturado_En { get; set; }
        public string Concepto { get; set; }
        public string Revertido_Por { get; set; }
    }
}
