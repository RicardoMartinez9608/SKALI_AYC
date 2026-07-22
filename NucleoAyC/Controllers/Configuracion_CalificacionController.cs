using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelNucleo.Model;
using NucleoAyC.Entities;

namespace NucleoAyC.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = "Configuracion_Calificacion")]
    public class Configuracion_CalificacionController : Controller
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
        public Configuracion_CalificacionController(
            NegocioContexto context,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [Authorize(Roles = "Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> Crear_Configuracion([FromBody] Configuracion_Calificacion configuracion)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Configuracion_Calificacion.Add(configuracion).State = EntityState.Added;
                    await _context.SaveChangesAsync();
                    return Ok("Configuracion almacenada exitosamente");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return BadRequest("Algo anda mal, intentalo de nuevo");
        }

        [Authorize(Roles = "Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Obtener_Calificaciones()
        {
            List<Configuracion_Calificacion> calificaciones = await _context.Configuracion_Calificacion
                .Include(a=>a.Buro).ToListAsync();
            return Ok(calificaciones);
        }

        [Authorize(Roles = "Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Obtener_Calificaciones_por_ID(int id_calificacion)
        {
            Configuracion_Calificacion calificacion = await _context.Configuracion_Calificacion.SingleAsync(g => g.Id_Configuracion == id_calificacion);
            return Ok(calificacion);
        }


        [Authorize(Roles = "Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPut]
        public async Task<IActionResult> Editar_Calificacion([FromBody] Configuracion_Calificacion calificacion_actualizar)
        {
            try
            {
                _context.Entry(calificacion_actualizar).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok("la calificación ha sido modificado");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace);
            }
        }

        [Authorize(Roles = "Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public List<Buro> Buros()
        {
            using (NegocioContexto db = new NegocioContexto())
            {
                return db.Buros.ToList();
            }
        }

    }
}
