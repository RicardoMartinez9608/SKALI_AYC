using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelNucleo.Model
{
    public class Persona_Predicado_Relacion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Persona_Predicado_Relacion { get; set; }

        public int Id_Predicado { get; set; }
        public Persona Predicado { get; set; }

        public int Id_Tipo_Relacion { get; set; }
        public Tipo_Relacion Tipo_Relacion { get; set; }
        
        public ICollection<Persona_Relaciones> Persona_Relaciones { get; set; }
    }
}
