using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelNucleo.Model;
using ModelNucleo.Model.ModelView;
using Newtonsoft.Json;
using NucleoAyC.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace NucleoAyC.Controllers
{
    /// <summary>
    /// Métodos de transacciones de credito
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = "Condonacion")]
    public class CondonacionController : Base_TransaccionController
    {

        /// <summary>
        /// constructor e injección de credito
        /// </summary>
        /// <param name="context"></param>
        /// /// <param name="userManager"></param>
        /// <param name="roleManager"></param>
        public CondonacionController(NegocioContexto context,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
            : base(context, userManager, roleManager)
        {
        }

        /// <summary>
        /// muestra los datos para ver la posibilidad de condonar y maniobrar
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Detalle_Condonacion(int id_credito)
        {
            DateTime fecha=DateTime.Now;
            Transaccion_Credito transaccion = await Task.Run(() => AmortizacionActualProc(id_credito, fecha, 0.00M, false, recargo: false));
            Condonacion condonacion = new Condonacion();
            condonacion.Id_Credito= id_credito;
            condonacion.Credito=transaccion.Credito;
            /*condonacion.Valor_Dinero_Gracia = transaccion.Capital_Mora;*/
            /*condonacion.Lapso_Tiempo = String.Format("Capital de mora de {0} días", transaccion.Dias);*/
            Linea_Credito_Historial linea = _context
                .Linea_Credito_Historial
                .Include(d=>d.Credito)
                .Include(c=>c.Credito.Periodo)
                .Single(c => c.Id_Credito == id_credito);
            decimal recargo = linea.Recargo;
            List<Amortizacion> amortizaciones = _context
                .Amortizacion
                .Where(d=>d.Id_Credito==id_credito).OrderBy(d=>d.No)
                .ToList();
            List<RecargoCondonacionVista> condonacionVistas = new List<RecargoCondonacionVista>();

            List<DistribucionRecargo> regargos = Distribucion_Recargo(
                transaccion.Pagos_Acumulado,
                linea.Recargo,
                transaccion.Dias,
                //linea.Credito.Periodo.dias
                amortizaciones.Max(a=>a.Dias),
                id_credito
                );

         
                int cantidad_distribuida = 0;
                foreach (Amortizacion amortizacion in amortizaciones)
                {
                    if (amortizacion.FechaPago.Date <= fecha.Date)
                    {
                        if (amortizacion.Recargo == 0.00M)
                        {
                            bool cubierto = false;
                            if (amortizacion.FechaPago.Date > transaccion.Fecha_Amortizacion.Date)
                            {
                                cubierto = true;
                                decimal recargo_distribuido = 0.00M;
                                cantidad_distribuida += 1;
                                if (regargos.Any(d => d.Id_Cuota == cantidad_distribuida && d.Valor_Recargo>0))
                                    recargo_distribuido = regargos.Single(d => d.Id_Cuota == cantidad_distribuida).Valor_Recargo;
                            //if (recargo_distribuido > 0) {
                                condonacionVistas.Add(new RecargoCondonacionVista
                                {
                                    Id_Amortizacion = amortizacion.Id_Amortizacion,
                                    No = amortizacion.No,
                                    Recargo = recargo_distribuido, //recargo,
                                    Condonar = false,
                                    Cubierto = cubierto,
                                    Fecha = amortizacion.FechaPago
                                });
                            //}
                                
                            }
                        }
                    }
                }
           

          
            condonacion.RecargoCondonacionVista = condonacionVistas;
            //condonacion.Lapso_Tiempo=transaccion.
            condonacion.Valor_Dinero_Recargos = 0.00M;
            return Ok(condonacion);
        }

        /// <summary>
        /// procesar las condonaciones en las amortizaciones
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [Authorize(Roles = "Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> ProcesarCondonacion(string ids, decimal recargo) {
            var ids_ar = ids.Split(',');
            decimal valor_condonacion = 0.00M;
            foreach (var id in ids_ar)
            {
                int idi = Convert.ToInt32(id);
                if (_context.Amortizacion.Any(d => d.Id_Amortizacion == idi))
                {
                    var amortizacion = _context.Amortizacion.Single(c => c.Id_Amortizacion == idi);
                    //decimal recargo = _context
                    //    .Linea_Credito_Historial
                    //    .Single(c => c.Id_Credito == amortizacion.Id_Credito).Recargo;
                    amortizacion.Recargo = recargo;
                    valor_condonacion += amortizacion.Recargo;
                    _context.Update(amortizacion).State = EntityState.Modified;
                }
            }
            await _context.SaveChangesAsync();

            /*guardar el registro de las condonaciones*/
            Condonacion condonacion = new Condonacion();
            int id_primero = Convert.ToInt32(ids_ar.FirstOrDefault());
            int id_credito = _context.Amortizacion.Single(d => d.Id_Amortizacion == id_primero).Id_Credito;
            condonacion.Id_Credito = id_credito;
            condonacion.Valor_Dinero_Recargos = valor_condonacion;
            condonacion.Condonar_Recargos = true;
            var userId = _userManager.GetUserId(HttpContext.User);
            condonacion.Id_Usuario = userId;
            condonacion.Fecha = DateTime.Now;
            _context.Condonacion.Add(condonacion).State = EntityState.Added;
            await _context.SaveChangesAsync();
            return Ok("Se han condonado los recargos requeridos");
        }

        /// <summary>
        /// algoritmo para distribuir el monto y su tope por semana
        /// </summary>
        /// <param name="pagos_acumulados"></param>
        /// <param name="valor_recargo"></param>
        /// <param name="dias_mora"></param>
        /// <param name="dias"></param>
        /// <returns></returns>
        private List<DistribucionRecargo> Distribucion_Recargo(
            int pagos_acumulados, 
            decimal valor_recargo,
            int dias_mora,
            int dias, 
            int id_credito)/*intervalo o periodo en días por pago*/
        {
            bool tiene_transacciones = _context.Transaccion_Credito
                .AsNoTracking().Any(f => f.Id_Credito == id_credito && !f.Borrado_Logico);
            Credito credito = _context.Credito.AsNoTracking().Single(a => a.Id_Credito == id_credito);
            DateTime fecha_para_Vencido = DateTime.Now;
            if (credito.Congelado)
                fecha_para_Vencido = credito.Congelado_En;
            List<Amortizacion> amortizacion_vencidas = new List<Amortizacion>();
            if (_context.Amortizacion.AsNoTracking().Any(d => d.FechaPago.Date <= fecha_para_Vencido.Date && d.Id_Credito == credito.Id_Credito))
            {
                amortizacion_vencidas = _context
                    .Amortizacion
                    .AsNoTracking()
                    .Where(d => d.FechaPago.Date <= fecha_para_Vencido.Date && d.Id_Credito == credito.Id_Credito)
                    .OrderBy(d => d.No).ToList();
            }
            int centinela_menos_dias = dias_mora;
            List<DistribucionRecargo> distribucion=new List<DistribucionRecargo>();
            for (int i = 1; i <= pagos_acumulados; i++)
            { 
                DistribucionRecargo distribucionRecargo = new DistribucionRecargo();
                distribucionRecargo.Id_Cuota = i;
                if (centinela_menos_dias > 0)
                {
                    if (i == pagos_acumulados)
                    {
                        if (!tiene_transacciones)
                        {
                            DateTime UltimaFecha = credito.Liquidado_En.Date;
                            if (amortizacion_vencidas.Count > 0)
                                UltimaFecha = amortizacion_vencidas.First().FechaPago;

                            TimeSpan diferencia_mora = fecha_para_Vencido.Date - UltimaFecha.Date;
                            if (pagos_acumulados > 0)
                                dias_mora = diferencia_mora.Days;
                            else
                                dias_mora = 0;

                        }
                        distribucionRecargo.Dias = dias_mora;
                    }
                    else
                    {
                        if (i == 1)
                            distribucionRecargo.Dias = dias_mora;
                        else
                            distribucionRecargo.Dias = centinela_menos_dias;
                        centinela_menos_dias = dias_mora - (dias * i);
                    }

                    distribucionRecargo.Semanas = (int)Math.Floor(Convert.ToDecimal(distribucionRecargo.Dias) / 7);
                    decimal monto = valor_recargo * distribucionRecargo.Semanas;
                    if (monto > 16)
                        distribucionRecargo.Valor_Recargo = 16;
                    else
                        distribucionRecargo.Valor_Recargo = monto;
                    distribucion.Add(distribucionRecargo);
                }
            }
            return distribucion;
        }

    


    }
}
