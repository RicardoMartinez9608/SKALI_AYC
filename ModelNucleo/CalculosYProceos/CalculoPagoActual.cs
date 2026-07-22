using ModelNucleo.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModelNucleo.CalculosYProceos
{
    public class CalculoPagoActual
    {
        /// <summary>
        /// devuelve la fila de amortización
        /// </summary>
        /// <param name="index"></param>
        /// <param name="saldo_al_capital"></param>
        /// <param name="valor_cuota"></param>
        /// <param name="dias"></param>
        /// <param name="tasa"></param>
        /// <param name="fecha_inicio"></param>
        /// <returns>Amortizacion</returns>
        public Amortizacion CalculoAmortizacionFila(int index, decimal saldo_al_capital, decimal valor_cuota, int dias, decimal tasa, DateTime fecha_inicio, decimal comision, decimal iva, bool actual = false)
        {
            decimal tasa_convertida = Math.Round((tasa / 365), 2);
            decimal intereses = Math.Round((saldo_al_capital * dias * tasa_convertida) / 100, 2);
            decimal abono_capital = Math.Round(valor_cuota - (intereses + comision + iva), 2);
            decimal nuevo_saldo_al_capital = saldo_al_capital - abono_capital;
            int numero = index + 1;
            //DateTime fecha_cuota;
            //if (actual)
            //    fecha_cuota = FijarFecha(fecha_inicio, index, dias, index);
            //else
            //    fecha_cuota = FijarFecha(fecha_inicio, index * dias, dias, index);

            return new Amortizacion
            {
                No = index,
                Interes = intereses,
                Comision = comision,
                /*IvaSobreComision = iva,*/
                Abono_capital = abono_capital,
                Saldo_Capital = nuevo_saldo_al_capital,
                Valor_Cuota = valor_cuota,
                FechaPago = fecha_inicio
            };
        }

        public DateTime FijarFecha(DateTime fecha, int dias, int frecuencia, int index)
        {
            DateTime nuevaFecha;
            if (frecuencia == 30)
                nuevaFecha = fecha.AddMonths(index);
            else
                nuevaFecha = fecha.AddDays(dias);
            if (nuevaFecha.DayOfWeek == DayOfWeek.Sunday)
                nuevaFecha = nuevaFecha.AddDays(1);
            return nuevaFecha;
        }
    }
}
