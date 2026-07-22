using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelNucleo.Model
{
    public class Definicion_Transaccion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Definicion_Transaccion { get; set; }
        public string Identificador { get; set; }
        public string Descripcion { get; set; }

        //[NotMapped]
        //public ICollection<Configuracion_Contabilidad> Configuraciones_Contabilidad { get; set; }
    }
}
