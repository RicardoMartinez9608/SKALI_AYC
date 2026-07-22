using ModelNucleo.Model;
using ModelNucleo.Model.ModelView;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ModelNucleo.CalculosYProceos
{
    /// <summary>
    /// centralización de los calculos y procesos para mejor mantenimiento
    /// esto no es un recurso de la web api
    /// </summary>
    public class CalculosYProcesoCredito
    {
        public decimal Monto = 0.00M;
        public ResultadoCalculoCreditio Resultado;
        public List<Amortizacion> Amortizacion;
        /// <summary>
        /// sobrecarga de constructor para otros calculos
        /// </summary>
        public CalculosYProcesoCredito()
        {

        }

        /// <summary>
        /// constructor para el calculo
        /// </summary>
        /// <param name="tasa">decimal</param>
        /// <param name="monto">decimal</param>
        /// <param name="frecuencia">int</param>
        /// <param name="iva">decimal</param>
        /// <param name="plazo">int</param>
        /// <param name="tasaAyC">decimal</param>
        /// <param name="fecha_inicio">DateTime</param>
        /// <param name="total_cobros_periodicos">decimal</param>
        /// <param name="cobros_dinero">bool</param>
        public CalculosYProcesoCredito(
            decimal tasa_comision,
            decimal monto,
            Periodo frecuencia,
            decimal iva,
            int plazo,
            decimal interes,
            DateTime fecha_inicio,
            decimal total_cobros_periodicos=0.00M,
            bool cobros_dinero=false,
            decimal pago_anticipado=0.00M,
            int dias_movible =0,
            bool generar_amortizacion=false)
        {
            int dias = frecuencia.dias; //CalculoDias(frecuencia);

            decimal numero_cuotas = CalcularNoCuotas(plazo, frecuencia.Valor);
            decimal CuotaPrimaria = CalcularCuotaPrimaria(tasa_comision, monto, plazo,dias, numero_cuotas);
            decimal CoutaAyC = CalcularPlanAyC(tasa_comision, monto, plazo,interes,dias, numero_cuotas, cobros_dinero);/*alterado para respetar el sistema antiguo*/
            decimal IVACalculado =Math.Round((iva / 100) * CoutaAyC,2);
            total_cobros_periodicos =cobros_dinero? total_cobros_periodicos:0.00M;
            decimal comisionMasIVA = CoutaAyC + IVACalculado;
            decimal CuotaTotal = CuotaPrimaria + IVACalculado + total_cobros_periodicos;
            

            if(generar_amortizacion)
                Amortizacion = CalcularAmortizacion(
                    CuotaTotal,
                    comisionMasIVA,
                    /*IVACalculado,*/
                    frecuencia,
                    tasa_comision, 
                    monto, 
                    numero_cuotas,
                    interes,
                    fecha_inicio,
                    total_cobros_periodicos,
                    cobros_dinero,
                    pago_anticipado,dias_movible);

            decimal sumatoria = generar_amortizacion? Amortizacion.Sum(f=>f.Valor_Cuota):0.00M;

            Resultado = new ResultadoCalculoCreditio {
                CuotaPrimaria = CuotaTotal,
                cuotaAyC = comisionMasIVA,
                /*IVACalculado = IVACalculado,*/
                CuotaTotal = CuotaTotal,
                Amortizacion= Amortizacion,
                Sumatoria_Cuotas=sumatoria,
                No_Cuotas= numero_cuotas
            };
        }

        /// <summary>
        /// retorna la cuota antes del pla AyC
        /// =monto*(tasa/(1-(1+tasa)^-PLAZO)) la formula que se trabaja
        /// </summary>
        /// <param name="interes_comision"></param>
        /// <param name="monto"></param>
        /// <param name="plazo"></param>
        /// <returns>decimal</returns>
        public decimal CalcularCuotaPrimaria(decimal interes_comision, decimal monto, decimal plazo,int dias, decimal numero_cuotas)
        {
            if (interes_comision <= 0)
                return 0.00M;
            double plazo_dbl = Convert.ToDouble(plazo);
            double tasaEnCentecimas = Math.Round((Convert.ToDouble(interes_comision) / 100) / 12,4);
            bool es_mensual = Convert.ToInt32(numero_cuotas) == plazo;
            if (!es_mensual)
            {
                tasaEnCentecimas = Math.Round((decimal.ToDouble(interes_comision) / 100.00) / (365 / dias), 4);
                plazo_dbl = Convert.ToDouble(numero_cuotas);
            }
            decimal cuota = Math.Round(monto * ((Convert.ToDecimal(tasaEnCentecimas) * Convert.ToDecimal(Math.Pow(Convert.ToDouble(1) + tasaEnCentecimas, plazo_dbl))) / (Convert.ToDecimal(Math.Pow(Convert.ToDouble(1) + tasaEnCentecimas, plazo_dbl)) - 1)), 2);
            return Convert.ToDecimal(Math.Round(cuota, 2)); //0.00M;
        }

        /// <summary>
        /// se hace el mismo calculo solo que con otra tasa
        /// y se resta el anterior
        /// </summary>
        /// <param name="tasa"></param>
        /// <param name="monto"></param>
        /// <param name="numero_cuotas"></param>
        /// <param name="tasaAyC"></param>
        /// <returns>decimal planAyC</returns>
        public decimal CalcularPlanAyC(
            decimal interes_comision, 
            decimal monto, 
            decimal plazo,
            decimal interes,
            int dias,
            decimal numero_cuotas,
            bool cobros_dinero=false)
        {
            decimal cuotaPrimaria= CalcularCuotaPrimaria(interes_comision, monto, plazo,dias,numero_cuotas);
            if (cobros_dinero)
                return interes;
            else
            {
                decimal cuotaPrimariaAyC = CalcularCuotaPrimaria(interes, monto, plazo,dias,numero_cuotas);//este cálculo siempre debe ser menor
                if(interes > 0.00M)
                    return Math.Round((cuotaPrimaria - cuotaPrimariaAyC), 2);
                return 0.00M;
            }
        }
        

        /// <summary>
        /// con el número de plazo y el de frecuencia obtener
        /// el número de cuotas
        /// </summary>
        /// <param name="plazo"></param>
        /// <param name="frecuencia"></param>
        /// <returns>int número de cuotas</returns>
        public decimal CalcularNoCuotas(int plazo, int frecuencia)
        {
            decimal NoCuotas = frecuencia * plazo;
            if (frecuencia == 4)
            {
                //alteración usurera semanal para que este tablas con el viejo (temporal)
                switch (plazo)
                {
                    case 1:
                        NoCuotas = 4;
                        break;
                    case 2:
                        NoCuotas = 9;
                        break;
                    case 3:
                        NoCuotas = 13;
                        break;
                    case 4:
                        NoCuotas = 18;
                        break;
                    case 5:
                        NoCuotas = 22;
                        break;
                    case 6:
                        NoCuotas = 26;
                        break;
                    case 7:
                        NoCuotas = 30;
                        break;
                    case 8:
                        NoCuotas = 35;
                        break;
                    case 9:
                        NoCuotas = 39;
                        break;
                    case 10:
                        NoCuotas = 44;
                        break;
                    case 11:
                        NoCuotas = 48;
                        break;
                    case 12:
                        NoCuotas = 52;
                        break;

                    default:
                        break;
                }
            }
                //return Convert.ToDecimal(4.34 * plazo);//alteración usurera
            return NoCuotas;
        }

        /// <summary>
        /// Se prepara la amortización en filas
        /// </summary>
        /// <param name="CuotaTotal"></param>
        /// <param name="comision_sinIVA"></param>
        /// <param name="IVACalculado"></param>
        /// <param name="frecuencia"></param>
        /// <param name="interes_comision"></param>
        /// <param name="monto"></param>
        /// <param name="numero_cuotas"></param>
        /// <param name="interes"></param>
        /// <param name="fecha_inicio"></param>
        /// <param name="total_cobros_periodicos"></param>
        /// <param name="cobros_dinero"></param>
        /// <param name="pago_anticipado"></param>
        /// <param name="dias_movible"></param>
        /// <returns></returns>
        public List<Amortizacion> CalcularAmortizacion(
            decimal CuotaTotal,
            decimal Comision,
            /*decimal IVACalculado,*/

            Periodo frecuencia,
            decimal interes_comision, 
            decimal monto,
            decimal numero_cuotas,
            decimal interes,
            DateTime fecha_inicio,
            decimal total_cobros_periodicos=0.00M,
            bool cobros_dinero = false,
            decimal pago_anticipado = 0.00M,
            int dias_movible = 0)
        {
            decimal saldo_al_capital = monto;
            
            interes = interes > 0 ? interes : interes_comision;

            List <Amortizacion> amortizacion = new List<Amortizacion>();
            int centinela_dias = 0;/*todo por los Ágil*/
            for (int i = 0; i < numero_cuotas; i++)
            {
                bool ultimo_pago = i== (numero_cuotas - 1) ? true:false;
                bool primer_pago = i == 0 ? true : false;

                centinela_dias++;

                int anterior = i-1;

                DateTime fecha_anterior = fecha_inicio;

                if (amortizacion.Count > 1)
                {
                    Amortizacion amortizacion_anterior = amortizacion.Single(d => d.No == anterior);/*para la fecha anterior*/
                    fecha_anterior = Convert.ToDateTime(amortizacion_anterior.FechaPago);
                }
                /*cambiar las fechas y sacar los días*/
                /*todo por Ágil*/
                
                if (frecuencia.Id_Periodo == 0)
                {
                    if (fecha_inicio.AddDays(centinela_dias).Date.DayOfWeek == DayOfWeek.Saturday)
                        centinela_dias += 2;
                    /*if (fecha_inicio.AddDays(centinela_dias).Date.DayOfWeek == DayOfWeek.Sunday)
                        centinela_dias += 2;*/
                }
                /*Todo por Ágil*/

                Amortizacion item = CalculoAmortizacionFila(i, 
                    saldo_al_capital,
                    CuotaTotal,
                    frecuencia.dias,
                    interes, 
                    fecha_inicio,
                    Comision, 
                    /*IVACalculado,*/
                    frecuencia.Id_Periodo,
                    0.00M,
                    false,
                    false,
                    total_cobros_periodicos,
                    cobros_dinero,
                    ultimo_pago,
                    pago_anticipado,
                    null,
                    primer_pago,
                    dias_movible,
                    fecha_anterior,
                    centinela_dias);

                saldo_al_capital = item.Saldo_Capital;
                amortizacion.Add(item);
            } 
            return amortizacion;
        }

        /// <summary>
        /// devuelve la fila de amortización
        /// </summary>
        /// <param name="index"></param>
        /// <param name="saldo_al_capital"></param>
        /// <param name="valor_cuota"></param>
        /// <param name="dias"></param>
        /// <param name="tasa"></param>
        /// <param name="fecha_inicio"></param>
        /// <param name="planAyC"></param>
        /// <param name="iva"></param>
        /// // <param name="recargo"></param>
        /// <param name="actual"></param>
        /// <param name="mismaFecha"></param>
        /// <returns>Amortizacion</returns>
        public Amortizacion CalculoAmortizacionFila(
            int index,
            decimal saldo_al_capital,
            decimal valor_cuota,
            int dias,
            decimal interes,
            DateTime fecha_inicio,
            decimal comision,
            /*decimal iva,*/
            int id_periodo,/*reciente el id para controlar el ágil y efectivo*/
            decimal recargo = 0.00M,
            bool actual = false,
            bool mismaFecha = false,
            decimal total_cobros_periodicos = 0.00M,
            bool cobros_dinero = false,
            bool ultimo_pago = false,
            decimal pago_anticipado = 0.00M,
            /*apartado para tener fecha que corresponde*/
            DateTime? fecha_amortizacion = null,
            bool primer_pago = false,
            int dias_movible = 0,
            DateTime? fecha_anterior = null,
            int centinela_dias=0,
            bool para_reporte= false
        )
        {
            index = index + 1;
            
            DateTime fecha_cuota;
            if (mismaFecha)
                fecha_cuota = fecha_inicio;
            else
                fecha_cuota = FijarFecha(fecha_inicio,dias, index, actual, id_periodo, centinela_dias);

            int diasPorMes = FijarDias(fecha_cuota, dias,true);

            decimal intereses = Math.Round((saldo_al_capital * diasPorMes * interes) / 36500, 2);

            if (primer_pago && dias_movible > 0)
            {
                decimal intereses_diarios = Math.Round((saldo_al_capital * (dias_movible * interes)) / 36500, 2);
                valor_cuota += intereses_diarios /*+ iva*/;
                diasPorMes += dias_movible;
                intereses +=  intereses_diarios;
            }

            total_cobros_periodicos = cobros_dinero ? total_cobros_periodicos : 0.00M;//probando que este correcto la lista de cobros periodos
            decimal iva = Math.Round((13 / 100) * comision, 2);
            Segregacion_Cuota segregacion = Segregacion_Cuota(valor_cuota, intereses, comision,iva, total_cobros_periodicos, recargo);
            decimal abono_capital = segregacion.Abono_Capital;
            intereses = segregacion.Intereses;
            comision = segregacion.PlanAyC;
            total_cobros_periodicos = segregacion.Total_cobros_periodicos;
            recargo = segregacion.Recargo;
            iva = segregacion.Iva;
            decimal nuevo_saldo_al_capital =saldo_al_capital - abono_capital;
      
            if (ultimo_pago)
            {
                //valor_cuota += iva;
                nuevo_saldo_al_capital = 0;
                abono_capital = saldo_al_capital;
                if(segregacion.PlanAyC == 0.00M)
                {
                    comision = 0.00M;
                    intereses += valor_cuota - (abono_capital + intereses + iva);
                }
                else { 
                comision = valor_cuota - (abono_capital+ intereses + iva);
                }
                if (comision < 0.00M)
                {
                    comision = 0.00M;
                }
            }    
            if(abono_capital > saldo_al_capital)
            {
                decimal diferencia = abono_capital - saldo_al_capital;

                abono_capital = abono_capital - diferencia;
                intereses = intereses + diferencia;
            }   
            Amortizacion amortizacion = new Amortizacion
            {
                No = index,
                Dias = diasPorMes,
                Interes = intereses,
                Comision = comision,
                /*IvaSobreComision = 0.00M,*/
                Otros_Cobros = total_cobros_periodicos,
                Abono_capital = abono_capital,
                Saldo_Capital = nuevo_saldo_al_capital < 0 ? 0.00M : nuevo_saldo_al_capital,
                Valor_Cuota = Math.Round(valor_cuota,2),
                FechaPago = fecha_cuota,
                Fecha_Amortizacion= Convert.ToDateTime(fecha_amortizacion),
                Pago_para_cancelar= valor_cuota + saldo_al_capital + Math.Round(saldo_al_capital * (pago_anticipado/100),2)
            };
            return amortizacion;
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
                dias = 7;
            if (frecuencia == 30)
                dias = 1;

            return dias;
        }

        public DateTime FijarFecha(
            DateTime fecha,
            int dias, 
            int index,
            bool actual,
            int id_periodo,
            int centinela_dias)
        {
            DateTime nuevaFecha = fecha.Date;
            if (actual)
                dias = index;
            else
                dias *= index;

            if (id_periodo == 4)
                nuevaFecha = ArmandoFechaMensual(fecha.Date, index);
            else
            {
                if (id_periodo == 0)/*si es ágil y efectivo se salta los fines de semana*/
                {
                    /*DateTime fechaAnterior = Convert.ToDateTime(fecha_anterior);
                    if (fechaAnterior.Date.AddDays(centinela_dias).DayOfWeek == DayOfWeek.Sunday)
                        centinela_dias += 1;*/
                    if (fecha.Date.AddDays(centinela_dias).DayOfWeek == DayOfWeek.Saturday)
                        centinela_dias += 2;
                    nuevaFecha = fecha.Date.AddDays(centinela_dias);
                }else
                    nuevaFecha = fecha.Date.AddDays(dias);
            }
            return nuevaFecha.Date;
        }


        /// <summary>
        /// fija cantidad de días entre fecha cuando es mensual
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="dias"></param>
        /// <param name="frecuencia"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public int FijarDias(DateTime fecha, int dias,bool anterior=false)
        {
            DateTime nuevaFecha;
            int nuevoDias;
            if (dias == 30)
            {
                nuevaFecha = ArmandoFechaMensual(fecha,1,anterior);
                    //nuevaFecha = fecha.AddMonths(1);
                TimeSpan espacio =anterior? (fecha.Date - nuevaFecha.Date) : (nuevaFecha.Date - fecha.Date);
                nuevoDias = espacio.Days;
            }
            else
                nuevoDias = dias;
            return nuevoDias;
        }

        /// <summary>
        /// algoritmo para segregar en orden la parte de la cuota
        /// </summary>
        /// <param name="valor_cuota"></param>
        /// <param name="intereses"></param>
        /// <param name="planAyC"></param>
        /// <param name="total_cobros_periodicos"></param>
        /// <param name="iva"></param>
        /// <param name="recargo"></param>
        /// <returns></returns>
        private Segregacion_Cuota Segregacion_Cuota(
            decimal valor_cuota,
            decimal intereses,
            decimal planAyC,
            decimal iva,
            decimal total_cobros_periodicos,
            decimal recargo)
        {
            decimal valor_cuota_inicial = valor_cuota;

            if (valor_cuota >= total_cobros_periodicos)
                valor_cuota -= total_cobros_periodicos;
            else
            {
                total_cobros_periodicos = valor_cuota;
                valor_cuota = 0;
            }

            if (valor_cuota >= recargo)
                valor_cuota -= recargo;
            else
            {
                recargo = valor_cuota;
                valor_cuota = 0;
            }

            decimal ivaconplan = planAyC + iva;
            if (valor_cuota >= ivaconplan)
                valor_cuota -= ivaconplan;
            else
            {
                planAyC = valor_cuota;
                valor_cuota = 0;
            }

            if (valor_cuota >= intereses)
                valor_cuota -= intereses;
            else
            {
                intereses = valor_cuota;
                valor_cuota = 0;
            }
            
            return new Segregacion_Cuota
            {
                Valor_cuota = valor_cuota_inicial,
                Intereses = intereses,
                PlanAyC = planAyC,
                Total_cobros_periodicos = total_cobros_periodicos,
                Iva = planAyC >= iva ? iva : 0.00M,
                Recargo = recargo,
                Abono_Capital = valor_cuota
            };
        }

        private DateTime ArmandoFechaMensual(DateTime fecha,int paso,bool anterior=false)
        {
            DateTime nuevaFechaPredeterminada =anterior? fecha.AddMonths(-paso) : fecha.AddMonths(paso);
            DateTime primer_dia = new DateTime(nuevaFechaPredeterminada.Year, nuevaFechaPredeterminada.Month, 1);
            DateTime ultimo_dia = primer_dia.AddMonths(1).AddDays(-1);
            int dia = fecha.Day;
            DateTime nuevaFecha;
            try
            {
                nuevaFecha = new DateTime(nuevaFechaPredeterminada.Year, nuevaFechaPredeterminada.Month, dia);
            }
            catch (Exception ex)
            {
                nuevaFecha = ultimo_dia;
            }
            return nuevaFecha;
        }

    }

    //tipo temporal
    public class Segregacion_Cuota
    {
        public decimal Valor_cuota { get; set; }
        public decimal Intereses { get; set; }
        public decimal PlanAyC { get; set; }
        public decimal Total_cobros_periodicos { get; set; }
        public decimal Iva { get; set; }
        public decimal Recargo { get; set; }
        public decimal Abono_Capital { get; set; }
    }
}
