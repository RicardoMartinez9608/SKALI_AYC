using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ModelNucleo.Model.Evaluacion_Tecnica
{
    public class Costo_Venta
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Costo_Venta { get; set; }

        public int Id_Evaluacion { get; set; }
        public Evaluacion Evaluacion { get; set; }

        public string Unidad_Medida { get; set; }
        public decimal Precio_Compra { get; set; }
        public decimal Precio_Venta { get; set; }
        public decimal Margen_Bruto { get; set; }
        public decimal Margen_Bruto_Porcentaje { get; set; }
    }
}
