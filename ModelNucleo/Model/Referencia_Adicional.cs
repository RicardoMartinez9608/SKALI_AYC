using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ModelNucleo.Model
{
    public class Referencia_Adicional
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Referencia_Adicional { get; set; }

        public int Id_Referencia { get; set; }
        public Referencia Referencia { get; set; }

        public string Parentesco { get; set; }
        public string Tipo_Referencia { get; set; }

        public string Telefono { get; set; }
        public string Residencia { get; set; }

        public string Nombre { get; set; }
    }
}
