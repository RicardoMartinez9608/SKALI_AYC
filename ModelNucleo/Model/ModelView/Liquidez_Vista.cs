using System;
using System.Collections.Generic;
using System.Text;

namespace ModelNucleo.Model.ModelView
{
    public class Liquidez_Vista
    {
        public DateTime Fecha_Reporte { get; set; }
        public decimal Ultimo_Saldo { get; set; }
        public string Agencia { get; set; }
        public List<Transaccion_Liquidez> Transacciones { get; set; }

        public decimal Total_Remesas_Desembolso { get; set; }
        public decimal Total_Facturas { get; set; }
        public decimal Total_Colecta_Cheque { get; set; }

        public decimal Disponibilidad { get; set; }
        public string Generado_Por { get; set; }
    }

    public class Transaccion_Liquidez
    {
        public Tipo_Flujo Flujo { get; set; }
        public string No_Comprobante { get; set; }
        public Categoria_Tipo Categoria_Tipo { get; set; }
        public DateTime Fecha { get; set; }
        public string Descripcion { get; set; }
        public decimal Monto { get; set; }
        public decimal Saldo { get; set; }
    }

    public class Categoria_Tipo 
    { 
        public Categoria_Tipo_Transaccion Categoria { get; set; }
        public int Tipo { get; set; }
        public string Descripcion { get; set; }
        public string Caja { get; set; }
    }

    public enum Categoria_Tipo_Transaccion { 
        Transaccion_Ahorro=1,
        Transaccion_Credito=2,
        Transaccion_Simple=3
    }

    public enum Tipo_Flujo { 
        Remesas_Desembolsos = 1,
        Facturas = 2,
        Colecta_Cheque=3,
        CortesCaja =4
    }
}
