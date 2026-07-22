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

namespace NucleoAyC.Controllers
{
    /// <summary>
    /// manejo de caja sencillo
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = "Caja")]
    public class CajaController : Controller
    {
        private readonly NegocioContexto _context;
        private readonly UserManager<IdentityUser> _userManager;

        /// <summary>
        /// Constructor de la clase de Caja
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userManager"></param>
        public CajaController(NegocioContexto context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        /// <summary>
        /// Todas las cajas y bovedas según la petición
        /// este debería ser solo para administración no para jefa de operación
        /// </summary>
        /// <returns>List Caja</returns>
        [Authorize(Roles = "Jefe,Administrador", AuthenticationSchemes = "Bearer")]

        [HttpGet]
        public async Task<IActionResult> Cajas(string usuario=null,int id_agencia=0,bool boveda=false)
        {
            List<Caja> cajas =  _context.Caja.Include(r => r.Agencia).AsEnumerable().ToList();
            if (!string.IsNullOrEmpty(usuario))
                cajas = cajas.Where(t=>t.Id_Usuario == usuario).ToList();
            if (!string.IsNullOrEmpty(usuario) && id_agencia > 0)
                cajas = cajas.Where(c => c.Id_Usuario == usuario && c.Id_Agencia == id_agencia).ToList();
            if (id_agencia > 0)
                cajas = cajas.Where(t => t.Id_Agencia == id_agencia).ToList();


            List<Caja> cajas_nombres = (from c in cajas 
                                        join ua in _context.Usuario_Agencia 
                                            on c.Id_Usuario equals ua.Id_Usuario
                                        select new Caja { 
                                            Id_Caja=c.Id_Caja,
                                            Id_Usuario=c.Id_Usuario,
                                            Id_Agencia=c.Id_Agencia,
                                            Nombre_PC=!string.IsNullOrEmpty(ua.Nombre_Completo)? string.Format("({0}) {1}", c.Id_Caja, ua.Nombre_Completo): string.Format("({0}) {1}", c.Id_Caja, c.Id_Usuario)
                                        }).ToList();
            return Ok(cajas_nombres);

        }


        /// <summary>
        /// Crear una caja nueva
        /// </summary>
        /// <param name="caja">Caja</param>
        /// <returns>200</returns>
        [Authorize(Roles = "Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> Crear_Caja([FromBody] Caja caja)
        {
            if(ModelState.IsValid)
            {
                if (caja.Boveda && _context.Caja.Any(f => f.Boveda == caja.Boveda && f.Id_Agencia == caja.Id_Agencia))
                    return BadRequest("Ya existe una bóveda para la agencia solicitada");
                
                _context.Add(caja);
                await _context.SaveChangesAsync();
                 return Ok("El registro se creó correctamente");
            }
            return BadRequest("El modelo no es correcto");
        }

        /// <summary>
        /// Obtiene una caja detallada
        /// </summary>
        /// <param name="id_caja">int</param>
        /// <returns>Caja</returns>
     //   [Authorize(Roles = "Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Detalle_Caja(int id_caja)
        {
            Caja caja =await _context.Caja
                .Include(i=>i.Agencia)
                .Include(a=>a.Transaccion_Cajas)
                .SingleOrDefaultAsync(c=>c.Id_Caja==id_caja);

            return Ok(caja);
        }

        /// <summary>
        /// Editar caja
        /// </summary>
        /// <param name="caja">Caja</param>
        /// <returns>200</returns>
        [Authorize(Roles = "Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPut]
        public async Task<IActionResult> Editar_Caja([FromBody] Caja caja)
        {
            if (_context.Caja.Any(y => y.Id_Caja == caja.Id_Caja))
            {
                    
                _context.Entry(caja).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok("El registro se editó correctamente");
            }
            return BadRequest("No existe la caja que intenta editar");
        }

        /// <summary>
        /// es la bóveda a la que puede acceder el usuario logueado siempre que sea administrativo o jefe de operación
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Jefe,Administrador", AuthenticationSchemes = "Bearer")]

        [HttpGet]
        public async Task<IActionResult> Boveda_Asignada()
        {
            try
            {
                string id_usuario = _userManager.GetUserId(HttpContext.User);
                /*saber en que agencia esta el usuario*/
                Usuario_Agencia relacion = await _context.Usuario_Agencia.AsNoTracking().SingleOrDefaultAsync(t => t.Id_Usuario == id_usuario);
                /*ver si tiene boveda asignada para esa agencia*/
                Caja boveda = _context.Caja
                    .AsNoTracking()
                    .SingleOrDefault(
                        v => v.Id_Usuario == id_usuario
                             && v.Id_Agencia == relacion.Id_Agencia
                             && v.Boveda && v.Activa);
                /*muestra las cajas habilitadas para la agencia en cuestión*/
                List<Caja> cajas_agencia = _context.Caja
                    .AsNoTracking()
                    .Where(c => c.Id_Agencia == relacion.Id_Agencia && !c.Boveda && c.Activa).ToList();

                if (boveda != null)
                    return Ok(new Boveda_Cajas { Boveda = boveda, Cajas = cajas_agencia });
                return BadRequest("No se encontró una bóveda para el usuario y agencia solicitada");
            }
            catch (Exception e)
            {
                return null;
            }
           
        }

        [Authorize(Roles = "Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> CajasporAgencia()
        {
            string id_usuario = _userManager.GetUserId(HttpContext.User);
            Usuario_Agencia agencia = await _context.Usuario_Agencia.AsNoTracking()
                   .Include(a => a.Agencia)
                   .SingleOrDefaultAsync(i => i.Id_Usuario == id_usuario);
            var id_agencia = agencia.Id_Agencia;
            Func<Caja, bool> filtro_agencia = new Func<Caja, bool>(t => t.Id_Agencia == id_agencia && t.Activa && !t.Boveda);
            bool hay_caja = _context.Caja.Any(filtro_agencia);
            List<Caja> cajas = null;
            if(hay_caja)
                cajas = _context.Caja.AsNoTracking().Where(filtro_agencia).ToList();
            return Ok(cajas.Select(d => new Caja
            {
                Id_Usuario = d.Id_Usuario,
                Id_Agencia = d.Id_Agencia,
                Id_Caja = d.Id_Caja,
                Nombre_PC = string.Format("({0}) {1}", d.Id_Caja, d.Nombre_PC)
            }).ToList());
        }

        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public List<Caja> Cajas_Agencias()
        {
            using (NegocioContexto db = new NegocioContexto())
            {
                return db.Caja.ToList();
            }
        }
        [Authorize(Roles = "Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public IActionResult ObtenerAgenciaCajas()
        {
            var id_usuario = _userManager.GetUserId(HttpContext.User);
            Agencia agencia = _context.Usuario_Agencia
                .Include(a => a.Agencia)
                .SingleOrDefault(a => a.Id_Usuario == id_usuario).Agencia;
            return Ok(agencia);
        }
        /// <summary>
        /// es la asignación de fondos a otra agencia
        /// </summary>
        /// <param name="ingreso"></param>
        /// <param name="cantidad"></param>
        /// <param name="excedente"></param>
        /// <returns></returns>
        [Authorize(Roles = "Asistente,Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPut]
        public async Task<IActionResult> Ingreso_Egreso_Caja(
            bool ingreso = false, 
            decimal cantidad = 0.00M,
            decimal excedente=0.00M)
        {
            try
            {
                string userId = _userManager.GetUserId(HttpContext.User);
                Caja caja_actual = _context.Caja.AsNoTracking().Single(a => a.Id_Usuario == userId && !a.Boveda);
                Caja caja_boveda = _context.Caja.AsNoTracking()
                    .Single(a => a.Id_Agencia == caja_actual.Id_Agencia && a.Boveda);

                int id_tipo_operacion_simple = ingreso ? 1 : 2;

                Transaccion_Simple t_simple = new Transaccion_Simple
                {
                    Creado_En = DateTime.Now,
                    Creado_Por = userId,
                    Id_Tipo_Operacion_Simple = id_tipo_operacion_simple,
                    Monto = cantidad,
                    Id_Caja = caja_actual.Id_Caja,
                    Id_Caja_Boveda = caja_actual.Id_Caja
                };
                _context.Transaccion_Simple.Add(t_simple).State = EntityState.Added;
                await _context.SaveChangesAsync();

                Transaccion_Simple transaccion_simple = _context.Transaccion_Simple.AsNoTracking()
                    .Single(
                        a => a.Id_Tipo_Operacion_Simple == id_tipo_operacion_simple && 
                        a.Creado_En.Date == DateTime.Now.Date &&
                        a.Id_Caja_Boveda == caja_actual.Id_Caja && a.Monto == cantidad);

                Transaccion_Caja t_caja = new Transaccion_Caja
                {
                    Creado_En = DateTime.Now,
                    Fecha = DateTime.Now.Date,
                    Creado_Por = userId,
                    Monto = cantidad,
                    Id_Caja = caja_actual.Id_Caja,
                    Id_Tipo_Transaccion = ingreso ? 1 : 2,
                    No_Comprobante = await Generar_No_Comprobante(),
                    Id_Transaccion_Simple = transaccion_simple.Id_Transaccion_Simple

                };
                _context.Transaccion_Caja.Add(t_caja).State = EntityState.Added;
                await _context.SaveChangesAsync();

                Transaccion_Caja transaccion_caja = _context.Transaccion_Caja.AsNoTracking()
                    .Single(a => a.Id_Transaccion_Simple == transaccion_simple.Id_Transaccion_Simple);
                Asiento_Contable asiento_contable = new Asiento_Contable
                {
                    Id_Transaccion_Caja = transaccion_caja.Id_Transaccion_Caja,
                    Fecha = DateTime.Now,
                    Concepto = "Cierre Caja"
                };
                _context.Asiento_Contable.Add(asiento_contable).State = EntityState.Added;
                await _context.SaveChangesAsync();
                if (ingreso)
                {
                  _context.Detalle_Asiento_Contable.Add(Armar_Detalle_Asiento(
                  asiento_contable,
                  cantidad,
                  true,
                  asiento_contable.Concepto,
                  caja_boveda.Cuenta_Contable,
                  caja_boveda.Id_Agencia,
                  true)).State = EntityState.Added;
                  await _context.SaveChangesAsync();

                   _context.Detalle_Asiento_Contable.Add(Armar_Detalle_Asiento(
                   asiento_contable,
                   cantidad,
                   false,
                   asiento_contable.Concepto,
                   caja_actual.Cuenta_Contable,
                   caja_actual.Id_Agencia,
                   true)).State = EntityState.Added;
                   await _context.SaveChangesAsync();

                    if (excedente > 0)
                    {
                        /*crear en ingreso*/
                        Transaccion_Simple simple_excedente = new Transaccion_Simple {
                            Creado_En = DateTime.Now,
                            Creado_Por = userId,
                            Id_Tipo_Operacion_Simple = 12,
                            Monto = cantidad,
                            Id_Caja = caja_actual.Id_Caja,
                            Id_Caja_Boveda = 0
                        };

                        _context.Transaccion_Simple.Add(simple_excedente).State = EntityState.Added;
                        await _context.SaveChangesAsync();

                        Transaccion_Caja caja_excedente = new Transaccion_Caja
                        {
                            Creado_En = DateTime.Now,
                            Fecha = DateTime.Now.Date,
                            Creado_Por = userId,
                            Monto = cantidad,
                            Id_Caja = caja_actual.Id_Caja,
                            Id_Tipo_Transaccion = ingreso ? 1 : 2,
                            No_Comprobante = await Generar_No_Comprobante(),
                            Id_Transaccion_Simple = simple_excedente.Id_Transaccion_Simple

                        };
                        _context.Transaccion_Caja.Add(caja_excedente).State = EntityState.Added;
                        await _context.SaveChangesAsync();
                        /*CONTABILIDAD PARA EL INGRESO PARA EL EXCEDENTE*/
                        Conf_Partida_Contable conf = _context.Conf_Partida_Contable
                            .Single(d=>d.Identificador=="EXCEDENTE_CAJA" && d.Id_Agencia==caja_actual.Id_Agencia);

                        if (conf != null)
                        {
                            Conf_Detalle_Partida_Contable detalle = _context
                                .Conf_Detalle_Partida_Contable
                                .FirstOrDefault(c=>c.Identificador== "EXCEDENTE_CAJA");
                            Asiento_Contable asiento_excedente = new Asiento_Contable { 
                                Id_Transaccion_Caja= caja_excedente.Id_Transaccion_Caja,
                                Fecha=DateTime.Now,
                                Concepto=conf.Descripcion
                            };
                            _context.Asiento_Contable.Add(asiento_excedente).State = EntityState.Added;
                            await _context.SaveChangesAsync();

                            _context.Detalle_Asiento_Contable.Add(Armar_Detalle_Asiento(
                                asiento_excedente,
                                cantidad,
                                false,
                                asiento_excedente.Concepto,
                                caja_actual.Cuenta_Contable,
                                caja_actual.Id_Agencia,
                                true)).State = EntityState.Added;

                            await _context.SaveChangesAsync();
                        }
                    }
                }
                else {
                   _context.Detalle_Asiento_Contable.Add(Armar_Detalle_Asiento(
                   asiento_contable,
                   cantidad,
                   false,
                   asiento_contable.Concepto,
                   caja_boveda.Cuenta_Contable,
                   caja_boveda.Id_Agencia,
                   true)).State = EntityState.Added;
                   await _context.SaveChangesAsync();

                   _context.Detalle_Asiento_Contable.Add(Armar_Detalle_Asiento(
                   asiento_contable,
                   cantidad,
                   true,
                   asiento_contable.Concepto,
                   caja_actual.Cuenta_Contable,
                   caja_actual.Id_Agencia,
                   true)).State = EntityState.Added;
                   await _context.SaveChangesAsync();
                }
                return Ok("Cierre de Caja procesado");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// es la asignación de fondos a otra agencia
        /// </summary>
        /// <param name="excedente"></param>
        /// <returns></returns>
        [Authorize(Roles = "Asistente,Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPut]
        public async Task<IActionResult> Ingreso_Excedente_Caja(decimal excedente)
        {
            try
            {
                string userId = _userManager.GetUserId(HttpContext.User);
                Caja caja_actual = _context.Caja.AsNoTracking().Single(a => a.Id_Usuario == userId && !a.Boveda);
               
                    if (excedente > 0)
                    {
                        /*crear en ingreso*/
                        Transaccion_Simple simple_excedente = new Transaccion_Simple
                        {
                            Id_Transaccion_Simple=0,
                            Creado_En = DateTime.Now,
                            Creado_Por = userId,
                            Id_Tipo_Operacion_Simple = 12,
                            Monto = excedente,
                            Id_Caja = caja_actual.Id_Caja,
                            Id_Caja_Boveda = 0
                        };

                        _context.Transaccion_Simple.Add(simple_excedente).State = EntityState.Added;
                        await _context.SaveChangesAsync();

                        Transaccion_Caja caja_excedente = new Transaccion_Caja
                        {
                            Creado_En = DateTime.Now,
                            Fecha = DateTime.Now.Date,
                            Creado_Por = userId,
                            Monto = excedente,
                            Id_Caja = caja_actual.Id_Caja,
                            Id_Tipo_Transaccion = 1,
                            No_Comprobante = await Generar_No_Comprobante(),
                            Id_Transaccion_Simple = simple_excedente.Id_Transaccion_Simple

                        };
                        _context.Transaccion_Caja.Add(caja_excedente).State = EntityState.Added;
                        await _context.SaveChangesAsync();
                        /*CONTABILIDAD PARA EL INGRESO PARA EL EXCEDENTE*/
                        Conf_Partida_Contable conf = _context.Conf_Partida_Contable
                            .Single(d => d.Identificador == "EXCEDENTE_CAJA" && d.Id_Agencia == caja_actual.Id_Agencia);

                        if (conf != null)
                        {
                            Conf_Detalle_Partida_Contable detalle = _context
                                .Conf_Detalle_Partida_Contable
                                .FirstOrDefault(c => c.Identificador == "EXCEDENTE_CAJA" && c.Id_Conf_Partida_Contable==conf.Id_Conf_Partida_Contable);
                            Asiento_Contable asiento_excedente = new Asiento_Contable
                            {
                                Id_Transaccion_Caja = caja_excedente.Id_Transaccion_Caja,
                                Fecha = DateTime.Now,
                                Concepto = conf.Descripcion
                            };
                            _context.Asiento_Contable.Add(asiento_excedente).State = EntityState.Added;
                            await _context.SaveChangesAsync();

                            _context.Detalle_Asiento_Contable.Add(Armar_Detalle_Asiento(
                                asiento_excedente,
                                excedente,
                                false,
                                asiento_excedente.Concepto,
                                detalle.Cuenta_Contable,
                                caja_actual.Id_Agencia,
                                false)).State = EntityState.Added;

                        _context.Detalle_Asiento_Contable.Add(Armar_Detalle_Asiento(
                                asiento_excedente,
                                excedente,
                                true,
                                asiento_excedente.Concepto,
                                caja_actual.Cuenta_Contable,
                                caja_actual.Id_Agencia,
                                true)).State = EntityState.Added;

                        await _context.SaveChangesAsync();
                        }
                    }
                
                return Ok("Excedente Ingresado");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        protected async Task<string> Generar_No_Comprobante()
        {
            //generador de comprobante
            int id_ultima_transaccion = 0;
            if (await _context.Transaccion_Caja.AnyAsync())
                id_ultima_transaccion = _context.Transaccion_Caja.AsEnumerable().Last().Id_Transaccion_Caja;
            return GeneradorCodigo.GenerarSoloPorEntidad(id_ultima_transaccion);
            //termina generador de comprobante
        }
        private Detalle_Asiento_Contable Armar_Detalle_Asiento(
           Asiento_Contable asiento_contable,
           decimal valor,
           bool cargo,
           string concepto,
           string cuenta_contable,
           int id_agencia,
           bool esCaja = false)
        {
            return new Detalle_Asiento_Contable
            {
                Id_Asiento_Contable = asiento_contable.Id_Asiento_Contable,
                Abono = !cargo ? valor : 0.00M,
                Cargo = cargo ? valor : 0.00M,
                Concepto = concepto,
                Cuenta_Contable = cuenta_contable,
                EsCaja = esCaja,
                Id_Agencia = id_agencia
            };
        }
    }
}