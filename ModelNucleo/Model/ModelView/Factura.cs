using System;
using System.Collections.Generic;

namespace ModelNucleo.Model.ModelView
{
    public class Factura
    {
        public DateTime Fecha { get; set; }
        public string Agencia { get; set; }
        public string Cliente { get; set; }
        public string No_Ahorro { get; set; }
        public string Concepto { get; set; }
        public string Cajero { get; set; }
        public int id_cajero { get; set; }
        public string Linea { get; set; }
        public decimal Saldo_Capital { get; set; }
        public decimal Total { get; set; }
        public decimal Tasa { get; set; }
        public Detalle_Factura_Credito Detalle_Factura_Credito { get; set; }
    }
    
    public class Detalle_Factura_Credito {
        public DateTime Proximo_Pago { get; set; }
        public decimal Abono_Capital { get; set; }
        public decimal Interes { get; set; }
        public decimal Plan_AyC { get; set; }
        public decimal Recargos { get; set; }
        public decimal Total_Pagar { get; set; }
    }


    public class Comprobante
    {
        public string Agencia { get; set; }
        public string Cajero { get; set; }
        public string NoComprobante { get; set; }
        public DateTime Fecha_Impresion { get; set; }
        public string ANombreDe { get; set; }
        public string Concepto { get; set; }
        public decimal Valor { get; set; }
        public decimal IVA { get; set; }
        public decimal Total { get; set; }
    }

    public class Impresion {
        public Factura Factura { get; set; }
        public Comprobante Comprobante { get; set; }
    }
}
