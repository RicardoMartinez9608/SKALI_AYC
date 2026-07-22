using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ModelNucleo.Model
{
   public class Denominacion_efectivo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id_denominacion { get; set; }

        public decimal denominacion { get; set; }
        public string tipo { get; set; }

        [NotMapped]
        public ICollection<Efectivo> Efectivos { get; set; }
    }
}
