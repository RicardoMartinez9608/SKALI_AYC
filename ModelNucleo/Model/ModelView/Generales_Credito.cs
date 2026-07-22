using System;
using System.Collections.Generic;
using System.Text;

namespace ModelNucleo.Model.ModelView
{
    public class Reporte_General_Credito
    {
        public string Nombre_agencia { get; set; }
        public string Generado_Por { get; set; }
        public DateTime Fecha { get; set; }
        public IEnumerable<Generales_Credito> Generales { get; set; }
        /*se puede agregar sumatorias si se quiere*/
    }

    public class Generales_Credito
    {
        public int No_Cartera { get; set; }
        public string Nombre_Asignado { get; set; }
        public decimal Colocacion { get; set; }
        public int Nuevos_Creditos { get; set; }
        public int Recuperados { get; set; }
        public int Recurrentes { get; set; }
        public decimal Valor_Cartera { get; set; }
        public int Creditos { get; set; }
        public int Clientes_Activos { get; set; }
        public int Clientes_Inactivos { get; set; }
        public decimal Capital_Recuperado { get; set; }
        public decimal Capital_Mora { get; set; }
        public decimal Colecta { get; set; }
        public decimal Indice { get; set; }

        public int Creditos_Mora { get; set; }
        public int Clientes_Mora { get; set; }
        public int Clientes_Vencidos { get; set; }
        public decimal Vencidos { get; set; }
    }
}