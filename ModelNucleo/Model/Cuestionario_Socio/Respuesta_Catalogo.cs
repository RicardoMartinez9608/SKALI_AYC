using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ModelNucleo.Model.Cuestionario_Socio
{
    public class Respuesta_Catalogo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Respuesta_Catalogo { get; set; }

        public int Id_Pregunta_Catalogo { get; set; }
        public Pregunta_Catalogo Pregunta_Catalogo { get; set; }

        public bool Es_Respuesta_Libre { get; set; }

        public string Contenido_Respuesta_Catalogo { get; set; }

        public ICollection<Catalogo_Socio> Catalogo_Socios { get; set; }
    }
}
