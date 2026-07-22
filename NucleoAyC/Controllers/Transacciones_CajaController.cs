using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.EntityFrameworkCore;
using ModelNucleo.Model;
using ModelNucleo.Model.ModelView;
using NucleoAyC.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Design;
using ModelNucleo.CalculosYProceos;
using ModelNucleo.Model.Contabilidad;

namespace NucleoAyC.Controllers
{

    /// <summary>
    /// manejo de transacciones de caja
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = "Transacciones_Caja")]
    public class Transacciones_CajaController : Base_TransaccionController
    {

        /// <summary>
        /// constructor e injección de credito
        /// </summary>
        /// <param name="context"></param>
        /// /// <param name="userManager"></param>
        /// <param name="roleManager"></param>
        public Transacciones_CajaController(NegocioContexto context,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
            : base(context, userManager, roleManager)
        {
        }

        /// <summary>
        /// muestra las transacción de caja como boveda
        /// </summary>
        /// <param name="fecha">DateTime</param>
        /// <returns>List Transaccion_Caja</returns>

        [Authorize(Roles = "Asistente,Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Transacciones_Como_Boveda(DateTime? fecha)
        {
            return await Transacciones(fecha, true);
        }

        /// <summary>
        /// muestra las transacciones de caja
        /// ya sea la caja que también posee
        /// una bóveda o una caja a partir de
        /// su identificación
        /// </summary>
        /// <param name="fecha">DateTime</param>
        /// <param name="id_caja">int</param>
        /// <returns>>List Transaccion_Caja</returns>
        [Authorize(Roles = "Asistente,Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Transacciones_Como_Caja(DateTime? fecha, int id_caja = 0)
        {
            return await Transacciones(fecha, false, id_caja);
        }

        /// <summary>
        /// es un monitor de todos los movimientos de una caja
        /// para ir comprobando
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Monitor_Movimiento(DateTime fecha, int id_caja)
        {
            var transacciones =await _context
                .Transaccion_Caja
                .AsNoTracking()
                .Include(d => d.Tipo_Transaccion)
                .Where(t => t.Id_Caja == id_caja && t.Fecha.Date == fecha.Date).ToListAsync();

            if (transacciones == null)
                return BadRequest("No hay transacciones");

            List<Monitor_Movimientos> monitor = new List<Monitor_Movimientos>();
            foreach (var transaccion in transacciones)
            {
                string concepto = null;
                string cuenta = null;

                if (transaccion.Id_Transaccion_Ahorro > 0)
                {
                    var ahorro = _context
                        .Transaccion_Ahorro
                        .AsNoTracking()
                        .Include(c=>c.Tipo_Operacion_Ahorro)
                        .Single(f=>f.Id_Transaccion_Ahorro==transaccion.Id_Transaccion_Ahorro);
                    concepto = ahorro.Tipo_Operacion_Ahorro.Descripcion;
                    cuenta = _context.Ahorro.Single(m=>m.Id_Ahorro==ahorro.Id_Ahorro).No_Ahorro;
                }

                if (transaccion.Id_Transaccion_Credito > 0)
                {
                    var credito = _context
                        .Transaccion_Credito
                        .AsNoTracking()
                        .Include(c => c.Tipo_Operacion_Credito)
                        .Single(f => f.Id_Transaccion_Credito == transaccion.Id_Transaccion_Credito);
                    concepto = "Abono a préstamo"; //credito.Tipo_Operacion_Credito.Descripcion;
                    cuenta = _context.Credito.Single(k=>k.Id_Credito==credito.Id_Credito).Codigo_Credito;
                }

                if (transaccion.Id_Transaccion_Simple > 0)
                {
                    var simple = _context
                        .Transaccion_Simple
                        .AsNoTracking()
                        .Include(c => c.Tipo_Operacion_Simple)
                        .Single(f => f.Id_Transaccion_Simple == transaccion.Id_Transaccion_Simple);
                    concepto = "Retiro por estructuración"; //simple.Tipo_Operacion_Simple.Descripcion;
                    if(simple.Id_Credito>0)
                        cuenta=_context.Credito.Single(k => k.Id_Credito == simple.Id_Credito).Codigo_Credito;
                }

                monitor.Add(new Monitor_Movimientos { 
                    Comprobante=transaccion.No_Comprobante,
                    Tipo=transaccion.Tipo_Transaccion.Descripcion,
                    Concepto=concepto,
                    Cuenta=cuenta,
                    Monto=transaccion.Monto
                });
            }
            return Ok(monitor);
        }


        /// <summary>
        /// Se obtienen los depositos para rastrear las transacciones de una caja
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [Authorize(Roles = "Asistente,Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Depositos(DateTime fecha)
        {
            int Id_Caja = 0;
            string Id_Usuario = _userManager.GetUserId(HttpContext.User);
            Id_Caja = _context.Caja
                .AsNoTracking()
                .Single(s => s.Id_Usuario == Id_Usuario && s.Activa).Id_Caja;

            decimal monto_deposito = await _context
                .Transaccion_Caja
                .AsNoTracking()
                .Where(
                    d => !d.Borrado_Logico &&
                    d.Id_Caja == Id_Caja &&
                    d.Fecha.Date == fecha.Date &&
                    d.Id_Tipo_Transaccion == 1 &&
                    d.Id_Transaccion_Ahorro > 0)
                .SumAsync(s => s.Monto);
            return Ok(monto_deposito);
        }


        /// <summary>
        /// obtiene los pagos por fecha y usuario es solo el resumen
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Asistente,Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Pagos(DateTime fecha)
        {
            int Id_Caja = 0;
            string Id_Usuario = _userManager.GetUserId(HttpContext.User);
            Id_Caja = _context.Caja
                .AsNoTracking()
                .Single(s => s.Id_Usuario == Id_Usuario && s.Activa).Id_Caja;

            List<Transaccion_Credito> pagos = await Pagos_Detalle_Principal(fecha, Id_Caja);

            TransaccionCreditoResumen resumen = new TransaccionCreditoResumen {
                Capital = pagos.Sum(a => a.Abono_capital),
                Comision = pagos.Sum(a => a.Interes_Comision),
                Interes = pagos.Sum(a => a.Interes),
                IVASobreComision = pagos.Sum(a => a.IvaSobreComision),
                Recargo = pagos.Sum(a => a.Recargo),
                Resumen = pagos.Sum(a => a.Valor_Cuota)
            };

            return Ok(resumen);
        }

        /// <summary>
        /// los pagos pero detallados
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [Authorize(Roles = "Asistente,Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Pagos_Detalle(DateTime fecha)
        {
            int Id_Caja = 0;
            string Id_Usuario = _userManager.GetUserId(HttpContext.User);
            Id_Caja = _context.Caja
                .AsNoTracking()
                .Single(s => s.Id_Usuario == Id_Usuario && s.Activa).Id_Caja;

            List<Transaccion_Credito> pagos = await Pagos_Detalle_Principal(fecha, Id_Caja);

            return Ok(pagos);
        }

        private async Task<List<Transaccion_Credito>> Pagos_Detalle_Principal(DateTime fecha, int Id_Caja)
        {
            List<Transaccion_Caja> transacciones = await _context
                .Transaccion_Caja
                .AsNoTracking()
                .Where(
                    d => !d.Borrado_Logico &&
                    d.Id_Caja == Id_Caja &&
                    d.Fecha.Date == fecha.Date &&
                    d.Id_Tipo_Transaccion == 1 &&
                    d.Id_Transaccion_Credito > 0).ToListAsync();

            List<Transaccion_Credito> pagos = new List<Transaccion_Credito>();
            foreach (Transaccion_Caja transaccion in transacciones)
            {
                Transaccion_Credito pago = await _context
                        .Transaccion_Credito
                        .AsNoTracking()
                        .Include(u => u.Credito)
                        .SingleAsync(s => s.Id_Transaccion_Credito == transaccion.Id_Transaccion_Credito);
                pagos.Add(pago);
            }
            return pagos;
        }

        /// <summary>
        /// obtiene el detalle de entrada y salida de una caja
        /// </summary>
        /// <param name="rango"></param>
        /// <param name="fecha_inicio"></param>
        /// <param name="fecha_final"></param>
        /// <param name="id_caja"></param>
        /// <returns></returns>
        [Authorize(Roles = "Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Corte_Movimiento_Caja(bool rango, DateTime fecha_inicio, DateTime fecha_final, int id_caja)
        {
            Corte_de_Caja corte = new Corte_de_Caja();
            List<Rama> rama = new List<Rama>();

            List<Detalle_Asiento_Contable> partidas_ingreso = new List<Detalle_Asiento_Contable>();

            List<Detalle_Asiento_Contable> pago_credito = (from dac in _context.Detalle_Asiento_Contable
                                                           join ac in _context.Asiento_Contable
                                                                on dac.Id_Asiento_Contable equals ac.Id_Asiento_Contable
                                                           join tc in _context.Transaccion_Caja
                                                                on ac.Id_Transaccion_Caja equals tc.Id_Transaccion_Caja
                                                           join tcr in _context.Transaccion_Credito
                                                                on tc.Id_Transaccion_Credito equals tcr.Id_Transaccion_Credito
                                                           where 
                                                                tc.Fecha.Date == fecha_inicio.Date && 
                                                                tc.Id_Caja == id_caja &&
                                                                !tc.Borrado_Logico &&
                                                                !tcr.Borrado_Logico &&
                                                                dac.EsCaja==false
                                                           select dac).ToList();

            List<Detalle_Asiento_Contable> operaciones_ahorro = (from dac in _context.Detalle_Asiento_Contable
                                                           join ac in _context.Asiento_Contable
                                                                on dac.Id_Asiento_Contable equals ac.Id_Asiento_Contable
                                                           join tc in _context.Transaccion_Caja
                                                                on ac.Id_Transaccion_Caja equals tc.Id_Transaccion_Caja
                                                           join ta in _context.Transaccion_Ahorro
                                                                on tc.Id_Transaccion_Ahorro equals ta.Id_Transaccion_Ahorro
                                                           where 
                                                                tc.Fecha.Date == fecha_inicio.Date &&
                                                                tc.Id_Caja==id_caja && ta.Id_Tipo_Operacion_Ahorro==1 &&
                                                                !tc.Borrado_Logico &&
                                                                dac.EsCaja==false
                                                                
                                                           select dac).ToList();



            List<Detalle_Asiento_Contable> otrosingresorazoncreditos = (from dac in _context.Detalle_Asiento_Contable
                                                           join ac in _context.Asiento_Contable
                                                                on dac.Id_Asiento_Contable equals ac.Id_Asiento_Contable
                                                           join tc in _context.Transaccion_Caja
                                                                on ac.Id_Transaccion_Caja equals tc.Id_Transaccion_Caja
                                                           join ts in _context.Transaccion_Simple
                                                                on tc.Id_Transaccion_Simple equals ts.Id_Transaccion_Simple
                                                                        where 
                                                                            tc.Fecha.Date == fecha_inicio.Date && 
                                                                            tc.Id_Caja == id_caja &&
                                                                            !tc.Borrado_Logico &&

                                                                            (ts.Id_Tipo_Operacion_Simple == 6 || /*hablamos de ventas de acciones*/
                                                                            ts.Id_Tipo_Operacion_Simple==7 || /*hablamos de cobro de estructuración*/
                                                                            ts.Id_Tipo_Operacion_Simple == 10) && /*hablamos de ingreso o egreso*/
                                                                            dac.EsCaja==false
                                                           select dac).ToList();
            List<Detalle_Asiento_Contable> ingreso_egreso = (from dac in _context.Detalle_Asiento_Contable
                                                                        join ac in _context.Asiento_Contable
                                                                             on dac.Id_Asiento_Contable equals ac.Id_Asiento_Contable
                                                                        join tc in _context.Transaccion_Caja
                                                                             on ac.Id_Transaccion_Caja equals tc.Id_Transaccion_Caja
                                                                        join ts in _context.Transaccion_Simple
                                                                             on tc.Id_Transaccion_Simple equals ts.Id_Transaccion_Simple
                                                                        where
                                                                            tc.Fecha.Date == fecha_inicio.Date &&
                                                                            tc.Id_Caja == id_caja &&
                                                                            !tc.Borrado_Logico &&
                                                                            (ts.Id_Tipo_Operacion_Simple == 1 
                                                                            && ts.Id_Caja == id_caja) && /*hablamos de ingreso o egreso*/
                                                                            dac.EsCaja == true
                                                                        select dac).ToList();
            List<Detalle_Asiento_Contable> egreso = (from dac in _context.Detalle_Asiento_Contable
                                                             join ac in _context.Asiento_Contable
                                                                  on dac.Id_Asiento_Contable equals ac.Id_Asiento_Contable
                                                             join tc in _context.Transaccion_Caja
                                                                  on ac.Id_Transaccion_Caja equals tc.Id_Transaccion_Caja
                                                             join ts in _context.Transaccion_Simple
                                                                  on tc.Id_Transaccion_Simple equals ts.Id_Transaccion_Simple
                                                             where
                                                                 tc.Fecha.Date == fecha_inicio.Date &&
                                                                 tc.Id_Caja == id_caja &&
                                                                 !tc.Borrado_Logico &&
                                                                 ( ts.Id_Tipo_Operacion_Simple == 2
                                                                 && ts.Id_Caja == id_caja) && /*hablamos de ingreso o egreso*/
                                                                 dac.EsCaja == true
                                                             select dac).ToList();
            List<Detalle_Asiento_Contable> excedentes = (from dac in _context.Detalle_Asiento_Contable
                                                             join ac in _context.Asiento_Contable
                                                                  on dac.Id_Asiento_Contable equals ac.Id_Asiento_Contable
                                                             join tc in _context.Transaccion_Caja
                                                                  on ac.Id_Transaccion_Caja equals tc.Id_Transaccion_Caja
                                                             join ts in _context.Transaccion_Simple
                                                                  on tc.Id_Transaccion_Simple equals ts.Id_Transaccion_Simple
                                                             where
                                                                 tc.Fecha.Date == fecha_inicio.Date &&
                                                                 ts.Id_Tipo_Operacion_Simple==12 &&
                                                                 !tc.Borrado_Logico &&
                                                                 ts.Id_Caja==id_caja
                                                                 && dac.EsCaja == false
                                                             select dac).ToList();
            decimal p = pago_credito.Sum(a=>a.Abono);
            partidas_ingreso.AddRange(pago_credito);
            partidas_ingreso.AddRange(operaciones_ahorro);
            partidas_ingreso.AddRange(otrosingresorazoncreditos);
            partidas_ingreso.AddRange(ingreso_egreso);
            partidas_ingreso.AddRange(excedentes);

            /*await Vista_Transacciones_Ejercicio(fecha_inicio, id_caja, 1);*/
            List<Detalle_Asiento_Contable> partidas_egreso = await Vista_Transacciones_Ejercicio(fecha_inicio, id_caja, 2);
            partidas_egreso.AddRange(egreso);
            List<int> idsagencias = new List<int>(); 
                idsagencias.AddRange((from d in partidas_ingreso select d.Id_Agencia).Distinct());
                idsagencias.AddRange((from b in partidas_egreso select b.Id_Agencia).Distinct());

            foreach (int i in idsagencias.Distinct())
            {
                Agencia agencia = _context.Agencia.AsNoTracking().Single(f=>f.Id_Agencia==i);
                rama.Add(new Rama {
                    Agencia = agencia
                });
            }

            List<OperacionCorte> operacion_ingreso = partidas_ingreso.GroupBy(
                d => d.Cuenta_Contable).Select(cl => new OperacionCorte
                {
                    Id_Agencia = cl.FirstOrDefault().Id_Agencia,
                    cuenta = cl.FirstOrDefault().Cuenta_Contable,
                    descripcion = cl.FirstOrDefault().Concepto,
                    valor =cl.Sum(c => c.Abono) /*> 0 ? cl.Sum(c => c.Abono): cl.Sum(c => c.Cargo)*/
                }).OrderBy(d=>d.cuenta).ToList();


            List<OperacionCorte> operacion_egreso = partidas_egreso.GroupBy(
                d => d.Cuenta_Contable).Select(cl => new OperacionCorte
                {
                    Id_Agencia = cl.FirstOrDefault().Id_Agencia,
                    cuenta = cl.FirstOrDefault().Cuenta_Contable,
                    descripcion = cl.FirstOrDefault().Concepto,
                    valor = cl.Sum(c => c.Cargo)
                }).OrderBy(d=>d.descripcion).ToList();

            decimal total_ingreso = operacion_ingreso.Sum(d => d.valor);
            decimal total_egreso = operacion_egreso.Sum(d => d.valor);

            //obtener agencia de donde obtenemos los datos
            Caja caja = _context.Caja
                .Include(f => f.Agencia)
                .AsEnumerable()
                .Single(d => d.Id_Caja == id_caja);


            foreach (var r in rama)
            {
                Func<OperacionCorte, bool> filtrarOeracionCorte = new Func<OperacionCorte, bool>(d => d.Id_Agencia == r.Agencia.Id_Agencia);

                r.Ingresos= operacion_ingreso.Where(filtrarOeracionCorte).ToList();
                r.Egresos= operacion_egreso.Where(filtrarOeracionCorte).ToList();
                r.TotalIngreso= r.Ingresos.Sum(d => d.valor);
                r.TotalEgreso = r.Egresos.Sum(d => d.valor);
            }

            var usuario_nombre =await _context
                .Usuario_Agencia
                .AsNoTracking()
                .SingleAsync(d=>d.Id_Usuario==caja.Id_Usuario);

            corte.caja = caja.Nombre_PC;
            corte.cajero = !string.IsNullOrEmpty(usuario_nombre.Nombre_Completo)? usuario_nombre.Nombre_Completo:caja.Id_Usuario;

            corte.TotalIngreso = total_ingreso;
            corte.TotalEgreso = total_egreso;
            corte.Disponible = total_ingreso - total_egreso;

            corte.Rama = rama;
            return Ok(corte);
        }

        /*directamento los mivimientos como partida*/
        /// <summary>
        /// se obtienen las partidas
        /// relacionadas a una caja determinada
        /// </summary>
        /// <param name="Id_Caja"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
       [HttpGet]
        public async Task<List<Detalle_Asiento_Contable>> Movimiento_Contable_Caja(int Id_Caja, DateTime fecha)
        {
            return await Vista_Transacciones_Ejercicio(fecha,Id_Caja,0,true);
        }


        #region ParaDetallesTransaccionesComplejas

        /*probablemente se use para la gran modificación    */
        private async Task<List<Detalle_Asiento_Contable>> Vista_Transacciones_Ejercicio(DateTime fecha, int Id_Caja, int tipo, bool crudo=false)
        {
            try
            {
                Func<Transaccion_Caja, bool> dif = new Func<Transaccion_Caja, bool>(
                  d => !d.Borrado_Logico &&
                  d.Id_Caja == Id_Caja &&
                  d.Fecha.Date == fecha.Date &&
                  d.Id_Tipo_Transaccion == tipo);

                if (crudo)
                    dif = new Func<Transaccion_Caja, bool>(
                    d => !d.Borrado_Logico &&
                    d.Id_Caja == Id_Caja &&
                    d.Fecha.Date == fecha.Date);

                List<Transaccion_Caja> transaccion = _context
                    .Transaccion_Caja
                    .AsNoTracking()
                    .Where(dif)
                    .OrderByDescending(a => a.Fecha)
                    .ToList();

                List<Detalle_Asiento_Contable> partidas_contables = new List<Detalle_Asiento_Contable>();

                List<OperacionCorte> vista = new List<OperacionCorte>();
                foreach (var t in transaccion)
                {
                    /*saber si agregar solo al corte*/
                    bool esgasto = false;
                    if (!crudo)
                    {
                        if (t.Id_Transaccion_Simple > 0 && t.Id_Tipo_Transaccion == 2)/*se resgistra un gasto que no debe salir en el corte*/
                            esgasto = _context.Transaccion_Simple.Any(b=>b.Id_Tipo_Operacion_Simple==5);
                    }
                    
                    Func<Asiento_Contable, bool> hay_asiento = new Func<Asiento_Contable, bool>(d => d.Id_Transaccion_Caja == t.Id_Transaccion_Caja);
                    if (_context.Asiento_Contable.Any(hay_asiento))
                    {
                        List<Asiento_Contable> a = _context.Asiento_Contable.Where(hay_asiento).ToList();
                        foreach (var asientos in a)
                        {
                            Func<Detalle_Asiento_Contable, bool> hay_detalle = new Func<Detalle_Asiento_Contable, bool>
                                (d => d.Id_Asiento_Contable == asientos.Id_Asiento_Contable && !d.EsCaja);
                            if (_context.Detalle_Asiento_Contable.Any(hay_detalle) && !esgasto)
                            {
                                List<Detalle_Asiento_Contable> detalles = _context.
                                    Detalle_Asiento_Contable
                                    .AsNoTracking()
                                    .Where(hay_detalle).ToList();//falta ver que no aparezca la cuenta de la caja
                                partidas_contables.AddRange(detalles);
                            }
                        }
                        
                    }
                }
                return await Task.Run(() => partidas_contables);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
  
        }

        private async Task<IActionResult> Transacciones(DateTime? fecha, bool es_boveda, int id_caja = 0)
        {
            DateTime cual_fecha = DateTime.Now;
            Caja caja_boveda_actual = null;
            if (es_boveda)
                caja_boveda_actual = await Boveda_Actual();
            else
                caja_boveda_actual = await Caja_Actual(id_caja);

            if (fecha != null)
                cual_fecha = (DateTime)fecha;

            List<Transaccion_Caja> transacciones = _context
            .Transaccion_Caja
            .Include(y => y.Caja)
            .Include(y => y.Tipo_Transaccion)
            .AsNoTracking()
            .Where(b => b.Id_Caja == caja_boveda_actual.Id_Caja &&
                         b.Fecha.Date == cual_fecha.Date &&
                         !b.Borrado_Logico).ToList();/*falta probar para que no se muestre sus regresiones*/

            if (transacciones.Any())
            {
                List<Transaccion_Credito> transaccions = await Transacciones_Credito_De_Caja(transacciones);
                List<Transaccion_Simple> simples = await Transacciones_Desembolsos_De_Caja(transacciones);
                Transacciones_Caja_Vista transacciones_vista = new Transacciones_Caja_Vista(transaccions, simples, cual_fecha);
                transacciones_vista.Datos_Caja = transacciones.FirstOrDefault()?.Caja;
                return Ok(transacciones_vista);
            }
            return NoContent();
        }

        private async Task<List<Transaccion_Credito>> Transacciones_Credito_De_Caja(List<Transaccion_Caja> transacciones)
        {
            List<Transaccion_Credito> transacciones_creditos = new List<Transaccion_Credito>();
            foreach (Transaccion_Caja transaccion in transacciones)
            {
                if (transaccion.Id_Transaccion_Credito > 0)
                {
                    Transaccion_Credito transaccion_credito = await _context
                        .Transaccion_Credito
                        .SingleAsync(s => s.Id_Transaccion_Credito == transaccion.Id_Transaccion_Credito);
                    transacciones_creditos.Add(transaccion_credito);
                }
            }
            return transacciones_creditos;
        }

        private async Task<List<Transaccion_Simple>> Transacciones_Desembolsos_De_Caja(List<Transaccion_Caja> transacciones)
        {
            List<Transaccion_Simple> simples = new List<Transaccion_Simple>();
            foreach (Transaccion_Caja transaccion in transacciones)
            {
                if (transaccion.Id_Transaccion_Simple > 0)
                {
                    Transaccion_Simple simple = await _context.Transaccion_Simple.SingleAsync(a => a.Id_Transaccion_Simple == transaccion.Id_Transaccion_Simple);
                    if (simple.Id_Credito > 0)
                        simples.Add(simple);
                }
            }
            return simples;
        }

        #endregion

        /// <summary>
        /// detalle simple de caja para conecer el tipo de transaccion
        /// </summary>
        /// <param name="id_transaccion"></param>
        /// <returns></returns>
        [Authorize(Roles = "Asistente,Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Transaccion_Caja_Detalle(int id_transaccion)
        {
            Transaccion_Caja transaccion = await _context
                .Transaccion_Caja
                .SingleAsync(c => c.Id_Transaccion_Caja == id_transaccion);
            return Ok(transaccion);
        }

        /// <summary>
        /// reporte de flujo de efectivo de una agencia
        /// </summary>
        /// <param name="fecha_flujo"></param>
        /// <param name="id_agencia"></param>
        /// <returns></returns>
        [Authorize(Roles = "Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<Reporte_Flujo_Efectivo_Vista> Flujo_Efectivo_Agencia(DateTime fecha_flujo, int id_agencia = 0)
        {
            try
            {
                /*trabajo para la caja máquina*/
                string caja_maquina = _context.Configuracion_General.Single(x => x.Nombre == "CAJA_MAQUINA").Valor;
                var id_caja_maquina = Convert.ToInt32(caja_maquina);

                if (User.IsInRole("Jefe"))
                {
                    id_agencia = Agencia_Usuario();
                    fecha_flujo = DateTime.Now;
                }//lista de cajas habilitada
                List<Transaccion_Caja> transaciones = await _context
                    .Caja.AsNoTracking()
                    .Include(t => t.Transaccion_Cajas)
                    .Where(t => t.Activa && t.Id_Agencia == id_agencia)
                    .SelectMany(y => y.Transaccion_Cajas)
                    .Where(g => g.Fecha.Date == fecha_flujo.Date && !g.Borrado_Logico).ToListAsync();

                Reporte_Flujo_Efectivo_Vista reporte = new Reporte_Flujo_Efectivo_Vista();

                Func<Ingreso_Cheque, bool> filtro_cheque = new Func<Ingreso_Cheque, bool>
                    (t => t.Id_Agencia == id_agencia && t.Fecha.Date == fecha_flujo.Date);
                if (_context.Ingreso_Cheque.AsNoTracking().Any(filtro_cheque))
                {
                    var cheques = _context
                    .Ingreso_Cheque
                    .AsNoTracking()
                    .Where(filtro_cheque);
                    reporte.Cheques_Recibidos_Cantidad = cheques.Count();
                    reporte.Cheques_Recibidos_Monto = cheques.Sum(s => s.Monto);
                }
                Func<Ingreso_Cheque, bool> cheque_Administrativa = new Func<Ingreso_Cheque, bool>
                    (t => t.Id_Agencia == id_agencia && t.Fecha.Date == fecha_flujo.Date && t.Id_Banco == 7);
                if (_context.Ingreso_Cheque.AsNoTracking().Any(cheque_Administrativa))
                {
                    var cheques = _context
                        .Ingreso_Cheque
                        .AsNoTracking()
                        .Where(cheque_Administrativa);
                    reporte.Fondos_de_Administracion = cheques.Sum(s => s.Monto);
                }
                foreach (Transaccion_Caja transaccion in transaciones)
                {
                    if (transaccion.Id_Transaccion_Simple > 0)
                    {
                        Func<Transaccion_Simple, bool> filtrar_deduccion = new Func<Transaccion_Simple, bool>(
                            t => t.Id_Transaccion_Simple == transaccion.Id_Transaccion_Simple &&
                                t.Id_Deduccion > 0);
                        Func<Transaccion_Simple, bool> filtrar_accion = new Func<Transaccion_Simple, bool>(
                            t => t.Id_Transaccion_Simple == transaccion.Id_Transaccion_Simple &&
                                 t.Id_Cobro_Acciones > 0);
                        Func<Transaccion_Simple, bool> filtrar_libreta = new Func<Transaccion_Simple, bool>(
                            t => t.Id_Transaccion_Simple == transaccion.Id_Transaccion_Simple &&
                                 t.Id_Libreta > 0);

                        Func<Transaccion_Simple, bool> filtrar_remesas = new Func<Transaccion_Simple, bool>(
                            t => t.Id_Transaccion_Simple == transaccion.Id_Transaccion_Simple && t.Id_Banco > 0 && t.Creado_En.Date == fecha_flujo.Date);

                        Func<Transaccion_Simple, bool> filtrar_bolsa = new Func<Transaccion_Simple, bool>(
                            t => t.Id_Transaccion_Simple == transaccion.Id_Transaccion_Simple && t.Creado_En.Date == fecha_flujo.Date && t.Id_Tipo_Operacion_Simple == 11);

                        Func<Transaccion_Simple, bool> filtrar_gastos = new Func<Transaccion_Simple, bool>(
                            t => t.Id_Transaccion_Simple == transaccion.Id_Transaccion_Simple && t.Creado_En.Date == fecha_flujo.Date && t.Id_Tipo_Operacion_Simple == 5);
                        Func<Transaccion_Simple, bool> filtrar_excedente = new Func<Transaccion_Simple, bool>(
                           t => t.Id_Transaccion_Simple == transaccion.Id_Transaccion_Simple && t.Creado_En.Date == fecha_flujo.Date && t.Id_Tipo_Operacion_Simple == 12);
                        var es_deduccion = _context
                            .Transaccion_Simple
                            .AsNoTracking().Any(filtrar_deduccion);
                        if (_context.Transaccion_Simple.Any(filtrar_excedente))
                        {
                            reporte.Fondos_de_Administracion += transaccion.Monto;
                        }
                        if (es_deduccion)
                        {
                            var simple = _context.Transaccion_Simple.Single(filtrar_deduccion);
                            var refinanciado = _context.Deduccion_Historial.Any(t => t.Id_Deduccion_Historial == simple.Id_Deduccion && t.Id_Credito_Financiado == 0);
                            if (refinanciado)
                                reporte.Ingreso_pago_creditos += transaccion.Monto;
                        }
                        if (_context.Transaccion_Simple.Any(filtrar_accion))
                        {
                            var accion = _context.Transaccion_Simple.Single(filtrar_accion);
                            var es_accion = _context.Accion.Any(t => t.Id_Accion == accion.Id_Cobro_Acciones);
                            if (es_accion)
                                reporte.Ingreso_pago_creditos += transaccion.Monto;
                        }
                        if (_context.Transaccion_Simple.Any(filtrar_libreta))
                        {
                            var libreta = _context.Transaccion_Simple.Single(filtrar_libreta);
                            var es_libreta = _context.LibretaTransacciones.Any(t => t.Id_Libreta == libreta.Id_Libreta);
                            if (es_libreta)
                                reporte.Ingreso_pago_creditos += transaccion.Monto;
                        }

                        if (_context.Transaccion_Simple.Any(filtrar_remesas))
                        {
                            var remesa = _context.Transaccion_Simple.Single(filtrar_remesas);
                            reporte.Resemas_AKI += remesa.Id_Banco == 9 ? remesa.Monto : 0.00M;
                            reporte.Remesas += remesa.Id_Banco != 9 ? remesa.Monto : 0.00M;
                        }

                        if (_context.Transaccion_Simple.Any(filtrar_bolsa))
                        {
                            var bolsa_tercero = _context.Transaccion_Simple.Single(filtrar_bolsa);
                            reporte.Colectores_AKI += bolsa_tercero.Monto;
                        }

                        if (_context.Transaccion_Simple.Any(filtrar_gastos))
                        {
                            var gastos_varios = _context.Transaccion_Simple.Single(filtrar_gastos);

                            reporte.Impuestos_municipales += gastos_varios.Id_Factura_Servicio == 1 ? gastos_varios.Monto : 0.00M;
                            reporte.Alquiler += gastos_varios.Id_Factura_Servicio == 2 ? gastos_varios.Monto : 0.00M;

                            reporte.Mantenimientos_y_Reparaciones += gastos_varios.Id_Factura_Servicio == 6  ? gastos_varios.Monto : 0.00M;
                            reporte.Servicios_TV_por_cable += gastos_varios.Id_Factura_Servicio == 7 ? gastos_varios.Monto : 0.00M;
                            reporte.Billetes_promocionales += gastos_varios.Id_Factura_Servicio == 8 ? gastos_varios.Monto : 0.00M;
                            reporte.Servicios_basicos += gastos_varios.Id_Factura_Servicio == 3 || gastos_varios.Id_Factura_Servicio == 4
                                || gastos_varios.Id_Factura_Servicio == 5 || gastos_varios.Id_Factura_Servicio == 9? gastos_varios.Monto : 0.00M;

                        }

                    }

                    if (transaccion.Id_Transaccion_Credito > 0 && transaccion.Id_Caja != id_caja_maquina)
                        reporte.Ingreso_pago_creditos += transaccion.Monto;
                    if (transaccion.Id_Transaccion_Ahorro > 0 && transaccion.Id_Caja != id_caja_maquina)
                    {
                        Transaccion_Ahorro ahorros = await _context
                            .Transaccion_Ahorro
                            .AsNoTracking().SingleAsync(
                                s => s.Id_Transaccion_Ahorro == transaccion.Id_Transaccion_Ahorro &&
                                !s.Borrado_Logico);
                        if (ahorros.Id_Tipo_Operacion_Ahorro == 1)
                            reporte.Depositos_en_cuenta_de_Ahorro += transaccion.Monto;
                        if (ahorros.Id_Tipo_Operacion_Ahorro == 3)
                            reporte.Retiro_de_ahorros += transaccion.Monto;
                        if (ahorros.Id_Tipo_Operacion_Ahorro == 2)
                            reporte.Pago_de_depositos_a_plazo_fijo += transaccion.Monto;
                    }
                    if (transaccion.Id_Transaccion_Deposito_Plazo > 0 && transaccion.Id_Caja != id_caja_maquina)
                    {
                        Transaccion_Deposito_Plazo deposito_Plazo = await _context
                            .Transaccion_Deposito_Plazo
                            .AsNoTracking().SingleAsync(
                                s => s.Id_Transaccion_Deposito_Plazo == transaccion.Id_Transaccion_Deposito_Plazo &&
                                !s.Borrado_Logico);
                        if (deposito_Plazo.Id_Tipo_Operacion_Ahorro == 1)
                            reporte.Fondos_de_Deposito_a_plazo += transaccion.Monto;
                        if (deposito_Plazo.Id_Tipo_Operacion_Ahorro == 3)
                            reporte.Pago_de_depositos_a_plazo_fijo += transaccion.Monto;
                  
                    }
                }

                //recolectar la información del dinero
                Caja boveda = await _context.Caja.SingleAsync(d => d.Id_Agencia == id_agencia && d.Boveda);
                SaldoDisponible saldo_anterior = await Saldo_Anterior(boveda.Id_Caja);
                if (saldo_anterior != null)
                {
                    //buscar todos los desembolso o liquidaciones efectuadas
                    List<Transaccion_Simple> desembolsos = await _context
                        .Transaccion_Simple
                        .Where(
                            c => c.Id_Tipo_Operacion_Simple == 8 &&
                            c.Creado_En.Date == fecha_flujo.Date &&
                            !c.Borrado_Logico)
                        .ToListAsync();

                    List<Creditos_Colocados> colocados = await Obtener_Creditos_Colocados(DateTime.MinValue, DateTime.MinValue, id_agencia, 0, fecha_flujo);

                    reporte.Depositos_a_Prestamos_en_CHC = colocados.Sum(a => a.Total);

                    reporte.Subtotal_de_pendendientes =
                        reporte.Impuestos_municipales +
                        reporte.Alquiler +
                        reporte.Combustible +
                        reporte.Mantenimientos_y_Reparaciones +
                        reporte.Servicios_TV_por_cable +
                        reporte.Billetes_promocionales +
                        reporte.Servicios_basicos;

                    reporte.Desembolsos_prestamos = colocados.Sum(r => r.Total);

                    reporte.Saldo_Anterior = saldo_anterior.saldo;

                    reporte.Subtotal_Entradas_ordinario =
                    reporte.Ingreso_pago_creditos +
                    reporte.Fondos_de_Administracion +
                    reporte.Depositos_en_cuenta_de_Ahorro +
                    reporte.Fondos_de_Deposito_a_plazo +
                    reporte.Depositos_a_Prestamos_en_CHC +
                    reporte.Colectores_AKI;

                    reporte.Subtotal_Salidas_ordinario =
                    reporte.Desembolsos_prestamos +
                    reporte.Pago_de_depositos_a_plazo_fijo +
                    reporte.Retiro_de_ahorros +
                    reporte.Remesas +
                    reporte.Resemas_AKI;

                    reporte.Subtotal_de_operaciones = reporte.Subtotal_Entradas_ordinario -
                                                      reporte.Subtotal_Salidas_ordinario;



                    /*ver la descripción del efectivo*/
                    Func<Efectivo, bool> hay_billete = new Func<Efectivo, bool>(d => d.id_saldodisponible == saldo_anterior.id_saldodisponible);
                    if (_context.Efectivo.AsNoTracking().Any(hay_billete))
                    {
                        reporte.Efectivo_Detallado = _context
                        .Efectivo
                        .AsNoTracking()
                        .Include(a => a.Denominacion)
                        .Where(hay_billete)
                        .Select(d => new Detalle_Efectivo_Vista
                        {
                            Efectivo = d,
                            Denominacion = d.Denominacion
                        }).ToList();
                    }

                    reporte.Disponible_a_la_fecha =
                        reporte.Saldo_Anterior +
                        reporte.Subtotal_de_operaciones -
                        reporte.Subtotal_de_pendendientes;

                    return reporte;
                }
                else
                {
                    //buscar todos los desembolso o liquidaciones efectuadas
                    List<Transaccion_Simple> desembolsos = await _context
                        .Transaccion_Simple
                        .Where(
                            c => c.Id_Tipo_Operacion_Simple == 8 &&
                            c.Creado_En.Date == fecha_flujo.Date &&
                            !c.Borrado_Logico)
                        .ToListAsync();
                    foreach (Transaccion_Simple transaccion in desembolsos)
                        reporte.Desembolsos_prestamos += transaccion.Monto;
                    //reporte.Saldo_Anterior = saldo_anterior.saldo;

                    reporte.Subtotal_Entradas_ordinario =
                    reporte.Ingreso_pago_creditos +
                    reporte.Fondos_de_Administracion +
                    reporte.Depositos_en_cuenta_de_Ahorro +
                    reporte.Fondos_de_Deposito_a_plazo +
                    reporte.Depositos_a_Prestamos_en_CHC +
                    reporte.Colectores_AKI;

                    reporte.Subtotal_Salidas_ordinario =
                    reporte.Desembolsos_prestamos +
                    reporte.Pago_de_depositos_a_plazo_fijo +
                    reporte.Retiro_de_ahorros +
                    reporte.Remesas +
                    reporte.Resemas_AKI;

                    reporte.Subtotal_de_operaciones = reporte.Subtotal_Entradas_ordinario -
                                                      reporte.Subtotal_Salidas_ordinario;

                    reporte.Subtotal_de_pendendientes =
                        reporte.Impuestos_municipales +
                        reporte.Alquiler +
                        reporte.Combustible +
                        reporte.Mantenimientos_y_Reparaciones +
                        reporte.Servicios_TV_por_cable +
                        reporte.Billetes_promocionales +
                        reporte.Servicios_basicos;

                    /*ver la descripción del efectivo*/
                    Func<Efectivo, bool> hay_billete = new Func<Efectivo, bool>(d => d.id_saldodisponible == saldo_anterior.id_saldodisponible);
                    if (_context.Efectivo.AsNoTracking().Any(hay_billete))
                    {
                        reporte.Efectivo_Detallado = _context
                        .Efectivo
                        .AsNoTracking()
                        .Include(a => a.Denominacion)
                        .Where(hay_billete)
                        .Select(d => new Detalle_Efectivo_Vista
                        {
                            Efectivo = d,
                            Denominacion = d.Denominacion
                        }).ToList();
                    }

                    reporte.Disponible_a_la_fecha =
                        reporte.Saldo_Anterior +
                        reporte.Subtotal_de_operaciones -
                        reporte.Subtotal_de_pendendientes;

                    /*guardar la disponibilidad*/
                    var disponible = new SaldoDisponible
                    {
                        fecha = fecha_flujo,
                        saldo = reporte.Disponible_a_la_fecha,
                        id_caja = boveda.Id_Caja,
                        Cantidad_aki = reporte.Detalle_bolsa_AKI,
                        Efectivo_total = reporte.Efectivo_neto_en_caja,
                        Numero_cheque = reporte.Cheques_Recibidos_Cantidad
                    };

                    await Guardar_Saldo_Disponible(disponible);

                    return reporte;
                }
            }
            catch (Exception ex)
            {
                return null;
            }

            
           
        }


        /// <summary>
        /// verdadero flujo por agencia de efectivo dado solo para Jefa o Administrador
        /// </summary>
        /// <returns></returns>
       // [Authorize(Roles = "Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Liquidez_Agencia_Lista( int id_agencia = 0)
        {
            DateTime fecha = DateTime.Now;
            if (id_agencia == 0)
            {
                if (User.IsInRole("Jefe"))
                    id_agencia = Agencia_Usuario();
            }
            /*obtener todas las transacciones de las cajas existentes en esa agencia*/
            List<Caja> cajas = await _context
                .Caja
                .AsNoTracking()
                .Where(t => t.Id_Agencia == id_agencia && t.Activa)
                .ToListAsync();
            string Id_Usuario = _userManager.GetUserId(HttpContext.User);
            var primerDiaActual = new DateTime(fecha.Year, fecha.Month, 1);
            var mesAnterior = primerDiaActual.AddMonths(-1);

            var cajaBOveda =_context.Caja.Single(d=>d.Id_Agencia==id_agencia && d.Boveda);

            var ultimaFecha = _context
                                    .SaldoDisponible
                                    .Where(
                                            c=>c.fecha.Month==mesAnterior.Month && 
                                            c.id_caja== cajaBOveda.Id_Caja)
                                    .Max(d=>d.fecha);   //await Saldo_Anterior(cajas.FirstOrDefault(d => d.Boveda).Id_Caja);
            var saldo = _context.SaldoDisponible.Single(d=>d.id_caja==cajaBOveda.Id_Caja && d.fecha.Date==ultimaFecha.Date);

            decimal inicial_encontrado = saldo.saldo;/*el saldo lo voy a degradar*/

            Liquidez_Vista vista = new Liquidez_Vista();
            List<Transaccion_Liquidez> transacciones = new List<Transaccion_Liquidez>();

            /*pongo todos los desembolsos como CHC*/
            decimal chc = 0.00M;
            DateTime fechainicio = new DateTime(fecha.Year, fecha.Month, 1);
            DateTime fechafin = fecha;

            DateTime fechatemp = saldo.fecha;
            List<DateTime> fechas = new List<DateTime>();
            //while (fechatemp < fechafin)
            //{
            //    if (fechatemp <= fechafin)
            //    {
            //        fechatemp = fechatemp.AddDays(1);
            //        fechas.Add(fechatemp);
            //    }
            //}
            while (fechatemp < fechafin)
            {
                fechatemp = fechatemp.AddDays(1);
                if (fechatemp.DayOfWeek != DayOfWeek.Saturday && fechatemp.DayOfWeek != DayOfWeek.Sunday)
                {
                    fechas.Add(fechatemp);
                }
            }
            foreach (DateTime transacion_caja in fechas)
            {
                List<Creditos_Colocados> colocados = await Obtener_Creditos_Colocados(DateTime.MinValue, DateTime.MinValue, id_agencia, 0, transacion_caja);
                if (colocados.Sum(v=>v.Monto) > 0)
                {
                    chc += colocados.Sum(a => a.Total);
                    Categoria_Tipo tipo = new Categoria_Tipo();
                    tipo = new Categoria_Tipo
                    {
                        Categoria = Categoria_Tipo_Transaccion.Transaccion_Credito,
                        Tipo = 1,
                        Descripcion = "DESEMBOLSOS",
                        Caja = cajas.Single(a => a.Boveda).Id_Usuario
                    };
                    Transaccion_Liquidez liquidez = new Transaccion_Liquidez
                    {
                        Flujo = Tipo_Flujo.Remesas_Desembolsos,
                        //No_Comprobante = transacion_caja.No_Comprobante,
                        Fecha = transacion_caja,
                        Categoria_Tipo = tipo,
                        Monto = colocados.Sum(a => a.Total),
                        Saldo = saldo.saldo
                    };
                    transacciones.Add(liquidez);
                }
                
            }
            foreach (DateTime fechacheque in fechas)
            {
                List<Ingreso_Cheque> cheques = _context.Ingreso_Cheque
                    .Where(a => a.Fecha.Date == fechacheque.Date && a.Id_Agencia == id_agencia && a.Monto > 0).ToList();
                Categoria_Tipo tipo = new Categoria_Tipo();
                if (cheques.Count() > 0)
                {
                    tipo = new Categoria_Tipo
                    {
                        Categoria = Categoria_Tipo_Transaccion.Transaccion_Simple,
                        Tipo = 3,
                        Descripcion = "FONDO PARA DESEMBOLSO",
                        // Caja = cajas.Single(a => a.Boveda).Id_Usuario
                    };
                    Transaccion_Liquidez liquidez = new Transaccion_Liquidez
                    {
                        Flujo = Tipo_Flujo.Colecta_Cheque,
                        Fecha = fechacheque,
                        Categoria_Tipo = tipo,
                        Monto = cheques.Sum(a => a.Monto),
                        Saldo = saldo.saldo
                    };
                    transacciones.Add(liquidez);
                }


            }
            foreach (Caja caja in cajas)
            {
             
                /*facturas, remesas, asignación y recepción de fondos*/
                List<Transaccion_Caja> otrastransacciones = (from tc in _context.Transaccion_Caja
                                                   join ts in _context.Transaccion_Simple
                                                       on tc.Id_Transaccion_Simple equals ts.Id_Transaccion_Simple
                                                   where
                                                       tc.Fecha.Date > saldo.fecha.Date &&
                                                       
                                                       (ts.Id_Caja == caja.Id_Caja || ts.Id_Caja_Boveda==caja.Id_Caja) &&
                                                       
                                                       (
                                                            ts.Id_Tipo_Operacion_Simple == 5 ||
                                                            ts.Id_Tipo_Operacion_Simple == 3 ||


                                                            ts.Id_Tipo_Operacion_Simple == 1 ||
                                                            ts.Id_Tipo_Operacion_Simple == 2 
                                                            ||
                                                            ts.Id_Tipo_Operacion_Simple == 11)

                                                   select tc).ToList();
                foreach (Transaccion_Caja otratransaccion in otrastransacciones)
                {
                    otratransaccion.Caja = caja;
                    decimal liquido_otra = otratransaccion.Monto;
                    var transimple = _context
                                    .Transaccion_Simple
                                    .Single(d => d.Id_Transaccion_Simple == otratransaccion.Id_Transaccion_Simple);

                    //decimal entrega = 0.00M;
                    if (otratransaccion.Id_Tipo_Transaccion == 1)
                        saldo.saldo += liquido_otra;
                    else
                        saldo.saldo -= liquido_otra;
                    //chc += liquido_factura;

                    Tipo_Flujo definir_flujo= Tipo_Flujo.Remesas_Desembolsos;
                    if(transimple.Id_Tipo_Operacion_Simple==3 )
                        definir_flujo = Tipo_Flujo.Remesas_Desembolsos;
                    if (transimple.Id_Tipo_Operacion_Simple == 2)
                        definir_flujo = Tipo_Flujo.Colecta_Cheque;
                    if (transimple.Id_Tipo_Operacion_Simple == 1)
                        definir_flujo = Tipo_Flujo.Remesas_Desembolsos;
                    if (transimple.Id_Tipo_Operacion_Simple == 11)
                        definir_flujo = Tipo_Flujo.Colecta_Cheque;
                    if (transimple.Id_Tipo_Operacion_Simple == 5)
                        definir_flujo = Tipo_Flujo.Facturas;


                    Transaccion_Liquidez liquidez = new Transaccion_Liquidez
                    {
                        Flujo = definir_flujo,
                        No_Comprobante = otratransaccion.No_Comprobante,
                        Fecha = otratransaccion.Fecha,
                        Categoria_Tipo = await Obtener_Descripcion(otratransaccion),
                        Monto = liquido_otra,
                        Saldo = saldo.saldo
                    };
                    transacciones.Add(liquidez);
                }

            }

            /*transacciones independientes*/
            Categoria_Tipo categoria = new Categoria_Tipo { 
                Categoria=Categoria_Tipo_Transaccion.Transaccion_Ahorro,
                Tipo=1,
                Descripcion="Depósitos por Prestamos CHC",
                Caja=cajas.FirstOrDefault().Id_Usuario
            };
            Transaccion_Liquidez transaccion_chc = new Transaccion_Liquidez
            {
                Flujo = Tipo_Flujo.Colecta_Cheque,
                No_Comprobante = "0009999",
                Fecha = fecha.Date,
                Categoria_Tipo = categoria,
                Monto = chc,
                Saldo = saldo.saldo += chc
            };
            transacciones.Add(transaccion_chc);

            if (saldo != null)
            {
                vista.Fecha_Reporte = saldo.fecha;
                vista.Ultimo_Saldo = inicial_encontrado;
            }
            else
            {
                vista.Fecha_Reporte = DateTime.Now;
                vista.Ultimo_Saldo = 0;
                vista.Agencia = _context.Agencia.AsNoTracking().Single(d => d.Id_Agencia == id_agencia).Nombre;
            }

            vista.Agencia = _context.Agencia.AsNoTracking().Single(d => d.Id_Agencia == id_agencia).Nombre;
            var agrupado = await Agrupacion_Resumen(transacciones);
            vista.Total_Remesas_Desembolso = agrupado.Where(f => f.Flujo == Tipo_Flujo.Remesas_Desembolsos).Sum(v => v.Monto);
            vista.Total_Facturas = agrupado.Where(f => f.Flujo == Tipo_Flujo.Facturas).Sum(v => v.Monto);
            vista.Total_Colecta_Cheque = agrupado.Where(f => f.Flujo == Tipo_Flujo.Colecta_Cheque).Sum(v => v.Monto);
            vista.Disponibilidad = (vista.Ultimo_Saldo + vista.Total_Colecta_Cheque) - (vista.Total_Remesas_Desembolso + vista.Total_Facturas);
            vista.Generado_Por = Id_Usuario;
            vista.Transacciones = agrupado;

            return Ok(vista);
        }

        private async Task<List<Transaccion_Liquidez>> Agrupacion_Resumen(List<Transaccion_Liquidez> transacciones)
        {
            List<Transaccion_Liquidez> transaccion_desembolso = transacciones
                .GroupBy(l => new { l.Categoria_Tipo.Descripcion, l.Fecha })
                .Select(cl => new Transaccion_Liquidez
                {
                    Flujo = cl.FirstOrDefault().Flujo,
                    No_Comprobante = "",
                    Fecha = cl.FirstOrDefault().Fecha,
                    Categoria_Tipo = cl.FirstOrDefault().Categoria_Tipo,
                    Monto = cl.Sum(c => c.Monto),
                    Saldo = cl.FirstOrDefault().Saldo

                }).ToList();


            return transaccion_desembolso;
        }

        private async Task<Categoria_Tipo> Obtener_Descripcion(Transaccion_Caja transacion_caja)
        {
            try
            {
                Categoria_Tipo tipo = new Categoria_Tipo();
                if (transacion_caja.Id_Transaccion_Ahorro > 0)
                {
                    int id_tipo = _context
                        .Transaccion_Ahorro
                        .AsNoTracking()
                        .Single(v => v.Id_Transaccion_Ahorro == transacion_caja.Id_Transaccion_Ahorro)
                        .Id_Tipo_Operacion_Ahorro;
                    tipo = new Categoria_Tipo { 
                        Categoria=Categoria_Tipo_Transaccion.Transaccion_Ahorro,
                        Tipo=id_tipo,
                        Descripcion= _context.Tipo_Operacion_Ahorro.Single(t => t.Id_Tipo_Operacion_Ahorro == id_tipo).Descripcion
                    };
                }

                if (transacion_caja.Id_Transaccion_Credito > 0)
                {
                    int id_tipo = _context
                        .Transaccion_Credito
                        .AsNoTracking()
                        .Single(v => v.Id_Transaccion_Credito == transacion_caja.Id_Transaccion_Credito)
                        .Id_Tipo_Operecion_Credito;
                    tipo = new Categoria_Tipo
                    {
                        Categoria = Categoria_Tipo_Transaccion.Transaccion_Credito,
                        Tipo = id_tipo,
                        Descripcion = _context.Tipo_Operacion_Credito.Single(t => t.Id_Tipo_Operacion_Credito == id_tipo).Descripcion
                    };
                }

                if (transacion_caja.Id_Transaccion_Simple > 0)
                {
                    var simple = _context
                        .Transaccion_Simple
                        .AsNoTracking()
                        .Single(v => v.Id_Transaccion_Simple == transacion_caja.Id_Transaccion_Simple);

                    string descripcion_complicada = "---";
                    if (simple.Id_Tipo_Operacion_Simple == 5)
                        descripcion_complicada = _context
                                                .Factura_Servicios
                                                .AsNoTracking()
                                                .Single(f => f.Id_Factura_Servicio == simple.Id_Factura_Servicio).Concepto;
                    else if (simple.Id_Tipo_Operacion_Simple == 1)
                        descripcion_complicada = "Asignación de fondos caja" + simple.Id_Caja;
                    else if (simple.Id_Tipo_Operacion_Simple == 2)
                        descripcion_complicada = "Colecta del " +simple.Creado_En.Date.ToString("dd/MM/yyyy") + " de caja " + simple.Id_Caja;
                    else if (simple.Id_Tipo_Operacion_Simple == 11)
                        descripcion_complicada = "COLECTA AKI";
                    else
                        descripcion_complicada = _context.Tipo_Operacion_Simple.Single(t => t.Id_Tipo_Operacion_Simple == simple.Id_Tipo_Operacion_Simple).Descripcion;

                    /*sacar la caja*/
                    
                    tipo = new Categoria_Tipo
                    {
                        Categoria = Categoria_Tipo_Transaccion.Transaccion_Simple,
                        Tipo =  simple.Id_Tipo_Operacion_Simple,
                        Descripcion = simple.Detalle_Factura_Servicio + " "+ descripcion_complicada
                    };
                }

                string id_usuario_transaccion= transacion_caja.Caja != null ? transacion_caja.Caja.Id_Usuario : "--";

                var usuario_agencia = await _context
                    .Usuario_Agencia
                    .AsNoTracking()
                    .SingleAsync(d=>d.Id_Usuario==id_usuario_transaccion);
                if (usuario_agencia != null)
                    tipo.Caja = !string.IsNullOrEmpty(usuario_agencia.Nombre_Completo) ? usuario_agencia.Nombre_Completo : id_usuario_transaccion;
                else
                    tipo.Caja = "---";
                return tipo;
            } catch (Exception ex)
            {
                return null;
            }
        }


        ///todos sobre el flujo de efectivo

        /*se hace el cierre con el flujo de efectivo reportado*/
        /// <summary>
        /// se obtiene todo el efectivo reportado al detalle
        /// de una caja o bóveda como tal
        /// </summary>
        /// <param name="efectivos"></param>
        /// <param name="numero_cheque"></param>
        /// <param name="cantidad_cheque"></param>
        /// <param name="cantidad_aki"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Recibir_Detalle_Efectivo(
            [FromBody] List<Efectivo> efectivos,
            [FromQuery] decimal numero_cheque,
            [FromQuery] decimal cantidad_cheque,
            [FromQuery] decimal cantidad_aki)
        {
            using (var dbContextTransaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    DateTime ahora = DateTime.Now;
                    Caja caja = await Boveda_Actual();
                    if (caja == null)
                        caja = await Caja_Actual();
                    if (caja == null)
                        return BadRequest("No tiene una caja asignada para registrar flujo de efectivo");

                    if (_context.SaldoDisponible.Any(a => a.id_caja == caja.Id_Caja && a.fecha.Date == ahora.Date))
                        return BadRequest("Ya existe un registro para el ejercicio actual para su caja asignada");

                    decimal monto_calculado = 0.00M;
                    SaldoDisponible saldo_anterior = await PrepararSaldo(caja.Id_Caja);

                    foreach (Efectivo ef in efectivos)
                        monto_calculado += await Denominacion(ef);

                    decimal saldo_actual = saldo_anterior.saldo + monto_calculado + cantidad_cheque + cantidad_aki;
                    //verificaciones
                    Faltante_Caja faltante =await Diferencia_Saldos(caja.Id_Caja,saldo_actual);

                    if (faltante.Correcto)
                    {
                        SaldoDisponible nuevo_saldo_disponible = new SaldoDisponible
                        {
                            fecha = ahora,
                            id_caja = caja.Id_Caja,
                            Efectivo_total = monto_calculado,

                            Numero_cheque = numero_cheque,//param
                            Cantidad_cheque = cantidad_cheque,//param
                            Cantidad_aki = cantidad_aki,//param

                            Saldo_anterior = saldo_anterior.saldo,
                            saldo = saldo_actual,
                        };

                        _context.Entry(nuevo_saldo_disponible).State = EntityState.Added;
                        await _context.SaveChangesAsync();
                        foreach (Efectivo efectivo in efectivos)
                        {
                            decimal monto_partial = await Denominacion(efectivo);
                            monto_calculado += monto_partial;
                            efectivo.monto = monto_partial;
                            efectivo.id_caja = caja.Id_Caja;
                            efectivo.id_saldodisponible = nuevo_saldo_disponible.id_saldodisponible;
                            _context.Entry(efectivo).State = EntityState.Added;
                            await _context.SaveChangesAsync();
                        }
                        dbContextTransaction.Commit();
                    }
                    return Ok(faltante);
                }
                catch (Exception ex) {
                    dbContextTransaction.Rollback();
                    return BadRequest(ex.Message);
                }
            }
        }

        /// <summary>
        /// obtiene el ultimo saldo disponible
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Asistente,Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Saldo_Anterior()
        {
            try
            {
                string Id_Usuario = _userManager.GetUserId(HttpContext.User);
                DateTime ahora = DateTime.Now;
                Caja caja = await Boveda_Actual();
                if (caja == null)
                    caja = await Caja_Actual();
                if (caja == null)
                    return BadRequest("No tiene una caja asignada para registrar flujo de efectivo");
                SaldoDisponible saldo_disponible = await Saldo_Anterior(caja.Id_Caja);
                if(saldo_disponible !=null)
                    return Ok(saldo_disponible.saldo);
                return Ok(0.00M);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// obtiene el ultimo saldo disponible
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> Cierre_Dia_Flujo()
        {
            using (var dbContextTransaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    NegocioContexto db = new NegocioContexto();
                  
                    //obtenemos fecha actual para el proceso
                    DateTime ahora = DateTime.Now;
                    //validamos que la persona posea una caja
                    Caja caja = await Boveda_Actual();
                    if (caja == null)
                        caja = await Caja_Actual();
                    if (caja == null)
                        return BadRequest("No tiene una caja asignada para registrar flujo de efectivo");
                    bool existe = db.SaldoDisponible.Any(t => t.fecha.Date == ahora.Date && t.id_caja == caja.Id_Caja);
                    if (!existe)
                    {
                        if (_context.SaldoDisponible.Any(a => a.id_caja == caja.Id_Caja && a.fecha.Date == ahora.Date))
                            return BadRequest("Ya existe un registro para el ejercicio actual para su caja asignada");

                        //obtenemos los datos del flujo de efectivo
                        Reporte_Flujo_Efectivo_Vista datos_flujo = await Flujo_Efectivo_Agencia(ahora.Date, caja.Id_Agencia);
                        //instanciamos la clase de SaldoDisponible
                        SaldoDisponible nuevo_saldo = new SaldoDisponible
                        {
                            fecha = ahora.Date,
                            saldo = datos_flujo.Disponible_a_la_fecha,
                            id_caja = caja.Id_Caja,
                            Cantidad_aki = 0.00M,
                            Cantidad_cheque = 0.00M,
                            Efectivo_total = 0.00M,
                            Numero_cheque = 0.00M,
                            Saldo_anterior = datos_flujo.Saldo_Anterior
                        };
                        await Guardar_Saldo_Disponible(nuevo_saldo);
                        dbContextTransaction.Commit();
                        return Ok("Cierre creado exitosamente");
                    }
                    else
                    {
                        return BadRequest("Ya se realizo el cierre del día");
                    }
                   
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    return BadRequest(ex.Message);
                }
            }
        }

        /// <summary>
        /// realiza sobre el último registro de saldo disponible
        /// el cierre
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Cierre_Mes(DateTime fecha,int id_agencia=0)
        {
            Caja caja = null;
            if (id_agencia != 0 && User.IsInRole("Administrador"))
            {
                caja = _context.Caja.Single(a => a.Id_Agencia == id_agencia && a.Activa && a.Boveda);
                if(caja==null)
                    return BadRequest("No existe una bóveda para la agencia seleccionada");
            }
            else
                return BadRequest("No eres Administrador o no ha seleccionado una agencia");

            if(User.IsInRole("Jefe"))
                caja = await Boveda_Actual();
            if (caja == null)
                return BadRequest("No tiene una bóveda asignada para registrar cierre del mes");
            try
            {
                var saldo_disponible_cierre = _context
                    .SaldoDisponible
                    .Single(d => d.id_caja == caja.Id_Caja && d.fecha.Date==fecha.Date);
                
                if (saldo_disponible_cierre.Cierre)
                    return BadRequest("Ya existe cierre para el mes corriente"); 
                saldo_disponible_cierre.Saldo_Cierre = saldo_disponible_cierre.saldo;
                saldo_disponible_cierre.Cierre = true;
                _context.Entry(saldo_disponible_cierre).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok("Cierre del mes efectuado");
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// para manejar solo el saldo de boveda
        /// </summary>
        /// <param name="id_caja"></param>
        /// <param name="saldo_fisico"></param>
        /// <returns></returns>
        private async Task<Faltante_Caja> Diferencia_Saldos(int id_caja,decimal saldo_fisico)
        {
            Faltante_Caja faltante = null;
            if (_context
                .Transaccion_Caja
                .Any(a => a.Id_Caja == id_caja))
            {

                int id_transaccion_caja_maximo = _context
                    .Transaccion_Caja
                    .Where(a => a.Id_Caja == id_caja)
                    .Max(d => d.Id_Transaccion_Caja);
                Transaccion_Caja transaccion = await _context
                    .Transaccion_Caja
                    .SingleAsync(c => c.Id_Transaccion_Caja == id_transaccion_caja_maximo && !c.Borrado_Logico);
                faltante = new Faltante_Caja(saldo_fisico, transaccion.Saldo);
            }
            else
                faltante = new Faltante_Caja(saldo_fisico, 0.00M);
            return faltante;
        }

        private async Task<SaldoDisponible> Saldo_Anterior(int id_caja)
        {
            try
            {
                int ultimo_saldo = 0;
                Func<SaldoDisponible,bool> filtrar=new Func<SaldoDisponible, bool>(c => c.id_caja == id_caja);
                if (_context.SaldoDisponible.Any(filtrar))
                {
                    ultimo_saldo = _context
                        .SaldoDisponible
                        .AsNoTracking()
                        .Where(filtrar).Max(d => d.id_saldodisponible);

                    SaldoDisponible saldo_disponible = await _context
                        .SaldoDisponible
                        .AsNoTracking()
                        .SingleAsync(c => c.id_saldodisponible == ultimo_saldo);
                    return saldo_disponible;
                }

                return null;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        private async Task<decimal> Denominacion(Efectivo efectivo)
        {
            Denominacion_efectivo denominacion = await _context
                        .Denominacion_Efectivo
                        .AsNoTracking()
                        .SingleAsync(q => q.Id_denominacion == efectivo.id_denominacion);

            return Math.Round(efectivo.cantidad * denominacion.denominacion, 2);
        }


        private async Task<SaldoDisponible> PrepararSaldo(int id_caja)
        {
            if (_context.SaldoDisponible.Any(d => d.id_caja == id_caja))
            {
                int id_saldo_disponible = _context.SaldoDisponible.Where(d => d.id_caja == id_caja).Max(t => t.id_saldodisponible);
                SaldoDisponible saldo_anterior = await _context.SaldoDisponible.SingleAsync(a => a.id_saldodisponible == id_saldo_disponible);
                return saldo_anterior;
            }
            else
            {
                //se guarda uno con saldo cero
                SaldoDisponible saldo = new SaldoDisponible { 
                    id_caja=id_caja,
                    saldo=0.00M,
                    fecha=DateTime.Now
                };
                _context.SaldoDisponible.Add(saldo).State=EntityState.Added;
                await _context.SaveChangesAsync();
                return saldo;
            }
        }

        /*guarda el efectivo disponibles después del ajercicio*/
        private async Task<bool> Guardar_Saldo_Disponible(SaldoDisponible saldo)
        {
            if (!ModelState.IsValid)
                return false;

            _context.Entry(saldo).State = EntityState.Added;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}