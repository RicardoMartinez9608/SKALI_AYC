using System;
using System.Collections.Generic;
using System.Text;

namespace ModelNucleo.Model.ModelView
{
    public class AmortizacionVolatil : Amortizacion
    {
        public bool Puntual { get; set; }
        public bool Vencido { get; set; }
        public bool Mora { get; set; }
        public bool Anticipado { get; set; }
        public int DiasTranscurridos { get; set; }
        public int Pagos_Acumulado { get; set; }
        public decimal Recargo { get; set; }

        /*datos para mostrar detalle y en reportes*/
        public decimal Capital_Mora { get; set; }
        public decimal Interes_Diario { get; set; }
        public decimal Interes_Pendiente { get; set; }
        public decimal Comsion_Pendiente { get; set; }
        public decimal Valor_Al_Dia { get; set; }
        /*sobre los recargos*/
        public decimal Recargos_Acumulados { get; set; } /*recargos pagados*/
    }
}
