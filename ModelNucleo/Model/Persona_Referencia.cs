using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ModelNucleo.Model
{
    public class Persona_Referencia
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Persona_Referencia { get; set; }
        public int Id_Persona { get; set; }
        public Persona Persona { get; set; }
        public int Id_Referencia { get; set; }
        public Referencia Referencia { get; set; }
        public string Parentesco { get; set; }
        public string Tipo_Referencia { get; set; }
    }
}
