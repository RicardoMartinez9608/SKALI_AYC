using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ModelNucleo.Model.ModelView
{
    public class Encabezado_Pie_Pagina
    {
        public Encabezado_Pie_Pagina()
        {

        }

        public string Encabezado()
        {
            return string.Format("--print-media-type --header-right \"{0}\" --header-font-size \"8\"", DateTime.Now.ToString("D", CultureInfo.CreateSpecificCulture("es-MX")));
        }

        public string Pie()
        {
            return "";
        }

        public string Encabezado_Pie()
        {
            return "";
        }
    }
}
