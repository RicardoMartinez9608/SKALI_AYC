using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ModelNucleo.Model
{
    public class Tipo_Mutuo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Tipo_Mutuo { get; set; }

        public int Id_Linea_Credito { get; set; }
        public Linea_Credito Linea_Credito { get; set; }

        public string Nombre_Extencion_Documento { get; set; }
        public bool Copia { get; set; }
        public bool Huella { get; set; }
        public int Integrantes_Grupal { get; set; }/*se utiliza si es un crédito grupal o solidario*/
        public bool Dos_Fechas { get; set; }/*se utiliza si un crédito es liquidado el o despues del 25*/
        public bool Dos_Codeudores { get; set; }/*se utiliza si un credito tiene 2 codeudores*/
        public bool Dos_Codeudores_Dos_Fechas { get; set; }/*se utiliza si un credito tiene 2 fechas y 2 codeudores*/
    }
}
