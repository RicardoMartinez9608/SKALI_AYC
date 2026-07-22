using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ModelNucleo.Model
{
    public class Tipo_Persona
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Tipo_Persona { get; set; }
        public string Tipo_Persona_definicion { get; set; }
        [NotMapped]
        public ICollection<Persona_Tipo_Persona> Persona_Tipo_Persona { get; set; }
    }
}
