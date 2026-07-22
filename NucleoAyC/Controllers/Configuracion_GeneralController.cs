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
    /// <summary>
    /// para la configuracion general parametros
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = "Configuracion_General")]
    public class Configuracion_GeneralController : Controller
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
        public Configuracion_GeneralController(
            NegocioContexto context,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        /// <summary>
        /// lista de las configuraciones
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Configuraciones()
        {
            List<Configuracion_General> configuraciones =await _context.Configuracion_General.ToListAsync();
            return Ok(configuraciones);
        }

        /// <summary>
        /// para mostrar la edición de una configuracion
        /// </summary>
        /// <param name="id_configuracion"></param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Detalle(int id_configuracion)
        {
            Configuracion_General configuracion = await _context.Configuracion_General.SingleAsync(c=>c.Id_Configuracion==id_configuracion);
            return Ok(configuracion);
        }

        /// <summary>
        /// edición de una configuración
        /// </summary>
        /// <param name="configuracion_actualizar"></param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPut]
        public async Task<IActionResult> Editar([FromBody] Configuracion_General configuracion_actualizar)
        {
            Configuracion_General configuracion = await _context.Configuracion_General.SingleAsync(f=>f.Nombre== configuracion_actualizar.Nombre);
            if (configuracion != null)
            {
                configuracion.Valor = configuracion_actualizar.Valor;
                _context.Entry(configuracion).State=EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok("Se ha actualizado el valor de la configuración");
            }
            return BadRequest("No se pudo configurar el valor de configuración");
        }



    }
}