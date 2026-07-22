using System;
using System.Collections.Generic;
using System.Text;

namespace ModelNucleo.Model.ModelView
{
    public class Agencia_Rendimiento
    {
        public string Agencia { get; set; }
        public string Generado_Por { get; set; }
        public DateTime Fecha { get; set; }
        public List<Rendimiento_Vista> Rendimientos { get; set; }
    }

    public class Rendimiento_Vista
    {
        public string Cartera { get; set; }
        public decimal Cartera_Refinanciada { get; set; }
        public decimal Retorno_Colecta { get; set; }
        public decimal Recuperacion_Capital { get; set; }
        public decimal Cobertura_Deuda { get; set; }
        public decimal Recargos { get; set; }
        public decimal Intereses { get; set; }
        public decimal Ingresos_Percibidos { get; set; }
        public decimal IVA { get; set; }
        public decimal Reinversion_Colocacion { get; set; }
        public decimal Efectivo_No_Rotativo { get; set; }
        public decimal Comision_Estructural { get; set; }
        public decimal Ingresos_Devengados { get; set; }
    }
}
