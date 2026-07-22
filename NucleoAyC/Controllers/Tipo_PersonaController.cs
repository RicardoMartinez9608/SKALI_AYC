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
    /// para el manejo de personas
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = "Tipo_Persona")]
    public class Tipo_PersonaController : Controller
    {
        private readonly NegocioContexto _context;
        /// <summary>
        /// simple constructor
        /// </summary>
        /// <param name="context"></param>
        public Tipo_PersonaController(NegocioContexto context)
        {
            _context = context;
        }
        /// <summary>
        /// Get Obtiene la lista de tipo de personas
        /// </summary>
        /// <returns>Lista Tipo_Persona</returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public IEnumerable<Tipo_Persona> Tipos_Personas()
        {
            return _context.Tipo_Persona;
        }
    }
}