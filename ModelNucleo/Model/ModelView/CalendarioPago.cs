using System;
using System.Collections.Generic;
using System.Text;

namespace ModelNucleo.Model.ModelView
{
    public class CalendarioPago
    {
        public string codigoCredito { get; set; }
        public string nombreCliente { get; set; }
        public decimal valorCuota { get; set; }
        public string asesor { get; set; }
        public List<FechasPagoCalendario> fechas { get; set; }

    }
    public class FechasPagoCalendario
    {
        public string fechaPago { get; set; }
        public string descripcion { get; set; }
    }
}
