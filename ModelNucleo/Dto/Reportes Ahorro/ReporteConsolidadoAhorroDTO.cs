using System;
using System.Collections.Generic;
using System.Text;

namespace ModelNucleo.Dto.Reportes_Ahorro
{
    public class ReporteConsolidadoAhorroDTO
    {
        public int IdLineaAhorro { get; set; }

        public string NombreLineaAhorro { get; set; }

        public DateTime FechaCorte { get; set; }

        public decimal TotalSaldo { get; set; }
        public int TotalCuentas { get; set; }

        public List<DetalleConsolidadoAhorroDTO> Detalle { get; set; }
    }
}
