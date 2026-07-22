using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ModelNucleo.Model.ModelView
{
    public class Transaccion_Ahorro_Vista
    {
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public Decimal Deposito { get; set; }
        public Decimal Retiro { get; set; }
        public Decimal Saldo { get; set; }
        public Caja caja { get; set; }
        public Transaccion_Caja Transacciones { get; set; }
        public static implicit operator List<object>(Transaccion_Ahorro_Vista v)
        {
            throw new NotImplementedException();
        }
    }
}
