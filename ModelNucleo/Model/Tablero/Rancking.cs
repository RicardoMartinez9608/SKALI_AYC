using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ModelNucleo.Model.Tablero
{
    public class Rancking
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Rancking { get; set; }
        public int Id_Cartera { get; set; }
        public Cartera Cartera { get; set; }
        public decimal Saldo_Cartera { get; set; }
        
        /*solo para cargarlo*/
        [NotMapped]
        public string Agencia { get; set; }
        [NotMapped]
        public string Asesor { get; set; }

        /*Colecta*/
        public decimal Valor_Pagado { get; set; }
        public decimal Refinanciado { get; set; }
        /*Colecta*/

        public decimal Abono_Capital { get; set; }

        public decimal Capital_Mora { get; set; }
        
        public decimal Ingresos { get; set; }
        public decimal Desembolso { get; set; }
        public int No_Clientes_Activos { get; set; }
        public int No_Creditos_Activos { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Fecha { get; set; }
        public decimal Colocacion { get; set; }
        public decimal Colecta{ get; set; }
        [NotMapped]
        public decimal Porcentaje_Clientes { get; set; }
        [NotMapped]
        public decimal Porcentaje_Cartera { get; set; }
        [NotMapped]
        public decimal Porcentaje_Colecta { get; set; }
        [NotMapped]
        public decimal colecta_diaria { get; set; }
        [NotMapped]
        public DateTime Fecha_Colecta { get; set; }
        [NotMapped]
        public decimal Caida_Cartera { get; set; }
        [NotMapped]
        public decimal Indice_Mora { get; set;
 }

    }
}
