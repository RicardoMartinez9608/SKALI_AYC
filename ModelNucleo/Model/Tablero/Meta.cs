using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ModelNucleo.Model.Tablero
{
    public class Meta
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Meta { get; set; }
        public int Id_Cartera { get; set; }
        public Cartera Cartera { get; set; }

        public decimal Colecta { get; set; }
        public decimal Capital { get; set; }

        public decimal Cosecha { get; set; }
        public int No_Clientes { get; set; }
        public decimal Colocacion { get; set; }
        public decimal Ingresos { get; set; }
        public decimal Mora { get; set; }
        public int Mes { get; set; }
        public int Anyo { get; set; }


        [NotMapped]
        public DateTime FechaCompleta { get; set; }


        public DateTime Creado_En { get; set; }
        public string Creado_Por { get; set; }
        public DateTime Modificado_En { get; set; }
        public string Modificado_Por { get; set; }
    }
}
