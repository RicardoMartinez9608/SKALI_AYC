using System;
using System.Collections.Generic;
using System.Text;

namespace ModelNucleo.Model.ModelView
{
    public class ClienteVIewLite
    {
        public int Id_Persona { get; set; }
        public string NombreCompleto { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public int Id_Cartera { get; set; }
        public string Codigo { get; set; }
        public string Dui { get; set; }
        public string Nit { get; set; }
    }
}
