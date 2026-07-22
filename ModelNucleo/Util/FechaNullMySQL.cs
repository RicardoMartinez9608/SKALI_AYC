using System;
using System.Collections.Generic;
using System.Text;

namespace ModelNucleo.Util
{
    public class FechaNullMySQL
    {
        public DateTime? Fecha { get; set; }

        public FechaNullMySQL(DateTime fecha)
        {
            if (fecha == DateTime.Parse("1/1/1901"))
                Fecha = null;
        }
    }
}
