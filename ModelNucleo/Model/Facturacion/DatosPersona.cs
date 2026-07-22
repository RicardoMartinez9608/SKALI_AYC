using System;
using System.Collections.Generic;
using System.Text;

namespace ModelNucleo.Model.Facturacion
{
    public class DatosPersona
    {
        public string nombre { get; set; }
        public string email { get; set; }
        public string direccion { get; set; }
        public string telefono { get; set; }
        public string dui { get; set; }

        //datos pago 
        public decimal valorPago { get; set; }
        public decimal interes { get; set; }
        public decimal comision { get; set; }
        //datos sucursal
        public string establecimiento { get; set; }
        public string puntoVenta { get; set; }
        public string departamento { get; set; }
        public string municipio { get; set; }
        //datos DTE 
        public string numeroControl { get; set; }

    }
}
