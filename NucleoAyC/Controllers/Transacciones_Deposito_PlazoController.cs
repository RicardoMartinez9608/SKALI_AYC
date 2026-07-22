using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelNucleo.CalculosYProceos;
using ModelNucleo.Model;
using ModelNucleo.Model.Contabilidad;
using ModelNucleo.Model.ModelView;
using NucleoAyC.Entities;
using NucleoAyC.Helper;

namespace NucleoAyC.Controllers
{
    /// <summary>
    /// Métodos de transacciones depositos a plazo
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = "Transacciones_Deposito_Plazo")]
    public class Transacciones_Deposito_PlazoController : Base_TransaccionController
    {
        /// <summary>
        /// constructor e injección de credito
        /// </summary>
        /// <param name="context"></param>
        /// /// <param name="userManager"></param>
        /// <param name="roleManager"></param>
        public Transacciones_Deposito_PlazoController(NegocioContexto context,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
            : base(context, userManager, roleManager)
        {
        }
        /// <summary>
        /// se registra un depósito simple a una cuenta de ahorro
        /// </summary>
        /// <param name="id_ahorro"></param>
        /// <param name="valor"></param>
        /// <param name="comprobante"></param>
        /// <param name="fecha_cambiada"></param>
        /// <param name="cambiar_fecha"></param>
        /// <returns></returns>
        [Authorize(Roles = "Asistente,Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPut]
        public async Task<IActionResult> GuardarDeposito(
            int id_deposito,
            decimal valor,
            string comprobante = "",
            DateTime? fecha_cambiada = null,
            bool cambiar_fecha = false)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (valor <= 0)
                    {
                        return BadRequest("Ingrese una cantidad mayor a cero");
                    }
                    else
                    {
                        await GuardarSimpleDepositoPlazo(id_deposito, valor, 1, false,false, comprobante, fecha_cambiada: fecha_cambiada, cambiar_fecha: cambiar_fecha);
                        dbContextTransaction.Commit();
                        return Ok("El depósito del ahorro se registró correctamente");
                    }

                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    return BadRequest("Ocurrio un problema, intentalo de nuevo");
                }
            }

        }
        /// <summary>
        /// se registra un retiro de una cuenta de ahorro
        /// </summary>
        /// <param name="id_ahorro">int</param>
        /// <param name="valor">decimal</param>
        /// <param name="comprobante"></param>
        /// <param name="fecha_cambiada"></param>
        /// <param name="cambiar_fecha"></param>
        /// <returns></returns>
        [Authorize(Roles = "Asistente,Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPut]
        public async Task<IActionResult> GuardarRetiro(
            int id_deposito,
            decimal valor,
            string comprobante = "",
            DateTime? fecha_cambiada = null,
            bool cambiar_fecha = false)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    Func<Transaccion_Deposito_Plazo, bool> filtro_trans = new Func<Transaccion_Deposito_Plazo, bool>
                        (t => t.Id_Deposito_Plazo == id_deposito);
                    if (_context.Transaccion_Deposito_Plazo.Any(filtro_trans))
                    {
                        if (valor <= 0)
                        {
                            return BadRequest("Ingrese una cantidad mayor a cero");
                        }
                        else
                        {
                            Transaccion_Deposito_Plazo lista_transacciones = _context.Transaccion_Deposito_Plazo
                           .AsNoTracking()
                           .Where(a => a.Id_Deposito_Plazo == id_deposito && !a.Borrado_Logico).OrderBy(d => d.Id_Transaccion_Deposito_Plazo).Last();
                            if (valor <= lista_transacciones.Saldo)
                            {
                                await GuardarSimpleDepositoPlazo(id_deposito, valor, 3, false,true, comprobante, fecha_cambiada: fecha_cambiada, cambiar_fecha: cambiar_fecha);
                                dbContextTransaction.Commit();
                                return Ok("El retiro del ahorro se registró correctamente");
                            }
                            else
                            {
                                return BadRequest("la cantidad ingresada es mayor que el saldo de la cuenta");
                            }
                        }

                    }
                    else
                    {
                        return BadRequest("El cliente no tiene saldo en su cuenta de ahorro");
                    }

                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    return BadRequest("Ocurrio un problema, intentalo de nuevo");
                }
            }

        }
        /// <summary>
        /// lista de transacciones para el ahorro
        /// </summary>
        /// <param name="id_ahorro"></param>
        /// <returns></returns>
        [Authorize(Roles = "Asistente,Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Transacciones(int id_deposito)
        {
            try
            {
                DateTime desde = await LapsoDias(false, id_deposito, 100,true);
                List<Transaccion_Deposito_Plazo> transacciones = await _context
                    .Transaccion_Deposito_Plazo
                    .AsNoTracking()
                    .Where(tc => tc.Id_Deposito_Plazo == id_deposito && !tc.Borrado_Logico && tc.Fecha.Date >= desde.Date)
                    .OrderByDescending(d => d.Id_Transaccion_Deposito_Plazo)
                    .ToListAsync();
                List<Transaccion_Ahorro_Vista> vistas = new List<Transaccion_Ahorro_Vista>();
                foreach (var trans in transacciones)
                {
                    Func<Transaccion_Caja, bool> filtro_trans = new Func<Transaccion_Caja, bool>(t => t.Id_Transaccion_Deposito_Plazo == trans.Id_Transaccion_Deposito_Plazo);
                    if (_context.Transaccion_Caja.Any(filtro_trans))
                    {
                        Transaccion_Caja trans_caja = _context.
                        Transaccion_Caja.AsNoTracking().Single(filtro_trans);
                        Caja caja = await _context.Caja.AsNoTracking().SingleAsync(t => t.Id_Caja == trans_caja.Id_Caja);
                        vistas.Add(new Transaccion_Ahorro_Vista
                        {
                            Id = trans.Id_Transaccion_Deposito_Plazo,
                            Transacciones = trans_caja != null ? trans_caja : null,
                            Fecha = trans.Fecha,
                            Deposito = trans.Id_Tipo_Operacion_Ahorro == 1 ? trans.Valor : 0,
                            Retiro = trans.Id_Tipo_Operacion_Ahorro == 3 ? trans.Valor : 0,
                            Saldo = trans.Saldo,
                            caja = caja != null ? caja : null
                        });
                    }
                    else
                    {

                        vistas.Add(new Transaccion_Ahorro_Vista
                        {
                            Id = trans.Id_Transaccion_Deposito_Plazo,
                            Fecha = trans.Fecha,
                            Deposito = trans.Id_Tipo_Operacion_Ahorro == 1 ? trans.Valor : 0,
                            Retiro = trans.Id_Tipo_Operacion_Ahorro == 3 ? trans.Valor : 0,
                            Saldo = trans.Saldo
                        });
                    }
                }
                return Ok(vistas);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace);
            }
        }

        /*reversión*/
        /// <summary>
        /// se revierte un abono a un credito
        /// </summary>
        /// <param name="ids">List int</param>
        /// <returns></returns>
        [Authorize(Roles = "Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpDelete]
        public async Task<IActionResult> Reversion_Deposito([FromBody] int ids)
        {
            using (var dbContextTransaccion = _context.Database.BeginTransaction())
            {
                try
                {
                    await Reversion(ids, false,true);
                    dbContextTransaccion.Commit();
                    return Ok("Se ha revertido transacciones de deposito a plazo seleccionadas");
                }
                catch (Exception ex)
                {
                    dbContextTransaccion.Rollback();
                    return BadRequest(ex.StackTrace);
                }
            }

        }

        /// <summary>
        /// Se muestra la factura para el abono de una transacción de ahorro
        /// </summary>
        /// <param name="id_transaccion_ahorro">int</param>
        /// <returns>Factura</returns>
        [Authorize(Roles = "Asistente,Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Mostrar_Factura_Transaccion(int id_transaccion_deposito)
        {
            try
            {
                Transaccion_Caja caja = new Transaccion_Caja();
                Transaccion_Deposito_Plazo ahorro = new Transaccion_Deposito_Plazo();
                Tipo_Operacion_Ahorro tipo = new Tipo_Operacion_Ahorro();
                Caja usuario = new Caja();
                Func<Transaccion_Caja, bool> filtro_trans = new Func<Transaccion_Caja, bool>(t => t.Id_Transaccion_Deposito_Plazo == id_transaccion_deposito);
                if (_context.Transaccion_Caja.Any(filtro_trans))
                {
                    caja = await _context.Transaccion_Caja.AsNoTracking().SingleAsync(d => d.Id_Transaccion_Deposito_Plazo == id_transaccion_deposito);
                    ahorro = await _context.Transaccion_Deposito_Plazo.AsNoTracking().SingleAsync(a => a.Id_Transaccion_Deposito_Plazo == caja.Id_Transaccion_Deposito_Plazo);
                    tipo = await _context.Tipo_Operacion_Ahorro.SingleAsync(b => b.Id_Tipo_Operacion_Ahorro == ahorro.Id_Tipo_Operacion_Ahorro);
                    usuario = await _context.Caja.SingleAsync(c => c.Id_Caja == caja.Id_Caja);
                }
                else
                {
                    ahorro = await _context.Transaccion_Deposito_Plazo.AsNoTracking().SingleAsync(a => a.Id_Transaccion_Deposito_Plazo == id_transaccion_deposito);
                    tipo = await _context.Tipo_Operacion_Ahorro.SingleAsync(b => b.Id_Tipo_Operacion_Ahorro == ahorro.Id_Tipo_Operacion_Ahorro);
                }

                string id_usuario = _userManager.GetUserId(HttpContext.User);
                Transaccion_Deposito_Plazo tc = await _context.Transaccion_Deposito_Plazo
                    .AsNoTracking()
                    .Include(c => c.Deposito_Plazo)
                    .Include(x => x.Deposito_Plazo.Persona)
                    .Include(h => h.Deposito_Plazo.Persona.Cartera)
                    .SingleOrDefaultAsync(i => i.Id_Transaccion_Deposito_Plazo == id_transaccion_deposito);

                Agencia agencia = _context
                    .Usuario_Agencia
                    .AsNoTracking()
                    .Include(d => d.Agencia)
                    .SingleOrDefault(g => g.Id_Usuario == id_usuario)
                    .Agencia;

                if (tc != null)
                {
                    Impresion impresion = new Impresion();
                    Factura factura = new Factura
                    {
                        Total = tc.Valor,
                        Fecha = tc.Creado_En,
                        Agencia = agencia != null ? agencia.Nombre : "",
                        Cliente = tc.Deposito_Plazo.Persona.NombreCompleto,
                        No_Ahorro = tc.Deposito_Plazo.No_Ahorro,
                        Concepto = tipo.Descripcion,
                        Cajero = usuario.Nombre_PC,
                        id_cajero = usuario.Id_Caja,
                        Saldo_Capital = 0.00M,
                    };
                    Comprobante comprobante = new Comprobante
                    {
                        NoComprobante = caja.No_Comprobante

                    };

                    impresion.Factura = factura;
                    impresion.Comprobante = comprobante;
                    return Ok(impresion);
                }
                return NoContent();
            }
            catch (NullReferenceException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Ejecuta el traslado de intereses ganados individual en depósitos a plazo
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [Authorize(Roles = "Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> EjecutarCapitalizacion([FromBody] List<int> ids)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    foreach (int id_deposito in ids)
                    {
                        string no_comprobante = await Generar_No_Comprobante();
                        await GuardarSimpleDepositoPlazo(id_deposito, 0.00M, 2, true,false, no_comprobante);
                    }
                    dbContextTransaction.Commit();
                    return Ok("Todos los traslados se han ejecutado correctamente");
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    return BadRequest("Hubo problemas con uno o varios traslados");
                }
            }
        }

        /// <summary>
        /// muestra la lista de los traslados pendientes de efectuar
        /// </summary>
        /// <param name="id_linea_ahorro">int</param>
        /// <returns></returns>
        [Authorize(Roles = "Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> ConsultarCapitalizacionDeposito(int id_linea_ahorro)
        {
            var personas = await (from p in _context.Personas
                                  join c in _context.Cartera on p.Id_Cartera equals c.Id_Cartera
                                  join u in _context.Usuario_Agencia on c.Id_Usuario equals u.Id_Usuario
                                  join a in _context.Deposito_Plazo on p.Id_Persona equals a.Id_Persona
                                  select p)
                           .Include(d => d.Deposito_Plazo)
                           .ToListAsync();
            try
            {
                DateTime ahora = DateTime.Now.Date;
                /*primero conseguir todos las transacciones con los ultimos saldo*/
                List<Capitalizacion> ahorros_capitalizables = new List<Capitalizacion>();
                foreach (Persona per in personas)
                {
                    DateTime fecha_ahora = DateTime.Now;
                    if (per.Ahorros != null)
                    {
                        foreach (var ahorro in per.Ahorros.Where(d => d.Id_Linea_Ahorro == id_linea_ahorro).ToList())
                        {
                            int dias_capitalizables = ahorro.Linea_Ahorro.Mes_Capitalizable * 30;
                            /*última transacción*/
                            Transaccion_Ahorro ultima_transaccion = new Transaccion_Ahorro();
                            DateTime ultima_capitalizacion = ahorro.Ultima_Fecha_Traslado;
                            decimal saldo = 0.00M;
                            decimal interes_aplicado = 0.00M;

                            if (_context.Transaccion_Ahorro.Any(v => v.Id_Ahorro == ahorro.Id_Ahorro && !v.Borrado_Logico))
                            {
                                ultima_transaccion = _context
                                    .Transaccion_Ahorro
                                    .Where(v => v.Id_Ahorro == ahorro.Id_Ahorro && !v.Borrado_Logico)
                                    .OrderBy(d => d.Id_Transaccion_Ahorro).Last();
                                ultima_capitalizacion = ultima_transaccion.Fecha;
                                saldo = ultima_transaccion.Saldo;
                                interes_aplicado = ultima_transaccion.Interes_Acumulado + ultima_transaccion.Interes_Aplicado;
                            }

                            var fecha_cero = DateTime.Parse("0001-01-01");
                            int verificar_fecha = DateTime.Compare(fecha_cero, ahorro.Ultima_Fecha_Traslado);
                            DateTime fecha_Ultima_Capitalizada = ahorro.Ultima_Fecha_Traslado;
                            if (verificar_fecha == 0)
                                fecha_Ultima_Capitalizada = ahorro.Fecha_Apertura;
                            /*sacar los días*/
                            TimeSpan diferencia = ahora.Date - fecha_Ultima_Capitalizada.Date;
                            int dias = diferencia.Days;
                            if (dias >= dias_capitalizables && interes_aplicado > 0.00M)
                                ahorros_capitalizables.Add(new Capitalizacion
                                {
                                    Id_Ahorro = ahorro.Id_Ahorro,
                                    CuentaHabiente = per.NombreCompleto,
                                    Cuenta = ahorro.No_Ahorro,
                                    UltimaCapitalizacion = ultima_capitalizacion,
                                    Saldo = saldo,
                                    Interes_Aplicar = interes_aplicado
                                });
                        }
                    }
                }
                return Ok(ahorros_capitalizables);
            }
            catch (Exception ex)
            {
                return BadRequest("Todo ha fallado");
            }
        }

        /*solo ver la capitalización pero individual*/

    }
}
