using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelNucleo.Model.Evaluacion_Tecnica
{
    public class Tipo_Ingreso
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Tipo_Ingreso { get; set; }
        public string Descripcion { get; set; }

        public ICollection<Otro_Ingreso> Otros_Ingresos { get; set; }
    }
}
