
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelNucleo.Model
{
    public class Transaccion_Ahorro:IAuditoria
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Transaccion_Ahorro { get; set; }

        public int Id_Ahorro { get; set; }
        public Ahorro Ahorro { get; set; }

        /*public int Id_Caja { get; set; }
        public Caja Caja { get; set; }*/

        public int Id_Tipo_Operacion_Ahorro { get; set; }
        public Tipo_Operacion_Ahorro Tipo_Operacion_Ahorro { get; set; }

        [DisplayFormat(DataFormatString = "{0:C0}")]
        public decimal Valor{ get; set; }
        public decimal Saldo { get; set; }
        public int Dias { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Fecha { get; set; }

        public decimal Interes_Aplicado { get; set; }
        public decimal Interes_Acumulado { get; set; }

        [DefaultValue("false")]
        public bool Borrado_Logico { get; set; }

        public DateTime Creado_En { get; set; }
        public DateTime Eliminado_En { get; set; }
        public DateTime Modificado_En { get; set; }
        public string Creado_Por { get; set; }
        public string Eliminado_Por { get; set; }
        public string Modificado_Por { get; set; }
        public bool Modificado { get; set; }
        public int Id_Libreta { get; set; }
        public int No_Renglon { get; set; }
        public bool esRetencionISR { get; set; }
    }
}
