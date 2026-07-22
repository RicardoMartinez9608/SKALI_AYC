using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelNucleo.Model
{
    public class Tipo_Relacion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Tipo_Relacion { get; set; }
        public string Tipo_Relacion_Definicion { get; set; }
        [NotMapped]
        public ICollection<Persona_Relaciones> Persona_Relaciones { get; set; }
    }
}
