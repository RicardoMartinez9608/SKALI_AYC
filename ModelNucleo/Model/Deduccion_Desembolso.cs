using ModelNucleo.Model.Interfaces;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelNucleo.Model
{
    public class Deduccion_Desembolso : ICobro_Deduccion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Deduccion_Desembolso { get; set; }

        public int Id_Linea_Credito { get; set; }
        public Linea_Credito Linea_Credito { get; set; }

        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Valor { get; set; }

        [DefaultValue(1)]
        public Tipo_Valor Tipo_Valor { get; set; }
    }

    public enum Tipo_Valor { 
        Porcentaje=1,
        Dinero=2
    };
}
