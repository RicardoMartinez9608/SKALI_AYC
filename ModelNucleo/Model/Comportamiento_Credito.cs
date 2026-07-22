using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ModelNucleo.Model
{
    public class Comportamiento_Credito
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Comportamiento_Credito { get; set; }
        public string Descripcion_Comportamiento_Credito { get; set; }

        [NotMapped]
        public ICollection<Credito> Creditos { get; set; }
    }
}
