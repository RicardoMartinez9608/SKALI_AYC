using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ModelNucleo.Model
{
  public class Efectivo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_flujo { get; set; }

        public int id_denominacion { get; set; }
        public Denominacion_efectivo Denominacion { get; set; }

        public int id_saldodisponible { get; set; }
        public SaldoDisponible SaldoDisponible { get; set; }

        public decimal monto { get; set; }
        public decimal cantidad { get; set; }
        public int id_caja { get; set; }
        public Caja Caja { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime fechaEjercicio { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime fechaCreacion { get; set; }


    }
}
