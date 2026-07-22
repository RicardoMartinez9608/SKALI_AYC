using System;
using System.Collections.Generic;
using System.Text;

namespace ModelNucleo.Model.ModelView
{
   public class Datos_Credito
    {
        public decimal Interes_Diario { get; set; }
        public decimal Interes_Pendiente { get; set; }
        public decimal Cobertura_Pendiente { get; set; }
        public decimal Total_Intereses { get; set; }
        public decimal Recargos_Acumulados { get; set; }
        public decimal Recargos_Pendientes { get; set; }
        public decimal Pago_al_Dia { get; set; }
        public decimal Capital_Mora { get; set; }
        public Transaccion_Credito Transacciones { get; set; }
    }
}
