using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Contracts;
using System.Text;

namespace ModelNucleo.Model
{
   public class Calificacion_Crediticia
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Calificacion_Crediticia { get; set; }
        public int Id_Persona { get; set; }
        public Persona Persona { get; set; }
        public int Id_Credito { get; set; }
        public Credito Credito { get; set; }
        public int Id_Configuracion_Calificacion { get; set; }
        public Configuracion_Calificacion Configuracion_Calificacion { get; set; }
        public int Dias { get; set; }
        public DateTime Fecha { get; set; }

        [NotMapped]
        public ICollection<Configuracion_Calificacion> Configuracion_Calificaciones { get; set; }


    }
}
