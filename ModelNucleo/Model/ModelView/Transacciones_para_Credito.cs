using System;
using System.Collections.Generic;
using System.Text;

namespace ModelNucleo.Model.ModelView
{
   public class Transacciones_para_Credito
    {
        public Transaccion_Credito Transacciones { get; set; }
        public Caja Caja { get; set; }
        public Transaccion_Caja Transaccion_Caja { get; set; }
    }
}
