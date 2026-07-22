using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ModelNucleo.Model
{
    public class Bitacora
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Bitacora { get; set; }
        public string Tabla_Operacion { get; set; }
        public string Proceso { get; set; }
        public DateTime Fecha_Proceso { get; set; }
        public string Id_Usuario { get; set; }
        public string Cliente { get; set; }
        public string Codigo { get; set; }
        public string Id_Transaccion { get; set; }

        [DefaultValue(0.00)]
        public decimal Monto_Afectado { get; set; }

        [NotMapped]
        public string Generado_Por{ get; set; }
    }
}
