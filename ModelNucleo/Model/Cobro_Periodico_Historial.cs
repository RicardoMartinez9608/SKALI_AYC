using ModelNucleo.Model.Interfaces;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelNucleo.Model
{
    public class Cobro_Periodico_Historial : ICobro_Deduccion, IIDentidad_Linea_Credito_Historial
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Cobro_Periodico_Historial { get; set; }

        public int Id_Linea_Credito_Historial { get; set; }
        public Linea_Credito_Historial Linea_Credito_Historial { get; set; }

        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Valor { get; set; }
        [DefaultValue(1)]
        public Tipo_Valor Tipo_Valor { get; set; }
    }
}
