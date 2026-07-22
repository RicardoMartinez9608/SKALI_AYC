using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ModelNucleo.Model.Contabilidad
{
    public class Detalle_Asiento_Contable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Detalle_Asiento_Contable { get; set; }

        public int Id_Agencia { get; set; }

        public bool EsCaja { get; set; }

        public int Id_Asiento_Contable { get; set; }
        public Asiento_Contable Asiento_Contable { get; set; }

        public string Cuenta_Contable { get; set; }

        public string Concepto { get; set; }
        public decimal Cargo { get; set; }
        public decimal Abono { get; set; }
    }
}
