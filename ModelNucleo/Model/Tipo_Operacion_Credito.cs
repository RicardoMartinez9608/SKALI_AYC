using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelNucleo.Model
{
    public class Tipo_Operacion_Credito
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Tipo_Operacion_Credito { get; set; }
        public string Descripcion { get; set; }

        [NotMapped]
        public ICollection<Transaccion_Credito> Transacciones_Credito { get; set; }
    }
}
