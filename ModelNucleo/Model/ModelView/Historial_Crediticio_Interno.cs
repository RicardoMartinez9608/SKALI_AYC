using System;
namespace ModelNucleo.Model.ModelView
{
    public class Historial_Crediticio_Interno
    {
        public string Codigo { get; set; }
        public DateTime Fecha_Apertura { get; set; }
        public decimal Monto { get; set; }
        public decimal Saldo { get; set; }
        public int Plazo { get; set; }
        public string Forma_Pago { get; set; }
        public decimal Cuota { get; set; }
        public int Dias_Mora { get; set; }
        public int Cuota_Mora { get; set; }
        public int Cuota_Dia { get; set; }
        public bool Cancelado { get; set;}
        public DateTime Cancelado_En { get; set; }
        public bool Vencido { get; set; }
        public DateTime Vencido_En { get; set; }
    }
}
