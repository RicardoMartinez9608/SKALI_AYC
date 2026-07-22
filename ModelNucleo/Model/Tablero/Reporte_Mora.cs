using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ModelNucleo.Model.Tablero
{
    public class Reporte_Mora
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Reporte_Mora { get; set; }
        public int Id_Cartera { get; set; }
        public Cartera Cartera { get; set; }
        public string Codigo { get; set; }
        public string Nombre_Cliente { get; set; }
        public decimal Saldo_Mora { get; set; }
        public DateTime Fecha_Vencimiento { get; set; }
        public DateTime Fecha_Ultimo_Movimiento { get; set; }
        public int Dias_Mora { get; set; }
        public DateTime Fecha { get; set; }
        [NotMapped]
        public string Agencia { get; set; }
        [NotMapped]
        public string Asesor { get; set; }
        [NotMapped]
        public decimal Mora_Total { get; set; }
        [NotMapped]
        public int Clientes_Mora { get; set; }
        [NotMapped]
        public decimal Vencidos { get; set; }

 //-------------------------------------------------------- tabla ultimo abono -----------------------------------------------------

        [NotMapped]
        public int Clientes_Menor_Quince { get; set; }
        [NotMapped]
        public decimal Saldo_Menor_Quince { get; set; }
        [NotMapped]
        public int Clientes_Dieciseis_treina { get; set; }
        [NotMapped]
        public decimal Saldo_Dieciseis_treina { get; set; }
        [NotMapped]
        public int Clientes_Treinta_Uno_Sesenta { get; set; }
        [NotMapped]
        public decimal Saldo_Treinta_Uno_Sesenta { get; set; }
        [NotMapped]
        public int Clientes_Mayor_Sesenta_Uno { get; set; }
        [NotMapped]
        public decimal Saldo_Mayor_Sesenta_Uno { get; set; }
    }
}
