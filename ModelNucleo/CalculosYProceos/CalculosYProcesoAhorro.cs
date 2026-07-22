using ModelNucleo.Model.ModelView;
using System;

namespace ModelNucleo.CalculosYProceos
{
    public class CalculosYProcesoAhorro
    {

        public AhorroRegistro CalcularInteres(AhorroRegistro ahorro,decimal interes)
        {
            decimal interes_actual = Math.Round((((interes / 100) * ahorro.Saldo) / 365) * ahorro.Dias,2);
            ahorro.Interes = decimal.Round(interes,2);
            ahorro.Deposito = +interes_actual;
            return ahorro;
        }

        /// <summary>
        /// calcula el interes en todas las transacciones de ahorros
        /// que se hagan
        /// </summary>
        /// <param name="fecha_anterior">fecha de la transacción anterior</param>
        /// <param name="fecha_actual">fecha de la transacción actual</param>
        /// <param name="saldo">el saldo de la transacción anterior</param>
        /// <param name="interes_anual">el interés </param>
        /// <returns></returns>
        public decimal CalcularInteres(
         DateTime fecha_anterior,
         DateTime fecha_actual,
         decimal saldo,
         decimal interes_anual)
        {
            decimal interesDecimal = interes_anual / 100;

            var diferencia = fecha_actual.Date - fecha_anterior.Date;
            int dias = diferencia.Days;

            if (dias <= 0)
                return 0;

            decimal interes = Math.Round((saldo * interesDecimal) / (365 * dias), 8);
            return interes;
        }

        /// <summary>
        /// determina el número de días
        /// para la amortización
        /// </summary>
        /// <param name="frecuencia"></param>
        /// <returns>int días</returns>
        public int CalculoDias(int frecuencia)
        {
            int dias = 0;
            if (frecuencia == 1)
                dias = 30;
            if (frecuencia == 2)
                dias = 15;
            if (frecuencia == 4)
                dias = 8;
            if (frecuencia == 30)
                dias = 1;

            return dias;
        }
    }
}
