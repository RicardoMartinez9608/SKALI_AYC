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
    /// para manejar el Tipo de relacion
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = "Tipo_Relacion")]
    public class Tipo_RelacionController : Controller
    {
        private readonly NegocioContexto _context;
        /// <summary>
        /// Manejo de Tipo Relación
        /// </summary>
        /// <param name="context"></param>
        public Tipo_RelacionController(NegocioContexto context)
        {
            _context = context;
        }
        /// <summary>
        /// Get Obtiene la lista de tipo de relaciones
        /// </summary>
        /// <returns>Lista de Tipo_Relacion</returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public IEnumerable<Tipo_Relacion> Tipos_Relaciones()
        {
            return _context.Tipo_Relacion;
        }
    }
}