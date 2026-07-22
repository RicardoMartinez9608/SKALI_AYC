using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ModelNucleo.Model.Contabilidad
{
    public class Asiento_Contable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Asiento_Contable { get; set; }
        public int Id_Transaccion_Caja { get; set; }
        public DateTime Fecha { get; set; }
        public string Concepto { get; set; }
        public int Referencia_Contabilidad { get; set; }//rescata el id de referencia con el que se guardó en sistema contable

        [DefaultValue(false)]
        public bool Migrada { get; set; }///bandera para saber que ha sido migrado o no a contabilidad
        [DefaultValue(false)]
        public bool Verificada { get; set; }

        [NotMapped]
        public ICollection<Detalle_Asiento_Contable> Detalles_Asientos_Contables { get; set; }
    }
}
