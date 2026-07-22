using System;
using System.Collections.Generic;
using System.Text;

namespace ModelNucleo.Dto
{
    public class DepositoPorVencer
    {
        public int IdDepositoPlazo { get; set; }

        public string Cuenta { get; set; }

        public string CuentaHabiente { get; set; }

        public decimal Saldo { get; set; }

        public DateTime FechaVencimiento { get; set; }

        public int DiasRestantes { get; set; }

        public string Telefono { get; set; }

        public string Celular { get; set; }

        public string ColorAlerta { get; set; }
    }
}
