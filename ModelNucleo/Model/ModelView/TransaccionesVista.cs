using System;
using System.Collections.Generic;
using System.Text;

namespace ModelNucleo.Model.ModelView
{
    public class TransaccionesBovedaVista
    {
        public decimal Entrada { get; set; }

        public decimal Salida { get; set; }
    }

    public class CajaVista 
    { 
        public decimal Entrada { get; set; }
        public decimal Salida { get; set; }
    }

    public class TransaccionCreditoResumen 
    {
        public decimal Capital { get; set; }
        public decimal Comision { get; set; }
        public decimal Interes { get; set; }
        public decimal IVASobreComision { get; set; }
        public decimal Recargo { get; set; }
        public decimal Resumen { get; set; }
    }

    public class TransaccionSimpleEntradaResumen 
    {
        public string Caja { get; set; }
        public string Descripcion { get; set; }
        public decimal Monto { get; set; }
    }
}
