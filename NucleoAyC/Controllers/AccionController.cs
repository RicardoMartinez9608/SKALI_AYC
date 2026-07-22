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
using ModelNucleo.Model.ModelView;
using NucleoAyC.Entities;

namespace NucleoAyC.Controllers
{
    /// <summary>
    /// Manejo de acciones
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = "Acción")]
    public class AccionController : Base_TransaccionController
    {
        /// <summary>
        /// constructor e injección de base de datos
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userManager"></param>
        /// <param name="roleManager"></param>
        public AccionController(NegocioContexto context,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
            : base(context, userManager, roleManager)
        {
        }

        /// <summary>
        /// se guarda una acción
        /// </summary>
        /// <param name="accion"></param>
        /// <returns></returns>
        [Authorize(Roles = "Asistente,Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> Nuevo([FromBody] Accion accion)
        {
            bool creado= await Nueva_Accion(accion);
            if (!creado)
                return BadRequest("La acción nueva no pudo crearse");

            return Ok("La acción ha sido creada"); 
        }

        /// <summary>
        /// se modifica una accion dada
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Asistente,Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPut]
        public async Task<IActionResult> Modificar([FromBody] Accion accion)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try {
                _context.Entry(accion).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok("la acción ha sido modificado");
            }
            catch (Exception ex) {
                return BadRequest(ex.StackTrace);
            }

        }
        /// <summary>
        /// registrar el numero de certificado
        /// en el que se va a imprimir la accion
        /// </summary>
        /// <param name="id_accion"></param>
        /// <param name="no_certificado"></param>
        /// <returns></returns>
        [Authorize(Roles = "Asistente,Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPut]
        public async Task<IActionResult> Registrar_Certificado(int id_accion =0, string no_certificado ="")
        {
            Imprimir_Accion imprimirAccion = new Imprimir_Accion();
            if (id_accion != 0)
            {
                Accion accion = await _context.Accion.SingleAsync(s => s.Id_Accion == id_accion);
                accion.No_Certificado = no_certificado;
                accion.Esta_Impreso = true;
                _context.SaveChanges();
                imprimirAccion = await Imprimir_Accion_Socio(id_accion, no_certificado);
            }
            return Ok(imprimirAccion);
        }

        [Authorize(Roles = "Asistente,Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Consultar_Accion(int id_persona = 0)
        {
            Imprimir_Accion imprimirAccion = new Imprimir_Accion();
            Func<Accion, bool> tiene_accion = (t => t.Id_Persona == id_persona);
            if (_context.Accion.Any(tiene_accion))
            {
                if (id_persona != 0)
                {
                    Accion accion_persona = await _context.Accion.AsNoTracking().SingleAsync(a => a.Id_Persona == id_persona);
                    imprimirAccion = await Imprimir_Accion_Socio(accion_persona.Id_Accion, accion_persona.No_Certificado);
                }
                return Ok(imprimirAccion);
            }
            return BadRequest("El cliente no tiene comprada una acción");
        }
        /// <summary>
        /// se muestra las acciones que posee una persona
        /// </summary>
        /// <param name="id_persona"></param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Acciones_Persona(int id_persona)
        {
            List<Accion> acciones =await _context.Accion.Where(t=>t.Id_Persona==id_persona).ToListAsync();
            return Ok(acciones);
        }

        /// <summary>
        /// lista de acciones pendientes pasar por caja
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Acciones_Pendientes()
        {
            List<Accion> acciones = await _context.Accion
                .AsNoTracking()
                .Include(d=>d.Persona)
                .Where(a=>!a.Esta_Impreso)
                .ToListAsync();

            return Ok(acciones);
        }

        /// <summary>
        /// acciones pendientes buscado por persona
        /// </summary>
        /// <param name="id_persona"></param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Acciones_Pendiente_Por_Persona(int id_persona)
        {
            List<Accion> acciones_pendientes = await _context
                .Accion
                .AsNoTracking()
                .Where(w=>w.Id_Persona==id_persona && !w.Cancelado).ToListAsync();
            return Ok(acciones_pendientes);
        }

        /// <summary>
        /// detalle de una acción
        /// </summary>
        /// <param name="id_accion"></param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Detalle(int id_accion)
        {
            Accion accion =await _context.Accion.SingleAsync(s=>s.Id_Accion==id_accion);
            return Ok(accion);
        }

        private async Task<Imprimir_Accion> Imprimir_Accion_Socio(int id_accion, string numero_accion)
        {
            try
            {
                Accion accion = _context.Accion.AsNoTracking()
                    .Include(b => b.Persona)
                    .Single(a => a.Id_Accion == id_accion);
                DateTime fecha = DateTime.Now;
                Imprimir_Accion imprimirAccion = new Imprimir_Accion
                {
                    Nombre_cliente = accion.Persona.NombreCompleto,
                    Numero_Accion = numero_accion,
                    Fecha = fecha
                };
                return imprimirAccion;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        
        }

        /// <summary>
        /// entrega acta con la lista de creditos aprobados
        /// </summary>
        /// <param name="id_acta">int id de acta</param>
        /// <returns>Lista de créditos</returns>
        [Authorize(Roles = "Jefe,Administrador,Asistente", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Acciones_Por_Agencia(DateTime fecha, int id_agencia = 0)
        {
            string userId = _userManager.GetUserId(HttpContext.User);
            List<Accion> accion = new List<Accion>();
            NegocioContexto db = new NegocioContexto();
            Usuario_Agencia agencia_usuario;
            var acciones = new List<Accion>();
            if (User.IsInRole("Jefe") || User.IsInRole("Asistente"))
            {
                agencia_usuario = await _context.Usuario_Agencia.AsNoTracking()
                    .SingleAsync(a => a.Id_Usuario == userId);
                agencia_usuario = await _context.Usuario_Agencia.AsNoTracking()
                 .SingleAsync(a => a.Id_Usuario == userId);

                acciones = (from c in _context.Accion
                            join p in _context.Personas on c.Id_Persona equals p.Id_Persona
                            join ca in _context.Cartera on p.Id_Cartera equals ca.Id_Cartera
                            join ua in _context.Usuario_Agencia on ca.Id_Usuario equals ua.Id_Usuario
                            where ua.Id_Agencia == agencia_usuario.Id_Agencia
                            select c).ToList();

            }
            else if (User.IsInRole("Administrador") && id_agencia > 0)
            {
                acciones = (from c in _context.Accion
                            join p in _context.Personas on c.Id_Persona equals p.Id_Persona
                            join ca in _context.Cartera on p.Id_Cartera equals ca.Id_Cartera
                            join ua in _context.Usuario_Agencia on ca.Id_Usuario equals ua.Id_Usuario
                            where ua.Id_Agencia == id_agencia
                            select c).ToList();
            }
            foreach (var ac in acciones)
            {
                bool tiene_accion = ac.Creado_En.Date == fecha.Date;
                if (tiene_accion)
                {
                    accion.AddRange(_context.Accion.AsNoTracking()
                        .Include(a=>a.Persona)
                        .Where(a=>a.Id_Accion== ac.Id_Accion).ToList());
                }

            }
            return Ok(accion);
        }
    }
}