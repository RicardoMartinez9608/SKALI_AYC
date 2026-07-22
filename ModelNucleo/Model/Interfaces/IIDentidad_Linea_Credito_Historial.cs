using System;
using System.Collections.Generic;
using System.Text;

namespace ModelNucleo.Model.Interfaces
{
    public interface IIDentidad_Linea_Credito_Historial
    {
        int Id_Linea_Credito_Historial { get; set; }
        decimal Valor { get; set; }
        Tipo_Valor Tipo_Valor { get; set; }
    }
}
