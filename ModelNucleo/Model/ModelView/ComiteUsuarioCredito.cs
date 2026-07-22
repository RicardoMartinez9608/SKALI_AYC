using System;
using System.Collections.Generic;
using System.Text;

namespace ModelNucleo.Model.ModelView
{
    public class ComiteUsuarioCredito
    {
        public Credito Credito { get; set; }
        public Comite Comite { get; set; }
        public Persona Cliente { get; set; }
        public Agencia Agencia { get; set; }
    }
}
