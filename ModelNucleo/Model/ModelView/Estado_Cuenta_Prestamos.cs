using System;
using System.Collections.Generic;
using System.Text;

namespace ModelNucleo.Model.ModelView
{
   public class Estado_Cuenta_Prestamos
    {
        public DateTime Fecha { get; set; }
        public string nombre_generado { get; set; }
        public string Nombre { get; set; }
        public string Codigo { get; set; }
        public string direccion { get; set; }
        public string Lugar_Trabajo { get; set; }
        public Detalle_Credito Detalle_Credito { get; set; }
        public List<Transaccion_Credito> Detalle_Transacciones { get; set; }
        public Totales_Estado_Cuenta Totales_Estado_Cuenta { get; set; }
    }

   public class Detalle_Credito
   {
       public DateTime Apertura { get; set; }
       public DateTime Vencimiento { get; set; }
       public DateTime Ultimo_Pago { get; set; }
       public int Plazo { get; set; }
       public int Cuotas { get; set; }
       public string F_Pago { get; set; }
       public string Telefonos { get; set; }
       public decimal Monto { get; set; }
       public decimal Saldo { get; set; }
       public decimal Cuota { get; set; }
       public string Dias { get; set; }
       public decimal Acu_Mora { get; set; }
       public decimal Cuot_Mora { get; set; }
       public string Asesor { get; set; }
   }


   public class Totales_Estado_Cuenta
   {
        public decimal Saldo { get; set; }
        public decimal Normales { get; set; }
        public decimal Pendientes { get; set; }
        public decimal Recargos { get; set; }
        public decimal Cobertura_Deuda { get; set; }
        public decimal Moratorios { get; set; }
        public decimal Recargo_Vencimiento { get; set; }
        public decimal Cuotas_Mora { get; set; }
        public decimal Cuotas_Pendientes_Pago { get; set; }
        public decimal Total_Pagar { get; set; }
   }
}
