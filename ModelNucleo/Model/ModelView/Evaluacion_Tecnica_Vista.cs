using ModelNucleo.Model.Evaluacion_Tecnica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelNucleo.Model.ModelView
{
    public class Evaluacion_Tecnica_Vista
    {
        public Evaluacion Evaluacion { get; set; }

        /*muestra de resultados*/
        /*izquierda*/
        public decimal Venta_Mensual { get; set; }//1
        public decimal Recuperacion_Cuentas_Cobrar { get; set; }//2
        public decimal Ingresos_Negocio { get; set; }//3 /*1+2*/

        public decimal Costo_Venta { get; set; }//4
        public decimal Gastos_Negocio { get; set; }//5
        public decimal Disponibilidad_Negocio { get; set; }//6 /*3-4-5*/

        public decimal Otros_Ingresos { get; set; }//7
        public decimal Total_Ingreso_Familiar { get; set; }//8 /*6+7*/
        
        public decimal Gastos_Familiares { get; set; }//9
        public decimal Total_Disponible { get; set; }//10 /*8-9*/

        /*derecha*/
        public decimal Disponible { get; set; }//1
        public decimal Cuentas_Cobrar { get; set; }//2
        public decimal Inventario { get; set; }//3
        public decimal Activo_Circulante { get; set; }//4 /*1+2+3*/

        public decimal Activo_Fijo { get; set; }//5
        public decimal Activo_Total { get; set; }//6 /*4+5*/

        public decimal Proveedores { get; set; }//7
        public decimal Prestamos { get; set; }//8
        public decimal Pasivo_Circulante { get; set; }//9 /*7+8*/

        public decimal Patrimonio { get; set; }//10 /*6-9*/

        public decimal Total_Pasivo_Patrimonio { get; set; }//11 /*9+10*/

        public Evaluacion_Tecnica_Vista(Evaluacion evaluacion)
        {
            Evaluacion = evaluacion;

            Venta_Mensual = evaluacion.Venta_Mensual_Malo + evaluacion.Venta_Mensual_Bueno;
            Recuperacion_Cuentas_Cobrar = evaluacion.Cuentas_Cobrar.Count > 0 ? evaluacion.Cuentas_Cobrar.FirstOrDefault().Recuperado:0.00M;
            Ingresos_Negocio = Venta_Mensual + Recuperacion_Cuentas_Cobrar;

            Costo_Venta =evaluacion.Costos_Ventas.Count > 0 ? evaluacion.Costos_Ventas.Sum(t=>t.Margen_Bruto_Porcentaje) * Venta_Mensual:0.00M;
            Gastos_Negocio =evaluacion.Gastos.Count > 0 ? evaluacion.Gastos.Where(g=>g.Tipo_Gasto.Id_Categoria_Gasto==1).Sum(g=>g.Valor):0.00M;//suponiendo que uno es de gastos de negocio
            Disponibilidad_Negocio = Ingresos_Negocio - Costo_Venta - Gastos_Negocio;

            Otros_Ingresos =evaluacion.Otros_Ingresos.Count > 0 ? evaluacion.Otros_Ingresos.Sum(o => o.Valor):0.00M;
            Total_Ingreso_Familiar = Disponibilidad_Negocio + Otros_Ingresos;

            Gastos_Familiares =evaluacion.Gastos.Count > 0 ? evaluacion.Gastos.Where(g => g.Tipo_Gasto.Id_Categoria_Gasto == 2).Sum(v=>v.Valor):0.00M;
            Total_Disponible = Total_Ingreso_Familiar - Gastos_Familiares;

            Disponible = 0;
            Cuentas_Cobrar =evaluacion.Cuentas_Cobrar.Count > 0 ? evaluacion.Cuentas_Cobrar.FirstOrDefault().Completo:0.00M;
            Inventario =evaluacion.Inventarios_Negocio.Count > 0 ? evaluacion.Inventarios_Negocio.Sum(n=>n.Total):0.00M;
            Activo_Circulante = Disponible + Cuentas_Cobrar + Inventario;

            Activo_Fijo =evaluacion.Activos_Negocio.Count > 0 ? evaluacion.Activos_Negocio.Sum(a=>a.Precio_Peritado):0.00M;
            Activo_Total = Activo_Circulante + Activo_Fijo;

            Proveedores = 0;
            Prestamos =evaluacion.Prestamos.Count > 0 ? evaluacion.Prestamos.Sum(p=>p.Saldo):0.00M;
            Pasivo_Circulante = Proveedores + Prestamos;

            Patrimonio = Activo_Total - Pasivo_Circulante;

            Total_Pasivo_Patrimonio = Pasivo_Circulante + Patrimonio;
        }
    }
}
