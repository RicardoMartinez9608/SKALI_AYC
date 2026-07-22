using System;
using System.Collections.Generic;
using System.Text;

namespace ModelNucleo.Model.ModelView
{
    public class DetalleCreditoVista
    {
        public List<Transacciones_para_Credito> Transacciones { get; set; }
        public Persona Codeudor { get; set; }
        public Credito Credito { get; set; }
    }
}
