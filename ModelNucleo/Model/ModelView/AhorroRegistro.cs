using System;
using System.Collections.Generic;
using System.Text;

namespace ModelNucleo.Model.ModelView
{
    public class AhorroRegistro
    {
        public decimal Deposito { get; set; }
        public DateTime Fecha { get; set; }
        public int Dias { get; set; }
        public decimal Interes { get; set; }
        public decimal Saldo { get; set; }
    }
}
