using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ModelNucleo.Model
{
    public class Linea_Deposito_Plazo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Linea_Deposito_Plazo { get; set; }

        public String Descripcíon { get; set; }

        public decimal Interes { get; set; }

        public ICollection<Deposito_Plazo> Depositos_Plazo { get; set; }

    }
}
