using System;
using System.ComponentModel.DataAnnotations;

namespace ModelNucleo.Util
{
    public class Fecha_Ordenada
    {
        public int No { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Fecha { get; set; }
        public int Dias { get; set; }
    }
}
