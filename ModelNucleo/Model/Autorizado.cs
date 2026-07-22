using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ModelNucleo.Model
{
    public class Autorizado
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Autorizado { get; set; }
        public int Id_Deposito_Plazo { get; set; }
        public Deposito_Plazo Deposito_Plazo { get; set; }
        public string Nombre { get; set; } 
        public string Dui { get; set; }
    }
}
