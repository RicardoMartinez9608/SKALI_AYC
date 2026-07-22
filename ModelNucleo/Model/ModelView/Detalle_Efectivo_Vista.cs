using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ModelNucleo.Model.ModelView
{
    public class Detalle_Efectivo_Vista
    {
        public Denominacion_efectivo Denominacion { get; set; }
        public Efectivo Efectivo { get; set; }
    }


    public class Reporte_Flujo_Efectivo_Vista
    {
        /* Saldo Anterior*/
        public decimal Saldo_Anterior { get; set; }

        /*Operaciones Ordinarias*/
        public decimal Ingreso_pago_creditos { get; set; }
        public decimal Fondos_de_Administracion { get; set; }
        public decimal Depositos_en_cuenta_de_Ahorro { get; set; }
        public decimal Fondos_de_Deposito_a_plazo { get; set; }
        public decimal Depositos_a_Prestamos_en_CHC { get; set; }
        public decimal Colectores_AKI { get; set; }
        public decimal Subtotal_Entradas_ordinario { get; set; }
        public decimal Desembolsos_prestamos { get; set; }
        public decimal Pago_de_depositos_a_plazo_fijo { get; set; }
        public decimal Retiro_de_ahorros { get; set; }
        public decimal Remesas { get; set; }
        public decimal Resemas_AKI { get; set; }
        public decimal Subtotal_Salidas_ordinario { get; set; }
        public decimal Subtotal_de_operaciones { get; set; }
        /*Pendientes de Liquidar*/
        public decimal Impuestos_municipales { get; set; }
        public decimal Alquiler { get; set; }
        public decimal Combustible { get; set; }
        public decimal Mantenimientos_y_Reparaciones { get; set; }
        public decimal Servicios_TV_por_cable { get; set; }
        public decimal Billetes_promocionales { get; set; }
        public decimal Servicios_basicos { get; set; }
        public decimal Subtotal_de_pendendientes { get; set; }
        /*Disponible a la fecha*/
        public decimal Disponible_a_la_fecha { get; set; }
        public decimal Cheques_Recibidos_Cantidad { get; set; }
        public decimal Cheques_Recibidos_Monto { get; set; }
        public decimal Detalle_bolsa_AKI { get; set; }
        public decimal Efectivo_neto_en_caja { get; set; }
        public List<Detalle_Efectivo_Vista> Efectivo_Detallado { get; set; }
    }
}
