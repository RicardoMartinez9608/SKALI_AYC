using System;
using System.Collections.Generic;
using System.Text;

namespace ModelNucleo.Model.ModelView
{
    public class RecargoCondonacionVista
    {
        public int Id_Amortizacion { get; set; }
        public int No { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Recargo { get; set; }
        public bool Cubierto { get; set; }
        public bool Condonar { get; set; }
    }
}
