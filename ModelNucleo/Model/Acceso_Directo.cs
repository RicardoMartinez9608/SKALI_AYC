using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ModelNucleo.Model
{
    public class Acceso_Directo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Acceso_Directo { get; set; }

        public int Id_Categoria_Acceso_Directo { get; set; }
        public Categoria_Acceso_Directo Categoria_Acceso_Directo { get; set; }

        public string Nombre { get; set; }
        public string Url { get; set; }
    }
}
