using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ModelNucleo.Model
{
   public class Manejo_Refinanciamiento
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Manejo_Refinanciamiento { get; set; }
        public int Id_Credito_Refinanciado { get; set; }
        public int Id_Credito_Nuevo { get; set; }
        public DateTime Fecha_Refinanciamiento { get; set; }
        public string Usuario { get; set; }
        public bool Descuento { get; set; }

        [NotMapped]
        public virtual Credito Credito { get; set; }
    }
}
