using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ModelNucleo.Model
{
    public class Persona_Tipo_Persona
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Persona_Tipo_Persona { get; set; }

        public int Id_Persona { get; set; }
        public Persona Persona { get; set; }

        public int Id_Tipo_Persona { get; set; }
        public Tipo_Persona Tipo_Persona { get; set; }
    }
}
