using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ModelNucleo.Model.Evaluacion_Referencia
{
    public class Pregunta
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Pregunta { get; set; }
        public int Id_Referencia_Evaluada_Catalogo { get; set; }
        public Evaluacion_Referencia_Catalogo Evaluacion_Referencia_Catalogo { get; set; }
        public string Contenido { get; set; }
        public decimal Ponderacion { get; set; }

        [NotMapped]
        public ICollection<Respuesta> Respuestas { get; set; }

        [NotMapped]
        public ICollection<Evaluacion_Referencia_Persona> Evaluaciones_Referencias { get; set; }
    }
}
