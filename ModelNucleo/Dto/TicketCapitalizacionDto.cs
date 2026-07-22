using System;
using System.Collections.Generic;
using System.Text;

namespace ModelNucleo.Dto
{
    public class TicketCapitalizacionDto
    {
        public int IdDepositoPlazo { get; set; }

        public string Cuenta { get; set; }

        public string CuentaHabiente { get; set; }

        public DateTime FechaCapitalizacion { get; set; }

        public decimal SaldoAnterior { get; set; }

        public decimal InteresCapitalizado { get; set; }

        public decimal SaldoPosterior { get; set; }

        public decimal TasaInteres { get; set; }

        public string FormaPago { get; set; }
    }
}
