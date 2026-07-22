using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ModelNucleo.Model
{
    public class Interes_Periodo_Linea_Credito
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id_Interes_Periodo_Linea_Credito { get; set; }

        public int Id_Linea_Credito { get; set; }
        public Linea_Credito LineaCredito { get; set; }
        public int Id_Periodo { get; set; }
        public Periodo Periodo { get; set; }
        public decimal Interes { get; set; }
        public decimal Interes_Comision { get; set; }

    }
}