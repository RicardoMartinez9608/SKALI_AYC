
using System;
using System.Collections.Generic;

namespace ModelNucleo.Model.ModelView
{
    public class Calificacion_Vista
    {
        public Persona Persona { get; set; }

        /*área propuesta por el asesor*/
        public decimal Monto_Propuesto { get; set; }
        public decimal Plazo_Propuesto { get; set; }
        public decimal Cuota_Propuesta { get; set; }

        /*área propuesto por la calificación*/
        public decimal Monto_Ideal { get; set; }
        public decimal Plazo_Ideal { get; set; }
        public decimal Cuota_Ideal { get; set; }

        /*Datos de calificación*/
        public Ponderacion_Datos_Calificacion Caracter { get; set; }
        public Ponderacion_Datos_Calificacion Capacidad { get; set; }
        public Ponderacion_Datos_Calificacion Capital { get; set; }
        public Ponderacion_Datos_Calificacion Condiciones { get; set; }
        public Ponderacion_Datos_Calificacion Colateral { get; set; }
        public Ponderacion_Datos_Calificacion Calificacion_Total
        {
            get
            {
                return new Ponderacion_Datos_Calificacion
                {
                    Valor= Caracter.Valor + Capacidad.Valor + Capital.Valor + Condiciones.Valor + Colateral.Valor,
                    Ponderacion=Ponderacion.Total
                };
                
            }
        }

        /*indicadores de la persona y de su negocio*/
        public decimal Razon_Circulante { get; set; }
        public decimal Capital_Trabajo { get; set; }
        public decimal Rotacion_Inventario { get; set; }
        public decimal Rotacion_Cuenta_Cobrar { get; set; }
        public decimal Endeudamiento { get; set; }
        public decimal Margen_Bruto { get; set; }
        public decimal Margen_Operativo { get; set; }
        public decimal Punto_Equilibrio { get; set; }
        public decimal Relacion_Utilidad_Negocio { get; set; }
        public decimal Cobertura_Cuota { get; set; }

        public List<Historial_Crediticio_Interno> Historial { get; set; }

        /*datos auxiliares*/
        public decimal Total_Garantias { get; set; }
        public decimal Solvencia { get; set; }
        public decimal Capacidad_pago { get; set; }
    }
}
