using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelNucleo.Model;
using NucleoAyC.Entities;
using ModelNucleo.Model.ModelView;
using Microsoft.AspNetCore.Authorization;

namespace NucleoAyC.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = "Libreta")]
    [Route("api/[controller]")]
    [ApiController]
    public class LibretaController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly NegocioContexto _context;

        /// <summary>
        /// injección de roles y usuario
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userManager"></param>
        /// <param name="roleManager"></param>

        public LibretaController(
            NegocioContexto context,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        /// <summary>
        /// crear nueva libreta para una cuenta de ahorro
        /// </summary>
        /// <param name="libreta"></param>
        /// <returns></returns>
        [Authorize(Roles = "Asistente,Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> Crear_Libreta([FromBody] Libreta_Transaccion libreta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                bool existe = _context.LibretaTransacciones.Any(s => s.Id_Ahorro == libreta.Id_Ahorro && s.Activo == libreta.Activo);
                if (existe)
                {
                    bool ahorro = _context.Ahorro.AsNoTracking()
                        .Any(a => a.Id_Ahorro == libreta.Id_Ahorro);
                    if (ahorro == true)
                    {
                        Libreta_Transaccion libreta_Activa_Actual = _context.LibretaTransacciones.AsNoTracking()
                            .Single(s => s.Id_Ahorro == libreta.Id_Ahorro && s.Activo == libreta.Activo);
                        libreta_Activa_Actual.Activo = false;
                        _context.Entry(libreta_Activa_Actual).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                        _context.Add(libreta);
                        await _context.SaveChangesAsync();
                       
                        return Ok("El registro se creó correctamente");
                    }
                }
                _context.Add(libreta);
                await _context.SaveChangesAsync();
                /*registrar las transacciones simple, caja y contabilidad*/
                return Ok("La nueva libreta se ha creado");
            }
            catch (Exception ex) {
                return BadRequest(ex.StackTrace);
            }
        

            return BadRequest("No se ha podido crear una nueva libreta");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_ahorro"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Libreta_Ahorro(int id_ahorro)
        {
            try
            {
                Libreta_Transaccion libreta = await _context.LibretaTransacciones.Include(b => b.Ahorro)
                    .SingleAsync((g => g.Id_Ahorro == id_ahorro && g.Activo == true));
                return Ok(libreta);
            }
            catch (Exception e)
            {
                return BadRequest("Algo anda mal, intentalo de nuevo");
            }
        }


        /// <summary>
        /// se edita la libreta
        /// </summary>
        /// <param name="libretaTransaccion"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Editar_Libreta([FromBody] Libreta_Transaccion libretaTransaccion)
        {
            if (_context.LibretaTransacciones.Any(y => y.Id_Libreta == libretaTransaccion.Id_Libreta))
            {
                _context.Entry(libretaTransaccion).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok("El registro se actualizo correctamente");
            }
            return BadRequest("No existe el registro que intenta actualizar");
        }

        /// <summary>
        /// borrar una libreta
        /// </summary>
        /// <param name="id_libreta"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> Borrar_Libreta(int id_libreta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var libreta = await _context.LibretaTransacciones.SingleOrDefaultAsync(m => m.Id_Libreta == id_libreta);
            if (libreta == null)
            {
                return NotFound("Algo anda mal, intentalo de nuevo");
            }
            try
            {
                _context.LibretaTransacciones.Remove(libreta);
                await _context.SaveChangesAsync();
                return Ok("Libreta Eliminada Exitosamente");
            }
            catch (Exception ex)
            {
                return BadRequest("La libreta no existe");
            }
        }

        /// <summary>
        /// datos del usuario y la cuenta de ahorro
        /// para rotular una libreta
        /// </summary>
        /// <param name="id_ahorro"></param>
        /// <returns></returns>
        [Authorize(Roles = "Asistente,Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Datos_Usuario(int id_ahorro)
        {
            try
            {
                Ahorro tc = await _context.Ahorro
                    .Include(c => c.Persona)
                    .Include(a=> a.Persona.Cartera)
                    .SingleOrDefaultAsync(i => i.Id_Ahorro == id_ahorro);
                var usuario = tc.Persona.Cartera.Id_Usuario;

                Usuario_Agencia agencia = await _context.Usuario_Agencia
                    .Include(a => a.Agencia)
                    .SingleOrDefaultAsync(i => i.Id_Usuario == usuario);
              
                if (tc != null)
                {
                    Rotulado_Libreta factura = new Rotulado_Libreta
                    {
                        No_Cuenta = tc.No_Ahorro,
                        Nombres = tc.Persona.Nombres,
                        Apellidos = tc.Persona.Apellidos, 
                        Agencia = agencia.Agencia.Nombre
                        
                    };

                    return Ok(factura);
                }
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest("Algo anda mal, intentalo de nuevo");
            }
        }

    }
}