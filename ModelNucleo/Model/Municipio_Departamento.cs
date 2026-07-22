using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ModelNucleo.Model
{
    public class Municipio_Departamento
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Municipio_Departamento { get; set; }
        public int Id_Departamento_Pais { get; set; }
        public Departamento_Pais Departamento_Pais { get; set; }
        public string Nombre_Municipio { get; set; }

        //[NotMapped]
        //public ICollection<Persona> Personas { get; set; }
    }
}
