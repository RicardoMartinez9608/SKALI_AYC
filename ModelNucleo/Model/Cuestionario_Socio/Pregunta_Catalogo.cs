using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ModelNucleo.Model.Cuestionario_Socio
{
    public class Pregunta_Catalogo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Pregunta_Catalogo { get; set; }

        public int Id_Catalogo_Cuestionario { get; set; }
        public Catalogo_Cuestionario Catalogo_Cuestionario { get; set; }

        public string Contenido_Pregunta { get; set; }

        [NotMapped]
        public ICollection<Respuesta_Catalogo> Respuestas { get; set; }

        [NotMapped]
        public ICollection<Catalogo_Socio> Catalogo_Socios { get; set; }
    }
}
