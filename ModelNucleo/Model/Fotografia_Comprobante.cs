using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelNucleo.Model
{
    public class Fotografia_Comprobante
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Fotografia_Comprobante { get; set; }
        public int Id_Colecta { get; set; }
        public Colecta Colecta { get; set; }
        public int Id_Cliente { get; set; }
        public string Nombre_Completo { get; set; }
        public string Foto { get; set; }
        public bool Verificado { get; set; }
        public string Verificado_Por { get; set; }
        public DateTime Verificado_En { get; set; }
    //    public string Verificacion_Legal { get; set; }
        public DateTime Verificado_Legal_En { get; set; }

    }
}
