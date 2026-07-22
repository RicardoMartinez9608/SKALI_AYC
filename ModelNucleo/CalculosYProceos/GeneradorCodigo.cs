using ModelNucleo.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModelNucleo.CalculosYProceos
{
    public static class GeneradorCodigo
    {
        public static string Generar(int id_entidad_ultimo,int id_persona)
        {
            string tiempo=DateTime.Now.ToString("MMddyyyyHHmmss");
            return string.Format("{0}-{1}-{2}",tiempo,(id_entidad_ultimo + 1), id_persona);
        }

        public static string GenerarSoloPorEntidad(int id_entidad_ultimo)
        {
            string tiempo = DateTime.Now.ToString("MMddyyyyHHmmss");
            return string.Format("{0}{1}", tiempo, (id_entidad_ultimo + 1));
        }
    }
}
