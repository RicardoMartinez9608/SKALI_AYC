using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelNucleo.Model
{
    public class Fotografia_Atm
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Fotografia_Atm { get; set; }
        public int Id_Banco { get; set; }
        public Banco Banco { get; set; }
        public string fotografia { get; set; }
        public string Creado_Por { get; set; }
        public DateTime Creado_En { get; set; }
        public bool Verificado { get; set; }
        public string Verificado_Por { get; set; }
        public DateTime Verificado_En { get; set; }

    }
}
