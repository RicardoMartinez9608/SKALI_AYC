using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ModelNucleo.Model.ModelView
{
    public class Reporte_Vencimiento_Cuotas_Mora_Diaria
    {
        public Encabezado Encabezado { get; set; }
        public List<Detalle_Vencimiento_Cuotas_Mora_Diaria> Detalle_Vencimiento_Cuotas_Mora_Diaria { get;set;}

    }
    //Objeto para Cuotas en Mora
    public class Encabezado
    {
        public Agencia agencia { get; set; }
        public string dia_ejecucion { get; set; }
        public string hora_ejecucion { get; set; }
        public string nombre_asesor { get; set; }
    }

    public class Detalle_Vencimiento_Cuotas_Mora_Diaria 
    {
        public Datos_Generales Datos_Generales { get; set; }
        public Mora Mora_Asesor { get; set; }
        public Vencimiento_Cuotas Vencimiento_Cuotas { get; set; }
       
    }

    public class Datos_Generales
    {
        /*datos del cliente*/
        public string Codigo_Credito { get; set; }
        public string Nombre_Cliente { get; set; }
        public string Telefono { get; set; }
   


        /*datos del crédito*/
        public DateTime Fecha_Pago { get; set; }
        public decimal Cuota { get; set; }
        public decimal Pago_Al_Dia { get; set; }
        public DateTime Ultimo_Movimiento { get; set; }

        public decimal Saldo { get; set; }
        public DateTime Vencimiento { get; set; } //supongo que el vencimiento del pago
    }

    public class Mora
    {
        public int Dias { get; set; } 
        public decimal Porcentaje_Mora { get; set; } 
        public decimal CapitalMora { get; set; } 
        public decimal InteresPendiente { get; set; } 
        public decimal InteresCorriente { get; set; } 
        public decimal Multa { get; set; } 
    }

    //Objeto Vencimiento Cuotas
    public class Vencimiento_Cuotas
    {
        public decimal Monto { get; set; }
        public decimal Saldo_Cuenta_Ahorro { get; set; }
        public decimal Abono_Capital { get; set; }
        public decimal Porcentaje_Saldo_Pendiente { get; set; }
    }

}
