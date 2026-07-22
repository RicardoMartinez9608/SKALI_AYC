using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ModelNucleo.Model
{
    public class Banco: IAuditoria
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Banco { get; set; }
        public string Nombre { get; set; }
        public string No_Cuenta { get; set; }

        public string Cuenta_Contable { get; set; }//cuenta contable directa

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
        public ICollection<Fotografia_Atm> Fotografia_Atms { get; set; }

    }
}
