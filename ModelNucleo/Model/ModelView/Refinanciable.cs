using System;
using System.Collections.Generic;
using System.Text;

namespace ModelNucleo.Model.ModelView
{
    public class Refinanciable
    {
        public int Id_Credito_Refinanciable { get; set; }
        public string Codigo_Refinanciable { get; set; }
        public string Socio_Refinanciado { get; set; }
        public string Dui_Socio_Refinanciado { get; set; }
        public int Id_Credito_Nuevo { get; set; }
        public decimal Monto_Refinanciable { get; set; }
        public decimal Monto_Nuevo { get; set; }
        public decimal Remanente { get; set; }
        public bool EsRefinanciable { get; set; }
    }
}
