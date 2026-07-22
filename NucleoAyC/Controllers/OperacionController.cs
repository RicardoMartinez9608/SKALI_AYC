using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelNucleo.Model;
using NucleoAyC.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NucleoAyC.Controllers
{
    /// <summary>
    /// catálogo de operaciones
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class OperacionController : Controller
    {
        private NegocioContexto _context;

        /// <summary>
        /// constructor simplemente
        /// </summary>
        /// <param name="contexto"></param>
        public OperacionController(NegocioContexto contexto)
        {
            _context = contexto;
        }

        /// <summary>
        /// lista de operaciones
        /// </summary>
        /// <returns>List de Operacion</returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Operaciones()
        {
            List<Operacion> op = await _context.Operacion.ToListAsync();
            return Ok(op);
        }
    }
}
