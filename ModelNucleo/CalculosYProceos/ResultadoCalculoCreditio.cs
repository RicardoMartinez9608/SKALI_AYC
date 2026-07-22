using ModelNucleo.Model;
using System.Collections.Generic;

namespace ModelNucleo.CalculosYProceos
{
    public class ResultadoCalculoCreditio
    {
        public decimal CuotaPrimaria { get; set; }
        public decimal cuotaAyC { get; set; }
        public decimal IVACalculado { get; set; }
        public decimal CuotaTotal { get; set; }

        public decimal No_Cuotas { get; set; }

        public List<Amortizacion> Amortizacion { get; set; }

        public decimal Sumatoria_Cuotas { get; set; }
    }

    public class ResultadoCalculoCreditioExtendido:ResultadoCalculoCreditio
    {
        public string Codigo_Credito { get; set; }

        public string Cliente { get; set; }

    }
}
