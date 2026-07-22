using System;
using System.Collections.Generic;
using System.Text;

namespace ModelNucleo.Model.ModelView
{
    public class Traslado_Abono
    {
        public int Id_Persona { get; set; }

        public DateTime Fecha { get; set; }
        public DateTime Fecha_Amortizacion { get; set; }
        public int Dias { get; set; }
        public string Id_Usuario { get; set; }
        public string Nombre_Completo { get; set; }
        public int Id_Credito { get; set; }
        public string Codigo_Credito { get; set; }
        public decimal Pago_Requerido { get; set; }
        public int Id_Ahorro { get; set; }
        public string Codigo_Ahorro { get; set; }
        public decimal Saldo_Ahorro { get; set; }
    }
}
