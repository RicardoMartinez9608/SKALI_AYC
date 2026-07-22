using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ModelNucleo.Model
{
    public class Referencia_Bancaria
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Referencia_Bancaria { get; set; }

        public int Id_Persona { get; set; }
        public Persona Persona { get; set; }

        public Tipo_Referencia_Num Tipo_Referencia { get; set; }
        public string Institucion { get; set; }
    }

    public enum Tipo_Referencia_Num
    {
        Seleccionar=0,
        Ahorro=1,
        [Display(Name = "Crédito")]
        Credito =2
    }
}
