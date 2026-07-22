using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ModelNucleo.Model.Evaluacion_Tecnica
{
    public class Inventario_Negocio
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Inventario_Negocio { get; set; }

        public int Id_Evaluacion { get; set; }
        public Evaluacion Evaluacion { get; set; }
        
        public decimal Cantidad { get; set; }
        public string Unidad_Compra { get; set; }
        public decimal Precio_Compra { get; set; }
        public string Detalle { get; set; }
        public decimal Total { get; set; }
    }
}
