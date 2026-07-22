using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ModelNucleo.Model
{
    public class Configuracion_Comite_Rol
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Configuracion_Comite { get; set; }
        public string Rol_Nombre { get; set; }
        public bool Ampliado { get; set; }
    }
}
