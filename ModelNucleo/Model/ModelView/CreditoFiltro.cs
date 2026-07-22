using System;
using System.ComponentModel.DataAnnotations;

namespace ModelNucleo.Model.ModelView
{
    public class CreditoFiltro:FechasFiltro
    {
        public int id_persona { get; set; }
        public bool porCliente { get; set; }
        public bool aprobado { get; set; }
        
    }

    public class AhorroFiltro : FechasFiltro
    {
        public int id_persona { get; set; }
        public bool porCliente { get; set; }
        //public bool aprobado { get; set; }

    }

    public class FechasFiltro
    {
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? inicio { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? final { get; set; }
    }
}
