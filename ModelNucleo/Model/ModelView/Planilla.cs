using System;
using System.Collections.Generic;
using System.Text;

namespace ModelNucleo.Model.ModelView
{
   public class Planilla
    {
        public string codigo { get; set; }
        public string nombre { get; set; }
        public decimal monto { get; set; }
        public string mensaje_error { get; set; }
        public bool procesado { get; set; }
    }
}
