using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ModelNucleo.Model
{
    public class Fotografia_Comite
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Fotografia_Comite { get; set; }
        public int Id_Comite_Ampliado { get; set; }
        public Comite_Ampliado Comite_Ampliado { get; set; }
        public string Foto { get; set; }
        public string Descripcion { get; set; }
    }
}
