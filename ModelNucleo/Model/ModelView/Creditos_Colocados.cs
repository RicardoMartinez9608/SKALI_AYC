using System;
using System.Collections.Generic;
using System.Text;

namespace ModelNucleo.Model.ModelView
{
    public class Creditos_Colocados
    {
        public string Codigo_Credito { get; set; }
        public string Nombre_Cliente { get; set; }
        public decimal Monto { get; set; }
        public decimal Tramites { get; set; }
        public decimal Refinanciamiento { get; set; }
        public decimal? totalExcentos { get; set; }
        public decimal? totalGravados { get; set; }
        public decimal Total { get; set; }

    }
}
