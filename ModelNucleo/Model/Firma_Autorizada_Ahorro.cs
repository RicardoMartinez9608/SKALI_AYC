using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ModelNucleo.Model
{
    public class Firma_Autorizada_Ahorro
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Firma_Autorizada_Ahorro { get; set; }

        public int Id_Ahorro { get; set; }
        public Ahorro Ahorro { get; set; }

        public string Nombre_Completo { get; set; }
        public string DUI { get; set; }
        public string Parentesco { get; set; }

        [DefaultValue(false)]
        public bool Activo { get; set; }
    }
}
