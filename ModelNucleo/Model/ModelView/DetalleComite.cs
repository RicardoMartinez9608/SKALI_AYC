using System.Collections.Generic;

namespace ModelNucleo.Model.ModelView
{
    public class DetalleComite:Credito_Amortizacion
    {
        public Persona Cliente { get; set; }
        public Comite ComiteActivo { get; set; }
        public List<Comite> Comites { get; set; }
        public Agencia Agencia { get; set; }
    }
}
