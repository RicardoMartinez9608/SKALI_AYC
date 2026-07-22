using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ModelNucleo.Model
{
   public class Documentos_Legales
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Documentos_Legales { get; set; }
        public string direccion { get; set; }
        public string nombre { get; set; }
        public Tipo_Documento Tipo_Documento { get; set; }
    }

   /*Enums para filtro de documentos*/
    public enum Tipo_Documento
   {
       Credito = 1,
       Ahorro = 2
   }


}
