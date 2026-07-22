using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ModelNucleo.Model.Cuestionario_Socio
{
    public class Catalogo_Socio
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Catalogo_Socio { get; set; }

        public int Id_Catalogo_Cuestionario { get; set; }
        public Catalogo_Cuestionario Catalogo_Cuestionario { get; set; }

        public int Id_Persona { get; set; }
        public Persona Persona { get; set; }

        public int Id_Pregunta_Catalogo { get; set; }
        public Pregunta_Catalogo Pregunta_Catalogo { get; set; }

        public int Id_Respuesta_Catalogo { get; set; }
        public Respuesta_Catalogo Respuesta_Catalogo { get; set; }

        public string Respuesta_Libre { get; set; }
    }
}
