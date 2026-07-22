using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ModelNucleo.Model
{
    public class Credito_Grupal
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Credito_Grupal { get; set; }

        /*enlace a las agencias*/
        public Agencia Agencia { get; set; }
        public int Id_Agencia { get; set; }

        public string Nombre_grupo { get; set; }
        [NotMapped]
        public ICollection<Detalle_Credito_Grupal> Detalle_Credito_Grupal { get; set; }
    }
}
