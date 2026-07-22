using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ModelNucleo.Model.Evaluacion_Tecnica
{
    public class Otro_Ingreso
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Otro_Ingreso { get; set; }

        public int Id_Tipo_Ingreso { get; set; }
        public Tipo_Ingreso Tipo_Ingreso { get; set; }

        public int Id_Evaluacion { get; set; }
        public Evaluacion Evaluacion { get; set; }

        public decimal Valor { get; set; }
        public string Frecuencia { get; set; }
    }
}
