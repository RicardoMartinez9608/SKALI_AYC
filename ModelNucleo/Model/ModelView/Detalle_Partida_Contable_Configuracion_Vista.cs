using ModelNucleo.Model.Contabilidad;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModelNucleo.Model.ModelView
{
    public class Detalle_Partida_Contable_Configuracion_Vista
    {
        public Conf_Partida_Contable Conf_Partida_Contable { get; set; }
        public Agencia Agencia { get; set; }
        public Linea_Credito Linea_Credito { get; set; }
        public Linea_Ahorro Linea_Ahorro { get; set; }
        public ICollection<Conf_Detalle_Partida_Contable> Conf_Detalle_Partida_Contables { get; set; }
    }
}
