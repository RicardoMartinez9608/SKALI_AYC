using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ModelNucleo.Model.Evaluacion_Tecnica
{
    public class Prestamo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Prestamo { get; set; }
        
        public int Id_Evaluacion { get; set; }
        public Evaluacion Evaluacion { get; set; }

        public decimal Monto_Otorgado { get; set; }
        public decimal Cuota { get; set; }
        public string Frecuencia { get; set; }
        public decimal Saldo { get; set; }
        public decimal Total { get; set; }
    }
}
