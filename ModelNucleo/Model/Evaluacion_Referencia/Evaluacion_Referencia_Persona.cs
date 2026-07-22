using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ModelNucleo.Model.Evaluacion_Referencia
{
    public class Evaluacion_Referencia_Persona
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Evaluacion_Referencia_Persona { get; set; }

        public int Id_Persona { get; set; }
        public Persona Persona { get; set; }

        public int Id_Referencia { get; set; }
        public Referencia Referencia { get; set; }

        public int Id_Evaluacion_Referencia_Catalogo { get; set; }
        public Evaluacion_Referencia_Catalogo Evaluacion_Referencia_Catalogo { get; set; }

        public int Id_Pregunta { get; set; }
        public Pregunta Pregunta { get; set; }

        public int Id_Respuesta { get; set; }
        public Respuesta Respuesta { get; set; }
        
        public decimal Ponderacion { get; set; }
    }
}
