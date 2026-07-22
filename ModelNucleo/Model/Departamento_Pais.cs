using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ModelNucleo.Model
{
    public class Departamento_Pais
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Departamento_Pais { get; set; }
        public string Nombre_Departamento { get; set; }
        [NotMapped]
        public ICollection<Municipio_Departamento> Municipios_Departamento { get; set; }
        //[NotMapped]
        //public ICollection<Persona> Personas { get; set; }
    }
}
