using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ModelNucleo.Model
{
    public class Deposito_Plazo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Deposito_Plazo { get; set; }

        public int Id_Linea_Deposito_Plazo { get; set; }

        public Linea_Deposito_Plazo Linea_Deposito_Plazo { get; set; }

        public int Id_Persona { get; set; }
        public Persona Persona { get; set; }
        public string No_Ahorro { get; set; }
        public DateTime Fecha_Apertura { get; set; }
        public DateTime Fecha_Vencimiento { get; set; }
        public decimal Monto { get; set; }
        public decimal Interes { get; set; }
        public int Plazo { get; set; }
        public int Id_Ahorro { get; set; }
        public string No_Certificado { get; set; }
        public DateTime? Fecha_Ultima_Capitalizacion { get; set; }

        [DefaultValue(false)]
        public bool Pagos_Interes_Mensual { get; set; }
        public int Id_Forma_Pago { get; set; }
        public Forma_Pago_DP Forma_Pago { get; set; }
        [NotMapped]
        public ICollection<Autorizado> Autorizado { get; set; }
        [NotMapped]
        public ICollection<Beneficiarios_Deposito_Plazo> Beneficiarios_Deposito_Plazo { get; set; }
        [NotMapped]
        public ICollection<Transaccion_Deposito_Plazo> Transacciones_Depositos { get; set; }
    }
}
