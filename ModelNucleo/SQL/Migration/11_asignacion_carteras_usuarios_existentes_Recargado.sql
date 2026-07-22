using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelNucleo.CalculosYProceos;
using ModelNucleo.Model;
using ModelNucleo.Model.Contabilidad;
using ModelNucleo.Model.ModelView;
using ModelNucleo.Util;
using NucleoAyC.Entities;

namespace NucleoAyC.Controllers
{
    /// <summary>
    /// clase que posee método en común para
    /// transacción crédito y ahorro y simple
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = true, GroupName = "Base_Transaccion")]
    public class Base_TransaccionController : Controller
    {
        protected readonly UserManager<IdentityUser> _userManager;
        protected readonly RoleManager<IdentityRole> _roleManager;
        protected readonly decimal IVA = 13.00M;
        protected readonly NegocioContexto _context;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userManager"></param>
        /// <param name="roleManager"></param>
        public Base_TransaccionController(
            NegocioContexto context,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;

            //información del iva
            string iva_str = _context.Configuracion_General.AsNoTracking().Single(d => d.Nombre == "IVA").Valor;
            IVA = Convert.ToDecimal(iva_str);
        }

        /*relativo a usuario, agencia y rol*/
        /// <summary>
        /// retorna el id de la agencia por el usuario que inició sesión
        /// </summary>
        /// <returns></returns>
        protected int Agencia_Usuario()
        {
            string Id_Usuario = _userManager.GetUserId(HttpContext.User);
            return _context.Usuario_Agencia.AsNoTracking().Single(s => s.Id_Usuario == Id_Usuario).Id_Agencia;
        }

        /*relativo a usuario, agencia y rol*/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="transaccion">cualquier tipo de las transaccines</param>
        /// <param name="id_caja">identificador de la caja</param>
        /// <param name="fecha">fecha que se efectúa la transacción</param>
        /// <param name="ingreso">se sabe si es o no un ingreso para la caja</param>
        /// <param name="comprobante">se sabe si es o no un ingreso para la caja</param>
        /// <returns></returns>
        protected Transaccion_Caja Registrar_Caja(object transaccion, int id_caja, DateTime fecha, bool ingreso, string comprobante = "")
        {
            string ingreso_str = "Egreso";
            if (ingreso)
                ingreso_str = "Ingreso";

            int id_transaccion_guardada = 0;
            decimal monto = 0.00M;
            if (transaccion is Transaccion_Credito)
            {
                Transaccion_Credito transaccion_credito = transaccion as Transaccion_Credito;
                id_transaccion_guardada = transaccion_credito.Id_Transaccion_Credito;
                monto = transaccion_credito.Valor_Cuota;
            }

            if (transaccion is Transaccion_Ahorro)
            {
                Transaccion_Ahorro transaccion_ahorro = transaccion as Transaccion_Ahorro;
                id_transaccion_guardada = transaccion_ahorro.Id_Transaccion_Ahorro;
                monto = transaccion_ahorro.Valor;
            }

            if (transaccion is Transaccion_Simple)
            {
                Transaccion_Simple transaccion_simple = transaccion as Transaccion_Simple;
                id_transaccion_guardada = transaccion_simple.Id_Transaccion_Simple;
                monto = transaccion_simple.Monto;
            }

            //int id_caja = _context.Caja.FirstOrDefault(y => y.Id_Usuario == Id_Usuario && y.Activa && !y.Boveda).Id_Caja;
            int id_tipo_trans = _context.Tipo_Transaccion.SingleOrDefault(t => t.Descripcion == ingreso_str).Id_Tipo_Transaccion;
            int id_ultimo = 0;
            if (_context.Transaccion_Caja.Any())
                id_ultimo = _context.Transaccion_Caja.Max(i => i.Id_Transaccion_Caja);
            //string comprobante = new GeneradorCodigo().GenerarSoloPorEntidad(id_ultimo);

            Transaccion_Caja ultima_transaccion = new Transaccion_Caja();
            Func<Transaccion_Caja, bool> filtrar_transaccion = new Func<Transaccion_Caja, bool>(t => t.Id_Caja == id_caja && !t.Borrado_Logico);
            if (_context.Transaccion_Caja.Any(filtrar_transaccion))
            {
                ultima_transaccion = _context.
                    Transaccion_Caja.
                    Where(filtrar_transaccion).AsEnumerable().Last();
            }

            decimal monto_transaccion = transaccion != null ? monto : 0.00M;
            decimal saldo_anterior = ultima_transaccion != null ? ultima_transaccion.Saldo : 0.00M;
            string id_usuario = _userManager.GetUserId(HttpContext.User);

            return new Transaccion_Caja
            {
                Id_Transaccion_Credito = transaccion is Transaccion_Credito ? id_transaccion_guardada : 0,
                Id_Transaccion_Ahorro = transaccion is Transaccion_Ahorro ? id_transaccion_guardada : 0,
                Id_Transaccion_Simple = transaccion is Transaccion_Simple ? id_transaccion_guardada : 0,
                Id_Caja = id_caja,
                Id_Tipo_Transaccion = id_tipo_trans,
                Monto = monto_transaccion,
                Saldo = ingreso ? saldo_anterior + monto_transaccion : saldo_anterior - monto_transaccion,
                Fecha = fecha.Date,
                No_Comprobante = comprobante,
                Creado_En = fecha,
                Creado_Por = id_usuario
            };
        }

        /// <summary>
        /// Asiento contable de un pago de crédito
        /// </summary>
        /// <param name="transaccion"></param>
        /// <param name="id_transaccion_caja"></param>
        /// <returns></returns>
        protected async Task Pago_Credito_Contabilidad(Transaccion_Credito transaccion, int id_transaccion_caja)
        {

            int id_agencia = Agencia_Usuario();
            Transaccion_Caja transaccion_caja = await _context
                .Transaccion_Caja
                .SingleAsync(n => n.Id_Transaccion_Caja == id_transaccion_caja);

            int id_linea = _context.Transaccion_Credito.
                Include(t => t.Credito).
                Include(d => d.Credito.Linea_Credito).
                Single(s => s.Id_Transaccion_Credito == transaccion_caja.Id_Transaccion_Credito).Credito.Id_Linea;

            Conf_Partida_Contable partida = await _context
                .Conf_Partida_Contable
                .Include(r => r.Conf_Detalles_Partidas_Contables)
                .SingleAsync(t => t.Identificador == "PAGO_CREDITO" && t.Id_Agencia == id_agencia && t.Id_Linea_Credito == id_linea);

            Caja caja = await Caja_Actual(transaccion_caja.Id_Caja);

            if (partida != null)
            {
                //guardar el asiento
                Asiento_Contable asiento_contable = new Asiento_Contable
                {
                    Id_Transaccion_Caja = id_transaccion_caja,
                    Fecha = DateTime.Now,
                    Concepto = "Pago de cuota de crédito"
                };
                _context.Asiento_Contable.Add(asiento_contable).State = EntityState.Added;
                await _context.SaveChangesAsync();
                if (partida.Conf_Detalles_Partidas_Contables != null)
                    foreach (Conf_Detalle_Partida_Contable detalle in partida.Conf_Detalles_Partidas_Contables.ToList())
                    {
                        if (detalle.Identificador == "CAPITAL")
                            _context.Detalle_Asiento_Contable.Add(Armar_Detalle_Asiento(
                                asiento_contable,
                                transaccion.Abono_capital,
                                detalle.Cargo,
                                detalle.Concepto,
                                detalle.Cuenta_Contable)).State = EntityState.Added;

                        if (detalle.Identificador == "INTERES")
                            _context.Detalle_Asiento_Contable.Add(Armar_Detalle_Asiento(
                                asiento_contable,
                                transaccion.Interes,
                                detalle.Cargo,
                                detalle.Concepto,
                                detalle.Cuenta_Contable)).State = EntityState.Added;

                        if (detalle.Identificador == "COMISION")
                            _context.Detalle_Asiento_Contable.Add(Armar_Detalle_Asiento(
                               asiento_contable,
                               transaccion.Interes_Comision,
                               detalle.Cargo,
                               detalle.Concepto,
                               detalle.Cuenta_Contable)).State = EntityState.Added;

                        if (detalle.Identificador == "IVA_COMISION")
                            _context.Detalle_Asiento_Contable.Add(Armar_Detalle_Asiento(
                                asiento_contable,
                                transaccion.IvaSobreComision,
                                detalle.Cargo,
                                detalle.Concepto,
                                detalle.Cuenta_Contable)).State = EntityState.Added;

                        await _context.SaveChangesAsync();
                    }

                _context.Detalle_Asiento_Contable.Add(Armar_Detalle_Asiento(
                    asiento_contable,
                    transaccion.Valor_Cuota,
                    true,
                    partida.Descripcion,
                    caja.Cuenta_Contable,
                    true)).State = EntityState.Added;
                await _context.SaveChangesAsync();
            }
            else
                throw new Exception("No posee configuración contable para esa agencia");
        }


        /// <summary>
        /// guarda las partidas para el ahorro
        /// </summary>
        /// <returns></returns>
        protected async Task Manejo_Contable_Ahorro(bool retiro, bool interes, Transaccion_Ahorro transaccion, int id_transaccion_caja)
        {
            int id_agencia = Agencia_Usuario();
            Ahorro ahorro = await _context.Ahorro.SingleAsync(t => t.Id_Ahorro == transaccion.Id_Ahorro);

            string identificador = "";
            string concepto = "";
            bool cargo = false;//solo para la caja
            if (retiro)
            {
                identificador = "RETIRO_AHORRO";
                concepto = "Retiro de cuenta de ahorro";
            }
            else
            {
                if (interes)
                {
                    identificador = "DEPOSITO_INTERES_AHORRO";
                    concepto = "Depósito de intereses a cuenta de ahorro";
                }
                else
                {
                    identificador = "DEPOSITO_AHORRO";
                    concepto = "Depósito de cuenta de ahorro";
                    cargo = true;
                }
            }

            Conf_Partida_Contable partida = await _context
                .Conf_Partida_Contable
                .Include(r => r.Conf_Detalles_Partidas_Contables)
                .SingleAsync(t => t.Identificador == identificador && t.Id_Agencia == id_agencia && t.Id_Linea_Ahorro == ahorro.Id_Linea_Ahorro);


            Asiento_Contable asiento_contable = new Asiento_Contable
            {
                Id_Transaccion_Caja = id_transaccion_caja,
                Fecha = DateTime.Now,
                Concepto = concepto
            };
            _context.Asiento_Contable.Add(asiento_contable).State = EntityState.Added;
            await _context.SaveChangesAsync();

            if (partida.Conf_Detalles_Partidas_Contables != null)
                foreach (Conf_Detalle_Partida_Contable detalle in partida.Conf_Detalles_Partidas_Contables.ToList())
                {
                    _context.Detalle_Asiento_Contable.Add(Armar_Detalle_Asiento(
                        asiento_contable,
                        transaccion.Valor,
                        detalle.Cargo,
                        concepto,
                        detalle.Cuenta_Contable)).State = EntityState.Added;
                    await _context.SaveChangesAsync();
                }
            Caja caja = await Caja_Actual();

            if (!interes)
            {
                _context.Detalle_Asiento_Contable.Add(Armar_Detalle_Asiento(
                    asiento_contable,
                    transaccion.Valor,
                    cargo,
                    concepto,
                    caja.Cuenta_Contable,
                    true)).State = EntityState.Added;
                await _context.SaveChangesAsync();
            }
        }


        /// <summary>
        /// manejo contable de las deducciones
        /// </summary>
        /// <param name="id_transaccion_simple"></param>
        /// <param name="id_linea_credito"></param>
        /// <param name="id_linea_ahorro"></param>
        /// <param name="otro"></param>
        /// <returns></returns>
        protected async Task Manejo_Contabilidad_Otros_Cobros(
            int id_transaccion_simple,
            Otro_Cobro otro,
            int id_linea_credito = 0,
            int id_linea_ahorro = 0)
        {
            Transaccion_Caja transaccion = await _context.
                Transaccion_Caja.
                AsNoTracking().
                Include(q => q.Caja).
                SingleAsync(f => f.Id_Transaccion_Simple == id_transaccion_simple);

            int id_caja = transaccion.Id_Caja;
            int id_agencia = transaccion.Caja.Id_Agencia;

            string Identificador = GetDisplayName(otro);

            Conf_Partida_Contable partida = await _context
                .Conf_Partida_Contable
                .Include(r => r.Conf_Detalles_Partidas_Contables)
                .SingleAsync(
                    t => t.Identificador == Identificador &&
                    t.Id_Agencia == id_agencia &&
                    (
                        (t.Id_Linea_Credito == id_linea_credito && id_linea_credito > 0) ||
                        (t.Id_Linea_Ahorro == id_linea_ahorro && id_linea_ahorro > 0)

                    )
                );
            string concepto = partida.Descripcion;
            Asiento_Contable asiento_contable = new Asiento_Contable
            {
                Id_Transaccion_Caja = transaccion.Id_Transaccion_Caja,
                Fecha = DateTime.Now,
                Concepto = concepto
            };

            _context.Asiento_Contable.Add(asiento_contable).State = EntityState.Added;
            await _context.SaveChangesAsync();

            if (partida.Conf_Detalles_Partidas_Contables != null)
                foreach (Conf_Detalle_Partida_Contable detalle in partida.Conf_Detalles_Partidas_Contables.ToList())
                {
                    _context.Detalle_Asiento_Contable.Add(Armar_Detalle_Asiento(
                        asiento_contable,
                        transaccion.Monto,
                        detalle.Cargo,
                        concepto,
                        detalle.Cuenta_Contable)).State = EntityState.Added;
                    await _context.SaveChangesAsync();
                }

            Caja caja = await Caja_Actual();

            _context.Detalle_Asiento_Contable.Add(Armar_Detalle_Asiento(
                asiento_contable,
                transaccion.Monto,
                true,
                concepto,
                caja.Cuenta_Contable,
                true)).State = EntityState.Added;
            await _context.SaveChangesAsync();
        }

        private string GetDisplayName(Enum enumValue)
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<DisplayAttribute>()
                            .GetName();
        }

        private Detalle_Asiento_Contable Armar_Detalle_Asiento(
            Asiento_Contable asiento_contable,
            decimal valor,
            bool cargo,
            string concepto,
            string cuenta_contable,
            bool esCaja = false)
        {
            return new Detalle_Asiento_Contable
            {
                Id_Asiento_Contable = asiento_contable.Id_Asiento_Contable,
                Id_Conf_Detalle_Partida_Contable = 0,
                Abono = !cargo ? valor : 0.00M,
                Cargo = cargo ? valor : 0.00M,
                Concepto = concepto,
                Cuenta_Contable = cuenta_contable,
                EsCaja = esCaja
            };
        }


        /// <summary>
        /// se efectúa una reversión
        /// </summary>
        /// <param name="id_transaccion"></param>
        /// <param name="es_credito"></param>
        /// <returns></returns>
        protected async Task Reversion(int id_transaccion, bool es_credito = true)
        {
            try
            {
                DateTime fecha = DateTime.Now;
                string id_usuario = _userManager.GetUserId(HttpContext.User);

                if (es_credito)
                {
                    Transaccion_Credito tc = _context.Transaccion_Credito.SingleOrDefault(y => y.Id_Transaccion_Credito == id_transaccion);
                    tc.Borrado_Logico = true;
                    tc.Eliminado_En = fecha;
                    tc.Eliminado_Por = id_usuario;
                    _context.Entry(tc).State = EntityState.Modified;
                }
                else
                {
                    Transaccion_Ahorro ta = _context.Transaccion_Ahorro.SingleOrDefault(y => y.Id_Transaccion_Ahorro == id_transaccion);
                    ta.Borrado_Logico = true;
                    ta.Eliminado_En = fecha;
                    ta.Eliminado_Por = id_usuario;
                    _context.Entry(ta).State = EntityState.Modified;
                }
                /*cuando esta en transaccion de caja hay que eliminado*/
                Transaccion_Caja transaccion_caja = new Transaccion_Caja();
                if (es_credito)
                    transaccion_caja = _context.Transaccion_Caja.SingleOrDefault(t => t.Id_Transaccion_Credito == id_transaccion);
                else
                    transaccion_caja = _context.Transaccion_Caja.SingleOrDefault(t => t.Id_Transaccion_Ahorro == id_transaccion);

                transaccion_caja.Borrado_Logico = true;
                transaccion_caja.Eliminado_En = fecha;
                transaccion_caja.Eliminado_Por = id_usuario;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Reversión solo de manejo de bóvedas
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        protected async Task Reversion_Simple(List<int> ids)
        {
            DateTime fecha = DateTime.Now;
            string id_usuario = _userManager.GetUserId(HttpContext.User);
            try
            {
                foreach (int id_transaccion in ids)
                {
                    Transaccion_Simple ts = _context.Transaccion_Simple.SingleOrDefault(y => y.Id_Transaccion_Simple == id_transaccion);
                    ts.Borrado_Logico = true;
                    ts.Eliminado_En = fecha;
                    ts.Eliminado_Por = id_usuario;
                    _context.Entry(ts).State = EntityState.Modified;

                    /*registro contable*/
                    await Contabilidad_Reversion_Simple(id_transaccion);/*nuevo hay que probarlo*/

                    /*cuando esta en transaccion de caja hay que eliminado*/
                    Transaccion_Caja transaccion_caja = _context.Transaccion_Caja.SingleOrDefault(t => t.Id_Transaccion_Simple == id_transaccion);
                    transaccion_caja.Borrado_Logico = true;
                    transaccion_caja.Eliminado_En = fecha;
                    transaccion_caja.Eliminado_Por = id_usuario;
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Registro Contable 
        /// </summary>
        /// <param name="id_transaccion_simple"></param>
        /// <returns></returns>
        protected async Task Contabilidad_Reversion_Simple(int id_transaccion_simple)
        {
            var transaccion = (from s in _context.Transaccion_Simple
                               join c in _context.Transaccion_Caja 
                                    on s.Id_Transaccion_Simple equals c.Id_Transaccion_Simple
                               where s.Id_Transaccion_Simple == id_transaccion_simple && !c.Borrado_Logico
                               select new { 
                                    c.Id_Transaccion_Caja
                               }).FirstOrDefault();
            var asientos = (from a in _context.Asiento_Contable
                            join d in _context.Detalle_Asiento_Contable
                                    on a.Id_Asiento_Contable equals d.Id_Asiento_Contable
                            where a.Id_Transaccion_Caja==transaccion.Id_Transaccion_Caja
                            select new {
                                a,
                                d
                            }).ToList();

            /*crear asiento de la reversión*/
            var asiento_reversion = asientos.FirstOrDefault().a;

            AutoMapper.MapperConfiguration config = new AutoMapper
                .MapperConfiguration(cfg => cfg.CreateMap<Asiento_Contable, Asiento_Contable>());
            AutoMapper.IMapper mapper = config.CreateMapper();
            var asiento_revertido = mapper.Map<Asiento_Contable>(asiento_reversion);
            asiento_revertido.Id_Asiento_Contable = 0;
            asiento_revertido.Concepto = "Reversión de Liquidación de crédito";

            _context.Add(asiento_revertido).State = EntityState.Added;
            await _context.SaveChangesAsync();

            /*transformar y guardar los detalles*/
            foreach (var detalle in asientos) 
            {
                AutoMapper.MapperConfiguration configd = new AutoMapper
                    .MapperConfiguration(cfg => cfg.CreateMap<Detalle_Asiento_Contable, Detalle_Asiento_Contable>());
                AutoMapper.IMapper mapperd = configd.CreateMapper();
                var detalle_revertido = mapperd.Map<Detalle_Asiento_Contable>(detalle.d);
                detalle_revertido.Id_Detalle_Asiento_Contable = 0;
                if (detalle_revertido.Abono > 0)
                    detalle_revertido.Cargo = detalle_revertido.Abono;
                else
                    detalle_revertido.Abono = detalle_revertido.Cargo;
                detalle_revertido.Concepto = "Reversión de liquidación de crédito";
                _context.Add(detalle_revertido).State=EntityState.Added;
                await _context.SaveChangesAsync();
            }
        }


        protected async Task<Caja> Boveda_Actual()
        {
            string Id_Usuario = _userManager.GetUserId(HttpContext.User);
            Usuario_Agencia relacion = await _context.Usuario_Agencia.SingleOrDefaultAsync(c => c.Id_Usuario == Id_Usuario);
            return await _context.Caja.SingleOrDefaultAsync(k => k.Id_Usuario == Id_Usuario && k.Id_Agencia == relacion.Id_Agencia && k.Boveda && k.Activa);
        }

        protected async Task<Caja> Caja_Actual(int id_caja = 0)
        {
            string Id_Usuario = _userManager.GetUserId(HttpContext.User);
            Usuario_Agencia relacion = await _context.Usuario_Agencia.SingleOrDefaultAsync(c => c.Id_Usuario == Id_Usuario);
            Caja caja = null;
            if (id_caja > 0)
                caja = await _context.Caja.SingleOrDefaultAsync(k => k.Id_Agencia == relacion.Id_Agencia && k.Id_Caja == id_caja && !k.Boveda && k.Activa);
            else
                caja = await _context.Caja.SingleOrDefaultAsync(k => k.Id_Usuario == Id_Usuario && k.Id_Agencia == relacion.Id_Agencia && !k.Boveda && k.Activa);
            if (caja == null)
                throw new Exception("El usuario no posee una caja");
            return caja;
        }

        protected async Task<DateTime> Fecha_Pago(int periodo_dias, int id_credito)
        {
            int dias = new CalculosYProcesoCredito().CalculoDias(periodo_dias);
            DateTime ultima_fecha = _context
                .Transaccion_Credito
                .Where(y => y.Id_Credito == id_credito && !y.Borrado_Logico)
                .AsEnumerable()
                .Last().FechaPago;

            return ultima_fecha.Date.AddDays(dias);
        }


        /*relacionado a la amortizacion*/
        protected Transaccion_Credito AmortizacionActualProc(int id_credito, DateTime fecha, decimal? valor_cuota = null, decimal? saldo_capital = null, bool recargo = false)
        {
            AmortizacionVolatil a_retorna = null;

            /*traer el iva legislado de las configuraciones*/
            decimal iva = 13.00M;
            Credito credito = _context.Credito
                .Include(d => d.Linea_Credito_Historial)
                .Include(p => p.Periodo)
                .Include(l => l.Linea_Credito_Historial.Cobros_Periodicos_Historial).SingleOrDefault(c => c.Id_Credito == id_credito);
            Periodo frecuencia = credito.Periodo;
            Linea_Credito_Historial historial = Historial(credito.Id_Credito);
            /*veremos si hay que cambiar el % a dinero*/
            CalculosYProcesoCredito cpc = new CalculosYProcesoCredito();
            if (historial.PlanAyC_Cobros_Periodico_Dinero)
            {
                decimal total = credito.Linea_Credito_Historial.Cobros_Periodicos_Historial.Count > 0 ? credito.Linea_Credito_Historial.Cobros_Periodicos_Historial.Sum(t => t.Valor) : 0.00M;

                cpc = new CalculosYProcesoCredito(
                historial.Interes_Comision,
                credito.Monto_pago,
                frecuencia,
                IVA,//no se había actualizado el iva
                credito.Plazo,
                historial.Interes,
                credito.Liquidado_En,
                total,
                true,
                pago_anticipado: historial.Cancelacion_anticipada);
            }
            else
                cpc = new CalculosYProcesoCredito(
                historial.Interes_Comision,
                credito.Monto_pago,
                frecuencia,
                iva,
                credito.Plazo,
                historial.Interes,
                credito.Liquidado_En);
            bool hayTransacciones = _context.Transaccion_Credito.Any(t => t.Id_Credito == id_credito && !t.Borrado_Logico);

            if (hayTransacciones)
                a_retorna = PagosConsecuentes(cpc, credito, historial, fecha, frecuencia, valor_cuota,recargo/*,saldo_capital*/);
            else
                a_retorna = PrimerPagoPuntualVencido(cpc, credito, historial, fecha, valor_cuota);

            return new Transaccion_Credito
            {
                No = a_retorna.No,
                Id_Transaccion_Credito = 0,
                Id_Tipo_Operecion_Credito = 1,
                Id_Credito = id_credito,
                FechaPago = a_retorna.FechaPago,
                Interes_Comision = a_retorna.Interes,
                Interes = a_retorna.Comision,
                IvaSobreComision = a_retorna.IvaSobreComision,
                Otros_Cobros = a_retorna.Otros_Cobros,
                Abono_capital = a_retorna.Abono_capital,
                Valor_Cuota = a_retorna.Valor_Cuota,
                Saldo_Capital = a_retorna.Saldo_Capital,
                Dias = a_retorna.DiasTranscurridos,
                Anticipado = a_retorna.Anticipado,
                Puntual = a_retorna.Puntual,
                Vencido = a_retorna.Vencido,
                Pagos_Acumulado = a_retorna.Pagos_Acumulado,
                Recargo = a_retorna.Recargo,
                Pago_para_cancelar = a_retorna.Pago_para_cancelar
            };
        }

        protected AmortizacionVolatil PagosConsecuentes(CalculosYProcesoCredito cpc, Credito credito, Linea_Credito_Historial historial, DateTime fecha, Periodo frecuencia, decimal? valor_cuota,bool pago_recargo = false/*,decimal? saldo_capital*/)
        {
            bool puntual = false, vencido = false, anticipado = false;
            decimal recargo = 0;
            int pagos_acumulados = 0;/*pagos vencidos*/

            Transaccion_Credito tr = _context.Transaccion_Credito
                  .Where(t => t.Id_Credito == credito.Id_Credito && !t.Borrado_Logico)
                  .OrderByDescending(f => f.Id_Transaccion_Credito).FirstOrDefault();
            /*contar registros*/
            int no_transaccion = _context.Transaccion_Credito.Count(r => r.Id_Credito == credito.Id_Credito && !r.Borrado_Logico);
            TimeSpan diferencias_amortizacion;
            Amortizacion ultima_amortizacion_cancelada = null;

            //conocer ultimo pago realizado
            DateTime ultimo_pago = tr.FechaPago.Date;
            //dias transcurridos desde el ultimo pago
            TimeSpan diferencia_dias_liquidacion = fecha.Date - ultimo_pago.Date;
            //dias a enteros
            int diferencia_dias = diferencia_dias_liquidacion.Days;
            //obtener la frecuencia 
            int frecuencia_pago = frecuencia.dias;
            //verificar tipo de pago (puntual/adelantado/atrasado)
            double validar = (double) diferencia_dias / frecuencia_pago;

            int dias = 0;
            int numero_cuotas = Convert.ToInt32(Math.Ceiling(cpc.CalcularNoCuotas(credito.Plazo, frecuencia.Valor)));
            DateTime fecha_cercana = Fecha_Cercana_Simple(credito.Id_Credito, fecha.Date);

            if (validar < 1)/*cuando es adelantado*/
            {
                ultima_amortizacion_cancelada = cpc.Amortizacion.Where(t => t.FechaPago.Date == tr.FechaPago.Date).Last();
                diferencias_amortizacion = fecha.Date - ultima_amortizacion_cancelada.FechaPago.Date;
                dias = diferencias_amortizacion.Days;
                anticipado = true;
            }
            if (validar == 1)/*cuando es puntual*/
            {
                ultima_amortizacion_cancelada = cpc.Amortizacion.Where(t => t.FechaPago.Date == tr.FechaPago.Date).Last();
                diferencias_amortizacion = fecha.Date - ultima_amortizacion_cancelada.FechaPago.Date;
                dias = diferencias_amortizacion.Days;
                puntual = true;
            }
            if (validar > 1)// pago retrasado
            {
           
                    int pagos_vencidos = (int)validar;

                    /*revisar si tenemos mayor a los acordados */
                    if (pagos_vencidos > numero_cuotas)
                        pagos_vencidos = numero_cuotas - no_transaccion;

                    IEnumerable<Amortizacion> rango_pago = cpc.Amortizacion.Skip(no_transaccion).Take(pagos_vencidos);/*solo para saber cuantas cuotas debe*/
                    pagos_acumulados = rango_pago.Count();
                    //es donde se trata el recargo configurado para el crédito
                    decimal recargo_por_frecuencia = historial.Recargo;
                    recargo = recargo_por_frecuencia * pagos_acumulados;
                    //validar limite de recargos
                    Configuracion_General limite_recargo =
                        _context.Configuracion_General.AsNoTracking().Single(a => a.Nombre == "limite_recargo");
                    if (recargo > decimal.Parse(limite_recargo.Valor))
                    {
                        recargo = decimal.Parse(limite_recargo.Valor);
                    }
                    dias = diferencia_dias;
                    vencido = true;
                    ultima_amortizacion_cancelada = cpc.Amortizacion.SingleOrDefault(t => t.FechaPago == fecha_cercana);
                    if (pago_recargo)
                        recargo = 0.00M;
                    ultima_amortizacion_cancelada.Valor_Cuota = ultima_amortizacion_cancelada.Valor_Cuota + recargo;
            }

            if (ultima_amortizacion_cancelada == null)
                ultima_amortizacion_cancelada = cpc.Amortizacion.OrderBy(a => a.No).FirstOrDefault();

            /*los siguientes anticipados no cobran plan ayc ni iva*/
            bool libre = tr.Anticipado && anticipado;

            Amortizacion a_retorna = cpc.CalculoAmortizacionFila(
                    no_transaccion + 1,
                    tr.Saldo_Capital,
                    valor_cuota != null ? Convert.ToDecimal(valor_cuota) : ultima_amortizacion_cancelada.Valor_Cuota,
                    Math.Abs(dias),
                    historial.Interes,
                    fecha,
                    libre ? 0.00M : ultima_amortizacion_cancelada.Comision,
                    libre ? 0.00M : ultima_amortizacion_cancelada.IvaSobreComision,
                    libre ? 0.00M : recargo,
                    true,
                    true,
                    pago_anticipado: historial.Cancelacion_anticipada
                    );

            return new AmortizacionVolatil
            {
                No = a_retorna.No,
                FechaPago = a_retorna.FechaPago,
                Interes = a_retorna.Interes,
                Comision = a_retorna.Comision,
                IvaSobreComision = a_retorna.IvaSobreComision,
                Otros_Cobros = a_retorna.Otros_Cobros,
                Abono_capital = a_retorna.Abono_capital,
                Valor_Cuota = a_retorna.Valor_Cuota,
                Saldo_Capital = a_retorna.Saldo_Capital,
                Puntual = puntual,
                Vencido = vencido,
                Anticipado = anticipado,
                DiasTranscurridos = Math.Abs(dias),
                Pagos_Acumulado = pagos_acumulados,
                Recargo = recargo,
                Pago_para_cancelar = a_retorna.Pago_para_cancelar
            };
        }

        /// <summary>
        /// Se usa si no hay pago o si incluso ya se vencío el primer pago
        /// </summary>
        /// <param name="cpc"></param>
        /// <param name="credito"></param>
        /// <param name="fecha"></param>
        /// <param name="valor_cuota"></param>
        /// <returns></returns>
        protected AmortizacionVolatil PrimerPagoPuntualVencido(CalculosYProcesoCredito cpc, Credito credito, Linea_Credito_Historial historial, DateTime fecha, decimal? valor_cuota)
        {
            Amortizacion a_retorna = new Amortizacion();
            bool vencido = false, puntual = false, anticipado = false;
            int dias_transcurridos = 0;
            Amortizacion primera_amortizacion = cpc.Amortizacion.FirstOrDefault(f => f.No == 1);
            if (primera_amortizacion == null)//por la migración no se encuentra hay que intentar con el segundo
                primera_amortizacion = cpc.Amortizacion.FirstOrDefault(f => f.No == 2);

            /*siendo el primer pago el retazado se ve la liquidacion*/
            TimeSpan diferencia_dias_liquidacion = primera_amortizacion.FechaPago.Date - credito.Liquidado_En.Date;
            TimeSpan diferencias_amortizacion = fecha.Date - primera_amortizacion.FechaPago.Date;
            int total_dias = credito.Periodo.dias==1? diferencias_amortizacion.Days: diferencia_dias_liquidacion.Days;

            if (diferencias_amortizacion.Days == 0)/*puntual*/
            {
                a_retorna = primera_amortizacion;
                dias_transcurridos = total_dias;
                puntual = true;
            }

            if (diferencias_amortizacion.Days > 0)/*se pasó del día del primer pago del crédito*/
            {
                //trabajar el index considerando los pagos que son diarios
                Amortizacion aldia_amortizacion = cpc.Amortizacion.SingleOrDefault(d => d.FechaPago.Date == fecha.Date);
                //obtendremos el valor al día si existe
                decimal valor_cuota_procesada = 0.00M;
                if (aldia_amortizacion != null)
                    valor_cuota_procesada = valor_cuota != null ? Convert.ToDecimal(valor_cuota) : primera_amortizacion.Valor_Cuota * (aldia_amortizacion.No - 1);
                else
                    valor_cuota_procesada = valor_cuota != null ? Convert.ToDecimal(valor_cuota) : primera_amortizacion.Valor_Cuota;

                a_retorna = cpc.CalculoAmortizacionFila(
                    0,
                    primera_amortizacion.Saldo_Capital,
                    valor_cuota_procesada,
                    total_dias,
                    historial.Interes,
                    fecha.Date,
                    primera_amortizacion.Comision,
                    primera_amortizacion.IvaSobreComision,
                    0.00M,
                    actual: true,
                    mismaFecha: true,
                    pago_anticipado: historial.Cancelacion_anticipada
                    );
                vencido = true;
                dias_transcurridos = total_dias;
            }

            if (diferencias_amortizacion.Days < 0)/*se consulta antes del pago*/
            {
                a_retorna = cpc.CalculoAmortizacionFila(
                    0,
                    primera_amortizacion.Saldo_Capital,
                    valor_cuota != null ? Convert.ToDecimal(valor_cuota) : primera_amortizacion.Valor_Cuota,
                    total_dias,
                    historial.Interes,
                    fecha.Date,
                    primera_amortizacion.Comision,
                    primera_amortizacion.IvaSobreComision,
                    0.00M,
                    actual:true,
                    mismaFecha:true,
                    pago_anticipado: historial.Cancelacion_anticipada
                    );

                anticipado = true;
                dias_transcurridos = total_dias;
            }
            return new AmortizacionVolatil
            {
                No = a_retorna.No,
                FechaPago = a_retorna.FechaPago,
                Interes = a_retorna.Interes,
                Comision = a_retorna.Comision,
                IvaSobreComision = a_retorna.IvaSobreComision,
                Otros_Cobros = a_retorna.Otros_Cobros,
                Abono_capital = a_retorna.Abono_capital,
                Valor_Cuota = a_retorna.Valor_Cuota,
                Saldo_Capital = a_retorna.Saldo_Capital,
                Puntual = puntual,
                Vencido = vencido,
                Anticipado = anticipado,
                DiasTranscurridos = dias_transcurridos,
                Pagos_Acumulado = 0,
                Pago_para_cancelar = a_retorna.Pago_para_cancelar
            };
        }

        protected DateTime Fecha_Cercana_Simple(int id_credito, DateTime fecha_comparar)
        {
            Linea_Credito_Historial historial = Historial(id_credito);
            try
            {
                Credito credito = _context.Credito.AsNoTracking()
                    .Include(s => s.Linea_Credito_Historial)
                    .Include(q => q.Linea_Credito)
                    .Include(s => s.Periodo)
                    .Include(t => t.Linea_Credito_Historial.Cobros_Periodicos_Historial)
                    .FirstOrDefault(f => f.Id_Credito == id_credito);
                ResultadoCalculoCreditio cpc = new CalculosYProcesoCredito(
                    historial.Interes_Comision,
                    credito.Monto_pago,
                    credito.Periodo,
                    IVA,
                    credito.Plazo,
                    historial.Interes,
                    credito.Liquidado_En,
                    historial.Cobros_Periodicos_Historial.Count > 0 ? credito.Linea_Credito_Historial.Cobros_Periodicos_Historial.Sum(v => v.Valor) : 0.00M,
                    historial.PlanAyC_Cobros_Periodico_Dinero).Resultado;
                List<DateTime> fechas_desordenadas = cpc.Amortizacion.Select(l => l.FechaPago).ToList();
                if (fechas_desordenadas.Any(t => t.Date == fecha_comparar.Date))
                    return fecha_comparar;
                return EncontrarFechaCercana(fechas_desordenadas, fecha_comparar);
            }
            catch (Exception ex)
            {
                return DateTime.Now;
            }
        }

        protected DateTime EncontrarFechaCercana(List<DateTime> fechas, DateTime hoy)
        {
            List<Fecha_Ordenada> fechas_ordenadas = new List<Fecha_Ordenada>();
            foreach (DateTime item in fechas)
            {
                /*sacar la la diferencia de fechas*/
                fechas_ordenadas.Add(new Fecha_Ordenada
                {
                    No = fechas_ordenadas.Count > 0 ? fechas_ordenadas.Count + 1 : 1,
                    Fecha = item.Date,
                    Dias = (hoy.Date - item.Date).Days
                });
            }

            int dias_menor_futuro = fechas_ordenadas.Any(s => s.Dias < 0) ? fechas_ordenadas.Where(s => s.Dias < 0).Max(m => m.Dias) : 0;
            int dias_menor_pasado = fechas_ordenadas.Any(s => s.Dias > 0) ? fechas_ordenadas.Where(s => s.Dias > 0).Min(m => m.Dias) : 0;
            int dias_menor = 0;
            if (dias_menor_futuro < dias_menor_pasado && dias_menor_futuro != 0)
                dias_menor = dias_menor_futuro;
            else
                dias_menor = dias_menor_pasado;

            return fechas_ordenadas.FirstOrDefault(q => q.Dias == dias_menor).Fecha;
        }

        protected Linea_Credito_Historial Historial(int id_credito)
        {
            return _context
                .Linea_Credito_Historial
                .Include(l => l.Cobros_Periodicos_Historial)
                .Single(c => c.Id_Credito == id_credito);
        }

        protected async Task<string> Generar_No_Comprobante()
        {
            //generador de comprobante
            int id_ultima_transaccion = 0;
            if (await _context.Transaccion_Caja.AnyAsync())
                id_ultima_transaccion = _context.Transaccion_Caja.AsEnumerable().Last().Id_Transaccion_Caja;
            GeneradorCodigo generador = new GeneradorCodigo();
            return generador.GenerarSoloPorEntidad(id_ultima_transaccion);
            //termina generador de comprobante
        }

        ///*con las acciones*/


        protected async Task<bool> Nueva_Accion(Accion accion)
        {
            if (!ModelState.IsValid)
                return false;
            try
            {
                bool ya_tiene = _context.Accion.Any(f => f.Id_Persona == accion.Id_Persona);
                if (!ya_tiene)
                {
                    int numero = UltimoIndice + 1;
                    accion.Codigo = numero.ToString();
                    accion.Cantidad_Accion = 1;//por defecto solo se compra una
                    //accion.No_Certificado = numero;
                    accion.Valor_Total = accion.Valor_Unitario * accion.Cantidad_Accion;
                    _context.Accion.Add(accion);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        protected int UltimoIndice
        {
            get
            {
                if (_context.Accion.AsNoTracking().Any())
                    return _context.Accion.AsNoTracking().Max(m => m.Id_Accion);
                return 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="EsCredito">es o no crédito</param>
        /// <param name="id"> id_credito o id_ahorro</param>
        /// <param name="dias">días de antiguedad</param>
        /// <returns></returns>
        protected async Task<DateTime> LapsoDias(bool EsCredito, int id, int dias)
        {
            DateTime ultima_fecha = DateTime.Now;
            if (EsCredito)
            {
                var credito = await _context.Credito.AsNoTracking().SingleAsync(f => f.Id_Credito == id);
                if (_context.Transaccion_Credito.AsNoTracking().Any(d => d.Id_Credito == credito.Id_Credito))
                    ultima_fecha = _context.Transaccion_Credito
                    .AsNoTracking().Where(d => d.Id_Credito == credito.Id_Credito).AsEnumerable().Last().FechaPago;
            }
            else
            {
                var ahorro = await _context.Ahorro.AsNoTracking().SingleAsync(f => f.Id_Ahorro == id);
                if (_context.Transaccion_Ahorro.AsNoTracking().Any(d => d.Id_Ahorro == ahorro.Id_Ahorro))
                    ultima_fecha = _context.Transaccion_Ahorro
                    .AsNoTracking().Where(d => d.Id_Ahorro == ahorro.Id_Ahorro).AsEnumerable().Last().Fecha;
            }

            return ultima_fecha.AddDays(-dias);
        }

        /// <summary>
        /// es para guardar la transacción
        /// de una cuenta de ahorro
        /// </summary>
        /// <param name="id_ahorro"></param>
        /// <param name="valor"></param>
        /// <param name="id_tipo_ahorro"></param>
        /// <param name="retiro"></param>
        /// <param name="interes"></param>
        /// <param name="comprobante"></param>
        /// <returns></returns>
        protected async Task<Transaccion_Ahorro> GuardarSimple(int id_ahorro, decimal valor, int id_tipo_ahorro, bool retiro, bool interes, string comprobante = "")
        {
            /*verificar el último saldo*/
            decimal saldo = 0.00M;
            if (_context.Transaccion_Ahorro.Any(w => w.Id_Ahorro == id_ahorro))
                saldo = _context
                        .Transaccion_Ahorro
                        .Where(w => w.Id_Ahorro == id_ahorro && !w.Borrado_Logico)
                        .OrderBy(d => d.Id_Ahorro)
                        .AsEnumerable()
                        .Last()
                        .Saldo;//saldo erróneo
            try
            {
                string Id_Usuario = _userManager.GetUserId(HttpContext.User);

                DateTime fecha = DateTime.Now;
                Transaccion_Ahorro transaccion = new Transaccion_Ahorro
                {
                    Id_Ahorro = id_ahorro,
                    Id_Tipo_Operacion_Ahorro = id_tipo_ahorro,
                    Saldo = retiro ? saldo - valor : saldo + valor,
                    Valor = valor,
                    Fecha = fecha,
                    Creado_En = fecha,
                    Creado_Por = Id_Usuario
                };
                _context.Transaccion_Ahorro.Add(transaccion).State = EntityState.Added;

                await _context.SaveChangesAsync();
                Caja caja = await Caja_Actual();
                Transaccion_Caja trans_caja = Registrar_Caja(transaccion, caja.Id_Caja, fecha, !retiro, comprobante);
                _context.Transaccion_Caja.Add(trans_caja).State = EntityState.Added;
                await _context.SaveChangesAsync();
                //guardar la partida contable
                await Manejo_Contable_Ahorro(retiro, interes, transaccion, trans_caja.Id_Transaccion_Caja);

                return transaccion;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// es el pago en general
        /// </summary>
        /// <param name="transaccion"></param>
        /// <param name="comprobante"></param>
        /// <returns></returns>
        protected async Task<Transaccion_Credito> EfectuarPago(Transaccion_Credito transaccion, string comprobante = "")
        {
            try
            {
                transaccion = AmortizacionActualProc(
                    transaccion.Id_Credito,
                    transaccion.FechaPago,
                    transaccion.Valor_Cuota, null);
                /*bitacora transaccion*/
                string Id_Usuario = _userManager.GetUserId(HttpContext.User);
                /*se registra como un ingreso a caja*/
                Caja caja = await Caja_Actual();
                DateTime fecha = DateTime.Now;
                transaccion.Creado_En = fecha;
                transaccion.Creado_Por = Id_Usuario;
                _context.Transaccion_Credito.Add(transaccion).State = EntityState.Added;
                _context.SaveChanges();
                Transaccion_Caja trans_caja = Registrar_Caja(transaccion, caja.Id_Caja, fecha, true, comprobante);
                _context.Transaccion_Caja.Add(trans_caja).State = EntityState.Added;
                //detalle asiento contable
                _context.SaveChanges();
                await Pago_Credito_Contabilidad(transaccion, trans_caja.Id_Transaccion_Caja);
                //ver si cancela el crédito
                await Cancelar_Credito(transaccion.Id_Credito);
                return transaccion;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// evalua y cancela un crédito
        /// cada vez que se paga
        /// </summary>
        /// <param name="id_credito">registro del crédito que desaa actualizar</param>
        /// <returns></returns>
        private async Task<bool> Cancelar_Credito(int id_credito)
        {
            bool respuesta=false;
            Credito credito = await _context.Credito
                .Include(f => f.Persona)
                .Include(c => c.Comites)
                .Include(a => a.Persona.Evaluaciones)
                .Include(h => h.Transacciones_Creditos)
                .Include(p => p.Periodo)
                .Include(p => p.Comportamiento_Credito)
                .SingleOrDefaultAsync(d => d.Id_Credito == id_credito);
            Linea_Credito_Historial lch =await _context.Linea_Credito_Historial.SingleAsync(t=>t.Id_Credito==credito.Id_Credito);
            decimal total_periodos = _context
                .Cobro_Periodico_Historial
                .AsNoTracking()
                .Where(a => a.Id_Linea_Credito_Historial == credito.Id_Linea_Credito_Historial)
                .Sum(d => d.Valor);
            var resultado = new CalculosYProcesoCredito(lch.Interes_Comision,
                    credito.Monto_pago,
                    credito.Periodo,
                    IVA,
                    credito.Plazo,
                    lch.Interes,
                    credito.Liquidado_En,
                    total_periodos,
                    lch.PlanAyC_Cobros_Periodico_Dinero).Resultado;
            decimal recargos = credito.Transacciones_Creditos.Where(f => !f.Borrado_Logico).Sum(d=>d.Recargo);
            decimal totalcuotas_amortizados = credito
                .Transacciones_Creditos
                .Where(f=>!f.Borrado_Logico)
                .Sum(v=>v.Valor_Cuota);
            decimal pagos_sin_recargos = totalcuotas_amortizados - recargos;
            if (pagos_sin_recargos >= resultado.Sumatoria_Cuotas)
            {
                /*se actualizar el estado del crédito*/
                credito.Cancelado = true;
                credito.Cancelado_En = DateTime.Now;
                _context.Entry(credito).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                respuesta= true;
            }
            return respuesta;
        }
    }

    public enum Otro_Cobro
    {
        [Display(Name = "COBRO_ESTRUCTURACION")]
        Deduccion = 1,
        [Display(Name = "PAGO_ACCION")]
        Accion = 2,
        [Display(Name = "PAGO_LIBRETA")]
        Libreta = 3
    }
}