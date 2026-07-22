using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelNucleo.Model
{
    public class Colecta: IAuditoria
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Colecta { get; set; }
        
        public int Id_Colecta_Diaria { get; set; }
        public Colecta_Diaria Colecta_Diaria { get; set; }

        public int Id_Cliente { get; set; }
        public string DUI { get; set; }
        public string Nombre_Completo { get; set; }

        public int Id_Credito { get; set; }
        public string Codigo_Credito { get; set; }

        public int Id_Ahorro { get; set; }
        public string Codigo_AHorro { get; set; }

        public int Id_Transaccion { get; set; }

        public decimal Monto { get; set; }
        public string No_Comprobante { get; set; }
        [DefaultValue("false")]
        public bool Procesado { get; set; }
        [DefaultValue("false")]
        public bool Anulado { get; set; }

        public DateTime Creado_En { get; set; }
        public DateTime Eliminado_En { get; set; }
        public DateTime Modificado_En { get; set; }
        public string Creado_Por { get; set; }
        public string Eliminado_Por { get; set; }
        public string Modificado_Por { get; set; }
        [DefaultValue("false")]
        public bool Modificado { get; set; }
        [DefaultValue("false")]
        public bool Borrado_Logico { get; set; }

        [NotMapped]
        public ICollection<Fotografia_Comprobante> Fotografia_Comprobantes { get; set; }
        
    }
}
