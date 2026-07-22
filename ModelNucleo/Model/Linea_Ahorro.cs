using ModelNucleo.Model.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelNucleo.Model
{
    public class Linea_Ahorro:ILinea_Ahorro
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Linea_Ahorro { get; set; }
        public decimal Tasa_Interes { get; set; }

        public int Id_Tipo_Linea_Ahorro { get; set; }
        public Tipo_Linea_Ahorro Tipo_Linea_Ahorro { get; set; }

        public string Descripcion { get; set; }

        public string Codigo { get; set; }

        public int Mes_Capitalizable { get; set; }
        public int Frecuencia_Deposito { get; set; }
        public int Plazo { get; set; }
        public decimal Monto_Minimo_Ahorro { get; set; }

        [NotMapped]
        public ICollection<Ahorro> Ahorros { get; set; }
    }
}
