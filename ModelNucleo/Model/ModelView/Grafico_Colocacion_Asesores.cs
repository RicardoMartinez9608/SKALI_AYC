using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ModelNucleo.Model.ModelView
{
    public class Grafico_Colocacion_Asesores
    {
        public int fecha { get; set; }
        public List<string> fecha_completa { get; set; }
        public decimal valor { get; set; }
        public List<decimal> valores { get; set; }
        public DateTime fechas { get; set; }
        public decimal Diferencia_Mora { get; set; }
    }
}
