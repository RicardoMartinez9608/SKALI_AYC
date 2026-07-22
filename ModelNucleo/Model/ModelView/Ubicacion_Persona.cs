using System;
using System.Collections.Generic;
using System.Text;

namespace ModelNucleo.Model.ModelView
{
    public class Ubicacion_Persona
    {
        public int Id_Persona { get; set; }
        public string Tipo_Ubicacion { get; set; }//Domicilio,Negocio y Ttrabajo
        public string Lat { get; set; }
        public string Longi { get; set; }
    }
}
