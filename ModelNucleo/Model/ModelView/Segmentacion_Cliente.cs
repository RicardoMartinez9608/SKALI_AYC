using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ModelNucleo.Model.ModelView
{
    public class Segmentacion_Cliente
    {
        public int Id_Credito { get; set; }
        public string Agencia { get; set; }
        public string Cartera { get; set; }
        public string Cliente { get; set; }
        public string Dui { get; set; }
        public string Codigo_Credito { get; set; }
        public decimal Cuota { get; set; }
        public decimal Abono { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Fecha_Abono { get; set; }
        public string Segmentacion { get; set; }
    }
}
