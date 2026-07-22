using System;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelNucleo.Model
{
    public class Porcentaje_Ahorro_Credito
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Porcentaje_Ahorro_Credito { get; set; }
        public decimal Porcentaje { get; set; }

        [NotMapped]
        public ICollection<Credito_Con_Ahorro>  Credito_Con_Ahorro { get; set; }
    }
}
