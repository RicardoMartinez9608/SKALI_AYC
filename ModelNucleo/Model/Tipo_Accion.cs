using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ModelNucleo.Model
{
    public class Tipo_Accion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Tipo_Accion { get; set; }
        public string Tipo { get; set; }

        [NotMapped]
        public ICollection<Accion> Acciones { get; set; }
    }
}
