using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelNucleo.CalculosYProceos;
using ModelNucleo.Dto.Reportes_Ahorro;
using ModelNucleo.Model;
using NucleoAyC.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NucleoAyC.Controllers
{
    /// <summary>
    /// Manejo de ahorros
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = "Ahorros")]
    public class AhorrosController : Base_TransaccionController
    {
        /// <summary>
        /// constructor e injección de base de datos
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userManager"></param>
        /// <param name="roleManager"></param>
        public AhorrosController(NegocioContexto context,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
            : base(context, userManager, roleManager) 
        { 
        }

        /// <summary>
        /// Get Obtener la lista de cuentas de ahorro por cliente
        /// </summary>
        /// <param name="id_persona">int id de persona</param>
        /// <returns>Lista de Ahorro</returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task <IActionResult> AhorrosPorPersona(int id_persona)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            List<Ahorro> ahorros=await _context.Ahorro.AsNoTracking().Where(h => h.Id_Persona == id_persona).ToListAsync();
           
            if (User.IsInRole("Asesor"))
            {
               var a = ahorros.Select(a => new Ahorro
                {
                    Id_Ahorro = a.Id_Ahorro,
                    No_Ahorro=a.No_Ahorro,
                    Id_Linea_Ahorro = a.Id_Linea_Ahorro,
                    Id_Persona = a.Id_Persona

                });
                return Ok(a);
            }
            return Ok( ahorros);
        }

        /// <summary>
        /// Get Se filtrar los ahorros por el prefil de quien los consulta
        /// o las personas que se tienen asignadas
        /// </summary>
        /// <returns>Lista de Ahorros</returns>
        [Authorize(Roles = "Administrador,Asesor,Gerente,Jefe", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<List<Ahorro>> Ahorros(int id_agencia=0)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            List<Ahorro> ahorros = null;
            if (User.IsInRole("Gerente") || User.IsInRole("Jefe"))
                ahorros= await FiltrarAhorrosPorAgencia(userId, id_agencia);
            if (User.IsInRole("Administrador"))
                ahorros = await FiltrarAhorrosPorAgencia(userId, id_agencia);/*solo la agencia*/
            if (User.IsInRole("Asesor"))
                ahorros= await FiltrarAhorrosPorCartera(userId);
            return ahorros;
        }

        private async Task<List<Ahorro>> FiltrarAhorrosPorAgencia(string userId, int id_agencia=0)
        {
            if (userId != null && id_agencia == 0)
                id_agencia = _context.Usuario_Agencia.SingleOrDefault(a => a.Id_Usuario == userId).Id_Agencia;
            List<string> id_usuarios = _context.Usuario_Agencia.Where(c => c.Id_Agencia == id_agencia).Select(q => q.Id_Usuario).ToList();
            List<Ahorro> ahorrosAgencia = new List<Ahorro>();
            foreach (string id_usuario in id_usuarios)
            {
                /*se verifica si es asesor para obtener su cartera*/
                IdentityUser identidad = await _userManager.FindByNameAsync(id_usuario);
                if (await _userManager.IsInRoleAsync(identidad, "Asesor"))
                {
                    List<Ahorro> ahorros = await FiltrarAhorrosPorCartera(id_usuario);
                    if(ahorros!=null)
                        ahorrosAgencia.AddRange(ahorros);
                }
            }
            return ahorrosAgencia;
        }

        private async Task<List<Ahorro>> FiltrarAhorrosPorCartera(string id_usuario)
        {
            if (_context.Cartera.AsNoTracking().Any(c => c.Id_Usuario == id_usuario))
            {
                Cartera cartera = await _context.Cartera.AsNoTracking().SingleAsync(d => d.Id_Usuario == id_usuario);
                List<Ahorro> ahorros = _context.Ahorro
                    .AsNoTracking()
                    .Include(r => r.Persona)
                    .Where(b => b.Persona.Id_Cartera == cartera.Id_Cartera)
                    .ToList();
                return ahorros;
            }
            return null;
        }

        /// <summary>
        /// Get Se obtiene un detalle de un ahorro dado
        /// </summary>
        /// <param name="id_ahorro">int id de ahorro</param>
        /// <returns>Ahorro</returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> GetAhorroDetalle(int id_ahorro)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ahorro = await _context.Ahorro.SingleOrDefaultAsync(m => m.Id_Ahorro == id_ahorro);

            if (ahorro == null)
            {
                return NotFound();
            }

            return Ok(ahorro);
        }

        /// <summary>
        /// Put Para modificar el detalle dado el id
        /// </summary>
        /// <param name="id_ahorro">int id de ahorro</param>
        /// <param name="ahorro">Ahorro objeto json</param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPut]
        public async Task<IActionResult> Modificar([FromBody] Ahorro ahorro)
        {
            Ahorro Ahorro_verificado = await _context.Ahorro.
                AsNoTracking()
                .SingleAsync(f => f.Id_Ahorro == ahorro.Id_Ahorro);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                _context.Update(ahorro).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return Ok("El registro se ha guardado");
                    
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace);
            }
        }

        /// <summary>
        /// Post Crear un registro de ahorro para una persona
        /// </summary>
        /// <param name="ahorro">Ahorro objeto json</param>
        /// <returns></returns>
        [Authorize(Roles = "Asistente,Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> NuevoAhorro([FromBody] Ahorro ahorro)
        {
            using (NegocioContexto db = new NegocioContexto())
            { 
                    if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                int id_ahorro_ultimo=0;
                if (_context.Ahorro.Any())
                    id_ahorro_ultimo = _context.Ahorro.AsEnumerable().Last().Id_Ahorro;
                ahorro.Fecha_Apertura = DateTime.Now;
                ahorro.No_Ahorro = GeneradorCodigo.Generar(id_ahorro_ultimo,ahorro.Id_Persona);

    #if RELEASE || DEBUG
                ahorro.No_Ahorro = GeneradorCodigo.Generar(id_ahorro_ultimo, ahorro.Id_Persona);
    #endif
    #if SOLOTEST
                ahorro.No_Ahorro = GenerarCodigo(ahorro.Id_Persona, ahorro.Id_Linea_Ahorro, false);
    #endif

                try
                {
                    Linea_Ahorro linea_ahorro = _context.Linea_Ahorro.AsNoTracking()
                        .Single(a=>a.Id_Linea_Ahorro == ahorro.Id_Linea_Ahorro);
                    ahorro.Tasa_Interes = linea_ahorro.Tasa_Interes;
                    _context.Ahorro.Add(ahorro);
                    await _context.SaveChangesAsync();
                    Persona estadoPersona = _context.Personas.AsNoTracking().Single(a => a.Id_Persona == ahorro.Id_Persona);
                    if (estadoPersona.Estado_Socio !=(Estado_Socio) 3 )
                    {
                        Estado_Socio estado = Estado_Socio.Activo;
                        estadoPersona.Estado_Socio = estado;
                        _context.Entry(estadoPersona).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                    }
                    /*crear la acción*/
                    /*definir el valor de la accion*/
                    decimal valor = 0.00M;
                    Configuracion_General conf =await _context.Configuracion_General.SingleAsync(c=>c.Nombre=="VALOR_ACCION");
                    if (conf.Tipo == "DECIMAL" && !string.IsNullOrEmpty(conf.Valor))
                        valor = Convert.ToDecimal(conf.Valor);
                    if (db.Accion.Any(a => a.Id_Persona != ahorro.Id_Persona))
                    {
                        Accion accion = new Accion
                        {
                            Id_Persona = ahorro.Id_Persona,
                            Id_Tipo_Accion = 1,
                            Valor_Unitario = valor,
                            Creado_En = DateTime.Now.Date
                        };
                        await Nueva_Accion(accion);
                    }
                    return Ok("La cuenta de ahorro ha sido registrado correctamente");
                } catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
           }
        }

        /// <summary>
        /// Delete Borrar un ahorro dado su id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpDelete]
        public async Task<IActionResult> DeleteAhorro([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ahorro = await _context.Ahorro.SingleOrDefaultAsync(m => m.Id_Ahorro == id);
            if (ahorro == null)
            {
                return NotFound();
            }
            try
            {
                _context.Ahorro.Remove(ahorro);
                await _context.SaveChangesAsync();
                return Ok(ahorro);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        //manejo de las firmas de autorizacion
        /// <summary>
        /// nueva firma autorizada
        /// </summary>
        /// <param name="firma"></param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> Nueva_Firma_Autorizada([FromBody] Firma_Autorizada_Ahorro firma)
        {
            bool firma_encontrada =_context
                .Firma_Autorizada_Ahorro
                .Any(s=>s.Id_Ahorro==firma.Id_Ahorro && s.DUI==firma.DUI);
            if (!firma_encontrada)
            {
                //se guarda
                if (!ModelState.IsValid)
                    return BadRequest("Los datos no están correctos");
                firma.Activo = true;
                _context.Add(firma);
                await _context.SaveChangesAsync();
                return Ok("La firma ha sido autorizada");
            }
            return BadRequest("Ya existe una firma autorizada con el mismo DUI");
        }

        /// <summary>
        /// obtener la lista de
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Firmas_Autorizada(int id_ahorro)
        {
            List<Firma_Autorizada_Ahorro> firmas = await _context
                .Firma_Autorizada_Ahorro.Where(k=>k.Id_Ahorro==id_ahorro && k.Activo).ToListAsync();
            return Ok(firmas);
        }

        /// <summary>
        /// detalle de un firma autorizada
        /// </summary>
        /// <param name="id_firma"></param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Firma_Autorizada_Detalle(int id_firma)
        {
            Firma_Autorizada_Ahorro firma =await _context
                .Firma_Autorizada_Ahorro
                .SingleAsync(f=>f.Id_Firma_Autorizada_Ahorro==id_firma && f.Activo);
            return Ok(firma);
        }

        /// <summary>
        /// Desactivar firma autorizada por su id
        /// </summary>
        /// <param name="id_firma_autorizada"></param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPut]
        public async Task<IActionResult> Desactivar_Firma_Autorizada(int id_firma_autorizada)
        {
            Firma_Autorizada_Ahorro firma = await _context
                .Firma_Autorizada_Ahorro
                .SingleAsync(a=>a.Id_Firma_Autorizada_Ahorro==id_firma_autorizada && a.Activo);
            if (firma != null)
            {
                firma.Activo = false;
                await _context.SaveChangesAsync();
                return Ok("Se ha desactivado una firma autorizada");
            }
            else
                return BadRequest("No se encontró la firma autorizada");
        }

        private bool AhorroExists(int id)
        {
            return _context.Ahorro.Any(e => e.Id_Ahorro == id);
        }

        //---------------------------------------------------------------------Documentos Ahorro--------------------------------------------------------------

        [Authorize(Roles = "Asistente,Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Solicitud_Socio(int id_persona)
        {
            try
            {
                string id_usuario = _userManager.GetUserId(HttpContext.User);
                Persona persona = await _context.Personas.AsNoTracking()
                    .SingleAsync(w => w.Id_Persona == id_persona);
                Usuario_Agencia agencia = await _context.Usuario_Agencia.AsNoTracking()
                    .SingleAsync(a => a.Id_Usuario == id_usuario);
                Agencia ubicacion = await _context.Agencia.AsNoTracking()
                    .SingleAsync(a => a.Id_Agencia == agencia.Id_Agencia);
                persona.Comentario = ubicacion.Ciudad;

                return Ok(persona);
            }
            catch (Exception e)
            {
                return BadRequest("Algo anda mal, intentalo de nuevo");
            }
        }

        [Authorize(Roles = "Asistente,Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Solicitud_Cambio_Beneficiario(int id_persona, int id_ahorro)
        {
            try
            {
                string id_usuario = _userManager.GetUserId(HttpContext.User);
                Ahorro ahorro = await _context.Ahorro.AsNoTracking().SingleAsync(a => a.Id_Ahorro == id_ahorro);
                Persona persona = await _context.Personas.AsNoTracking()
                    .Include(a=>a.Ahorros)
                    .SingleAsync(w => w.Id_Persona == id_persona && w.Id_Persona == ahorro.Id_Persona);
                Usuario_Agencia agencia = await _context.Usuario_Agencia.AsNoTracking()
                    .SingleAsync(a => a.Id_Usuario == id_usuario);
                Agencia ubicacion = await _context.Agencia.AsNoTracking()
                    .SingleAsync(a => a.Id_Agencia == agencia.Id_Agencia);
                persona.Comentario = ubicacion.Ciudad;
                return Ok(persona);
            }
            catch (Exception e)
            {
                return BadRequest("Algo anda mal, intentalo de nuevo");
            }
        }

        [Authorize(Roles = "Asistente,Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Perfil_Socio(int id_persona)
        {
            try
            {
                string id_usuario = _userManager.GetUserId(HttpContext.User);
                Persona persona = await _context.Personas.AsNoTracking()
                    .SingleAsync(w => w.Id_Persona == id_persona);
                Usuario_Agencia agencia = await _context.Usuario_Agencia.AsNoTracking()
                    .SingleAsync(a => a.Id_Usuario == id_usuario);
                Agencia ubicacion = await _context.Agencia.AsNoTracking()
                    .SingleAsync(a => a.Id_Agencia == agencia.Id_Agencia);
                persona.Comentario = ubicacion.Ciudad;

                return Ok(persona);
            }
            catch (Exception e)
            {
                return BadRequest("Algo anda mal, intentalo de nuevo");
            }
        }

        [Authorize(Roles = "Asistente,Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Declaracion_Jurada_Persona_Natural(int id_persona)
        {
            try
            {
                string id_usuario = _userManager.GetUserId(HttpContext.User);
                Persona persona = await _context.Personas.AsNoTracking()
                    .SingleAsync(w => w.Id_Persona == id_persona);
                Usuario_Agencia agencia = await _context.Usuario_Agencia.AsNoTracking()
                    .SingleAsync(a => a.Id_Usuario == id_usuario);
                Agencia ubicacion = await _context.Agencia.AsNoTracking()
                    .SingleAsync(a => a.Id_Agencia == agencia.Id_Agencia);
                persona.Comentario = ubicacion.Ciudad;

                return Ok(persona);
            }
            catch (Exception e)
            {
                return BadRequest("Algo anda mal, intentalo de nuevo");
            }
        }
        [Authorize(Roles = "Asistente,Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Contrato_Deposito(int id_persona,int id_ahorro)
        {
            try
            {
                string id_usuario = _userManager.GetUserId(HttpContext.User);
                Ahorro ahorro = await _context.Ahorro.AsNoTracking().SingleAsync(a => a.Id_Ahorro == id_ahorro);
                Persona persona = await _context.Personas.AsNoTracking()
                    .Include(a => a.Ahorros)
                    .SingleAsync(w => w.Id_Persona == id_persona && w.Id_Persona == ahorro.Id_Persona);
                Usuario_Agencia agencia = await _context.Usuario_Agencia.AsNoTracking()
                    .SingleAsync(a => a.Id_Usuario == id_usuario);
                Agencia ubicacion = await _context.Agencia.AsNoTracking()
                    .SingleAsync(a => a.Id_Agencia == agencia.Id_Agencia);
                persona.Comentario = ubicacion.Ciudad;

                return Ok(persona);
            }
            catch (Exception e)
            {
                return BadRequest("Algo anda mal, intentalo de nuevo");
            }
        }
        [HttpGet]
        public async Task<IActionResult> ReporteConsolidado(
         int idLineaAhorro,
         int idAgencia,
         DateTime fecha)
        {
            try
            {
                var linea = await _context.Linea_Ahorro
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id_Linea_Ahorro == idLineaAhorro);
                var fechaCorte = fecha.Date.AddDays(1);
                if (linea == null)
                    return NotFound();

                var detalle = await (
                    from ahorro in _context.Ahorro.AsNoTracking()

                    join persona in _context.Personas.AsNoTracking()
                        on ahorro.Id_Persona equals persona.Id_Persona

                    join cartera in _context.Cartera.AsNoTracking()
                        on persona.Id_Cartera equals cartera.Id_Cartera

                    join usuarioAgencia in _context.Usuario_Agencia.AsNoTracking()
                        on cartera.Id_Usuario equals usuarioAgencia.Id_Usuario

                    where ahorro.Id_Linea_Ahorro == idLineaAhorro
                       && usuarioAgencia.Id_Agencia == idAgencia

                    orderby persona.Apellidos, persona.Nombres

                    select new DetalleConsolidadoAhorroDTO
                    {
                        IdAhorro = ahorro.Id_Ahorro,
                        NoCuenta = ahorro.No_Ahorro,
                        Socio = persona.Nombres + " " + persona.Apellidos,
                        Dui = persona.Dui,

                        Saldo = _context.Transaccion_Ahorro
                     .Where(t =>
                         t.Id_Ahorro == ahorro.Id_Ahorro &&
                         t.Fecha < fechaCorte)

                     // Primero obtenemos la última fecha de transacción
                     .Where(t =>
                         t.Fecha ==
                         _context.Transaccion_Ahorro
                             .Where(x =>
                                 x.Id_Ahorro == ahorro.Id_Ahorro &&
                                 x.Fecha < fechaCorte)
                             .Max(x => x.Fecha))

                     // Si todas las transacciones de esa fecha
                     // son migradas, tomamos el saldo menor.
                     .All(t => t.Creado_Por == "Migra")

                     ? _context.Transaccion_Ahorro
                         .Where(t =>
                             t.Id_Ahorro == ahorro.Id_Ahorro &&
                             t.Fecha < fechaCorte &&
                             t.Fecha ==
                             _context.Transaccion_Ahorro
                                 .Where(x =>
                                     x.Id_Ahorro == ahorro.Id_Ahorro &&
                                     x.Fecha < fechaCorte)
                                 .Max(x => x.Fecha) &&
                             t.Creado_Por == "Migra")
                         .Min(t => t.Saldo)

                     // Si no son todas migradas,
                     // mantenemos EXACTAMENTE tu lógica original.
                     : _context.Transaccion_Ahorro
                         .Where(t =>
                             t.Id_Ahorro == ahorro.Id_Ahorro &&
                             t.Fecha < fechaCorte)
                         .OrderByDescending(t => t.Fecha)
                         .ThenByDescending(t => t.Id_Transaccion_Ahorro)
                         .Select(t => t.Saldo)
                         .FirstOrDefault()
                    })
                    .ToListAsync();


                var reporte = new ReporteConsolidadoAhorroDTO
                {
                    IdLineaAhorro = linea.Id_Linea_Ahorro,
                    NombreLineaAhorro = linea.Descripcion,
                    FechaCorte = fecha,
                    TotalSaldo = detalle.Sum(x => x.Saldo),
                    TotalCuentas = detalle.Count,
                    Detalle = detalle
                };

                return Ok(reporte);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}