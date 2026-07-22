using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ModelNucleo.Model
{
    public class Accion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Accion { get; set; }

        public int Id_Persona { get; set; }
        public Persona Persona { get; set; }

        public int Id_Tipo_Accion { get; set; }
        public Tipo_Accion Tipo_Accion { get; set; }

        public string Codigo { get; set; }
        public string No_Certificado { get; set; }
        public int Cantidad_Accion { get; set; }
        public decimal Valor_Unitario { get; set; }
        public decimal Valor_Total { get; set; }
        public DateTime Creado_En { get; set; }

        [DefaultValue("false")]
        public bool Cancelado { get; set; }
        [DefaultValue("false")]
        public bool Esta_Impreso { get; set; }
        [NotMapped]
        public string Id_usuario { get; set; }

    }
}
