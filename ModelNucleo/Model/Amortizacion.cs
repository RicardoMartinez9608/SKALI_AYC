using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelNucleo.Model
{
    public class Amortizacion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Amortizacion { get; set; }

        public int No { get; set; }

        public int Id_Credito { get; set; }
        public Credito Credito { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FechaPago { get; set; }

        /*fecha de amortizacion para comparar*/
        public DateTime Fecha_Amortizacion { get; set; }

        public int Dias { get; set; }

        public decimal Interes { get; set; }
        public decimal Comision { get; set; }/*El la comisión ya incluye el iva*/
        
        public decimal Otros_Cobros { get; set; }
        public decimal Abono_capital { get; set; }
        public decimal Valor_Cuota { get; set; }
        public decimal Saldo_Capital { get; set; }
        public decimal Pago_para_cancelar { get; set; }
        public decimal Recargo { get; set; }

        /*solo para saber la fecha anterior 
         * y controlar los fines de semana y los asuentos cuando se tenga*/
        public DateTime? fecha_anterior { get; set; }
    }
}
