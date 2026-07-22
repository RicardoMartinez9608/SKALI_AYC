using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ModelNucleo.Model
{
   public class SaldoDisponible
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int id_saldodisponible { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime fecha { get; set; }

        public decimal Saldo_anterior { get; set; }//simplemente el saldo del registro anterior
        public decimal Efectivo_total { get; set; }//es el total de ICollection<Efectivo> Efectivos
        public decimal Numero_cheque { get; set; }
        public decimal Cantidad_cheque { get; set; }
        public decimal Cantidad_aki { get; set; }

        /*datos para liquidez y cierre*/
        [DefaultValue(0.00)]
        public decimal Saldo_Cierre { get; set; }
        
        [DefaultValue(false)]
        public bool Cierre { get; set; }
        /*para el manejo de liquidez y cierre*/

        public decimal saldo { get; set; }//suma de todo lo anterior
        public int id_caja { get; set; }
        public Caja Caja { get; set; }
        [NotMapped]
        public ICollection<Efectivo> Efectivos { get; set; }
    }
}
