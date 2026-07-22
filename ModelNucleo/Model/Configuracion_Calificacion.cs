using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ModelNucleo.Model
{
    public class Configuracion_Calificacion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Configuracion { get; set; }
        public int Id_Buro { get; set; }
        public Buro Buro { get; set; }
        public string Calificacion { get; set; }
        public int Dia_Inicio { get; set; }
        public int Dia_Fin { get; set; }
        public decimal Saneamiento { get;set; }
        [NotMapped]
        public ICollection<Calificacion_Crediticia> Calificacion_Crediticias { get; set; }
    }
}
