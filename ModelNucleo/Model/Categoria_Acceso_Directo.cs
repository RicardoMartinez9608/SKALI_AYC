using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ModelNucleo.Model
{
    public class Categoria_Acceso_Directo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Categoria_Acceso_Directo { get; set; }
        public string Id_Usuario { get; set; }
        public string Nombre_Categoria { get; set; }

        [NotMapped]
        public IEnumerable<Acceso_Directo> Acceso_Directos { get; set; }
    }
}
