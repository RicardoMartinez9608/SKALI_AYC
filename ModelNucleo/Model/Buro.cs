using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ModelNucleo.Model
{
   public class Buro
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Buro { get; set; }

        public string Nombre { get; set; }
        [NotMapped]
        public ICollection<Configuracion_Calificacion> Configuracion_Calificacion { get; set; }
    }
}
