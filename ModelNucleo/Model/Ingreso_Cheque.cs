using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ModelNucleo.Model
{
    /// <summary>
    /// solo se ocupara para registrar
    /// los cheques nada más
    /// </summary>
    public class Ingreso_Cheque
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Ingreso_Cheque { get; set; }
        public string No_Cheque { get; set; }
        public int Id_Banco { get; set; }/*solo para tener el id*/
        public decimal Monto { get; set; }
        public int Id_Agencia { get; set; }

        public Agencia Agencia { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Fecha { get; set; }
        public string Comentario { get; set; }
    }
}
