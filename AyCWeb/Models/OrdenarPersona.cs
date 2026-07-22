using ModelNucleo.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AyCWeb.Models
{
    public class OrdenarPersona
    {
        public int No { get; set; }
        public Persona Persona { get; set; } 
    }

    public class ParametroPersonaDocumento
    { 
        public int No { get; set; }
        public string NombreCompleto { get; set; }
        public string DUI { get; set; }
        public string NIT { get; set; }
        public string DireccionCasa { get; set; }
        public string Edad { get; set; }
        public string Ciudad { get; set; }
        public string Departamento { get; set; }
        public string MontoCredito { get; set; }
    }
}
