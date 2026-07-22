using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelNucleo.Model;
using NucleoAyC.Entities;

namespace NucleoAyC.Controllers
{
    /// <summary>
    /// Configuracion de comite por rol
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = "Configuracion_Comite_Rol")]
    public class Configuracion_Comite_RolController : Controller
    {
        private readonly NegocioContexto _context;

        /// <summary>
        /// Constructor para la injección
        /// </summary>
        /// <param name="context"></param>
        public Configuracion_Comite_RolController(NegocioContexto context)
        {
            _context = context;
        }

        /// <summary>
        /// lista de los roles que son los que forman el comité
        /// </summary>
        /// <returns>Lista de Configuracion_Comite_Rol</returns>
        // GET: api/Configuracion_Comite_Rol
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public IEnumerable<Configuracion_Comite_Rol> ConfiguracionComite()
        {
            return _context.Configuracion_Comite_Rol;
        }

        // GET: api/Configuracion_Comite_Rol/5
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetConfiguracion_Comite_Rol(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var configuracion_Comite_Rol = await _context.Configuracion_Comite_Rol.SingleOrDefaultAsync(m => m.Id_Configuracion_Comite == id);

            if (configuracion_Comite_Rol == null)
            {
                return NotFound();
            }

            return Ok(configuracion_Comite_Rol);
        }

        // PUT: api/Configuracion_Comite_Rol/5
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutConfiguracion_Comite_Rol([FromRoute] int id, [FromBody] Configuracion_Comite_Rol configuracion_Comite_Rol)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != configuracion_Comite_Rol.Id_Configuracion_Comite)
            {
                return BadRequest();
            }

            _context.Entry(configuracion_Comite_Rol).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Configuracion_Comite_RolExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Configuracion_Comite_Rol
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> PostConfiguracion_Comite_Rol([FromBody] Configuracion_Comite_Rol configuracion_Comite_Rol)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Configuracion_Comite_Rol.Add(configuracion_Comite_Rol);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetConfiguracion_Comite_Rol", new { id = configuracion_Comite_Rol.Id_Configuracion_Comite }, configuracion_Comite_Rol);
        }

        // DELETE: api/Configuracion_Comite_Rol/5
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConfiguracion_Comite_Rol([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var configuracion_Comite_Rol = await _context.Configuracion_Comite_Rol.SingleOrDefaultAsync(m => m.Id_Configuracion_Comite == id);
            if (configuracion_Comite_Rol == null)
            {
                return NotFound();
            }

            _context.Configuracion_Comite_Rol.Remove(configuracion_Comite_Rol);
            await _context.SaveChangesAsync();

            return Ok(configuracion_Comite_Rol);
        }

        private bool Configuracion_Comite_RolExists(int id)
        {
            return _context.Configuracion_Comite_Rol.Any(e => e.Id_Configuracion_Comite == id);
        }
    }
}