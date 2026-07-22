using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ModelNucleo.Model
{
    public class Descripcion_Documento
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Descripcion_Documento { get; set; }
        public string Descripcion { get; set; }

        [NotMapped]
        public ICollection<Documento> Documentos { get; set; }
    }
}
