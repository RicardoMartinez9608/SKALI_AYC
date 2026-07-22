using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AyCWeb.Models
{
    public class Referencia_Encabezado
    {
        public int id_referencia { get; set; }
        public string nombre_socio { get; set; }
        public string nombre_referencia { get; set; }
        public string telefono_disponible { get; set; }
        public string tipo_referencia { get; set; }
    }
}
