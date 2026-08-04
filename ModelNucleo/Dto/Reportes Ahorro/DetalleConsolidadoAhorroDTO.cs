using System;
using System.Collections.Generic;
using System.Text;

namespace ModelNucleo.Dto.Reportes_Ahorro
{
    public class DetalleConsolidadoAhorroDTO
    {
        public int IdAhorro { get; set; }

        public string NoCuenta { get; set; }

        public string Socio { get; set; }

        public string Dui { get; set; }

        public decimal Saldo { get; set; }
    }
}
