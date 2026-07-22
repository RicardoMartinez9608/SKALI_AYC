using System;
using System.Collections.Generic;
using System.Text;

namespace ModelNucleo.Model.ModelView
{
    public class Reporte_Vista<T>
    {
        public Encabezado_Reporte Encabezado { get; set; }
        public T Contenido { get; set; }
    }
}
