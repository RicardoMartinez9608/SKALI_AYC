
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelNucleo.Model
{
    public class Tipo_Operacion_Ahorro
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Tipo_Operacion_Ahorro { get; set; }
        public string Descripcion { get; set; }
        [NotMapped]
        public ICollection<Transaccion_Ahorro> Transaccion_Ahorros { get; set; }
        [NotMapped]
        public ICollection<Transaccion_Deposito_Plazo> Transaccion_Deposito_Plazo { get; set; }
    }
}
