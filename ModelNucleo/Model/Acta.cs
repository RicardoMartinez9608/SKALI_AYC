using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelNucleo.Model
{
    public class Acta
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Acta { get; set; }
        public int NoActa { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Fecha_Celebrado { get; set; }

        /*para saber si ya están todos créditos Liquidados*/
        [DefaultValue("false")]
        public bool Completado { get; set; }
        [DataType(DataType.Date)]
        public DateTime Fecha_Completado { get; set; }

        [NotMapped]
        public ICollection<Credito> Creditos { get; set; }
    }
}
