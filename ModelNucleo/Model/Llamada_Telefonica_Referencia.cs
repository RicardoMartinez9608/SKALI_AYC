using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ModelNucleo.Model
{
    public class Llamada_Telefonica_Referencia
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Llamada_Telefonica_Referencia { get; set; }

        public int Id_Referencia { get; set; }
        public Referencia Referencia { get; set; }

        public DateTime Registrado_Para { get; set; } //citado para la hora que se le quiere llamar
        public string Usuario { get; set; }
        public DateTime Efectuado_En { get; set; }/// registra a que horas estaba haciendo la llamada
        public int Intentos { get; set; }/// son los intentos que se ha efectuado
    }
}
