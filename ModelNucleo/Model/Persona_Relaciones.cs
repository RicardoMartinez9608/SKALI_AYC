using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelNucleo.Model
{
    public class Persona_Relaciones
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Persona_Relaciones { get; set; }

        public int Id_Sujeto { get; set; }
        public Persona Sujeto { get; set; }

        public int Id_Predicado { get; set; }
        public Persona Predicado { get; set; }

        public int Id_Tipo_Relacion { get; set; }
        public Tipo_Relacion Tipo_Relacion { get; set; }

        /*nuevas relaciones para saber en que credito y ahorros*/
        [DefaultValue(0)]
        public int Id_Credito { get; set; }
        //public virtual Credito Credito { get; set; }
        [DefaultValue(0)]
        public int Id_Ahorro { get; set; }
        public decimal Porcentaje_Beneficiario { get; set; }
        //public virtual Ahorro Ahorro { get; set; }

        public string Parentesco { get; set; }

        public bool Activo { get; set; }
        
    }
}
