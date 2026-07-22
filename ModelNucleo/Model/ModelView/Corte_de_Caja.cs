using System;
using System.Collections.Generic;
using System.Text;

namespace ModelNucleo.Model.ModelView
{
    public class Corte_de_Caja
    {
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string caja { get; set; }
        public string cajero { get; set; }

        public decimal TotalIngreso { get; set; }
        public decimal TotalEgreso { get; set; }
        public decimal Disponible { get; set; }

        public List<Rama> Rama { get; set; }
        public RemesaBanco Remesa { get; set; }
    }

    public class Rama
    {
        public Agencia Agencia { get; set; }
        public List<OperacionCorte> Ingresos { get; set; }
        public decimal TotalIngreso { get; set; }
        public List<OperacionCorte> Egresos { get; set; }
        public decimal TotalEgreso { get; set; }
    }
    public class OperacionCorte
    {
        public int Id_Agencia { get; set; }
        public string cuenta { get; set; }
        public string descripcion { get; set; }
        public decimal valor { get; set; }
    }

    public class RemesaBanco
    {
        public decimal fectivo { get; set; }
        public decimal Cheques { get; set; }
        public decimal total { get; set; }
    }
}
