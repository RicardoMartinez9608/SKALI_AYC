using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelNucleo.CalculosYProceos;
using ModelNucleo.Dto;
using ModelNucleo.Model;
using ModelNucleo.Model.Contabilidad;
using ModelNucleo.Model.ModelView;
using NucleoAyC.Entities;
using NucleoAyC.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Threading.Tasks;

namespace NucleoAyC.Controllers
{
    /// <summary>
    /// Métodos de transacciones de ahorros
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = "Transacciones_Ahorro")]
    public class Transacciones_AhorroController : Base_TransaccionController
    {
        /// <summary>
        /// constructor e injección de credito
        /// </summary>
        /// <param name="context"></param>
        /// /// <param name="userManager"></param>
        /// <param name="roleManager"></param>
        public Transacciones_AhorroController(NegocioContexto context,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
            : base(context, userManager, roleManager)
        {
        }

        /// <summary>
        /// depósito de cuentas contables
        /// </summary>
        /// <param name="cuenta"></param>
        /// <param name="valor"></param>
        /// <returns></returns>
        [Authorize(Roles = "Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> Procesar_Planilla(string cuenta,decimal valor)
        {
            /*verificar la cuenta de ahorro antes de procesar*/
            string no_ahorro = cuenta.Trim();
            DateTime fecha = DateTime.Now;
            string Id_Usuario = _userManager.GetUserId(HttpContext.User);
            var ahorro =await _context.Ahorro.SingleAsync(t=>t.No_Ahorro==no_ahorro);
            /*verificar que no sea una transacción repetida*/
            bool eslamismatransaccion =await _context
                .Transaccion_Ahorro
                .AnyAsync(
                    c=>c.Id_Ahorro==ahorro.Id_Ahorro && 
                    c.Valor==valor && 
                    c.Fecha.Date==fecha.Date &&
                    c.Creado_Por==Id_Usuario &&
                    c.Id_Tipo_Operacion_Ahorro==1);

            if (ahorro!=null && !eslamismatransaccion)
            {
                try
                {
                    string comprobante = await Generar_No_Comprobante();
                    await GuardarSimple(ahorro.Id_Ahorro, valor, 1, false, false, comprobante,es_salario:true);
                    return Ok("El sueldo ha sido depositado");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.StackTrace);
                }
            }
            else
                return BadRequest("El código de cuenta no es correcto o es una transacción repetida");
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
            int id_ahorro,
            decimal valor, 
            string comprobante ="",
            DateTime? fecha_cambiada=null,
            bool cambiar_fecha=false)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (valor <= 0)
                    {
                        return BadRequest("Ingrese una cantidad mayor a cero");
                    }
                    else {
                        await GuardarSimple(id_ahorro, valor, 1, false, false, comprobante, fecha_cambiada: fecha_cambiada, cambiar_fecha: cambiar_fecha);
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
        int id_ahorro,
        decimal valor,
        string comprobante = "",
        DateTime? fecha_cambiada = null,
        bool cambiar_fecha = false)
        {
            await using var dbContextTransaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var ultimaTransaccion = await _context.Transaccion_Ahorro
                                    .Where(t => t.Id_Ahorro == id_ahorro && !t.Borrado_Logico)
                                    .OrderByDescending(t => t.Fecha)
                                    .ThenByDescending(t => t.Id_Transaccion_Ahorro)
                                    .FirstOrDefaultAsync();

                if (ultimaTransaccion == null)
                    return BadRequest("El cliente no tiene saldo en su cuenta de ahorro");

                if (valor <= 0)
                    return BadRequest("Ingrese una cantidad mayor a cero");

                if (valor > ultimaTransaccion.Saldo)
                    return BadRequest("La cantidad ingresada es mayor que el saldo de la cuenta");

                await GuardarSimple(id_ahorro, valor, 3, true, false, comprobante, fecha_cambiada: fecha_cambiada, cambiar_fecha: cambiar_fecha);
                await dbContextTransaction.CommitAsync();

                return Ok("El retiro del ahorro se registró correctamente");
            }
            catch (Exception)
            {
                await dbContextTransaction.RollbackAsync();
                return BadRequest("Ocurrió un problema, intentalo de nuevo");
            }
        }

        /// <summary>
        /// efectua el procesamiento de una colecta específica
        /// </summary>
        /// <param name="id_colecta_diaria">int</param>
        /// <returns></returns>
        [Authorize(Roles = "Asistente,Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPut]
        public async Task<IActionResult> ProcesarColectaAhorro(int id_colecta_diaria)
        {
            try
            {
                bool no_procesado = _context.Colecta_Diaria.Any(l => l.Id_Colecta_Diaria == id_colecta_diaria && !l.Procesado_Total && l.Verificado);
                if (no_procesado)
                {
                    Colecta_Diaria colecta_diaria = _context.Colecta_Diaria
                        .Include(d => d.Colectas)
                        .SingleOrDefault(s => s.Id_Colecta_Diaria == id_colecta_diaria);

                    /*sentinelas*/
                    List<Colecta> colectas = colecta_diaria.Colectas.Where(d => d.Id_Ahorro > 0 && !d.Borrado_Logico).ToList();
                    int son_ahorros = colectas.Count;
                    int ejecutados = 0;
                    foreach (Colecta c in colectas)
                    {
                        if(c.Procesado) continue;
                        Transaccion_Ahorro transaccion =await GuardarSimple(
                            c.Id_Ahorro, 
                            c.Monto, 
                            1, 
                            false,
                            false,
                            comprobante:c.No_Comprobante,
                            fecha_cambiada:c.Creado_En.Date,
                            cambiar_fecha:true );

                        c.Id_Transaccion = transaccion.Id_Transaccion_Ahorro;
                        c.Procesado = true;
                        _context.Entry(c).State = EntityState.Modified;
                        ejecutados += 1;
                    }
                    _context.SaveChanges();
                    if (ejecutados == son_ahorros)
                    {
                        colecta_diaria.Procesado_Total = true;
                        _context.Entry(colecta_diaria).State = EntityState.Modified;
                        _context.SaveChanges();
                        return Ok("Colectas para ahorros procesadas en su totalidad");
                    }
                    return Ok("Colectas para ahorros procesadas parcialmente");
                }
                else
                    return Ok("No se registraron colectas de Ahorro");
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Muestra en factura los totales de una colecta que ya haya sido procesada
        /// </summary>
        /// <param name="id_colecta_diaria">int</param>
        /// <returns>Factura</returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Mostrar_Factura_Colecta(int id_colecta_diaria)
        {
            try
            {
                string id_usuario = _userManager.GetUserId(HttpContext.User);
                Colecta cliente =  _context.Colecta.AsNoTracking().FirstOrDefault(a => a.Id_Colecta_Diaria == id_colecta_diaria && a.Id_Ahorro >0);
                Transaccion_Caja caja = await _context.Transaccion_Caja.AsNoTracking().SingleAsync(d => d.Id_Transaccion_Ahorro == cliente.Id_Transaccion);
                Caja usuario = await _context.Caja.SingleAsync(c => c.Id_Caja == caja.Id_Caja);
                Colecta_Diaria colecta_diaria = await _context.Colecta_Diaria
                    .Include(v => v.Colectas)
                    .SingleOrDefaultAsync(v => v.Id_Colecta_Diaria == id_colecta_diaria &&
                    v.Procesado_Total);

                if (colecta_diaria != null)
                {
                    Transaccion_Ahorro tc = await _context.Transaccion_Ahorro
                        .AsNoTracking()
                        .Include(c => c.Ahorro)
                        .Include(q => q.Ahorro.Linea_Ahorro)
                        .Include(x => x.Ahorro.Persona)
                        .Include(h => h.Ahorro.Persona.Cartera)
                        .SingleOrDefaultAsync(i => i.Id_Transaccion_Ahorro == cliente.Id_Transaccion);
                    List<Transaccion_Ahorro> transaccion_ahorros = new List<Transaccion_Ahorro>();
                    foreach (var c in colecta_diaria.Colectas.Where(z => z.Id_Ahorro > 0))
                    {
                        Transaccion_Ahorro transaccion = _context.Transaccion_Ahorro.SingleOrDefault(t => t.Id_Transaccion_Ahorro == c.Id_Transaccion);//revisar la fecha de creación
                        transaccion_ahorros.Add(transaccion);
                    }
                    Impresion impresion = new Impresion();
                    Agencia agencia = _context
                        .Usuario_Agencia
                        .AsNoTracking()
                        .Include(d => d.Agencia)
                        .SingleOrDefault(g => g.Id_Usuario == id_usuario)
                        .Agencia;
                    Factura factura = new Factura
                    {
                        Total = transaccion_ahorros.Sum(u => u.Valor),
                        Fecha = colecta_diaria.Fecha,
                        Agencia = agencia != null ? agencia.Nombre : "",
                        Cliente =cliente.Nombre_Completo,
                        No_Ahorro = tc.Ahorro.No_Ahorro,
                        Concepto = "Colecta Ahorros Asesor",
                        Cajero = usuario.Nombre_PC,
                        id_cajero = usuario.Id_Caja,
                        Linea = "",
                        Saldo_Capital = 0.00M,
                        Tasa = 0.00M
                    };
                    string no_comprobante = await Generar_No_Comprobante();
                    Comprobante comprobante = new Comprobante
                    {
                        NoComprobante = no_comprobante

                    };
                    impresion.Factura = factura;
                    impresion.Comprobante = comprobante;
                    return Ok(impresion);
                }
                else
                {
                    return BadRequest("el registro que busca no se ha encontrado o no ha sido procesado en su totalidad");
                }
            }
            catch (Exception e)
            {
                throw;
            }
           
        }

        /// <summary>
        /// Se muestra la factura para el abono de una transacción de ahorro
        /// </summary>
        /// <param name="id_transaccion_ahorro">int</param>
        /// <returns>Factura</returns>
        [Authorize(Roles = "Asistente,Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Mostrar_Factura_Transaccion(int id_transaccion_ahorro)
        {
            try
            {
                Transaccion_Caja caja = new Transaccion_Caja();
                Transaccion_Ahorro ahorro = new Transaccion_Ahorro();
                Tipo_Operacion_Ahorro tipo = new Tipo_Operacion_Ahorro();
                Caja usuario = new Caja();
                Func<Transaccion_Caja, bool> filtro_trans = new Func<Transaccion_Caja, bool>(t => t.Id_Transaccion_Ahorro == id_transaccion_ahorro);
                if (_context.Transaccion_Caja.Any(filtro_trans))
                {
                    caja = await _context.Transaccion_Caja.AsNoTracking().SingleAsync(d => d.Id_Transaccion_Ahorro == id_transaccion_ahorro);
                    ahorro = await _context.Transaccion_Ahorro.AsNoTracking().SingleAsync(a => a.Id_Transaccion_Ahorro == caja.Id_Transaccion_Ahorro);
                    tipo = await _context.Tipo_Operacion_Ahorro.SingleAsync(b => b.Id_Tipo_Operacion_Ahorro == ahorro.Id_Tipo_Operacion_Ahorro);
                    usuario = await _context.Caja.SingleAsync(c => c.Id_Caja == caja.Id_Caja);
                }
                else
                {
                    ahorro = await _context.Transaccion_Ahorro.AsNoTracking().SingleAsync(a => a.Id_Transaccion_Ahorro == id_transaccion_ahorro);
                    tipo = await _context.Tipo_Operacion_Ahorro.SingleAsync(b => b.Id_Tipo_Operacion_Ahorro == ahorro.Id_Tipo_Operacion_Ahorro);
                }
               
                string id_usuario = _userManager.GetUserId(HttpContext.User);
                Transaccion_Ahorro tc = await _context.Transaccion_Ahorro
                    .AsNoTracking()
                    .Include(c => c.Ahorro)
                    .Include(q => q.Ahorro.Linea_Ahorro)
                    .Include(x => x.Ahorro.Persona)
                    .Include(h => h.Ahorro.Persona.Cartera)
                    .SingleOrDefaultAsync(i => i.Id_Transaccion_Ahorro == id_transaccion_ahorro);
               
                Agencia agencia = _context
                    .Usuario_Agencia
                    .AsNoTracking()
                    .Include(d=>d.Agencia)
                    .SingleOrDefault(g => g.Id_Usuario == id_usuario)
                    .Agencia;

                if (tc != null)
                {
                    Impresion impresion=new Impresion();
                    Factura factura = new Factura
                    {
                        Total = tc.Valor,
                        Fecha = tc.Creado_En,
                        Agencia = agencia != null ? agencia.Nombre : "",
                        Cliente = tc.Ahorro.Persona.NombreCompleto,
                        No_Ahorro = tc.Ahorro.No_Ahorro,
                        Concepto = tipo.Descripcion,
                        Cajero = usuario.Nombre_PC,
                        id_cajero = usuario.Id_Caja,
                        Linea = tc.Ahorro.Linea_Ahorro.Descripcion,
                        Saldo_Capital = 0.00M,
                        Tasa = tc.Ahorro.Linea_Ahorro.Tasa_Interes
                    };
                    Comprobante comprobante=new Comprobante
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
        /// lista de transacciones para el ahorro
        /// </summary>
        /// <param name="id_ahorro"></param>
        /// <returns></returns>
        [Authorize(Roles = "Asistente,Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Transacciones(int id_ahorro, int page = 1, int pageSize = 20, bool usarFiltroFecha = false)
        {
            try
            {
                DateTime? desde = null;

                if (usarFiltroFecha)
                {
                    desde = await LapsoDias(false, id_ahorro, 100);
                }

                var query = from t in _context.Transaccion_Ahorro.AsNoTracking()
                            where t.Id_Ahorro == id_ahorro
                            && !t.Borrado_Logico
                            && (desde == null || t.Fecha >= desde)
                            join tc in _context.Transaccion_Caja.AsNoTracking()
                                on t.Id_Transaccion_Ahorro equals tc.Id_Transaccion_Ahorro into tcGroup
                            from tc in tcGroup.DefaultIfEmpty()
                            join c in _context.Caja.AsNoTracking()
                                on tc.Id_Caja equals c.Id_Caja into cGroup
                            from c in cGroup.DefaultIfEmpty()
                            orderby t.Fecha descending
                            select new Transaccion_Ahorro_Vista
                            {
                                Id = t.Id_Transaccion_Ahorro,
                                Fecha = t.Fecha,
                                Deposito = t.Id_Tipo_Operacion_Ahorro == 1 ? t.Valor : 0,
                                Retiro = t.Id_Tipo_Operacion_Ahorro == 3 ? t.Valor : 0,
                                Saldo = t.Saldo,
                                Transacciones = tc,
                                caja = c
                            };

                var total = await query.CountAsync();

                var data = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return Ok(new
                {
                    total,
                    page,
                    pageSize,
                    data
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Ejecuta el traslado de intereses ganados individual
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="id_ahorro"></param>
        /// <returns></returns>
        [Authorize(Roles = "Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> EjecutarCapitalizacion([FromBody] List<CapitalizacionDto> datos)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var transaccionesAhorro = new List<Transaccion_Ahorro>();
                    var transaccionesCaja = new List<Transaccion_Caja>();
                    foreach (var item in datos)
                    { 
                        string no_comprobante = await Generar_No_Comprobante();
                        await GuardarSimple(
                                           item.IdAhorro,
                                           item.Valor,
                                           1,
                                           false,
                                           true,
                                           no_comprobante,
                                           false,
                                           false,
                                           item.Fecha,    // <-- Fecha enviada desde el DTO
                                           true,          // <-- Indica que queremos cambiar la fecha
                                           true,
                                           item.IdDepositoPlazo           
                                       );
                        if (item.EsDepositoPlazo)
                        {
                            await RegistrarCapitalizacionDepositoPlazo(
                                item.IdDepositoPlazo,
                                item.Valor,
                                item.Fecha
                            );
                        }
                    }

                    await _context.SaveChangesAsync(); // ✅ UNA SOLA VEZ
                   
                    dbContextTransaction.Commit();
                    return Ok("Todos los traslados se han ejecutado correctamente");
                }
                catch (Exception ex)
                {
                    var innerException = ex.InnerException;
                    Console.WriteLine(innerException?.Message); 
                    dbContextTransaction.Rollback();
                    return BadRequest("Hubo problemas con uno o varios traslados");
                }
            }
        }
        
        /// <summary>
        /// muestra la lista de los traslados pendientes de efectuar
        /// </summary>
        /// <param name="id_agencia">int</param>
        /// <param name="id_linea_ahorro">int</param>
        /// <returns></returns>
        // [Authorize(Roles = "Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> ConsultarCapitalizacionAhorro(int id_linea_ahorro, DateTime fecha)
        {
            try
            {
                // 🔹 Obtener todas las transacciones válidas HASTA la fecha indicada
                var transaccionesValidas = await _context.Transaccion_Ahorro
                    .Where(t => !t.Borrado_Logico && t.Fecha <= fecha)
                    .ToListAsync();

                // 🔹 Obtener último saldo por ahorro
                var ultimoSaldoPorAhorro = transaccionesValidas
                    .GroupBy(t => t.Id_Ahorro)
                    .Select(g => g
                        .OrderByDescending(t => t.Fecha)
                        .ThenByDescending(t => t.Id_Transaccion_Ahorro)
                        .FirstOrDefault())
                    .Where(t => t != null)
                    .ToDictionary(t => t.Id_Ahorro, t => t.Saldo);

                // 🔹 IDs de ahorros con saldo > 0
                var idsAhorrosConSaldo = ultimoSaldoPorAhorro
                    .Where(x => x.Value > 0)
                    .Select(x => x.Key)
                    .ToList();

                // 🔹 Obtener personas con ahorros de la línea
                var personas = await (
                    from p in _context.Personas
                    join a in _context.Ahorro on p.Id_Persona equals a.Id_Persona
                    join l in _context.Linea_Ahorro on a.Id_Linea_Ahorro equals l.Id_Linea_Ahorro
                    where l.Id_Linea_Ahorro == id_linea_ahorro
                          && idsAhorrosConSaldo.Contains(a.Id_Ahorro)
                    select p
                )
                .Distinct()
                .Include(p => p.Ahorros)
                    .ThenInclude(a => a.Linea_Ahorro)
                .ToListAsync();

                // 🔹 Filtrar ahorros relevantes
                foreach (var persona in personas)
                {
                    persona.Ahorros = persona.Ahorros
                        .Where(a => a.Id_Linea_Ahorro == id_linea_ahorro
                                 && idsAhorrosConSaldo.Contains(a.Id_Ahorro))
                        .ToList();
                }

                List<Capitalizacion> ahorrosCapitalizables = new List<Capitalizacion>();

                int tiempo = 365;

                foreach (var persona in personas)
                {
                    foreach (var ahorro in persona.Ahorros)
                    {
                        DateTime ultimaCapitalizacion = ahorro.CapitalizacionAhorro.Date;
                        int diasCapitalizables = (fecha.Date - ultimaCapitalizacion.Date).Days;

                        if (diasCapitalizables <= 0)
                            continue;

                        decimal tasaInteres = ahorro.Linea_Ahorro.Tasa_Interes / 100;

                        // 🔹 Transacciones del periodo ordenadas
                        var transaccionesPeriodo = transaccionesValidas
                            .Where(t => t.Id_Ahorro == ahorro.Id_Ahorro
                                     && t.Fecha > ultimaCapitalizacion
                                     && t.Fecha <= fecha)
                            .OrderBy(t => t.Fecha)
                            .ThenBy(t => t.Id_Transaccion_Ahorro)
                            .ToList();

                        // 🔹 Obtener saldo inicial al inicio del periodo
                        var transaccionInicial = transaccionesValidas
                            .Where(t => t.Id_Ahorro == ahorro.Id_Ahorro
                                     && t.Fecha <= ultimaCapitalizacion)
                            .OrderByDescending(t => t.Fecha)
                            .ThenByDescending(t => t.Id_Transaccion_Ahorro)
                            .FirstOrDefault();

                        decimal saldoActual = transaccionInicial?.Saldo ?? 0;
                        decimal saldoFinal = ultimoSaldoPorAhorro[ahorro.Id_Ahorro];

                        // 🔹 Cálculo de interés por tramos
                        decimal interesAplicar = 0;
                        DateTime fechaInicio = ultimaCapitalizacion;

                        foreach (var transaccion in transaccionesPeriodo)
                        {
                            int dias = (transaccion.Fecha.Date - fechaInicio.Date).Days;

                            if (dias > 0)
                            {
                                interesAplicar += saldoActual * tasaInteres / tiempo * dias;
                            }

                            saldoActual = transaccion.Saldo;
                            fechaInicio = transaccion.Fecha;
                        }

                        // 🔹 Tramo final
                        int diasFinales = (fecha.Date - fechaInicio.Date).Days;
                        if (diasFinales > 0)
                        {
                            interesAplicar += saldoActual * tasaInteres / tiempo * diasFinales;
                        }

                        interesAplicar = Math.Round(interesAplicar, 2);

                        if (interesAplicar > 0)
                        {
                            ahorrosCapitalizables.Add(new Capitalizacion
                            {
                                Id_Ahorro = ahorro.Id_Ahorro,
                                CuentaHabiente = persona.NombreCompleto,
                                Cuenta = ahorro.No_Ahorro,
                                UltimaCapitalizacion = ultimaCapitalizacion,
                                Saldo = saldoFinal,
                                Interes_Aplicar = interesAplicar,
                                DiasCapitalizables = diasCapitalizables
                            });
                        }
                    }
                }

                return Ok(ahorrosCapitalizables);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al consultar capitalización: {ex.Message}");
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetEstadoCuenta(
            int idAhorro,
            DateTime fechaInicio,
            DateTime fechaFin)
                {
                    var ahorro = await _context.Ahorro
                        .Include(a => a.Persona)
                        .FirstOrDefaultAsync(a => a.Id_Ahorro == idAhorro);

                    if (ahorro == null)
                        return NotFound("Ahorro no encontrado");

                    var transacciones = await _context.Transaccion_Ahorro
                        .Include(t => t.Tipo_Operacion_Ahorro)
                       .Where(t => t.Id_Ahorro == idAhorro &&
                                t.Fecha >= fechaInicio &&
                                t.Fecha < fechaFin.AddDays(1) &&
                                !t.Borrado_Logico)
                        .OrderBy(t => t.Fecha)
                        .ToListAsync();

                    // 🔹 saldo inicial = última transacción antes del rango
                    var saldoInicial = await _context.Transaccion_Ahorro
                        .Where(t => t.Id_Ahorro == idAhorro &&
                                    t.Fecha < fechaInicio &&
                                    !t.Borrado_Logico)
                        .OrderByDescending(t => t.Fecha)
                        .Select(t => t.Saldo)
                        .FirstOrDefaultAsync();

                    var estado = new EstadoCuentaDTO
                    {
                        No_Ahorro = ahorro.No_Ahorro,
                        NombrePersona = ahorro.Persona.NombreCompleto,
                        EmailPersona = ahorro.Persona.Correo.Trim(),
                        FechaInicio = fechaInicio,
                        FechaFin = fechaFin,
                        SaldoInicial = saldoInicial,
                        SaldoFinal = transacciones.LastOrDefault()?.Saldo ?? saldoInicial,
                        Movimientos = transacciones.Select(t => new MovimientoDTO
                        {
                            Fecha = t.Fecha,
                            TipoOperacion = t.Tipo_Operacion_Ahorro.Descripcion,
                            Valor = t.Valor,
                            Saldo = t.Saldo
                        }).ToList()
            };

            return Ok(estado);
        }
        [HttpGet]
        public async Task<IActionResult> ConsultarCapitalizacionPorAhorro(int id_ahorro, DateTime fecha)
        {
            try
            {
                // 🔹 Obtener todas las transacciones válidas HASTA la fecha indicada
                var transaccionesValidas = await _context.Transaccion_Ahorro
                    .Where(t => !t.Borrado_Logico && t.Fecha <= fecha)
                    .ToListAsync();

                // 🔹 Obtener último saldo por ahorro (hasta la fecha)
                var ultimoSaldoPorAhorro = transaccionesValidas
                    .GroupBy(t => t.Id_Ahorro)
                    .Select(g => g
                        .OrderByDescending(t => t.Fecha)
                        .ThenByDescending(t => t.Id_Transaccion_Ahorro)
                        .FirstOrDefault())
                    .Where(t => t != null)
                    .ToDictionary(t => t.Id_Ahorro, t => t.Saldo);

                // 🔹 Verificar si el ahorro tiene saldo
                if (!ultimoSaldoPorAhorro.ContainsKey(id_ahorro) || ultimoSaldoPorAhorro[id_ahorro] <= 0)
                    return Ok(new List<Capitalizacion>());

                // 🔹 Obtener el ahorro con persona y línea
                var ahorro = await _context.Ahorro
                    .Include(a => a.Persona)
                    .Include(a => a.Linea_Ahorro)
                    .FirstOrDefaultAsync(a => a.Id_Ahorro == id_ahorro);

                if (ahorro == null)
                    return NotFound("No se encontró el ahorro especificado.");

                // 🔹 Datos base
                var persona = ahorro.Persona;
                decimal saldoFinal = ultimoSaldoPorAhorro[id_ahorro];

                DateTime ultimaCapitalizacion = ahorro.CapitalizacionAhorro.Date;
                int diasCapitalizables = (fecha.Date - ultimaCapitalizacion.Date).Days;

                if (diasCapitalizables <= 0)
                    return Ok(new List<Capitalizacion>());

                int tiempo = 365;
                decimal tasaInteres = ahorro.Linea_Ahorro.Tasa_Interes / 100;

                // 🔹 Transacciones del periodo (ordenadas correctamente)
                var transaccionesPeriodo = transaccionesValidas
                    .Where(t => t.Id_Ahorro == id_ahorro
                             && t.Fecha > ultimaCapitalizacion
                             && t.Fecha <= fecha)
                    .OrderBy(t => t.Fecha)
                    .ThenBy(t => t.Id_Transaccion_Ahorro)
                    .ToList();

                // 🔹 Cálculo de interés por tramos
                decimal interesAplicar = 0;
                DateTime fechaInicio = ultimaCapitalizacion;
                decimal saldoActual = saldoFinal;

                // ⚠️ Obtener el saldo correcto al inicio (antes de la primera transacción del periodo)
                var saldoInicial = transaccionesValidas
                    .Where(t => t.Id_Ahorro == id_ahorro && t.Fecha <= ultimaCapitalizacion)
                    .OrderByDescending(t => t.Fecha)
                    .ThenByDescending(t => t.Id_Transaccion_Ahorro)
                    .FirstOrDefault();

                if (saldoInicial != null)
                    saldoActual = saldoInicial.Saldo;

                foreach (var transaccion in transaccionesPeriodo)
                {
                    int dias = (transaccion.Fecha.Date - fechaInicio.Date).Days;

                    if (dias > 0)
                    {
                        interesAplicar += saldoActual * tasaInteres / tiempo * dias;
                    }

                    // 🔹 Actualizar saldo según la transacción
                    saldoActual = transaccion.Saldo;
                    fechaInicio = transaccion.Fecha;
                }

                // 🔹 Tramo final hasta la fecha consultada
                int diasFinales = (fecha.Date - fechaInicio.Date).Days;
                if (diasFinales > 0)
                {
                    interesAplicar += saldoActual * tasaInteres / tiempo * diasFinales;
                }

                interesAplicar = Math.Round(interesAplicar, 2);

                // 🔹 Armar resultado
                var resultado = new List<Capitalizacion>();

                if (interesAplicar > 0)
                {
                    resultado.Add(new Capitalizacion
                    {
                        Id_Ahorro = ahorro.Id_Ahorro,
                        CuentaHabiente = persona.NombreCompleto,
                        Cuenta = ahorro.No_Ahorro,
                        UltimaCapitalizacion = ultimaCapitalizacion,
                        Saldo = saldoFinal,
                        Interes_Aplicar = interesAplicar,
                        DiasCapitalizables = diasCapitalizables
                    });
                }

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al consultar la capitalización: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ConsultarCapitalizacionDepositoPlazo(DateTime fecha)
        {
            try
            {
                var fechaConsulta = fecha.Date;

                // Obtener todas las transacciones
                var transacciones = await _context.Transaccion_Deposito_Plazo
                    .ToListAsync();

                // Última transacción de cada depósito
                var ultimasTransacciones = transacciones
                    .GroupBy(t => t.Id_Deposito_Plazo)
                    .Select(g => g
                        .OrderByDescending(x => x.Fecha)
                        .ThenByDescending(x => x.Id_Transaccion_Deposito_Plazo)
                        .First())
                    .ToList();

                var ultimasTransaccionesDict = ultimasTransacciones
                    .ToDictionary(
                        x => x.Id_Deposito_Plazo,
                        x => x);

                var depositos = await (
                    from dp in _context.Deposito_Plazo
                        .Include(x => x.Forma_Pago)
                    join p in _context.Personas
                        on dp.Id_Persona equals p.Id_Persona
                    select new
                    {
                        Deposito = dp,
                        Persona = p
                    }
                ).ToListAsync();

                var resultado = depositos
                    .Where(x =>
                    {
                        if (!ultimasTransaccionesDict.TryGetValue(
                            x.Deposito.Id_Deposito_Plazo,
                            out var ultimaTransaccion))
                        {
                            return false;
                        }

                        // Debe tener saldo
                        if (ultimaTransaccion.Saldo <= 0)
                            return false;

                        // No vencido
                        if (x.Deposito.Fecha_Vencimiento.Date < fechaConsulta)
                            return false;

                        // Días desde la última transacción
                        var diasTranscurridos =
                            (fechaConsulta - ultimaTransaccion.Fecha.Date).Days;

                        // Toca capitalizar
                        return diasTranscurridos == 30;
                    })
                    .Select(x =>
                    {
                        var ultimaTransaccion =
                            ultimasTransaccionesDict[x.Deposito.Id_Deposito_Plazo];

                        var interesMensual = Math.Round(
                            (
                                ultimaTransaccion.Saldo *
                                (x.Deposito.Interes / 100m) /
                                360m
                            ) * 30m,
                            2);

                        return new Capitalizacion
                        {
                            Id_Deposito_Plazo =
                                x.Deposito.Id_Deposito_Plazo,

                            Id_Ahorro =
                                x.Deposito.Id_Ahorro,

                            Cuenta =
                                x.Deposito.No_Ahorro,

                            CuentaHabiente =
                                $"{x.Persona.Nombres} {x.Persona.Apellidos}",

                            Saldo =
                                ultimaTransaccion.Saldo,

                            UltimaCapitalizacion =
                                ultimaTransaccion.Fecha.Date,

                            Interes_Aplicar =
                                interesMensual,
                            Tasa_Interes = x.Deposito.Interes,

                            FormaPago =
                                x.Deposito.Forma_Pago.Descripcion
                        };
                    })
                    .OrderBy(x => x.Cuenta)
                    .ToList();

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //[HttpGet]
        //public async Task<IActionResult> ConsultarCapitalizacionDepositoPlazo(DateTime fecha)
        //{
        //    try
        //    {
        //        var fechaConsulta = fecha.Date;

        //        // Obtener todas las transacciones
        //        var transacciones = await _context.Transaccion_Deposito_Plazo
        //            .ToListAsync();

        //        // Última transacción de cada depósito
        //        var ultimasTransacciones = transacciones
        //            .GroupBy(t => t.Id_Deposito_Plazo)
        //            .Select(g => g
        //                .OrderByDescending(x => x.Fecha)
        //                .ThenByDescending(x => x.Id_Transaccion_Deposito_Plazo)
        //                .First())
        //            .ToList();

        //        var ultimasTransaccionesDict = ultimasTransacciones
        //            .ToDictionary(
        //                x => x.Id_Deposito_Plazo,
        //                x => x);

        //        var depositos = await (
        //            from dp in _context.Deposito_Plazo
        //                .Include(x => x.Forma_Pago)
        //            join p in _context.Personas
        //                on dp.Id_Persona equals p.Id_Persona
        //            select new
        //            {
        //                Deposito = dp,
        //                Persona = p
        //            }
        //        ).ToListAsync();

        //        // ===========================================================
        //        // Saldo total de depósitos activos por persona
        //        // (No vencidos y con saldo mayor a cero)
        //        // ===========================================================
        //        var saldoTotalPorPersona = depositos
        //            .Where(x =>
        //            {
        //                if (!ultimasTransaccionesDict.TryGetValue(
        //                    x.Deposito.Id_Deposito_Plazo,
        //                    out var ultima))
        //                    return false;

        //                return ultima.Saldo > 0 &&
        //                       x.Deposito.Fecha_Vencimiento.Date >= fechaConsulta;
        //            })
        //            .GroupBy(x => x.Deposito.Id_Persona)
        //            .ToDictionary(
        //                g => g.Key,
        //                g => g.Sum(x =>
        //                    ultimasTransaccionesDict[x.Deposito.Id_Deposito_Plazo].Saldo)
        //            );

        //        var resultado = depositos
        //            .Where(x =>
        //            {
        //                if (!ultimasTransaccionesDict.TryGetValue(
        //                    x.Deposito.Id_Deposito_Plazo,
        //                    out var ultimaTransaccion))
        //                {
        //                    return false;
        //                }

        //                // Debe tener saldo
        //                if (ultimaTransaccion.Saldo <= 0)
        //                    return false;

        //                // No vencido
        //                if (x.Deposito.Fecha_Vencimiento.Date < fechaConsulta)
        //                    return false;

        //                // Días desde la última transacción
        //                var diasTranscurridos =
        //                    (fechaConsulta - ultimaTransaccion.Fecha.Date).Days;

        //                // Toca capitalizar
        //                return diasTranscurridos == 30;
        //            })
        //            .Select(x =>
        //            {
        //                var ultimaTransaccion =
        //                    ultimasTransaccionesDict[x.Deposito.Id_Deposito_Plazo];

        //                // Interés bruto
        //                var interesBruto = Math.Round(
        //                    (
        //                        ultimaTransaccion.Saldo *
        //                        (x.Deposito.Interes / 100m) /
        //                        360m
        //                    ) * 30m,
        //                    2);

        //                decimal retencion = 0m;

        //                // Total de depósitos activos de la persona
        //                var saldoTotalPersona = saldoTotalPorPersona.ContainsKey(x.Deposito.Id_Persona)
        //                    ? saldoTotalPorPersona[x.Deposito.Id_Persona]
        //                    : 0m;

        //                // Si posee 25,000 o más en depósitos activos aplica ISR del 10%
        //                if (saldoTotalPersona >= 25000m)
        //                {
        //                    retencion = Math.Round(interesBruto * 0.10m, 2);
        //                }

        //                var interesNeto = interesBruto - retencion;

        //                return new Capitalizacion
        //                {
        //                    Id_Deposito_Plazo =
        //                        x.Deposito.Id_Deposito_Plazo,

        //                    Id_Ahorro =
        //                        x.Deposito.Id_Ahorro,

        //                    Cuenta =
        //                        x.Deposito.No_Ahorro,

        //                    CuentaHabiente =
        //                        $"{x.Persona.Nombres} {x.Persona.Apellidos}",

        //                    Saldo =
        //                        ultimaTransaccion.Saldo,

        //                    UltimaCapitalizacion =
        //                        ultimaTransaccion.Fecha.Date,

        //                    // Interés que realmente se acreditará
        //                    Interes_Aplicar = interesNeto,

        //                    // Si agregas estas propiedades al DTO
        //                    //RetencionISR = retencion,
        //                    //InteresBruto = interesBruto,

        //                    Tasa_Interes = x.Deposito.Interes,

        //                    FormaPago =
        //                        x.Deposito.Forma_Pago.Descripcion
        //                };
        //            })
        //            .OrderBy(x => x.Cuenta)
        //            .ToList();

        //        return Ok(resultado);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
        [HttpGet]
        public async Task<IActionResult> ConsultarDepositosPorVencer()
        {
            try
            {
                var fechaHoy = DateTime.Today;

                //Últimas transacciones
                var transacciones = await _context.Transaccion_Deposito_Plazo
                    .ToListAsync();

                var ultimas = transacciones
                    .GroupBy(x => x.Id_Deposito_Plazo)
                    .Select(g => g.OrderByDescending(x => x.Fecha)
                    .ThenByDescending(x => x.Id_Transaccion_Deposito_Plazo)
                    .First())
                    .ToDictionary(x => x.Id_Deposito_Plazo);

                var lista = await (
                    from dp in _context.Deposito_Plazo
                    join p in _context.Personas
                        on dp.Id_Persona equals p.Id_Persona
                    select new
                    {
                        Deposito = dp,
                        Persona = p
                    }).ToListAsync();

                var resultado = lista
                    .Where(x =>
                    {
                        if (!ultimas.ContainsKey(x.Deposito.Id_Deposito_Plazo))
                            return false;

                        var saldo = ultimas[x.Deposito.Id_Deposito_Plazo].Saldo;

                        if (saldo <= 0)
                            return false;

                        var dias = (x.Deposito.Fecha_Vencimiento.Date - fechaHoy).Days;

                        return dias >= 0 && dias <= 30;
                    })
                    .Select(x =>
                    {
                        var saldo = ultimas[x.Deposito.Id_Deposito_Plazo].Saldo;

                        var dias = (x.Deposito.Fecha_Vencimiento.Date - fechaHoy).Days;

                        string color = "";

                        if (dias < 7)
                            color = "danger";
                        else if (dias < 15)
                            color = "warning";
                        else
                            color = "success";

                        return new DepositoPorVencer
                        {
                            IdDepositoPlazo = x.Deposito.Id_Deposito_Plazo,
                            Cuenta = x.Deposito.No_Ahorro,
                            CuentaHabiente = x.Persona.Nombres + " " + x.Persona.Apellidos,
                            Saldo = saldo,
                            FechaVencimiento = x.Deposito.Fecha_Vencimiento,
                            DiasRestantes = dias,
                            Celular = x.Persona.Telefono,
                            ColorAlerta = color
                        };
                    })
                    .OrderBy(x => x.DiasRestantes)
                    .ToList();

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /*solo ver la capitalización pero individual*/
        [HttpGet]
        public async Task<IActionResult> ConsultarTicketCapitalizacion(
        int idDepositoPlazo,
        DateTime fecha)
        {
            try
            {
                var deposito = await _context.Deposito_Plazo
                    .Include(x => x.Forma_Pago)
                    .FirstOrDefaultAsync(x =>
                        x.Id_Deposito_Plazo == idDepositoPlazo);

                if (deposito == null)
                    return BadRequest("Depósito no encontrado.");

                var persona = await _context.Personas
                    .FirstOrDefaultAsync(x =>
                        x.Id_Persona == deposito.Id_Persona);

                if (persona == null)
                    return BadRequest("Persona no encontrada.");

                // Última capitalización <= fecha enviada
                var capitalizacion = await _context.Transaccion_Deposito_Plazo
                    .Where(x =>
                        x.Id_Deposito_Plazo == idDepositoPlazo &&
                        x.Id_Tipo_Operacion_Ahorro == 1 &&
                        x.Fecha.Date <= fecha.Date)
                    .OrderByDescending(x => x.Fecha)
                    .ThenByDescending(x => x.Id_Transaccion_Deposito_Plazo)
                    .FirstOrDefaultAsync();

                if (capitalizacion == null)
                    return BadRequest("No existen capitalizaciones para este depósito.");

                var ticket = new TicketCapitalizacionDto
                {
                    IdDepositoPlazo = deposito.Id_Deposito_Plazo,

                    Cuenta = deposito.No_Ahorro,

                    CuentaHabiente =
                        $"{persona.Nombres} {persona.Apellidos}",

                    FechaCapitalizacion =
                        capitalizacion.Fecha,

                    InteresCapitalizado =
                        capitalizacion.Valor,

                    SaldoAnterior =
                        capitalizacion.Saldo - capitalizacion.Valor,

                    SaldoPosterior =
                        capitalizacion.Saldo,

                    TasaInteres =
                        deposito.Interes,

                    FormaPago =
                        deposito.Forma_Pago.Descripcion
                };

                return Ok(ticket);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> CancelarDepositoPlazo(
        int idDepositoPlazo,
        DateTime fechaCancelacion)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var deposito = await _context.Deposito_Plazo
                    .FirstOrDefaultAsync(x =>
                        x.Id_Deposito_Plazo == idDepositoPlazo);

                if (deposito == null)
                    return BadRequest("Depósito no encontrado.");

                var ultima = await _context.Transaccion_Deposito_Plazo
                    .Where(x => x.Id_Deposito_Plazo == idDepositoPlazo)
                    .OrderByDescending(x => x.Fecha)
                    .ThenByDescending(x => x.Id_Transaccion_Deposito_Plazo)
                    .FirstOrDefaultAsync();

                if (ultima == null)
                    return BadRequest("No existen transacciones.");

                if (ultima.Saldo <= 0)
                    return BadRequest("El depósito ya fue cancelado.");

                deposito.Fecha_Vencimiento = fechaCancelacion.Date;

                var transaccion = new Transaccion_Deposito_Plazo
                {
                    Id_Deposito_Plazo = deposito.Id_Deposito_Plazo,

                    Fecha = fechaCancelacion.Date,

                    Id_Tipo_Operacion_Ahorro = 2, // Cancelación

                    Valor = ultima.Saldo,

                    Saldo = 0
                };

                _context.Transaccion_Deposito_Plazo.Add(transaccion);

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return Ok("Depósito cancelado correctamente.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> RenovarDepositoPlazo(
             int idDepositoPlazo,
             int nuevoPlazo,
             DateTime fechaRenovacion)
        {
            try
            {
                var deposito = await _context.Deposito_Plazo
                    .FirstOrDefaultAsync(x =>
                        x.Id_Deposito_Plazo == idDepositoPlazo);

                if (deposito == null)
                    return BadRequest("Depósito no encontrado.");

                // Validar que aún tenga saldo
                var ultima = await _context.Transaccion_Deposito_Plazo
                    .Where(x => x.Id_Deposito_Plazo == idDepositoPlazo)
                    .OrderByDescending(x => x.Fecha)
                    .ThenByDescending(x => x.Id_Transaccion_Deposito_Plazo)
                    .FirstOrDefaultAsync();

                if (ultima == null)
                    return BadRequest("El depósito no posee transacciones.");

                if (ultima.Saldo <= 0)
                    return BadRequest("No es posible renovar un depósito cancelado.");

                deposito.Plazo = nuevoPlazo;

                deposito.Fecha_Vencimiento =
                    fechaRenovacion.Date.AddDays(nuevoPlazo);

                await _context.SaveChangesAsync();

                return Ok("Depósito renovado correctamente.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        private async Task<IEnumerable<Transaccion_Ahorro_Vista>> ConsultaTransacciones(int id_ahorro)
        {
            return await _context.Transaccion_Ahorro.Where(t => t.Id_Ahorro == id_ahorro && !t.Borrado_Logico).Select(s => new Transaccion_Ahorro_Vista
            {
                Id = s.Id_Transaccion_Ahorro,
                Fecha = s.Fecha,
                Deposito = s.Id_Tipo_Operacion_Ahorro == 1 ? s.Valor : 0,
                Retiro = s.Id_Tipo_Operacion_Ahorro == 3 ? s.Valor : 0,
                Saldo = s.Saldo
            }).ToListAsync();
        }
        [HttpPost]
        public async Task<IActionResult> RecalcularAhorroPorFecha(int idAhorro, DateTime fechaInicio)
        {
            try
            {
                var ahorro = await _context.Ahorro
                    .FirstOrDefaultAsync(a => a.Id_Ahorro == idAhorro);

                if (ahorro == null)
                    return NotFound("Ahorro no encontrado.");

                // 🔹 Obtener la última transacción ANTES de la fecha indicada
                var transAntes = await _context.Transaccion_Ahorro
                    .Where(t => t.Id_Ahorro == idAhorro && !t.Borrado_Logico && t.Fecha < fechaInicio)
                    .OrderByDescending(t => t.Fecha)
                    .ThenByDescending(t => t.Id_Transaccion_Ahorro)
                    .FirstOrDefaultAsync();

                decimal saldoPrevio = transAntes?.Saldo ?? 0;

                // 🔹 Obtener transacciones desde la fecha indicada (orden cronológico)
                var transaccionesDesde = await _context.Transaccion_Ahorro
                    .Where(t => t.Id_Ahorro == idAhorro && !t.Borrado_Logico && t.Fecha >= fechaInicio)
                    .OrderBy(t => t.Fecha)
                    .ThenBy(t => t.Id_Tipo_Operacion_Ahorro == 3 ? 1 : 0)
                    .ThenBy(t => t.Id_Transaccion_Ahorro)
                    .ToListAsync();

                if (!transaccionesDesde.Any())
                {
                    return Ok($"No hay transacciones para recalcular desde {fechaInicio:yyyy-MM-dd}. " +
                              $"Saldo actual: {ahorro.Saldo_Actual:N2}.");
                }

                // 🔹 Detectar duplicadas SOLO desde la fecha indicada
                var duplicadas = new List<Transaccion_Ahorro>();

                var grupos = transaccionesDesde
                    .GroupBy(t => new { t.Valor, t.Id_Tipo_Operacion_Ahorro });

                foreach (var grupo in grupos)
                {
                    var ordenadas = grupo.OrderBy(t => t.Fecha).ToList();

                    for (int i = 1; i < ordenadas.Count; i++)
                    {
                        var actual = ordenadas[i];
                        var anterior = ordenadas[i - 1];

                        // ⏱ diferencia en segundos
                        var diferencia = (actual.Fecha - anterior.Fecha).TotalSeconds;

                        // 🔥 regla: si fue dentro de 10 segundos → duplicado probable
                        if (diferencia >= 0 && diferencia <= 10)
                        {
                            duplicadas.Add(actual);
                        }
                    }
                }

                int eliminadas = 0;

                if (duplicadas.Any())
                {
                    eliminadas = duplicadas.Count;
                    _context.Transaccion_Ahorro.RemoveRange(duplicadas);
                    await _context.SaveChangesAsync(); // guardar eliminación
                }

                // 🔹 Recargar transacciones válidas (ya sin duplicadas)
                var transValidas = await _context.Transaccion_Ahorro
                    .Where(t => t.Id_Ahorro == idAhorro && !t.Borrado_Logico && t.Fecha >= fechaInicio)
                    .OrderBy(t => t.Fecha)
                    .ThenBy(t => t.Id_Tipo_Operacion_Ahorro == 3 ? 1 : 0)
                    .ThenBy(t => t.Id_Transaccion_Ahorro)
                    .ToListAsync();

                decimal saldoActual = saldoPrevio;

                // 🔹 Recalcular los saldos a partir de la fecha indicada
                foreach (var t in transValidas)
                {
                    if (t.Id_Tipo_Operacion_Ahorro == 1) // 🟢 depósito
                        saldoActual += t.Valor;
                    else if (t.Id_Tipo_Operacion_Ahorro == 3) // 🔴 retiro
                        saldoActual -= t.Valor;

                    if (saldoActual < 0)
                        saldoActual = 0;

                    t.Saldo = saldoActual;
                    _context.Transaccion_Ahorro.Update(t);
                }

                // 🔹 Actualizar saldo actual del ahorro
                ahorro.Saldo_Actual = saldoActual;
                _context.Ahorro.Update(ahorro);

                await _context.SaveChangesAsync();

                return Ok(
                    $"✅ Recalculo completado para ahorro #{idAhorro} desde {fechaInicio:yyyy-MM-dd}.\n" +
                    $"Saldo anterior: {saldoPrevio:N2}\n" +
                    $"Nuevo saldo: {saldoActual:N2}\n" +
                    $"Transacciones recalculadas: {transValidas.Count}\n" +
                    (eliminadas > 0 ? $"Duplicadas eliminadas: {eliminadas}" : "No se encontraron transacciones duplicadas.")
                );
            }
            catch (Exception ex)
            {
                return BadRequest($"❌ Error en el recalculo: {ex.Message}");
            }
        }
        [HttpPost]
        public async Task<IActionResult> RecalcularLineaAhorro(
        int idLineaAhorro,
        DateTime fechaInicio)
        {
            var ahorros = await _context.Ahorro
                .Where(a => a.Id_Linea_Ahorro == idLineaAhorro)
                .Select(a => a.Id_Ahorro)
                .ToListAsync();

            foreach (var idAhorro in ahorros)
            {
                await RecalcularAhorro(idAhorro, fechaInicio);
            }

            return Ok($"Se recalcularon {ahorros.Count} cuentas.");
        }
        private async Task RecalcularAhorro (int idAhorro, DateTime fechaInicio)
        {
            try
            {
                var ahorro = await _context.Ahorro
                    .FirstOrDefaultAsync(a => a.Id_Ahorro == idAhorro);

                if (ahorro == null)
                    return;

                // 🔹 Obtener la última transacción ANTES de la fecha indicada
                var transAntes = await _context.Transaccion_Ahorro
                    .Where(t => t.Id_Ahorro == idAhorro && !t.Borrado_Logico && t.Fecha < fechaInicio)
                    .OrderByDescending(t => t.Fecha)
                    .ThenByDescending(t => t.Id_Transaccion_Ahorro)
                    .FirstOrDefaultAsync();

                decimal saldoPrevio = transAntes?.Saldo ?? 0;

                // 🔹 Obtener transacciones desde la fecha indicada (orden cronológico)
                var transaccionesDesde = await _context.Transaccion_Ahorro
                    .Where(t => t.Id_Ahorro == idAhorro && !t.Borrado_Logico && t.Fecha >= fechaInicio)
                    .OrderBy(t => t.Fecha)
                    .ThenBy(t => t.Id_Tipo_Operacion_Ahorro == 3 ? 1 : 0)
                    .ThenBy(t => t.Id_Transaccion_Ahorro)
                    .ToListAsync();

                if (!transaccionesDesde.Any())
                {
                    return;
                }

                // 🔹 Detectar duplicadas SOLO desde la fecha indicada
                var duplicadas = new List<Transaccion_Ahorro>();

                var grupos = transaccionesDesde
                    .GroupBy(t => new { t.Valor, t.Id_Tipo_Operacion_Ahorro });

                foreach (var grupo in grupos)
                {
                    var ordenadas = grupo.OrderBy(t => t.Fecha).ToList();

                    for (int i = 1; i < ordenadas.Count; i++)
                    {
                        var actual = ordenadas[i];
                        var anterior = ordenadas[i - 1];

                        // ⏱ diferencia en segundos
                        var diferencia = (actual.Fecha - anterior.Fecha).TotalSeconds;

                        // 🔥 regla: si fue dentro de 10 segundos → duplicado probable
                        if (diferencia >= 0 && diferencia <= 10)
                        {
                            duplicadas.Add(actual);
                        }
                    }
                }

                int eliminadas = 0;

                if (duplicadas.Any())
                {
                    eliminadas = duplicadas.Count;
                    _context.Transaccion_Ahorro.RemoveRange(duplicadas);
                    await _context.SaveChangesAsync(); // guardar eliminación
                }

                // 🔹 Recargar transacciones válidas (ya sin duplicadas)
                var transValidas = await _context.Transaccion_Ahorro
                    .Where(t => t.Id_Ahorro == idAhorro && !t.Borrado_Logico && t.Fecha >= fechaInicio)
                    .OrderBy(t => t.Fecha)
                    .ThenBy(t => t.Id_Tipo_Operacion_Ahorro == 3 ? 1 : 0)
                    .ThenBy(t => t.Id_Transaccion_Ahorro)
                    .ToListAsync();

                decimal saldoActual = saldoPrevio;

                // 🔹 Recalcular los saldos a partir de la fecha indicada
                foreach (var t in transValidas)
                {
                    if (t.Id_Tipo_Operacion_Ahorro == 1) // 🟢 depósito
                        saldoActual += t.Valor;
                    else if (t.Id_Tipo_Operacion_Ahorro == 3) // 🔴 retiro
                        saldoActual -= t.Valor;

                    if (saldoActual < 0)
                        saldoActual = 0;

                    t.Saldo = saldoActual;
                    _context.Transaccion_Ahorro.Update(t);
                }

                // 🔹 Actualizar saldo actual del ahorro
                ahorro.Saldo_Actual = saldoActual;
                _context.Ahorro.Update(ahorro);

                await _context.SaveChangesAsync();

                return;
            }
            catch (Exception ex)
            {
                throw new Exception( $"Error recalculando ahorro {idAhorro}: {ex.Message}");
            }
        }
        /// <summary>
        /// consulta el interes ganado a la fecha
        /// </summary>
        /// <param name="id_ahorro"></param>
        /// <returns></returns>
        [Authorize(Roles = "Jefe,Administrador", AuthenticationSchemes = "Bearer")]

        public async Task<Transaccion_Ahorro_Vista> TrasladoIndividualConsulta(int id_ahorro)
        {
            Transaccion_Ahorro ta=await TrasladoAhorro(id_ahorro);
            return new Transaccion_Ahorro_Vista {
                Id = 0,
                Fecha = ta.Fecha,
                Deposito = ta.Id_Tipo_Operacion_Ahorro == 1 ? ta.Valor : 0,
                Retiro = ta.Id_Tipo_Operacion_Ahorro == 3 ? ta.Valor : 0,
                Saldo = ta.Saldo
            };
        }

        private async Task<Transaccion_Ahorro> TrasladoAhorro(int id_ahorro)
        {
            Transaccion_Ahorro ta = _context
                .Transaccion_Ahorro
                .Include("Ahorro")
                .Where(h=>h.Id_Ahorro==id_ahorro)
                .AsEnumerable()
                .Last();
            Linea_Ahorro la =await _context.Linea_Ahorro.FirstOrDefaultAsync(d=>d.Id_Linea_Ahorro==ta.Ahorro.Id_Linea_Ahorro);
            DateTime fecha_ahora = DateTime.Now;
            int diferencia_dias = (fecha_ahora - ta.Fecha).Days;
            AhorroRegistro ar = new AhorroRegistro {
                Saldo=ta.Saldo,
                Interes=la.Tasa_Interes,
                Fecha=fecha_ahora,
                Dias= diferencia_dias,
                Deposito=0.00M
            };
            CalculosYProcesoAhorro cpa = new CalculosYProcesoAhorro();
            AhorroRegistro ar_ci=  cpa.CalcularInteres(ar,la.Tasa_Interes);

            //int dias_capitabizables = la.Mes_Capitalizable * 30;
            if (diferencia_dias > 0)
                return new Transaccion_Ahorro
                {
                    Id_Ahorro = ta.Id_Ahorro,
                    Id_Tipo_Operacion_Ahorro = 1,
                    Valor = ar_ci.Deposito,
                    Saldo = ar_ci.Deposito+ar.Saldo,
                    Fecha = fecha_ahora,
                    Dias = ar.Dias
                };
            else
                return null;
        }

        /*reversión*/
        /// <summary>
        /// se revierte un abono a un credito
        /// </summary>
        /// <param name="ids">List int</param>
        /// <returns></returns>
        [Authorize(Roles = "Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpDelete]
        public async Task<IActionResult> Reversion_Ahorro([FromBody]int ids)
        {
            using (var dbContextTransaccion = _context.Database.BeginTransaction())
            {
                try {
                    await Reversion(ids, false);
                    dbContextTransaccion.Commit();
                    return Ok("Se ha revertido transacciones de ahorro seleccionadas");
                }
                catch(Exception ex)
                {
                    dbContextTransaccion.Rollback();
                    return BadRequest(ex.StackTrace);
                }
            }
                
        }
        //---------------------------------------------LIBRETA --------------------------------------------------------
        [Authorize(Roles = "Asistente,Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Rango_Transacciones(int id_ahorro ,int id_transaccion)
        {
            if (id_ahorro== 0 || id_transaccion == 0)
            {
                return BadRequest("seleccione una transaccion");
            }
            else
            {
                var transacciones = await ConsultaTransacciones(id_ahorro);
                IEnumerable<Transaccion_Ahorro_Vista> transaccionAhorroVistas = transacciones.ToList();
                var extraidos = transaccionAhorroVistas.Where(d => d.Id >= id_transaccion).ToList();
                return Ok(extraidos);
            }
        }
    }
}