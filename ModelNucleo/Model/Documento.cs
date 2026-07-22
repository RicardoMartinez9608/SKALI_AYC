using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelNucleo.Model
{
    public class Documento
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Documento { get; set; }

        public int Id_Persona { get; set; }
        public Persona Persona { get; set; }

        public int Id_Descripcion_Documento { get; set; }
        public Descripcion_Documento Descripcion { get; set; }

        public string NombreArchivo { get; set; }/*incluido el formato*/
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Fecha { get; set; }

        /*probar base64 imagen*/
        public string Archivo_Base64 { get; set; }
        public bool Es_Imagen { get; set; }
        public bool Es_PDF { get; set; }
    }
}
