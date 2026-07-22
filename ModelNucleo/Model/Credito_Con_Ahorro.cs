using System;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelNucleo.Model
{
    public class Credito_Con_Ahorro
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Credito_Con_Ahorro { get; set; }
        public int Id_Credito { get; set; }
        public Credito Credito { get; set; }
        public decimal Monto_Ahorro { get; set; }
        public int Id_Porcentaje_Ahorro_Credito { get; set; }
        public Porcentaje_Ahorro_Credito Porcentaje_Ahorro_Credito { get; set; }


    }
}
