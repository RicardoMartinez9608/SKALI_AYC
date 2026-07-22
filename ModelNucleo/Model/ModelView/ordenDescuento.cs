using System;
using System.Collections.Generic;
using System.Text;

namespace ModelNucleo.Model.ModelView
{
    public class ordenDescuento
    {
        public DateTime fecha { get; set; }
        public string ciudad { get; set; }
        public string nombreCliente { get; set; }
        public int edad { get; set; }
        public string direccion { get; set; }
        public string departamento { get; set; }
        public string dui { get; set; }
        public string expedicionDui { get; set; }
        public int numCuotas { get; set; }
        public decimal valCuota { get; set; }
        public string codigoCredito { get; set; }
        public decimal monto { get; set; }
    }
}
