using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelNucleo.Model
{
    public class Comite
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Comite { get; set; }
        public string Id_Usuario { get; set; }

        public int Id_Credito { get; set; }
        public Credito Credito { get; set; }

        [DefaultValue(false)]
        public bool Ampliado { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Fecha_Verificado { get; set; }

        [DefaultValue(false)]
        public bool Verificado { get; set; }
        [DefaultValue(false)]
        public bool Aprobado { get; set; }

        public string Comentario { get; set; }
    }
}
