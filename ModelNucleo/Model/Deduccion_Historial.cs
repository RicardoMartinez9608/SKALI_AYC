using ModelNucleo.Model.Interfaces;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelNucleo.Model
{
    public class Deduccion_Historial : ICobro_Deduccion, IIDentidad_Linea_Credito_Historial
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Deduccion_Historial { get; set; }
        
        public int Id_Linea_Credito_Historial { get; set; }
        public Linea_Credito_Historial Linea_Credito_Historial { get; set; }

        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Valor { get; set; }

        [DefaultValue(1)]
        public Tipo_Valor Tipo_Valor { get; set; }

        public bool Cancelado { get; set; }//para saber si ya se pago

        /*para el manejo de refinanciamiento */
        [DefaultValue(0)]
        public int Id_Credito_Financiado { get; set; }
        public string Codigo_Credito_Financiado { get; set; }
    }
}
