using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelNucleo.Model
{
    public class Factura_Servicio
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Factura_Servicio { get; set; }

        public string Concepto { get; set; }
        public bool Es_Servicio_Basico { get; set; }
    }
}
