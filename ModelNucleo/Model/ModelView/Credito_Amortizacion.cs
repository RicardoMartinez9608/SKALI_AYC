using System;
using System.Collections.Generic;
using System.Text;

namespace ModelNucleo.Model.ModelView
{
    public class Credito_Amortizacion
    {
        public Persona Cliente { get; set; }
        public Credito Credito { get; set; }
        public List<Amortizacion> Amortizacion { get; set; }
    }
}
