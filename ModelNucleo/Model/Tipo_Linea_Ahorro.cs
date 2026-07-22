
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelNucleo.Model
{
    public class Tipo_Linea_Ahorro
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Tipo_Linea_Ahorro { get; set; }
        public string Descripcion { get; set; }

        [NotMapped]
        public ICollection<Linea_Ahorro> Linea_Ahorros { get; set; }
    }
}
