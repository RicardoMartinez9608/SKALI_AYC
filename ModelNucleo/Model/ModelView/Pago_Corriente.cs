using System;
using System.Collections.Generic;
using System.Text;

namespace ModelNucleo.Model.ModelView
{
    public class Pago_Corriente
    {
        public int Id_Persona { get; set; }
        public string Id_Usuario { get; set; }
        public string Nombre_Completo { get; set; }
        public string Codigo_Credito { get; set; }
        public Transaccion_Credito Transaccion { get; set; }
    }
}
