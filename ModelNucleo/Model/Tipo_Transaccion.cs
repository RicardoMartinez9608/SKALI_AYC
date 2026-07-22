using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelNucleo.Model
{
    public class Tipo_Transaccion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Tipo_Transaccion { get; set; }
        public string Descripcion { get; set; }

        [NotMapped]
        public ICollection<Transaccion_Caja> Transacciones_Caja { get; set; }
    }
}
