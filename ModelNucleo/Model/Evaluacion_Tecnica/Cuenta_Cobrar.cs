using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ModelNucleo.Model.Evaluacion_Tecnica
{
    public class Cuenta_Cobrar
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Cuenta_Cobrar { get; set; }

        public int Id_Evaluacion { get; set; }
        public Evaluacion Evaluacion { get; set; }

        public decimal Recuperado { get; set; }
        public decimal Completo { get; set; }
    }
}
