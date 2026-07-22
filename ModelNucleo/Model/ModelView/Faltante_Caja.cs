using System;
using System.Collections.Generic;
using System.Text;

namespace ModelNucleo.Model.ModelView
{
    public class Faltante_Caja
    {
        public decimal Saldo_Fisico { get; set; }
        public decimal Saldo_Electronico { get; set; }
        public decimal Diferencia { get; set; }
        public bool Faltante_Fisico { get; set; }
        public bool Faltante_Electronico { get; set; }
        public bool Correcto { get; set; }

        public Faltante_Caja()
        { 
        
        }

        public Faltante_Caja(decimal saldo_fisico,decimal saldo_elect)
        {
            Diferencia = Math.Round(saldo_fisico - saldo_elect,2);
            if (Diferencia < 0)
            {
                Faltante_Fisico = true;
                Faltante_Electronico = false;
                Correcto = false;
            }
            if (Diferencia > 0)
            {
                Faltante_Fisico = false;
                Faltante_Electronico = true;
                Correcto = false;
            }
            if (Diferencia == 0)
            {
                Faltante_Fisico = false;
                Faltante_Electronico = false;
                Correcto = true;
            }
        }
    }
}
