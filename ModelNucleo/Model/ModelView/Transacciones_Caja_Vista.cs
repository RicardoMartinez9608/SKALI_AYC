using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelNucleo.Model.ModelView
{
    public class Transacciones_Caja_Vista
    {
        public Caja Datos_Caja { get; set; }
        public string Tipo_reporte { get; set; }
        public DateTime Fecha_reporte { get; set; }
        public Transaccion_Caja Ultima_transaccion_anterior { get; set; }

        //desglose de ingresos
        public decimal Ingreso_Interes { get; set; }
        public decimal Ingreso_Comision { get; set; }
        public decimal Ingreso_Iva { get; set; }
        public decimal Ingreso_Capital { get; set; }
        public decimal Ingreso_Total { get; set; }

        //desglose de egresos
        public decimal Egreso_Desembolso { get; set; }

        public decimal Egreso_Total { get; set; }

        public decimal Disponible { get; set; }
        public Transacciones_Caja_Vista()
        {

        }

        public Transacciones_Caja_Vista(
            IEnumerable<Transaccion_Credito> transacciones,
            IEnumerable<Transaccion_Simple> simples,
            DateTime fecha)
        {
            /*buscar la transformacion*/
            Tipo_reporte ="Caja";
            Fecha_reporte = fecha;

            /*Egresos*/
            //List<Transaccion_Caja> Transacciones_egresos = transacciones
            //    .Where(
            //            g => g.Tipo_Transaccion.Descripcion == "Egreso"
            //            && g.Id_Transaccion_Caja != Ultima_transaccion_anterior.Id_Transaccion_Caja)
            //            .ToList();

            //repartir el monto ingresado
            //desglose de ingresos
            var transaccionCreditos = transacciones.ToList();
            Ingreso_Interes = transaccionCreditos.Sum(s=>s.Interes_Comision);
            Ingreso_Comision = transaccionCreditos.Sum(s => s.Interes);
            Ingreso_Iva = transaccionCreditos.Sum(s => s.IvaSobreComision);
            Ingreso_Capital = transaccionCreditos.Sum(s => s.Abono_capital);
            Ingreso_Total= transaccionCreditos.Sum(j => j.Valor_Cuota);

            Egreso_Desembolso = simples.ToList().Sum(a=>a.Monto);
            Egreso_Total = Egreso_Desembolso;

            Disponible = Ingreso_Total- Egreso_Total;
        }
    }
}
