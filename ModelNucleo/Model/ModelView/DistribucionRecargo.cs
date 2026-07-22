using System;
using System.Collections.Generic;
using System.Text;

namespace ModelNucleo.Model.ModelView
{
    public class DistribucionRecargo
    {
        public int Id_Cuota { get; set; }
        public int Dias { get; set; }
        public int Semanas { get; set; }
        public decimal Valor_Recargo { get; set; }

    }
}
