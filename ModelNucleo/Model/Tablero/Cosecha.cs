using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ModelNucleo.Model.Tablero
{
    public class Cosecha
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Cosecha { get; set; }
        public int Id_Cartera { get; set; }
        public Cartera Cartera { get; set; }

        public decimal Saldo { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Fecha { get; set; }
        /*solo para cargarlo*/
        [NotMapped]
        public string Agencia { get; set; }
        [NotMapped]
        public string Asesor { get; set; }
    }
}
