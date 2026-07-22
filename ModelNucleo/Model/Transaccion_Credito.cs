using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ModelNucleo.Model
{
    public class Transaccion_Credito:IAuditoria
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Transaccion_Credito { get; set; }

        public int Id_Tipo_Operecion_Credito { get; set; }
        public Tipo_Operacion_Credito Tipo_Operacion_Credito { get; set; }

        public int Id_Credito { get; set; }
        public Credito Credito { get; set; }

        /*los mismos del modelo Amortización*/
        public int No { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FechaPago { get; set; }
        /*fecha de la amortizacion*/
        public DateTime Fecha_Amortizacion { get; set; }
        public decimal Interes_Comision { get; set; }
        public decimal Interes { get; set; }
        public decimal IvaSobreComision { get; set; }
        public decimal Otros_Cobros { get; set; }
        public decimal Abono_capital { get; set; }
        [DisplayFormat(DataFormatString = "{0:#.##}")]
        public decimal Valor_Cuota { get; set; }
        public decimal Saldo_Capital { get; set; }
        /*termina el modelo de amortización*/
        public int Dias { get; set; }
        public decimal Recargo { get; set; }
        [DefaultValue("false")]
        public bool Mora { get; set; }
        public bool Puntual { get; set; }
        public bool Vencido { get; set; }
        public bool Anticipado { get; set; }

        /*si está en mora se guardará la cantidad de pagos atrazados*/
        [DefaultValue("0")]
        public int Pagos_Acumulado { get; set; }

        [NotMapped]
        public bool Al_Dia { get; set; }

        public DateTime Creado_En { get; set; }
        public DateTime Eliminado_En { get; set; }
        public DateTime Modificado_En { get; set; }
        public string Creado_Por { get; set; }
        public string Eliminado_Por { get; set; }
        public string Modificado_Por { get; set; }
        [DefaultValue("false")]
        public bool Modificado { get; set; }
        [DefaultValue("false")]
        public bool Borrado_Logico { get; set; }
        
        /*campos solo de resultado para mostrar en el abono*/
        [NotMapped]
        public decimal Pago_para_cancelar { get; set; }

        /*datos para mostrar detalle y en reportes*/
        [NotMapped]
        public decimal Capital_Mora { get; set; }
        [NotMapped]
        public decimal Interes_Diario { get; set; }
        [NotMapped]
        public decimal Interes_Pendiente { get; set; }
        [NotMapped]
        public decimal Comsion_Pendiente { get; set; }
        [NotMapped]
        public decimal Valor_Al_Dia { get; set; }
        [NotMapped]
        public decimal Recargos_Acumulados { get; set; }

    }
}
