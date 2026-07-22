using System;
using System.Collections.Generic;
using System.Text;

namespace ModelNucleo.Model.Interfaces
{
    /*Es el cobro adicionales sean esto periodicos o no*/
    interface ICobro_Deduccion
    {
        string Nombre { get; set; }
        string Descripcion { get; set; }
        decimal Valor { get; set; }
    }
}
