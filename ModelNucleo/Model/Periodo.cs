using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ModelNucleo.Model
{
    public class Periodo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Periodo { get; set; }

        public int Valor { get; set; }
        public int dias { get; set; }
        public string Descripcion { get; set; }
        [NotMapped]
        public ICollection<Credito> Creditos { get; set; }
        [NotMapped]
        public ICollection<Interes_Periodo_Linea_Credito> InteresPeriodoLineaCreditos { get; set; }
    }
}
