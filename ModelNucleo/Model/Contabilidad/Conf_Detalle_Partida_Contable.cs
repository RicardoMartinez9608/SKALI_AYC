using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ModelNucleo.Model.Contabilidad
{
    public class Conf_Detalle_Partida_Contable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Conf_Detalle_Partida_Contable { get; set; }

        public int Id_Conf_Partida_Contable { get; set; }
        public Conf_Partida_Contable Conf_Partida_Contable { get; set; }

        public string Cuenta_Contable { get; set; }
        
        public string Identificador { get; set; }//es para el programador
        public string Concepto { get; set; }
        public bool Cargo { get; set; }
        public bool Abono { get; set; }

        //[NotMapped]
        //public ICollection<Detalle_Asiento_Contable> Detalles_Asientos_Contables { get; set; }
    }
}
