using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ModelNucleo.Model
{
    public class Codigo_Agencia
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Codigo_Agencia { get; set; }
        [DefaultValue(false)]
        public bool Es_Credito { get; set; }
        [DefaultValue(false)]
        public bool Es_Ahorro { get; set; }
        public int Id_Agencia { get; set; }
        public int Id_Linea { get; set; }
        public int Id_Persona { get; set; }
        public int Correlativo { get; set; }
        public int Correlativo_Producto { get; set; }
    }
}
