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

namespace NucleoAyC.Controllers
{
    /// <summary>
    /// Acceso Directo
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = "Acceso_Directo")]
    public class Acceso_DirectoController : ControllerBase
    {
        private readonly NegocioContexto _context;
        private readonly UserManager<IdentityUser> _userManager;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userManager"></param>
        public Acceso_DirectoController(NegocioContexto context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        /// <summary>
        /// lista de accesos directos
        /// categorizados
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Accesos_Directos()
        {
            var id_usuario = _userManager.GetUserId(HttpContext.User);
            if (_context.Categoria_Acceso_Directo.Any(c => c.Id_Usuario == id_usuario))
            {
                var categorias = await _context
                    .Categoria_Acceso_Directo
                    .AsNoTracking()
                    .Include(t => t.Acceso_Directos)
                    .Where(c => c.Id_Usuario == id_usuario)
                    .ToListAsync();
                return Ok(categorias);
            }
            else
                return BadRequest("No posee accesos directos");
        }

        /// <summary>
        /// detalle de la
        /// una Categoría de Acceso directo
        /// </summary>
        /// <param name="id_categoria"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Categoria_Acceso_Directo(int id_categoria)
        {
            var id_usuario = _userManager.GetUserId(HttpContext.User);
            if (_context
                .Categoria_Acceso_Directo
                .AsNoTracking()
                .Any(c => c.Id_Categoria_Acceso_Directo == id_categoria && c.Id_Usuario == id_usuario))
            {
                Categoria_Acceso_Directo categoria = await _context
                    .Categoria_Acceso_Directo
                    .AsNoTracking()
                    .SingleAsync(c => c.Id_Categoria_Acceso_Directo == id_categoria && c.Id_Usuario == id_usuario);
                return Ok(categoria);
            }
            else
                return BadRequest("no se encontró la categoría");
        }

        /// <summary>
        /// detalle de acceso directo
        /// </summary>
        /// <param name="id_acceso_directo"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Acceso_Directo(int id_acceso_directo)
        {
            if (_context
                .Acceso_Directo
                .AsNoTracking()
                .Any(c => c.Id_Acceso_Directo == id_acceso_directo))
            {
                Acceso_Directo categoria = await _context
                    .Acceso_Directo
                    .AsNoTracking()
                    .SingleAsync(c => c.Id_Acceso_Directo == id_acceso_directo);
                return Ok(categoria);
            }
            else
                return BadRequest("no se encontró la categoría");
        }

        /// <summary>
        /// crear una nueva categoría
        /// </summary>
        /// <param name="categoria"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Crear_Categoria_Acceso_Directo([FromBody] Categoria_Acceso_Directo categoria)
        {
            if (!ModelState.IsValid)
                return BadRequest("No ha llenado todos los datos");
            try
            {
                var id_usuario = _userManager.GetUserId(HttpContext.User);
                categoria.Id_Usuario = id_usuario;
                categoria.Id_Categoria_Acceso_Directo = 0;
                _context.Categoria_Acceso_Directo.Add(categoria);
                await _context.SaveChangesAsync();
                return Ok("Se ha guardado la Categoría");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace);
            }
        }

        /// <summary>
        /// Crear nuevo acceso directo
        /// </summary>
        /// <param name="acceso"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Crear_Acceso_Directo([FromBody] Acceso_Directo acceso)
        {
            if (!ModelState.IsValid)
                return BadRequest("No ha llenado todos los datos");
            try
            {
                _context.Acceso_Directo.Add(acceso);
                await _context.SaveChangesAsync();
                return Ok("Se ha guardado el Acceso Directo");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace);
            }
        }

        /// <summary>
        /// editar una categoría
        /// </summary>
        /// <param name="categoria"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Editar_Categoria_Acceso_Directo([FromBody] Categoria_Acceso_Directo categoria)
        {
            if (!ModelState.IsValid)
                return BadRequest("No ha llenado todos los datos");
            try
            {
                _context.Entry(categoria).State=EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok("Se ha modificado la Categoría");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace);
            }
        }

        /// <summary>
        /// editar acceso directo
        /// </summary>
        /// <param name="acceso"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Editar_Acceso_Directo([FromBody] Acceso_Directo acceso)
        {
            if (!ModelState.IsValid)
                return BadRequest("No ha llenado todos los datos");
            try
            {
                _context.Entry(acceso).State=EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok("Se ha modificado el Acceso Directo");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace);
            }
        }


        /// <summary>
        /// eliminar una categoría
        /// </summary>
        /// <param name="id_categoria"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> Eliminar_Categoria_Acceso_Directo(int id_categoria)
        {
            try
            {
                var categoria = _context.Categoria_Acceso_Directo.Find(id_categoria);
                _context.Remove(categoria).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
                return Ok("Se ha eliminado la Categoría");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace);
            }
        }

        /// <summary>
        /// eliminar acceso directo
        /// </summary>
        /// <param name="id_acceso_directo"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> Eliminar_Acceso_Directo(int id_acceso_directo)
        {
            try
            {
                var acceso = _context.Acceso_Directo.Find(id_acceso_directo);
                _context.Remove(acceso).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
                return Ok("Se ha eliminado el Acceso Directo");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace);
            }
        }
    }
}
