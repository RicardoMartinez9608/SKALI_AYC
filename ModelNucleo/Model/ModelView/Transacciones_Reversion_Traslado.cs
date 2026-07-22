using System;
using System.Collections.Generic;
using System.Text;

namespace ModelNucleo.Model.ModelView
{
    public class Transacciones_Reversion_Traslado
    {
        public int id_Transaccion_Credito { get; set; }
        public int id_Transaccion_Caja { get; set; }
        public int id_Transaccion_Ahorro { get; set; }
        public string nombre_persona { get; set; }
        public DateTime fecha_Traslado { get; set; }
        public int numero_caja { get; set; }
        public string codigo_credito { get; set; }
        public decimal valor_traslado { get; set; }
        public string codigo_ahorro { get; set; }
        public decimal valor_retiro { get; set; }
    }
}
