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

namespace AyCWeb.Controllers
{
    /// <summary>
    /// Manejo de carteras de sus asesores
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = "Carteras")]
    public class CarterasController : Controller
    {
        private readonly NegocioContexto _context;
        private readonly UserManager<IdentityUser> _userManager;
        /// <summary>
        /// Constructor de manejo de Carteras
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userManager"></param>
        public CarterasController(NegocioContexto context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        /// <summary>
        /// agregar cartera a un nuevo usuario
        /// </summary>
        /// <param name="id_usuario"></param>
        /// <param name="nombre_cartera"></param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> Nueva_Cartera(string id_usuario, string nombre_cartera)
        {
            using (var dbContextTransaccion = _context.Database.BeginTransaction())
            {
                bool tiene_cartera=_context
                    .Cartera
                    .Any(a=>a.Id_Usuario==id_usuario);
                if (!tiene_cartera)
                {
                    try
                    {
                        int max = _context.Cartera.Count()>0?_context.Cartera.Max(m => m.Id_Cartera):1;
                        _context.Cartera.Add(new Cartera
                        {
                            Id_Usuario = id_usuario,
                            Nombre = nombre_cartera,
                            Colector = max.ToString()
                        }).State=EntityState.Added;
                        await _context.SaveChangesAsync();
                        dbContextTransaccion.Commit();
                        return Ok("El registro se guardó correctamente");
                    }
                    catch (DbUpdateException ex)
                    {
                        dbContextTransaccion.Rollback();
                        return BadRequest(ex.Message);
                    }
                }
                else
                    return BadRequest("Ya existe cartera para el usuario: " + id_usuario );
            }
        }

        /// <summary>
        /// Delete Borra una cartera vacía si tiene clientes no la borra
        /// </summary>
        /// <param name="id_cartera">int id de la cartera</param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpDelete]
        public async Task<IActionResult> BorrarCartera(int id_cartera)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var cartera = await _context.Cartera.SingleOrDefaultAsync(m => m.Id_Cartera == id_cartera);
            bool tieneClientes = await _context.Personas.AnyAsync(a=>a.Id_Cartera==id_cartera);
            if (cartera == null || tieneClientes)
            {
                return BadRequest("La cartera no se puede borrar porque posee clientes o no existe");
            }
            _context.Cartera.Remove(cartera);
            await _context.SaveChangesAsync();
            return Ok(cartera);
        }

        /// <summary>
        /// Get La lista de carteras por usuario dado
        /// </summary>
        /// <param name="id_usuario">string id de usuario</param>
        /// <returns>Lista de Cartera</returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> CarterasPorUsuario(string id_usuario)
        {
            List<Cartera> carteras= await _context.Cartera.Where(d=>d.Id_Usuario==id_usuario).ToListAsync();
            return Ok(carteras);
        }

        /// <summary>
        /// busca una cartera asignada
        /// al usuario logueado
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Cartera_Usuario_Autenticado()
        {
            string userId = _userManager.GetUserId(HttpContext.User);
            Cartera cartera =null;
            if(_context.Cartera.Any(d => d.Id_Usuario == userId))
                cartera = await _context.Cartera.SingleAsync(d => d.Id_Usuario == userId);
            if (cartera != null)
                return Ok(cartera);
            else
            {
                Usuario_Agencia agencia = await _context
                    .Usuario_Agencia
                    .SingleAsync(d=>d.Id_Usuario==userId);
                List<Usuario_Agencia> carteras = await _context
                    .Usuario_Agencia
                    .Where(d=>d.Id_Agencia==agencia.Id_Agencia)
                    .ToListAsync();
                foreach (Usuario_Agencia cartera_ag in carteras) {
                    Cartera cartera_con = null;
                    if(_context.Cartera.Any(s => s.Por_Defecto && s.Id_Usuario == cartera_ag.Id_Usuario))
                        cartera_con = _context.Cartera
                            .Single(s => s.Por_Defecto && s.Id_Usuario == cartera_ag.Id_Usuario);
                    if (cartera_con != null)
                    {
                        cartera = cartera_con;
                        break; 
                    }
                }
                if (cartera != null)
                    return Ok(cartera);
                else
                    return BadRequest("No tiene cartera asignada y la agencia a la que pertenece no tiene cartera por defecto");
            }
        }

        /// <summary>
        /// Get Carteras por agencia
        /// </summary>
        /// <param name="id_agencia">int id de la agencia</param>
        /// <returns>Lista de carteras</returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> CarterasPorAgencia(int id_agencia)
        {
            List<Cartera> carteras =await CarterasPorAgenciaComun(id_agencia);
            return Ok(carteras);
        }

        /// <summary>
        /// Se obtiene las carteras, de acuerdo con el usuario que se loguea
        /// el usuario debe estar enlazado a una agencia
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> CarterasPorAgenciaEnUsuario()
        {
            // Obtener ID de usuario
            string userId = _userManager.GetUserId(HttpContext.User);

            // Verificar si es Administrador
            bool esAdmin = HttpContext.User.IsInRole("Administrador");

            List<Cartera> carteras;

            if (esAdmin)
            {
                // ADMINISTRADOR → todas las carteras
                carteras = await _context.Cartera.AsNoTracking().ToListAsync();
            }
            else
            {
                // OTROS ROLES → obtener agencia del usuario
                int id_agencia = _context.Usuario_Agencia
                    .AsNoTracking()
                    .Where(u => u.Id_Usuario == userId)
                    .Select(u => u.Id_Agencia)
                    .SingleOrDefault();

                // Si el usuario no tiene agencia asignada, devuelve vacío
                if (id_agencia == 0)
                    return Ok(new List<Cartera>());

                carteras = await CarterasPorAgenciaComun(id_agencia);
            }

            return Ok(carteras);
        }

        /// <summary>
        /// Put Cambiar a una persona socia de cartera
        /// </summary>
        /// <param name="id_usuario">string id usuario como cartera destino</param>
        /// <param name="id_persona">int id persona como cliente que se moverá</param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPut]
        public async Task<IActionResult> CambiarCartera(string id_usuario,int id_persona)
        {
            try
            {
                bool tieneCartera = _context.Cartera.Any(q => q.Id_Usuario == id_usuario);
                if (!tieneCartera)
                {
                    _context.Add(new Cartera { Id_Usuario = id_usuario });
                    await _context.SaveChangesAsync();
                }
                Cartera cartera = await _context.Cartera.SingleOrDefaultAsync(d => d.Id_Usuario == id_usuario);
                Persona persona = await _context.Personas.SingleOrDefaultAsync(c => c.Id_Persona == id_persona);
                Credito credito = await _context.Credito.SingleOrDefaultAsync(c => c.Id_Persona == persona.Id_Persona);
                persona.Id_Cartera = cartera.Id_Cartera;
                credito.Id_Cartera = cartera.Id_Cartera;
                _context.Entry(credito).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                _context.Entry(persona).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok("El registro fue actualizado correctamente");
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Put Mueve la cartera de un usuario a otro
        /// </summary>
        /// <param name="id_cartera">int id de la cartera</param>
        /// <param name="id_usuario">string id usuario al que se le asigna la cartera</param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPut]
        public async Task<IActionResult> CambiarUsuario(int id_cartera, string id_usuario)
        {
            try
            {
                Cartera cartera = await _context.Cartera.SingleOrDefaultAsync(s => s.Id_Cartera == id_cartera);
                cartera.Id_Usuario = id_usuario;
                _context.Entry(cartera).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok("EL registro se actualizó correctamente");
            } catch (DbUpdateException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get Lista de usuarios filtrados por agencias
        /// </summary>
        /// <param name="id_agencia">int id de la agencia</param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> UsuariosPorAgencia(int id_agencia)
        {
            List<string> id_usuarios = await _context.Usuario_Agencia.
                Where(l=>l.Id_Agencia==id_agencia).
                Select(f=>f.Id_Usuario).ToListAsync();
            return Ok(id_usuarios);
        }

        private async Task<List<Cartera>> CarterasPorAgenciaComun(int id_agencia)
        {
            List<string> id_usuarios = await _context.Usuario_Agencia.
                                       Where(a => a.Id_Agencia == id_agencia).
                                       AsNoTracking().
                                       Select(s => s.Id_Usuario).ToListAsync();
            List<Cartera> carteras = new List<Cartera>();
            foreach (string id in id_usuarios)
            {
                List<Cartera> carterasporusuario = _context.Cartera.Where(f => f.Id_Usuario == id).ToList();
                carteras.AddRange(carterasporusuario);
            }
            return carteras;
        }

        /// <summary>
        /// colocar por defecto un
        /// </summary>
        /// <param name="id_cartera"></param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPut]
        public async Task<IActionResult> Poner_Cartera_Defecto(int id_cartera)
        {
            Cartera cartera = await _context
                .Cartera
                .SingleAsync(v=>v.Id_Cartera==id_cartera);

            Usuario_Agencia union = await _context
                .Usuario_Agencia
                .SingleAsync(d=>d.Id_Usuario==cartera.Id_Usuario);
            List<Usuario_Agencia> carteras = await _context
                .Usuario_Agencia
                .Where(l=>l.Id_Agencia==union.Id_Agencia).ToListAsync();
            if (cartera != null)
            {
                //poner todos los de la agencia en false
                foreach (Usuario_Agencia relacion in carteras)
                {
                    if (_context.Cartera.Any(c => c.Id_Usuario == relacion.Id_Usuario && c.Por_Defecto))
                    {
                        Cartera cartera_ag = await _context.Cartera.SingleAsync(c => c.Id_Usuario == relacion.Id_Usuario);
                        cartera_ag.Por_Defecto = false;
                    }
                }
                cartera.Por_Defecto = true;//actualizar ponerlo por defecto
                await _context.SaveChangesAsync();

                return Ok("La cartera está por defecto para la agencia detectada");
            }
            return BadRequest("No se encontró la cartera solicitada");
        }

        private bool CarteraExists(int id)
        {
            return _context.Cartera.Any(e => e.Id_Cartera == id);
        }
        //metodo para editar la cartera
        [HttpPut]
        public async Task<IActionResult> EditarCartera([FromBody] Cartera cartera_actualizar)
        {
            if (cartera_actualizar != null)
            {
                _context.Entry(cartera_actualizar).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok("Se ha actualizado el valor de la cartera");
            }
            return BadRequest("No se pudo configurar el valor de la cartera");
        }
        [HttpGet]
        public async Task<IActionResult> DetalleCartera(int idcartera)
        {
            Cartera cartera = await _context.Cartera.SingleAsync(g => g.Id_Cartera == idcartera);
            return Ok(cartera);
        }

    }
}