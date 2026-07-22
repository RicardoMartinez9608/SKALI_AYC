using System;
using System.Collections.Generic;
using System.Text;

namespace ModelNucleo.Model.ModelView
{
    public class ComiteAmpliadoDetalle
    {
        public Credito Credito { get; set; }
        public Persona Cliente { get; set; }
        public Comite_Ampliado Comite_Ampliado { get; set; }
    }
}
