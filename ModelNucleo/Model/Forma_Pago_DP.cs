using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ModelNucleo.Model
{
    public class Forma_Pago_DP
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Forma_Pago { get; set; }
        public string Descripcion { get; set; }
        [NotMapped]
        public ICollection<Deposito_Plazo> Deposito_Plazo { get; set; }
    }
}
