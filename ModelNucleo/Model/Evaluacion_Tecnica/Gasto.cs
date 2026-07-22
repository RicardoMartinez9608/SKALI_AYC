using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ModelNucleo.Model.Evaluacion_Tecnica
{
    public class Gasto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Gasto { get; set; }

        public int Id_Evaluacion { get; set; }
        public Evaluacion Evaluacion { get; set; }

        /*public int Id_Categoria_Gasto { get; set; }
        public Categoria_Gasto Categoria_Gasto { get; set; }*/

        public int Id_Tipo_Gasto { get; set; }
        public Tipo_Gasto Tipo_Gasto { get; set; }

        public decimal Valor { get; set; }
        public string Descripcion { get; set; }
    }
}
