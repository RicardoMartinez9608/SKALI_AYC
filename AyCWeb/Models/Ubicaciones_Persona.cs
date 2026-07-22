using ModelNucleo.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AyCWeb.Models
{
    public class Ubicacion_Politica_Etiqueta_Persona {
        public Persona Persona{ get; set; }
        public Etiqueta_Municipio_Departamento Etiquetas { get; set; }
    }


    public class Etiqueta_Municipio_Departamento
    {
        public string Municipio_Nacimiento { get; set; }
        public string Departamento_Nacimiento { get; set; }

        public string Municipio_Domicilio { get; set; }
        public string Departamento_Domicilio { get; set; }

        public string Municipio_Trabajo { get; set; }
        public string Departamento_Trabajo { get; set; }

        public string Municipio_Negocio { get; set; }
        public string Departamento_Negocio { get; set; }
    }
}
