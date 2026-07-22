using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ModelNucleo.Model
{
   public class Parentesco_Persona
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Parentesco { get; set; }
        public string Descripcion { get; set; }
        [NotMapped]
        public ICollection<Beneficiarios_Deposito_Plazo> Beneficiarios_Deposito_Plazo { get; set; }
    }
}
