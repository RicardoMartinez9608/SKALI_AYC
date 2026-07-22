using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelNucleo.CalculosYProceos;
using ModelNucleo.Model;
using ModelNucleo.Model.ModelView;
using ModelNucleo.Model.Tablero;
using NucleoAyC.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NucleoAyC.Controllers
{
    /// <summary>
    /// Reportes solo para el estado de las créditos
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = "Reporte_Credito")]
    public class Reporte_CreditoController : Base_TransaccionController
    {

        private Func<Transaccion_Credito, bool> filtrar_transacciones;
        private Func<Credito, bool> filtrando_periodo;

        /// <summary>
        /// constructor para Identity y connexión a base de datos
        /// </summary>
        /// <param name="context">contexto de la base de datos</param>
        /// <param name="userManager">manejo de usuarios</param>
        /// <param name="roleManager">manejo de roles</param>
        public Reporte_CreditoController(NegocioContexto context,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
            : base(context, userManager, roleManager)
        {
        }

        /// <summary>
        /// genera el objeto para reporte de generales
        /// </summary>
        /// <param name="id_agencia">identificador de la agencia</param>
        /// <param name="fecha_inicio">fecha inicia rango</param>
        /// <param name="fecha_final">fecha termina rango</param>
        /// <returns></returns>
      //  [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Reporte_Generales_Credito(int id_agencia, DateTime fecha_inicio, DateTime fecha_final)
        {
            try
            {
                var reportesAgencias = new List<Reporte_General_Credito>();
                var generales = new Reporte_General_Credito();
                if (id_agencia == 0) //significa que quiere todas las agencias
                {
                    var agencias = _context.Agencia.ToList();
                    foreach (var agencia in agencias)
                    {
                        if (await AgenciaCreditos(agencia.Id_Agencia))
                        {
                            generales =
                                await Generales_Por_Agencia(agencia, fecha_inicio, fecha_final);
                            reportesAgencias.Add(generales);
                        }
                    }
                }
                else
                {
                    var agencia = _context.Agencia.SingleOrDefault(c => c.Id_Agencia == id_agencia);
                    generales =
                        await Generales_Por_Agencia(agencia, fecha_inicio, fecha_final);
                    reportesAgencias.Add(generales);
                }

                return Ok(reportesAgencias);
            }
            catch (Exception ex)
            {
                return NoContent();
            }
        }

        private async Task<bool> AgenciaCreditos(int id_agencia)
        {
            var tiene_credito = false;
            var carteras_credito = _context.Usuario_Agencia.Where(r => r.Id_Agencia == id_agencia).ToList();
            foreach (var cartera in carteras_credito)
            {
                var tiene_cartera = _context.Cartera
                    .Any(t => t.Id_Usuario == cartera.Id_Usuario);
                if (tiene_cartera)
                {
                    var cartera_individual = _context.Cartera
                        .Single(t => t.Id_Usuario == cartera.Id_Usuario);
                    var creditos = _context.Credito.Any(d => d.Id_Cartera == cartera_individual.Id_Cartera);
                    if (creditos)
                    {
                        tiene_credito = true;
                        break;
                    }
                }
            }

            return tiene_credito;
        }

        private async Task<Reporte_General_Credito> Generales_Por_Agencia(Agencia agencia, DateTime fecha_inicio, DateTime fecha_final)
        {
            List<string> ids_usuario = _context.Agencia
                .AsNoTracking()
                .Include(f => f.Usuario_Agencias)
                .FirstOrDefault(t => t.Id_Agencia == agencia.Id_Agencia)
                ?.Usuario_Agencias
                .Select(h => h.Id_Usuario).ToList();
            string Id_Usuario = _userManager.GetUserId(HttpContext.User);

            List<Cartera> carteras = new List<Cartera>();
          

            foreach (string id in ids_usuario)
            {
                if (_context.Cartera.Any(f => f.Id_Usuario == id))
                {
                    Cartera carteras_por_usuario = _context.Cartera.AsNoTracking()
                        .Include("Creditos")
                        .Include("Creditos.Transacciones_Creditos")
                        .Include("Creditos.Periodo")
                      
                        .SingleOrDefault(j => j.Id_Usuario == id && !j.Excluido);
                    if (carteras_por_usuario != null && carteras_por_usuario.Creditos.Count > 0)
                    {
                        carteras.Add(carteras_por_usuario);
                    }
                }
            }

            Reporte_General_Credito reporte_individual = new Reporte_General_Credito();
            reporte_individual.Nombre_agencia = agencia.Nombre;
            reporte_individual.Generado_Por = Id_Usuario;
            reporte_individual.Fecha = DateTime.Now.Date;
            List<Generales_Credito> generales = await Generales(carteras, fecha_inicio, fecha_final);
            reporte_individual.Generales = generales;
            return reporte_individual;
        }

        private async Task<List<Generales_Credito>> Generales(List<Cartera> carteras, DateTime fecha_inicio, DateTime fecha_final)
        {

            filtrando_periodo = new Func<Credito, bool>(
                  d => d.Liquidado_En.Date >= fecha_inicio.Date
                  && d.Liquidado_En.Date <= fecha_final.Date);

            filtrar_transacciones = new Func<Transaccion_Credito, bool>(
                a => a.FechaPago.Date >= fecha_inicio.Date
                && a.FechaPago.Date <= fecha_final.Date);

            List<Generales_Credito> generales = new List<Generales_Credito>();
            foreach (Cartera cartera in carteras)
            {

                List<Persona> persona = new List<Persona>();
                List<Credito> creditos = cartera.Creditos.Where(a=>!a.Cancelado).ToList();
           
                decimal suma_abono = Suma_Abono_Capital(creditos);
                decimal cartera_asesor = Valor_Cartera(creditos, fecha_inicio, fecha_final);
                Generales_Credito gn = new Generales_Credito();
                gn.No_Cartera = cartera.Id_Cartera;
                gn.Nombre_Asignado = cartera.Nombre;
                gn.Colocacion = creditos
                    .Where(filtrando_periodo)
                    .Sum(d => d.Monto_pago);
                gn.Nuevos_Creditos = creditos.Where(filtrando_periodo).Where(a=>a.Id_Comportamiento_Credito ==1).Count();
                gn.Recuperados = creditos.Where(filtrando_periodo).Where(d => d.Id_Comportamiento_Credito == 2).Count();
                gn.Recurrentes = creditos.Where(filtrando_periodo).Where(d => d.Id_Comportamiento_Credito == 3).Count(); 
                gn.Valor_Cartera = cartera_asesor;
                gn.Creditos = creditos.Count(a => a.Cancelado == false && a.Id_Cartera == cartera.Id_Cartera );
                gn.Clientes_Activos = Clientes_Activos(cartera);
                gn.Clientes_Inactivos = Clientes_Inactivos(cartera);
                gn.Capital_Recuperado = Capital_Recuperado(creditos, fecha_inicio, fecha_final);
            //    gn.Capital_Mora = Suma_Capital_Mora(creditos, fecha_inicio, fecha_final);
                if(cartera_asesor == 0M)
                {
                    gn.Colecta = 0;
                }
                else {
                    gn.Colecta = Colectas(creditos, fecha_inicio, fecha_final) / cartera_asesor > 0 ? Colectas(creditos, fecha_inicio, fecha_final) / cartera_asesor : 0;

                }
                //     gn.Indice = await Indice_Mora(creditos,fecha_inicio,fecha_final)/cartera_asesor;

                //    gn.Creditos_Mora = await Creditos_Mora(creditos, fecha_inicio, fecha_final);
                //  gn.Clientes_Mora = await Clientes_En_Mora(creditos, fecha_inicio, fecha_final);
                //  gn.Clientes_Vencidos =  Clientes_Vencidos(cartera, fecha_final);
                // gn.Vencidos = gn.Valor_Cartera > 0 ? (Saldo_Capital_Total(creditos, fecha_final, true) / gn.Valor_Cartera) : 0;
                generales.Add(gn);
            }
            return generales;



        }
        private int Clientes_Activos(Cartera cartera)
        {
            var personas = (from a in _context.Personas
                            join p in _context.Credito
                                on a.Id_Persona equals p.Id_Persona
                            where p.Id_Cartera == cartera.Id_Cartera && p.Cancelado == false
                            select new
                            {
                                a
                            }).Distinct().Count();
            return personas;
        }
        private async Task<int> Clientes_En_Mora(List<Credito> creditos,
          DateTime fecha_inicio, DateTime fecha_final)
        {

            var detalle = Llenar_Reporte_MoraOVenciendo(creditos, fecha_inicio, fecha_final, true, true);
            Reporte_Vencimiento_Cuotas_Mora_Diaria det = await detalle;
            int i = 0;
            foreach (var d in det.Detalle_Vencimiento_Cuotas_Mora_Diaria) {
                string codigo = "";
                codigo = d.Datos_Generales.Codigo_Credito;
                Credito cr = _context.Credito.AsNoTracking().Single(a => a.Codigo_Credito == codigo);
                bool existe = creditos.Any(a=>a.Id_Persona == cr.Id_Persona);
                if (existe) {
                    i++;
             }
            }

            return i;
        }
        private int Clientes_Inactivos(Cartera cartera)
        {
            var personas_inactivas = (from a in _context.Personas
                                      join p in _context.Credito
                                          on a.Id_Persona equals p.Id_Persona
                                      where p.Id_Cartera == cartera.Id_Cartera && p.Cancelado == true
                                      select new
                                      {
                                          a
                                      }).Distinct().Count();
            return personas_inactivas;
        }
        private int Clientes_Vencidos(Cartera cartera, DateTime fecha)
        {
            var personas_inactivas = (from a in _context.Personas
                                      join p in _context.Credito
                                          on a.Id_Persona equals p.Id_Persona
                                      where p.Id_Cartera == cartera.Id_Cartera && !p.Cancelado && p.Vencido_En <= fecha
                                      select new
                                      {
                                          p
                                      }).Distinct().Count();
            return personas_inactivas;
        }
        //nuevo orden arreglando
        private decimal Valor_Cartera(List<Credito> creditos, DateTime fecha_inicio, DateTime fecha_final)
        {
            try
            {
                decimal monto = 0.00M;
                foreach (Credito c in creditos)
                {
                    if (c.Transacciones_Creditos.Any(a => a.FechaPago <= fecha_final && !a.Borrado_Logico))
                        monto += Saldo_Capital_Final(c, fecha_inicio, fecha_final);
                    else
                    {
                        if (c.Liquidado_En <= fecha_final && !c.Cancelado)
                            monto += c.Monto_pago;
                    }
                }

                return monto;
            }
            catch (Exception ex)
            {
                return 0.00M;
            }
        }

        private decimal Capital_Recuperado(List<Credito> creditos, DateTime fecha_inicio, DateTime fecha_final)
        {
            try
            {
                decimal monto = 0.00M;
                foreach (Credito c in creditos)
                {
                    if (c.Transacciones_Creditos.Any(a => a.FechaPago >= fecha_inicio && a.FechaPago <= fecha_final && !a.Borrado_Logico))
                        monto += c.Transacciones_Creditos.Where(a => a.FechaPago >= fecha_inicio && a.FechaPago <= fecha_final && !a.Borrado_Logico).Sum(s => s.Abono_capital);
                    
                }
                return monto;
            }
            catch (Exception ex)
            {
                return 0.00M;
            }
        }

        private decimal Saldo_Capital_Final(Credito c, DateTime fecha_inicio, DateTime fecha_final, bool es_mora = false, int dias = 0, bool fuera_rango = false)
        {
            Func<Transaccion_Credito, bool> filtro_transaccion = new Func<Transaccion_Credito, bool>(a => a.FechaPago <= fecha_final && !a.Borrado_Logico);
            if (es_mora)
                filtro_transaccion = new Func<Transaccion_Credito, bool>(a => a.FechaPago >= fecha_inicio && a.FechaPago <= fecha_final && !a.Borrado_Logico && a.Saldo_Capital > 0 /*&& a.Dias > dias*/);
            
            int id_transaccion_maxima = c.Transacciones_Creditos
                .Where(filtro_transaccion)
                .Max(d => d.Id_Transaccion_Credito);
            Transaccion_Credito transaccionesCreditos = c.Transacciones_Creditos
                .Single(a => a.Id_Transaccion_Credito == id_transaccion_maxima);
            if (es_mora)
            {
                Transaccion_Credito transaccion_amortizacion = AmortizacionActual(
                    transaccionesCreditos.Id_Credito,
                    transaccionesCreditos.FechaPago);
                if (transaccion_amortizacion != null && transaccionesCreditos.Valor_Cuota < transaccion_amortizacion.Valor_Cuota)
                    return transaccionesCreditos.Saldo_Capital;
                else
                    return 0.00M;
            }
            else
                return transaccionesCreditos.Saldo_Capital;
        }

        private decimal Colectas(List<Credito> creditos, DateTime fecha_inicio, DateTime fecha_final)
        {

            decimal colecta = 0.00M;
            foreach (Credito c in creditos)
                colecta += c.Transacciones_Creditos
                    .Where(a => a.FechaPago >= fecha_inicio && a.FechaPago <= fecha_final && !a.Borrado_Logico)
                    .Sum(t => t.Valor_Cuota);
            return colecta;
        }
        private async Task<decimal> Indice_Mora(List<Credito> creditos, 
            DateTime fecha_inicio, DateTime fecha_final)
        {
           
            var detalle = Llenar_Reporte_MoraOVenciendo(creditos, fecha_inicio, fecha_final, true,true);
            Reporte_Vencimiento_Cuotas_Mora_Diaria det = await detalle;
            decimal monto = det.Detalle_Vencimiento_Cuotas_Mora_Diaria.Sum(a=>a.Datos_Generales.Saldo);
        
            return monto;
        }
        private async Task<int> Creditos_Mora(List<Credito> creditos,
          DateTime fecha_inicio, DateTime fecha_final)
        {

            var detalle = Llenar_Reporte_MoraOVenciendo(creditos, fecha_inicio, fecha_final, true, true);
            Reporte_Vencimiento_Cuotas_Mora_Diaria det = await detalle;

            return det.Detalle_Vencimiento_Cuotas_Mora_Diaria.Count();
        }
        //termina nuevo orden

        private decimal Suma_Abono_Capital(List<Credito> creditos)
        {
            return creditos.SelectMany(f => f.Transacciones_Creditos.Where(filtrar_transacciones)).Sum(t => t.Abono_capital);
        }

        private decimal Suma_Capital_Mora(List<Credito> creditos, DateTime fecha_inicio, DateTime fecha_final)
        {
            try
            {
                decimal monto = 0.00M;
             //   CalculosYProcesoCredito calculo = new CalculosYProcesoCredito();
                foreach (Credito c in creditos.Where(d => !d.Cancelado))
                {
                  
                    Transaccion_Credito transaccion = AmortizacionActualProc(c.Id_Credito, fecha_final, null);
                    var pago_dia = transaccion.Valor_Al_Dia;
                    //if (c.Transacciones_Creditos.Any(a => a.FechaPago >= fecha_inicio && a.FechaPago <= fecha_final && !a.Borrado_Logico && a.Saldo_Capital > 0 /*&& a.Dias > intervalo*/))
                    //{
                        monto += pago_dia;
                    //}
                }
                return monto;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }


        private decimal Saldo_Capital_Total(List<Credito> creditos, DateTime fecha_final, bool vencidos = false)
        {
            try
            {
                decimal monto = 0.00M;
                foreach (Credito c in creditos)
                {
                    if (c.Transacciones_Creditos.Any(a => a.FechaPago <= fecha_final) & c.Vencido_En <= fecha_final.Date)
                    {
                        //if (vencidos)
                        //{
                            Transaccion_Credito tc = c.Transacciones_Creditos.OrderByDescending(m => m.Id_Transaccion_Credito).Last();
                           // Transaccion_Credito tc_Modificado = AmortizacionActual(tc.Id_Credito, fecha_final);
                           // if (tc_Modificado != null)
                                monto += tc.Valor_Cuota;
                        //}
                        //else
                        //{
                        //    int id_transaccion_maxima = c.Transacciones_Creditos
                        //        .Where(a => a.FechaPago <= fecha_final).Max(d => d.Id_Transaccion_Credito);
                        //    monto += c.Transacciones_Creditos.Single(a => a.Id_Transaccion_Credito == id_transaccion_maxima).Saldo_Capital;
                        //}
                    }
                }

                return monto;
            }
            catch (Exception ex)
            {
                return 0.00M;
            }
        }

        private Transaccion_Credito AmortizacionActual(int id_credito, DateTime fecha, decimal? valor_cuota = null, bool para_refinanciable = false)
        {
            try
            {
                Transaccion_Credito transaccion = AmortizacionActualProc(id_credito, fecha, valor_cuota, para_refinanciable);
                return transaccion;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Reporte_Generales_Rendimiento(int id_agencia, DateTime fecha_inicio, DateTime fecha_final)
        {
            try
            {
                List<Agencia_Rendimiento> reportesAgencias = new List<Agencia_Rendimiento>();
                List<Rendimiento_Vista> generales = new List<Rendimiento_Vista>();
                string Id_Usuario = _userManager.GetUserId(HttpContext.User);
                if (id_agencia == 0) //significa que quiere todas las agencias
                {
                    List<Agencia> agencias = _context.Agencia.ToList();
                    foreach (Agencia agencia in agencias)
                    {
                        if (await AgenciaCreditos(agencia.Id_Agencia))
                        {
                            generales =
                                await Rendimiento(fecha_inicio, fecha_final, agencia.Id_Agencia);
                            reportesAgencias.Add(new Agencia_Rendimiento
                            {
                                Agencia = agencia.Nombre,
                                Generado_Por = Id_Usuario,
                                Fecha = DateTime.Now.Date,
                                Rendimientos = generales
                            });
                        }
                    }

                }
                else
                {
                    Agencia agencia = _context.Agencia.SingleOrDefault(c => c.Id_Agencia == id_agencia);
                    generales =
                        await Rendimiento(fecha_inicio, fecha_final, agencia.Id_Agencia);
                    reportesAgencias.Add(new Agencia_Rendimiento
                    {
                        Agencia = agencia.Nombre,
                        Generado_Por = Id_Usuario,
                        Fecha = DateTime.Now.Date,
                        Rendimientos = generales
                    });
                }
                return Ok(reportesAgencias);
            }
            catch (Exception ex)
            {
                return NoContent();
            }
        }

        //para sacar reporte de rendimiento
        /// <summary>
        /// retorna el rendimiento de una agencia específica
        /// </summary>
        /// <param name="fecha_inicio"></param>
        /// <param name="fecha_final"></param>
        /// <param name="id_agencia"></param>
        /// <returns></returns>

        private async Task<List<Rendimiento_Vista>> Rendimiento(DateTime fecha_inicio, DateTime fecha_final, int id_agencia)
        {
            List<Usuario_Agencia> usuarios = await _context
                .Usuario_Agencia
                .AsNoTracking()
                .Where(a => a.Id_Agencia == id_agencia)
                .ToListAsync();
            List<string> ids_usuario = _context.Agencia
                .AsNoTracking()
                .Include(f => f.Usuario_Agencias)
                .FirstOrDefault(t => t.Id_Agencia == id_agencia)
                ?.Usuario_Agencias
                .Select(h => h.Id_Usuario).ToList();
          
            List<Rendimiento_Vista> rendimientos = new List<Rendimiento_Vista>();

            foreach (string id in ids_usuario)
            {
                Cartera carteras = new Cartera();
                if (_context.Cartera.Any(f => f.Id_Usuario == id))
                {
                    Cartera carteras_por_usuario = _context.Cartera.AsNoTracking()
                      .Include("Creditos")
                      .Include("Creditos.Transacciones_Creditos")
                      .Include("Creditos.Linea_Credito_Historial.Deducciones_Historial")
                      .Include("Creditos.Periodo")
                      .SingleOrDefault(j => j.Id_Usuario == id && !j.Excluido);
                    if (carteras_por_usuario != null && carteras_por_usuario.Creditos.Count > 0)
                    {
                        carteras=carteras_por_usuario;
                    }
                        rendimientos.Add(new Rendimiento_Vista
                        {
                            Cartera = id,
                            Cartera_Refinanciada = Cartera_Refinanciada(fecha_inicio, fecha_final, carteras),
                            Retorno_Colecta = Retorno_Colecta(fecha_inicio, fecha_final, carteras) 
                            + Cartera_Refinanciada(fecha_inicio, fecha_final, carteras), 
                            Recuperacion_Capital = Recuperacion_Capital(fecha_inicio, fecha_final, carteras),
                            Cobertura_Deuda = Cobertura_Deuda(fecha_inicio, fecha_final, carteras)/1.13M,
                            Recargos = Recargos(fecha_inicio, fecha_final, carteras) /1.13M,
                            Intereses = Intereses(fecha_inicio, fecha_final, carteras) / 1.13M,

                            Ingresos_Percibidos = Cobertura_Deuda(fecha_inicio, fecha_final, carteras)
                            + Recargos(fecha_inicio, fecha_final, carteras)+ Intereses(fecha_inicio, fecha_final, carteras),

                            IVA = Math.Round((Cobertura_Deuda(fecha_inicio, fecha_final, carteras)
                            + Recargos(fecha_inicio, fecha_final, carteras)+ 
                            ComisionXEstructuracion(fecha_inicio, fecha_final, carteras) * 0.13M), 2),
                            Reinversion_Colocacion = Reinversion_Colocacion(fecha_inicio, fecha_final, carteras),
                            Efectivo_No_Rotativo = 0.00M,
                            Comision_Estructural = ComisionXEstructuracion(fecha_inicio, fecha_final, carteras),
                            Ingresos_Devengados = Math.Round((Cobertura_Deuda(fecha_inicio, fecha_final, carteras)
                            + Recargos(fecha_inicio, fecha_final, carteras) +
                            Intereses(fecha_inicio, fecha_final, carteras)+
                            ComisionXEstructuracion(fecha_inicio, fecha_final, carteras) ) / 1.13M, 2)
                        });
                   
                }
                    
            }

            return rendimientos;
        }

        private decimal Cartera_Refinanciada(DateTime fecha_inicio, DateTime fecha_final,Cartera cartera)
        {
            try
            {
                var personas = (from cr in _context.Credito
                                join lh in _context.Linea_Credito_Historial on cr.Id_Credito equals lh.Id_Credito
                                join dh in _context.Deduccion_Historial on lh.Id_Linea_Credito_Historial
                                equals dh.Id_Linea_Credito_Historial
                                where cr.Id_Cartera == cartera.Id_Cartera && !cr.Cancelado && dh.Nombre == "Refinanciamiento"
                                && cr.Id_Comportamiento_Credito == 4 && cr.Liquidado_En >= fecha_inicio &&
                                cr.Liquidado_En <= fecha_final
                                select new
                                {
                                    dh.Valor
                                });
                decimal refinanciamientos = personas.Sum(a => a.Valor);
                return refinanciamientos;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }
        private decimal Retorno_Colecta(DateTime fecha_inicio, DateTime fecha_final, Cartera cartera)
        {
            try
            {
                var personas = (from cr in _context.Credito
                                join tc in _context.Transaccion_Credito on cr.Id_Credito equals tc.Id_Credito
                                where cr.Id_Cartera == cartera.Id_Cartera && !cr.Cancelado
                                && tc.FechaPago >= fecha_inicio && tc.FechaPago <= fecha_final && !tc.Borrado_Logico
                                select new
                                {
                                   tc.Valor_Cuota
                                });
                decimal retorno = personas.Sum(a => a.Valor_Cuota);
                return retorno;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }
        private decimal Recuperacion_Capital(DateTime fecha_inicio, DateTime fecha_final, Cartera cartera)
        {
            try
            {
                var personas = (from cr in _context.Credito
                                join tc in _context.Transaccion_Credito on cr.Id_Credito equals tc.Id_Credito
                                where cr.Id_Cartera == cartera.Id_Cartera && !cr.Cancelado
                                && tc.FechaPago >= fecha_inicio && tc.FechaPago <= fecha_final && !tc.Borrado_Logico
                                select new
                                {
                                    tc.Abono_capital
                                });
                decimal retorno = personas.Sum(a => a.Abono_capital);
                return retorno;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }
        private decimal Cobertura_Deuda(DateTime fecha_inicio, DateTime fecha_final, Cartera cartera)
        {
            try
            {
                var personas = (from cr in _context.Credito
                                join tc in _context.Transaccion_Credito on cr.Id_Credito equals tc.Id_Credito
                                where cr.Id_Cartera == cartera.Id_Cartera && !cr.Cancelado
                                && tc.FechaPago >= fecha_inicio && tc.FechaPago <= fecha_final && !tc.Borrado_Logico
                                select new
                                {
                                    tc.Interes_Comision
                                });
                decimal retorno = personas.Sum(a => a.Interes_Comision);
                return retorno;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }
        private decimal Recargos(DateTime fecha_inicio, DateTime fecha_final, Cartera cartera)
        {
            try
            {
                var personas = (from cr in _context.Credito
                                join tc in _context.Transaccion_Credito on cr.Id_Credito equals tc.Id_Credito
                                where cr.Id_Cartera == cartera.Id_Cartera && !cr.Cancelado
                                && tc.FechaPago >= fecha_inicio && tc.FechaPago <= fecha_final && !tc.Borrado_Logico
                                select new
                                {
                                    tc.Recargo
                                });
                decimal retorno = personas.Sum(a => a.Recargo);
                return retorno;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }
        private decimal Intereses(DateTime fecha_inicio, DateTime fecha_final, Cartera cartera)
        {
            try
            {
                var personas = (from cr in _context.Credito
                                join tc in _context.Transaccion_Credito on cr.Id_Credito equals tc.Id_Credito
                                where cr.Id_Cartera == cartera.Id_Cartera && !cr.Cancelado
                                && tc.FechaPago >= fecha_inicio && tc.FechaPago <= fecha_final && !tc.Borrado_Logico
                                select new
                                {
                                    tc.Interes
                                });
                decimal retorno = personas.Sum(a => a.Interes);
                return retorno;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }
        private decimal ComisionXEstructuracion(DateTime fecha_inicio, DateTime fecha_final, Cartera cartera)
        {
            try
            {
                var personas = (from cr in _context.Credito
                                join lh in _context.Linea_Credito_Historial on cr.Id_Credito equals lh.Id_Credito
                                join dh in _context.Deduccion_Historial on lh.Id_Linea_Credito_Historial
                                equals dh.Id_Linea_Credito_Historial
                                join ts in _context.Transaccion_Simple on dh.Id_Deduccion_Historial equals ts.Id_Deduccion
                                where cr.Id_Cartera == cartera.Id_Cartera && !cr.Cancelado && dh.Nombre == "HONORARIOS Y GASTOS"
                                && cr.Id_Comportamiento_Credito == 4 && cr.Liquidado_En >= fecha_inicio &&
                                cr.Liquidado_En <= fecha_final
                                select new
                                {
                                    ts.Monto
                                });
                decimal refinanciamientos = personas.Sum(a => a.Monto);
                return refinanciamientos;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }
        private decimal Reinversion_Colocacion(DateTime fecha_inicio, DateTime fecha_final, Cartera cartera)
        {
            try
            {
                var personas = (from cr in _context.Credito
                              
                                where cr.Id_Cartera == cartera.Id_Cartera && !cr.Cancelado 
                                && cr.Liquidado_En >= fecha_inicio &&
                                cr.Liquidado_En <= fecha_final && !cr.Revertido
                                select new
                                {
                                    cr.Monto_pago
                                });
                decimal refinanciamientos = personas.Sum(a => a.Monto_pago);
                return refinanciamientos;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }
        /// <summary>
        /// segmentación de cliente
        /// para el covid
        /// </summary>
        /// <param name="inicio"></param>
        /// <param name="fin"></param>
        /// <returns></returns>
        [Authorize(Roles = "Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Segmentacion_Cliente(DateTime inicio, DateTime fin)
        {
            List<Segmentacion_Cliente> segmentaciones = (from t in _context
                 .Transaccion_Credito
                 .AsNoTracking()
                 .Where(c => c.FechaPago.Date >= inicio.Date && c.FechaPago <= fin.Date).ToList()
                                                         join c in _context.Credito on t.Id_Credito equals c.Id_Credito
                                                         join p in _context.Personas on c.Id_Persona equals p.Id_Persona
                                                         join ca in _context.Cartera on c.Id_Cartera equals ca.Id_Cartera
                                                         join ua in _context.Usuario_Agencia on ca.Id_Usuario equals ua.Id_Usuario
                                                         join a in _context.Agencia on ua.Id_Agencia equals a.Id_Agencia
                                                         select new Segmentacion_Cliente
                                                         {
                                                             Id_Credito = c.Id_Credito,
                                                             Agencia = a.Nombre,
                                                             Cartera = ca.Id_Usuario,
                                                             Cliente = p.NombreCompleto,
                                                             Dui = p.Dui,
                                                             Codigo_Credito = c.Codigo_Credito,
                                                             Cuota = c.Cuota_Total,
                                                             Abono = t.Valor_Cuota,
                                                             Fecha_Abono = t.FechaPago,
                                                             Segmentacion = Enum.GetName(typeof(Segmentacion_Moral), c.segmentacion_Moral),
                                                         }).ToList();

            return Ok(await Task.Run(() => segmentaciones));
        }

        /*trabajar la mora y pago al día*/
        /// <summary>
        /// reporte de mora filtrado
        /// </summary>
        /// <param name="id_cartera"></param>
        /// <param name="inicio"></param>
        /// /// <param name="final"></param>
        /// <param name="es_mora"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Reporte_Mora_Cuotas_Vencidas(
            int id_cartera,
            DateTime inicio,
            DateTime final,
            bool es_mora = false)
        {
            var creditos = _context
                .Credito
                .Include(u => u.Periodo)
                .Include(f=>f.Linea_Credito)
                .Include(a => a.Persona)
                .Include(c=>c.Cartera)
                .Include(q=>q.Transacciones_Creditos)
                .AsNoTracking()
                .Where(
                    v => v.Id_Cartera == id_cartera && 
                    !v.Cancelado && 
                    v.Liquidado &&
                    v.Transacciones_Creditos.Any(d=>!d.Borrado_Logico)
                ).ToList();
            var detalle = Llenar_Reporte_MoraOVenciendo(creditos, inicio, final, es_mora);
            return Ok(await Task.Run(() => detalle));
        }
        /*trabajar la mora y pago al día*/
        /// <summary>
        /// reporte de mora filtrado
        /// </summary>
        /// <param name="id_cartera"></param>
        /// <param name="inicio"></param>
        /// /// <param name="final"></param>
        /// <param name="es_mora"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Reporte_Mora_Credito_Diario(
            int id_cartera,
            DateTime inicio,
            DateTime final,
            bool es_mora = false)
        {
            var creditos = _context
                .Credito
                .Include(u => u.Periodo)
                .Include(f => f.Linea_Credito)
                .Include(a => a.Persona)
                .Include(c => c.Cartera)
                .Include(q => q.Transacciones_Creditos)
                .AsNoTracking()
                .Where(
                    v => v.Id_Cartera == id_cartera &&
                    !v.Cancelado &&
                    v.Liquidado &&
                    v.Id_Linea ==17 &&
                    v.Transacciones_Creditos.Any(d => !d.Borrado_Logico)
                ).ToList();/*todos los créditos para buscar*/
            if (creditos.Count > 0) {
                var detalle = Llenar_Reporte_MoraOVenciendo(creditos, inicio, final, es_mora);
                return Ok(await Task.Run(() => detalle));
            }
            else
            {
                return Ok();
            }
          
        }
        private List<DateTime> Rango_Fecha(DateTime inicio, DateTime final)
        {
            var diferencia = final - inicio;
            int cuenta = diferencia.Days;
            List<DateTime> fechas = new List<DateTime>();
            for (int j = 0; j <= cuenta; j++)
            {
                var fecha = inicio.AddDays(j);
                fechas.Add(fecha);
            }
            return fechas;
        }

        private Detalle_Vencimiento_Cuotas_Mora_Diaria Armar_Reporte(
            Credito credito,
            Transaccion_Credito aldia,
            Transaccion_Credito ultima,
            decimal valor_dia = 0.00M,
            decimal saldo_ahorro = 0.00M,
            decimal abono_capital = 0.00M,
            decimal cartera = 0.00M,
            bool generales = false,
            int dias_mora = 0)
        {
            Datos_Generales datos = new Datos_Generales();
            if (!generales)
            {

                /*datos del cliente*/
                
                datos.Nombre_Cliente = credito.Persona.NombreCompleto;
                datos.Telefono = credito.Persona.Telefono;

                /*datos del crédito*/
             
            }
            datos.Codigo_Credito = credito.Codigo_Credito;
            datos.Fecha_Pago = aldia.Fecha_Amortizacion;
            datos.Cuota = credito.Cuota_Total;
            datos.Pago_Al_Dia = valor_dia;
           

            datos.Saldo = aldia.Saldo_Capital;
            datos.Vencimiento = credito.Vencido_En;


            Mora datos_mora = new Mora
            {
               Dias = dias_mora, //supongo se refiere a los días acumulados en mora
               CapitalMora = aldia.Capital_Mora,
               InteresPendiente = aldia.Interes_Pendiente,
               InteresCorriente = aldia.Interes_Diario,
               Multa = aldia.Recargo
               
            };




            Vencimiento_Cuotas vencimiento = new Vencimiento_Cuotas
            {
                Monto = credito.Monto_pago,
                Saldo_Cuenta_Ahorro = saldo_ahorro, //falta resolver
                Abono_Capital = abono_capital,
                Porcentaje_Saldo_Pendiente = 0.00M
            };

            return new Detalle_Vencimiento_Cuotas_Mora_Diaria
            {
                Datos_Generales = datos,
                Mora_Asesor = datos_mora,
                Vencimiento_Cuotas = vencimiento
            };
        }

        private async Task<Reporte_Vencimiento_Cuotas_Mora_Diaria> Llenar_Reporte_MoraOVenciendo(
            List<Credito> creditos,
            DateTime inicio,
            DateTime final,
            bool es_mora,
            bool generales = false)
        {
            try
            {
                List<Detalle_Vencimiento_Cuotas_Mora_Diaria> detalle = new List<Detalle_Vencimiento_Cuotas_Mora_Diaria>();
                Encabezado Encabezado = new Encabezado();
                string Id_Usuario = _userManager.GetUserId(HttpContext.User);
                Agencia agencia = _context
                    .Usuario_Agencia
                    .AsNoTracking()
                    .Include(d => d.Agencia)
                    .SingleOrDefault(g => g.Id_Usuario == Id_Usuario)
                    ?.Agencia;
                string dia = DateTime.Now.ToString("dd/MM/yyyy");
                string hora = DateTime.Now.ToString("h:mm:ss tt");

                decimal valor_cartera = 0.00M;

                var nombre_asesor = creditos.FirstOrDefault().Cartera.Nombre;

                foreach (var credito in creditos)
                {
                    Transaccion_Credito ultima_transaccion = new Transaccion_Credito();
                    decimal suma_intereses = 0.00M;
                    decimal suma_interes_comision = 0.00M;
                    bool tiene_transacciones = credito.Transacciones_Creditos.Any();
                    if (tiene_transacciones)
                    {
                        var transacciones = credito.Transacciones_Creditos.OrderBy(d => d.Id_Transaccion_Credito).ToList();
                        ultima_transaccion = transacciones.Last();
                        suma_intereses = transacciones.Sum(i=>i.Interes);
                        suma_interes_comision = transacciones.Sum(v=>v.Interes_Comision);
                        valor_cartera += ultima_transaccion.Saldo_Capital;
                    }
                    Transaccion_Credito aldia = AmortizacionActualProc(credito.Id_Credito, final.Date,para_reporte:true);

                    if (es_mora)
                    {
                        if (tiene_transacciones)
                        {
                            var valorAlDia = aldia.Valor_Al_Dia;
                            TimeSpan diferencia = aldia.Pagos_Acumulado==0? aldia.FechaPago.Date - aldia.Fecha_Amortizacion.Date: aldia.FechaPago.Date-ultima_transaccion.FechaPago.Date;
                            int diasDiferencias = diferencia.Days;
                            int dias = await ObtenerDiasMora(credito.Id_Credito, final.Date);
                            
                            if (aldia.Valor_Al_Dia>0.00M && dias>0)
                                detalle.Add(Armar_Reporte(credito, aldia, ultima_transaccion,valorAlDia,cartera:valor_cartera,generales:generales, dias_mora:dias));
                        }
                    }
                    else
                    {
                        /*pagos vencidos corrientes se utiliza rango*/
                        var rango_fechas = Rango_Fecha(inicio, final);
                        List<Amortizacion> am =await _context
                             .Amortizacion
                             .AsNoTracking()
                             .Where(d => d.Id_Credito == credito.Id_Credito).ToListAsync();   //Amortizacion(credito.Id_Linea, credito.Monto_pago, credito.Id_Periodo, credito.Plazo,credito.Id_Credito).Amortizacion;
                        foreach (var fecha in rango_fechas)
                        {
                            foreach (var amortizacion in am)
                            {
                                if (amortizacion.FechaPago.Date == fecha.Date)
                                {
                                    var valorAlDia = aldia.Valor_Al_Dia;
                                    Transaccion_Credito tr = new Transaccion_Credito();
                                    tr.Fecha_Amortizacion = amortizacion.FechaPago;
                                    detalle.Add(Armar_Reporte(credito, tr, ultima_transaccion,valorAlDia,
                                        await Obtener_Saldo_Ahorro(credito.Id_Persona), amortizacion.Abono_capital));
                                }
                            }
                        }
                    }
                }
                Reporte_Vencimiento_Cuotas_Mora_Diaria vencimiento = new Reporte_Vencimiento_Cuotas_Mora_Diaria();
                Encabezado.agencia = agencia;
                Encabezado.dia_ejecucion = dia;
                Encabezado.hora_ejecucion = hora;
                Encabezado.nombre_asesor = nombre_asesor;
                vencimiento.Encabezado = Encabezado;
                vencimiento.Detalle_Vencimiento_Cuotas_Mora_Diaria = detalle
                    .OrderBy(v=>v.Mora_Asesor.Dias).ToList();
                return vencimiento;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }
        private async Task<int> ObtenerDiasMora(int id_credito, DateTime fecha)
        {
            List<Amortizacion> amortizacion_vencidas = new List<Amortizacion>();
            Amortizacion ultima_amortizacion_cubierta = null;
            DateTime fecha_para_Vencido = fecha.AddDays(-1);
            decimal intereses_pagados = 0.00M;
            decimal ultimo_saldo = 0.00M;
            int pagos_acumulados = 0;
            bool completado = false;
            decimal intereses_vencidos = amortizacion_vencidas.Count() > 0 ? amortizacion_vencidas.Sum(v => v.Interes) : 0.00M;
            if (_context.Amortizacion.AsNoTracking().Any(d => d.FechaPago.Date <= fecha_para_Vencido.Date && d.Id_Credito == id_credito))
            {
                amortizacion_vencidas = _context
                    .Amortizacion
                    .AsNoTracking()
                    .Where(d => d.FechaPago.Date <= fecha_para_Vencido.Date && d.Id_Credito == id_credito)
                    .OrderBy(d => d.No).ToList();
            }
            var tcs = _context.Transaccion_Credito
               .Where(f => f.Id_Credito == id_credito && !f.Borrado_Logico).OrderBy(d => d.Id_Transaccion_Credito).ToList();
            ultimo_saldo = tcs.Min(u => u.Saldo_Capital);
            decimal ultimo_saldo_vencido = 0.00M;
            ultimo_saldo_vencido = amortizacion_vencidas.Count() > 0 ? amortizacion_vencidas.Min(d => d.Saldo_Capital) : 0.00M;
            completado = ultimo_saldo_vencido == 0.00M;
            completado = (ultimo_saldo <= ultimo_saldo_vencido);
            if (!completado&& amortizacion_vencidas.Count() > 0)//encontrar las cuotas vencidas sin pagar
            {
                var amortizaciones_desc = amortizacion_vencidas.OrderByDescending(y => y.No);
                var limite_amortizaciones = amortizaciones_desc.Where(c => c.Interes <= intereses_pagados);
                foreach (var am in amortizaciones_desc)
                {
                    decimal suma_intereses = amortizaciones_desc.Where(n => n.No <= am.No).Sum(i => i.Interes);
                    decimal suma_comision = amortizaciones_desc.Where(n => n.No <= am.No).Sum(i => i.Comision);
                    if (ultimo_saldo <= am.Saldo_Capital)
                    {
                        ultima_amortizacion_cubierta = amortizaciones_desc.Single(c => c.No == am.No);
                        intereses_vencidos = suma_intereses;
                        break;
                    }

                }
            }
            else
            {
                if (amortizacion_vencidas.Count > 0)
                    ultima_amortizacion_cubierta = amortizacion_vencidas.Last();
            }
            Amortizacion ultima_amortizacion_grabada = amortizacion_vencidas.Count() > 0 ? amortizacion_vencidas.Last() : null;

            int diferencia_amortizada = 0;

            if (ultima_amortizacion_grabada != null)
            {
                if (ultima_amortizacion_cubierta != null)
                    diferencia_amortizada = ultima_amortizacion_grabada.No - ultima_amortizacion_cubierta.No;
                else
                    diferencia_amortizada = ultima_amortizacion_grabada.No - 0;
            }
            pagos_acumulados = diferencia_amortizada > 0 ? diferencia_amortizada : 0;
            return pagos_acumulados;
        }
        /// <summary>
        /// solo para ejecutar una vez
        /// </summary>
        [Authorize(Roles = "Asesor,Administrador,Gerente,Jefe", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Movimiento_Contabilidad(DateTime fecha,int id_agencia = 0)
        {
            try {
                List<Movimientos_Contabilidad> rama = new List<Movimientos_Contabilidad>();
                var movimientos = _context.Asiento_Contable
                            .Include(d => d.Detalles_Asientos_Contables)
                            .Where(d => d.Fecha.Date == fecha.Date)
                            .ToList();
                
                foreach (var mov in movimientos)
                {
                    string concepto_partida = "";
                    string codigo = "";
                    decimal total_cargo = 0.00M;
                    decimal total_abono = 0.00M;
                    int id_asiento_contable = 0;
                    List<Cargo_Abono_Contabilidad> detalle = new List<Cargo_Abono_Contabilidad>();
                    Totales_Contabilidad totales = new Totales_Contabilidad();
                    bool tiene_movimientos = _context.Detalle_Asiento_Contable
                           .Any(d => d.Id_Agencia == id_agencia && d.Id_Asiento_Contable == mov.Id_Asiento_Contable);
                    if (tiene_movimientos) {
                        var detalle_movimientos = _context.Detalle_Asiento_Contable
                           .Where(d => d.Id_Agencia == id_agencia && d.Id_Asiento_Contable == mov.Id_Asiento_Contable)
                           .ToList();

                        foreach (var det in detalle_movimientos.OrderBy(a=>a.Abono)) {
                            total_cargo += det.Cargo;
                            total_abono += det.Abono;
                            totales.Total_Cargo = total_cargo;
                            totales.Total_Abono = total_abono;
                            detalle.Add(new Cargo_Abono_Contabilidad
                            {
                                Cuenta_Contable = det.Cuenta_Contable,
                                Concepto= det.Concepto,
                                Cargo = det.Cargo,
                                Abono = det.Abono
                            });
                          
                        }
                        Transaccion_Caja tc = _context.Transaccion_Caja.AsNoTracking()
                            .Single(a=>a.Id_Transaccion_Caja == mov.Id_Transaccion_Caja);
                        if (tc.Id_Transaccion_Credito > 0) {
                            Transaccion_Credito tr_c = _context.Transaccion_Credito
                                .AsNoTracking().Single(a=>a.Id_Transaccion_Credito == tc.Id_Transaccion_Credito);
                            Credito cr = _context.Credito.AsNoTracking().Single(a=>a.Id_Credito == tr_c.Id_Credito);
                            codigo = cr.Codigo_Credito;
                        }
                        if (tc.Id_Transaccion_Ahorro > 0)
                        {
                            Transaccion_Ahorro tr_a = _context.Transaccion_Ahorro
                                .AsNoTracking().Single(a => a.Id_Transaccion_Ahorro == tc.Id_Transaccion_Ahorro);
                            Ahorro ah = _context.Ahorro.AsNoTracking().Single(a => a.Id_Ahorro == tr_a.Id_Ahorro);
                            codigo =ah.No_Ahorro;
                        }
                        id_asiento_contable = mov.Id_Asiento_Contable;
                        concepto_partida = mov.Concepto;
                        rama.Add(new Movimientos_Contabilidad
                        {
                            id_asiento_contable = id_asiento_contable,
                            Codigo = codigo,
                            Concepto = concepto_partida,
                            Distribucion= detalle,
                            totales = totales
                        }); 
                    }

                }
                return Ok( rama);
                }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace);
            }


        }

        
        [Authorize(Roles = "Administrador,Gerente,Jefe", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<List<Cartera_Asesor>> Cartera_Asesor(int id_cartera = 0)
        {
            try
            {
                
                string userId = _userManager.GetUserId(HttpContext.User);
                List<Cartera_Asesor> datos = new List<Cartera_Asesor>();
                List<Credito> pe = _context.Credito
                     .Include(a => a.Persona)
                     .Include(b=>b.Transacciones_Creditos)
                     .Where(a => a.Id_Cartera == id_cartera && !a.Cancelado).ToList();
                Cartera asesor = _context.Cartera.AsNoTracking().Single(a=>a.Id_Cartera == id_cartera);
                foreach (var d in pe) {
                    decimal saldo = 0.00M;
                    bool tiene_transacciones = _context.Transaccion_Credito.Any(a=>a.Id_Credito == d.Id_Credito);
                    if (tiene_transacciones)
                    {
                        List<Transaccion_Credito> tr = _context.Transaccion_Credito
                       .AsNoTracking().Where(a => a.Id_Credito == d.Id_Credito).ToList();
                        Transaccion_Credito tc = tr.Last();
                        saldo = tc.Saldo_Capital;
                        datos.Add(new Cartera_Asesor
                        {
                            Codigo = d.Codigo_Credito,
                            Nombre_Persona = d.Persona.NombreCompleto,
                            Direccion_Persona = d.Persona.Domicilio,
                            Telefono_Persona = d.Persona.Telefono,
                            Saldo_Credito = saldo,
                            Monto_Credito = d.Monto_pago,
                            Cuota_Credito = d.Cuota_Total,
                            Fecha_Apertura = d.Liquidado_En,
                            Fecha_Vencimiento = d.Vencido_En,
                            Nombre_Asesor = asesor.Nombre +" "+ asesor.Colector
                            

                        });
                    }
                    else {

                        datos.Add(new Cartera_Asesor
                        {
                            Codigo = d.Codigo_Credito,
                            Nombre_Persona = d.Persona.NombreCompleto,
                            Direccion_Persona = d.Persona.Domicilio,
                            Telefono_Persona = d.Persona.Telefono,
                            Saldo_Credito = 0.00M,
                            Monto_Credito = d.Monto_pago,
                            Cuota_Credito = d.Cuota_Total,
                            Fecha_Apertura = d.Liquidado_En,
                            Fecha_Vencimiento = d.Vencido_En,
                            Solicitado_Por = userId + " " + DateTime.Now

                        }) ;
                    }
                }
                return datos;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        [HttpGet()]
        public IActionResult ObtenerReporteDTE( DateTime? fechaInicio, DateTime? fechaFin,string tipo = "todos" )
         {
                try
                {
                    var query = _context.Bitacora
                        .Where(b => b.Tabla_Operacion == "DTE");

                    if (fechaInicio.HasValue)
                        query = query.Where(b => b.Fecha_Proceso >= fechaInicio.Value);

                    if (fechaFin.HasValue)
                        query = query.Where(b => b.Fecha_Proceso <= fechaFin.Value);

                    if (tipo == "exitos")
                        query = query.Where(b => b.Proceso.Contains("exitosa"));
                    else if (tipo == "errores")
                        query = query.Where(b => b.Proceso.Contains("error"));

                    var data = query
                        .OrderByDescending(b => b.Fecha_Proceso)
                        .Select(b => new
                        {
                            b.Fecha_Proceso,
                            b.Codigo,
                            b.Cliente,
                            b.Proceso,
                            b.Monto_Afectado,
                            b.Generado_Por

                        })
                        .ToList();

                    return Ok(data);
                }
                catch (Exception)
                {
                    return BadRequest("Error al obtener datos del reporte");
                }
         }
     }
}