using System;
using System.Collections.Generic;
using System.Text;

namespace ModelNucleo.Model.ModelView
{
    public class Expediente
    {
        public Persona Socio { get; set; }
        public List<Documento_Clasificado> Documentos_Clasificados { get; set; }
    }

    public class Documento_Clasificado
    {
        public int Id_Documento { get; set; }
        public DateTime? Fecha { get; set; }
        public bool Agregado { get; set; }
        public string NombreArchivo { get; set; }
        public string Descripcion { get; set; }
        public string Archivo_Base64 { get; set; }
        public bool Es_PDF { get; set; }
        public bool Es_Imagen { get; set; }
    }
}
