using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ModelNucleo.Model
{
    public class Otras_Operaciones
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Otras_Operaciones { get; set; }

        public string Concepto { get; set; }
    }
}
