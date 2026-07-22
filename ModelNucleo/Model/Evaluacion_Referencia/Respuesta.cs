using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ModelNucleo.Model.Evaluacion_Referencia
{
    public class Respuesta
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Respuesta { get; set; }
        public int Id_Pregunta { get; set; }
        public Pregunta Pregunta { get; set; }
        public string Contenido { get; set; }
        public decimal Ponderacion { get; set; }

        [NotMapped]
        public ICollection<Evaluacion_Referencia_Persona> Evaluaciones_Referencias { get; set; }
    }
}
