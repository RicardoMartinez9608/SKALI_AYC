using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelNucleo.Model;
using NucleoAyC.Entities;

namespace NucleoAyC.Controllers
{
    /// <summary>
    /// obtiene la realacion de una persona en cuantos tipos es conocido
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = "Persona_Tipo_Persona")]
    public class Persona_Tipo_PersonaController : Controller
    {
        private readonly NegocioContexto _context;
        /// <summary>
        /// es solo el constructor
        /// </summary>
        /// <param name="context"></param>
        public Persona_Tipo_PersonaController(NegocioContexto context)
        {
            _context = context;
        }

        /// <summary>
        /// Get Es el que se utiliza para obtener los tipos de persona asignada a una personsa
        /// </summary>
        /// <param name="id_persona">int id de la persona</param>
        /// <returns>Lista Tipo_Persona</returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public IEnumerable<Tipo_Persona> Personas_Tipo_Persona(int id_persona)
        {
            IEnumerable<Tipo_Persona> tipo= _context.Persona_Tipo_Persona.Where(a=>a.Id_Persona==id_persona).Select(d=>d.Tipo_Persona);
            return tipo;
        }

        /// <summary>
        /// Post Crear una relación de tipo de persona con persona
        /// </summary>
        /// <param name="persona_Tipo_Persona">Persona_Tipo_Persona</param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> CrearPersona_Tipo_Persona([FromBody] Persona_Tipo_Persona persona_Tipo_Persona)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!_context.Persona_Tipo_Persona.
                Any(b=>b.Id_Persona==persona_Tipo_Persona.Id_Persona && 
                b.Id_Tipo_Persona==persona_Tipo_Persona.Id_Tipo_Persona))
            {
                _context.Persona_Tipo_Persona.Add(persona_Tipo_Persona);
                await _context.SaveChangesAsync();
            }
            return Ok();
        }
        
        private bool Persona_Tipo_PersonaExists(int id)
        {
            return _context.Persona_Tipo_Persona.Any(e => e.Id_Persona_Tipo_Persona == id);
        }
    }
}