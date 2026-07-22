using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ModelNucleo.Model
{
    public class Detalle_Credito_Grupal
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Detalle_Credito_Grupal { get; set; }
        public int Id_Credito { get; set; }
        public Credito Credito { get; set; }
        public int Id_Credito_Grupal { get; set; }
        public Credito_Grupal Credito_Grupal { get; set; }

        [DefaultValue(false)]
        public bool Lider { get; set; }
      
    }
}
