using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ModelNucleo.Model
{
   public class Libreta_Transaccion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Libreta { get; set; }
        public string No_Libreta { get; set; }
        public bool Activo { get; set; }
        public int Id_Ahorro { get; set; }
        public Ahorro Ahorro { get; set; }
     
    }
}
