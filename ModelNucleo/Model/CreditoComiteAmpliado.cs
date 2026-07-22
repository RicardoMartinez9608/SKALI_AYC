using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ModelNucleo.Model
{
    public class CreditoComiteAmpliado
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Credito_Comite_Ampliado { get; set; }
        public int Id_Comite_Ampliado { get; set; }
        public Comite_Ampliado Comite_Ampliado { get; set; }
        public string Codigo_Credito { get; set; }
        public decimal Cuota_Total { get; set; }
        public string Linea { get; set; }
        public decimal Monto { get; set; }
        public decimal Numero_Cuotas { get; set; }
    }
}
