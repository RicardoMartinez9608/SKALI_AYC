using System;
using System.Collections.Generic;
using System.Text;

namespace ModelNucleo.Model.ModelView
{
    public class Reporte_Mora_Deudores
    {
        public string Codigo_Cliente { get; set; }
        public string Nombre_Cliente { get; set; }
        public decimal Saldo_mora { get; set; }
        public decimal Porcentaje { get; set; }
        public int Dias { get; set; }
    }
}
