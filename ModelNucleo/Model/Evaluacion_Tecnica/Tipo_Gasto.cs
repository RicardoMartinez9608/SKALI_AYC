using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ModelNucleo.Model.Evaluacion_Tecnica
{
    public class Tipo_Gasto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Tipo_Gasto { get; set; }
        public string Descripcion { get; set; }

        public int Id_Categoria_Gasto { get; set; }
        public Categoria_Gasto Categoria_Gasto { get; set; }

        [NotMapped]
        public ICollection<Gasto> Gastos { get; set; }
    }
}
