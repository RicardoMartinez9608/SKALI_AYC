using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ModelNucleo.Model.Evaluacion_Tecnica
{
    public class Activo_Negocio
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Activo_Negocio { get; set; }

        public int Id_Evaluacion { get; set; }
        public Evaluacion Evaluacion { get; set; }

        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }
        public decimal Precio_Compra { get; set; }
        public decimal Precio_Peritado { get; set; }
    }
}
