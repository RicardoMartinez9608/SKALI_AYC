using ModelNucleo.Model.Tablero;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ModelNucleo.Model
{
    public class Cartera
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Cartera { get; set; }
        public string Id_Usuario { get; set; }
        public string Colector { get; set; }
        public string Nombre { get; set; }
        public bool Por_Defecto { get; set; }
        public bool Excluido { get; set; }
        
        [NotMapped]
        public ICollection<Persona> Personas { get; set; }
        [NotMapped]
        public ICollection<Credito> Creditos { get; set; }
        [NotMapped]
        public ICollection<Rancking> Rankings { get; set; }
        [NotMapped]
        public ICollection<Meta> Metas { get; set; }
        [NotMapped]
        public ICollection<Cosecha> Cosechas { get; set; }
        [NotMapped]
        public ICollection<Reporte_Mora> Reporte_Moras { get; set; }
    }
}
