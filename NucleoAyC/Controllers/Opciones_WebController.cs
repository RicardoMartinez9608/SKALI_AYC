using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Query.Internal;
using ModelNucleo.Model;
using NucleoAyC.Entities;

namespace NucleoAyC.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = "Opciones_Web")]
    public class Opciones_WebController : Controller
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
        public Opciones_WebController(
            NegocioContexto context,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: api/Opciones_Web
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> GetMenuCategoriaVista()
        {
            List<Menu_Categoria_vista> menu = _context.MenuCategoriaVista.Include(
                b=>b.Menu_Contenido).ToList();
            return Ok(menu);
        }

        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> GetContenidoporID(int idcategoria)
        {
            List<Menu_Contenido_Vista> menu =await _context.MenuContenidoVista.Where(g => g.Id_Menu_Categoria_Vista == idcategoria)
                .ToListAsync();
            return Ok(menu);
        }

        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> DetalleCategoria(int idcategoria)
        {
            Menu_Categoria_vista menu = await _context.MenuCategoriaVista.SingleAsync(g => g.Id_Menu_Categoria_Vista == idcategoria);
            return Ok(menu);
        }

        /// <summary>
        /// edición de una configuración
        /// </summary>
        /// <param name="categoria_actualizar"></param>
        /// <returns></returns>

        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPut]
        public async Task<IActionResult> Editar([FromBody] Menu_Categoria_vista categoria_actualizar)
        {
           // Menu_Categoria_vista categoria = await _context.MenuCategoriaVista.SingleAsync(f => f.Titulo == categoria_actualizar.Titulo);
            if (categoria_actualizar != null)
            {
           //     categoria.Titulo = categoria_actualizar.Titulo;
                _context.Entry(categoria_actualizar).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok("Se ha actualizado el valor de la Categoria");
            }
            return BadRequest("No se pudo configurar el valor de Categoria");
        }

        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> DetalleContenido(int idcontenido)
        {
            Menu_Contenido_Vista menu = await _context.MenuContenidoVista.SingleAsync(g => g.Id_Menu_Contenido_Vista == idcontenido);
            return Ok(menu);
        }

        /// <summary>
        /// edición de una configuración
        /// </summary>
        /// <param name="contenido_actualizar"></param>
        /// <returns></returns>

        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPut]
        public async Task<IActionResult> EditarContenido([FromBody] Menu_Contenido_Vista contenido_actualizar)
        {
            if (contenido_actualizar != null)
            {
                _context.Entry(contenido_actualizar).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok("Se ha actualizado el valor del Contenido");
            }
            return BadRequest("No se pudo configurar el valor del Contenido");
        }
        //Metodo para agregar roles de usuario a las categorias de los menus
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> AgregarRolCategoria(int idcategoria, string rol)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool existe =await
                        _context.MenuRolCategoria.AnyAsync(d => d.Id_Menu_Categoria_Vista == idcategoria && d.Rol == rol);
                    if (existe)
                    {
                        return BadRequest("Ya existe ese permiso");
                    }

                    Menu_Rol_Categoria menu_rol = new Menu_Rol_Categoria
                    {
                        Id_Menu_Categoria_Vista = idcategoria,
                        Rol = rol
                    };
                    _context.MenuRolCategoria.Add(menu_rol).State = EntityState.Added;
                    await _context.SaveChangesAsync();
                    return Ok("El Rol Fue agregado");
                }
                else
                {
                    return BadRequest(ModelState.ValidationState.ToString());
                }
            }
            catch (Exception e)
            {
                return await Task.Run(() => BadRequest());
            }
         
        }
        //metodo para ver los roles que existen
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> RolesporCategoria(int idcategoria)
        {
           List< Menu_Rol_Categoria> roles = await _context.MenuRolCategoria.Where
               (g => g.Id_Menu_Categoria_Vista == idcategoria).ToListAsync();
            return Ok(roles);
        }

        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpDelete]
        public async Task<IActionResult> BorrarRolCategoria(int idrol)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var rol = await _context.MenuRolCategoria.SingleOrDefaultAsync(m => m.Id_Menu_Rol_Categoria == idrol);
            if (rol == null)
            {
                return NotFound();
            }
            try
            {
                _context.MenuRolCategoria.Remove(rol);
                await _context.SaveChangesAsync();
                return Ok("Rol de Categoria Eliminado Exitosamente");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /*----------------------------------------------métodos para contenido------------------------------------------------------------------*/
        //Metodo para agregar roles de usuario a las categorias de los menus
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> AgregarRolContenido(int idcontenido, string rol)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool existe = await
                        _context.MenuRolContenido.AnyAsync(d => d.Id_Menu_Contenido_Vista == idcontenido && d.Rol == rol);
                    if (existe)
                    {
                        return BadRequest("Ya existe ese permiso");
                    }

                    Menu_Rol_Contenido menu_rol = new Menu_Rol_Contenido()
                    {
                        Id_Menu_Contenido_Vista = idcontenido,
                        Rol = rol
                    };
                    _context.MenuRolContenido.Add(menu_rol).State = EntityState.Added;
                    await _context.SaveChangesAsync();
                    return Ok("El Rol Fue agregado");
                }
                else
                {
                    return BadRequest(ModelState.ValidationState.ToString());
                }
            }
            catch (Exception e)
            {
                return await Task.Run(() => BadRequest());
            }

        }
        
        //metodo para ver los roles que existen
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> RolesporContenido(int idcontenido)
        {
            List<Menu_Rol_Contenido> roles = await _context.MenuRolContenido.Where
                (g => g.Id_Menu_Contenido_Vista == idcontenido).ToListAsync();
            return Ok(roles);
        }

        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpDelete]
        public async Task<IActionResult> BorrarRolContenido(int idrol)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var rol = await _context.MenuRolContenido.SingleOrDefaultAsync(m => m.Id_Menu_Rol_Contenido == idrol);
            if (rol == null)
            {
                return NotFound();
            }
            try
            {
                _context.MenuRolContenido.Remove(rol);
                await _context.SaveChangesAsync();
                return Ok("Rol del Contenido Eliminado Exitosamente");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}