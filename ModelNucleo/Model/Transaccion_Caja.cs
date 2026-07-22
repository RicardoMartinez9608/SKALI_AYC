using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelNucleo.Model
{
    public class Transaccion_Caja:IAuditoria
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Transaccion_Caja { get; set; }

        public int Id_Caja { get; set; }
        public Caja Caja { get; set; }

        public int Id_Tipo_Transaccion { get; set; }
        public Tipo_Transaccion Tipo_Transaccion{ get; set; }

        public decimal Monto { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Fecha { get; set; }

        public string No_Comprobante { get; set; }
        
        [DefaultValue("false")]
        public bool Comprobante_Impreso { get; set; }/*saber si ya fue impreso*/

        [DefaultValue("false")]
        public bool Es_Traslado { get; set; }/*es necesario saber si es generado por un traslado*/

        public int Id_Transaccion_Credito { get; set; }
        public int Id_Transaccion_Ahorro { get; set; }
        public int Id_Transaccion_Simple { get; set; }
        public int Id_Transaccion_Deposito_Plazo { get; set; }

        /*conocer el efectivo de caja*/
        public decimal Saldo { get; set; }

        public DateTime Creado_En { get; set; }
        public DateTime Eliminado_En { get; set; }
        public DateTime Modificado_En { get; set; }
        public string Creado_Por { get; set; }
        public string Eliminado_Por { get; set; }
        public string Modificado_Por { get; set; }
        public bool Modificado { get; set; }

        [DefaultValue("false")]
        public bool Borrado_Logico { get; set; }
    }
}
