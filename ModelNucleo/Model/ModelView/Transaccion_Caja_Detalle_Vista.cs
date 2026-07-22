using System;
using System.Collections.Generic;
using System.Text;

namespace ModelNucleo.Model.ModelView
{
    public class Transaccion_Caja_Detalle_Vista
    {
        public Transaccion_Credito Transaccion_Credito { get; set; }
        public Transaccion_Ahorro Transaccion_Ahorro { get; set; }
        public Transaccion_Simple Transaccion_Simple { get; set; }
        public Transaccion_Caja Transaccion { get; set; }
    }
}
