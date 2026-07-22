using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ModelNucleo.Model.Cuestionario_Socio
{
    public class Catalogo_Cuestionario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Catalogo_Cuestionario { get; set; }
        public string Nombre_Evaluado { get; set; }

        [NotMapped]
        public ICollection<Pregunta_Catalogo> Preguntas { get; set; }

        [NotMapped]
        public ICollection<Catalogo_Socio> Catalogo_Socios { get; set; }
    }
}
