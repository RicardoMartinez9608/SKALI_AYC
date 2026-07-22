using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ModelNucleo.Model
{
    public class Beneficiarios_Deposito_Plazo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Beneficiario_Deposito_Plazo { get; set; }
        public int Id_Deposito_Plazo { get; set; }
        public Deposito_Plazo Deposito_Plazo { get; set; }
        public int Id_Parentesco { get; set; }
        public Parentesco_Persona Parentesco { get; set; }
        public string Nombre { get; set; }
        public int Edad { get; set; }
        public string Sexo { get; set; }
        public DateTime Fecha_Nacimiento { get; set; }
        public string Residencia { get; set; }
        public string Telefono { get; set; }
        public decimal Porcentaje { get; set; }

    }
}
