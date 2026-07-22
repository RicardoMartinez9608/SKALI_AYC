using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ModelNucleo.Model
{
    public class Operacion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Operacion { get; set; }
        public string Descripcion { get; set; }
        //[NotMapped]
        //public ICollection<Configuracion_Contabilidad> Configuraciones_Contabilidad { get; set; }
    }
}
