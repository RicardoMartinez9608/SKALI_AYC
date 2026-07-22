using System;
using System.Collections.Generic;
using System.Text;

namespace ModelNucleo.Model.ModelView
{
    public class Pago_Anticipado
    {
        public decimal Saldo_Capital { get; set; }
        public decimal Intereses_Dia { get; set; }
        public decimal Penalizacion { get; set; }
        public decimal Cobertura_Transito { get; set; }
        public decimal Valor_Pago_Anticipado { get; set; }
        // diferencia pago para cancelar - pago anticipado
        public decimal Pago_Para_Cancelar { get; set; }
        // sumatorita de saldo, ineteres, penalizacion y cobertura
        public decimal Descuento { get; set; }

    }
}
