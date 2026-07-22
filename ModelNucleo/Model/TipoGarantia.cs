using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ModelNucleo.Model
{
    public class TipoGarantia
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Tipo_Garantia { get; set; }
        public string Descripcion { get; set; }
        [NotMapped]
        public ICollection<Linea_Credito> Linea_Creditos { get; set; }
    }
}
