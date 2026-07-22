using System;
using System.Collections.Generic;
using System.Text;

namespace ModelNucleo.Model.ModelView
{
    public class Capitalizacion
    {
        public int Id_Ahorro { get; set; }
        public string CuentaHabiente { get; set; }
        public string Cuenta { get; set; }
        public DateTime UltimaCapitalizacion { get; set; }
        public decimal Saldo { get; set; }
        public decimal Interes_Aplicar { get; set; }
        public decimal Tasa_Interes { get; set; }
        public int DiasCapitalizables { get; set; }
        public string FormaPago { get; set; }
        public int Id_Deposito_Plazo { get; set; }
    }
}
