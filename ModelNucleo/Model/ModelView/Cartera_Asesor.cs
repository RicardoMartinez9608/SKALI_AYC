using System;
using System.Collections.Generic;
using System.Text;

namespace ModelNucleo.Model.ModelView
{
    public class Cartera_Asesor
    {
        public string Codigo { get; set; }
        public string Nombre_Persona { get; set; }
        public string Direccion_Persona { get; set; }
        public string Telefono_Persona { get; set; }
        public decimal Monto_Credito { get; set; }
        public decimal Saldo_Credito { get; set; }
        public decimal Cuota_Credito { get; set; }
        public DateTime Fecha_Apertura { get; set; }
        public DateTime Fecha_Vencimiento { get; set; }
        public string Nombre_Asesor { get; set; }
        public string Solicitado_Por { get; set; }

    }
}
