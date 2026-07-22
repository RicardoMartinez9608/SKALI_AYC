using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ModelNucleo.Model.ModelView
{
    public class Movimiento_Diario_Prestamos
    {
        public string Cartera { get; set; }
        public string Generado_Por { get; set; }
        public List<Linea_Credito_Movimiento> datos_linea { get; set; }

    }
    public class Linea_Credito_Movimiento
    {
        public string Linea_Credito { get; set; }
        public List<Datos_Movimientos_Diarios> datos { get; set; }
    }
    public class Datos_Movimientos_Diarios
    {
        
        public decimal valor { get; set; }
        public string codigo { get; set; }
        public string persona { get; set; }
        
        [NotMapped]
        public string Id_usuario { get; set; }
    }
}
