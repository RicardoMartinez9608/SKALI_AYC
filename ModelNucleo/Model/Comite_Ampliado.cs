using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ModelNucleo.Model
{
    public class Comite_Ampliado
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Comite_Ampliado { get; set; }
        public int Id_Credito { get; set; }
        public Credito Credito { get; set; }
        public DateTime Creado_En { get; set; }
        public string Creado_Por { get; set; }
        public DateTime Aprobado_En { get; set; }
        public string Aprobado_Por { get; set; }
        public ICollection<Fotografia_Comite> Fotografia_Comites { get; set; }
        public ICollection<CreditoComiteAmpliado> CreditoComiteAmpliado { get; set; }
    }
}
