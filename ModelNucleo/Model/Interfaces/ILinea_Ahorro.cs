using System;
using System.Collections.Generic;
using System.Text;

namespace ModelNucleo.Model.Interfaces
{
    public interface ILinea_Ahorro
    {
        decimal Tasa_Interes { get; set; }
        int Mes_Capitalizable { get; set; }
        int Frecuencia_Deposito { get; set; }
        int Plazo { get; set; }
        decimal Monto_Minimo_Ahorro { get; set; }
    }
}
