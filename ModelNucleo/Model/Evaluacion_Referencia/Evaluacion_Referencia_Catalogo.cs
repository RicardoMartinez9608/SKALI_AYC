using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ModelNucleo.Model.Evaluacion_Referencia
{
    public class Evaluacion_Referencia_Catalogo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Evaluacion_Referencia_Catalogo { get; set; }
        public string Nombre_Evaluacion { get; set; }

        [NotMapped]
        public ICollection<Pregunta> Preguntas { get; set; }

        [NotMapped]
        public ICollection<Evaluacion_Referencia_Persona> Evaluaciones_Referencias { get; set; }
    }
}
