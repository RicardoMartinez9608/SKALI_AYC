using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ModelNucleo.Model
{
    public class Tipo_Operacion_Simple
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Tipo_Operacion_Simple { get; set; }
        public string Descripcion { get; set; }

        [NotMapped]
        public ICollection<Transaccion_Simple> Transacciones_Simple { get; set; }
    }
}
