using ModelNucleo.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ModelNucleo.Model
{
    public class Cobro_Periodico:ICobro_Deduccion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Cobro_Periodico { get; set; }

        public int Id_Linea_Credito { get; set; }
        public Linea_Credito Linea_Credito { get; set; }

        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        public decimal Valor { get; set; }

        [DefaultValue(1)]
        public Tipo_Valor Tipo_Valor { get; set; }
    }
}
