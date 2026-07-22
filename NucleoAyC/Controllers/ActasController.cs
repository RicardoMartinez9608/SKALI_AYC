using System;
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
    /// Actas
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = "Actas")]
    public class ActasController : Controller
    {
        private readonly NegocioContexto _context;

        /// <summary>
        /// constructor de actas
        /// </summary>
        /// <param name="context"></param>
        public ActasController(NegocioContexto context)
        {
            _context = context;
        }

        /// <summary>
        /// Get Lista de actas con los créditos incluidos
        /// </summary>
        /// <returns>Lista de actas</returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Actas(DateTime inicio,DateTime final)
        {
            List<Acta> actas= await _context.Acta.AsNoTracking()
                .Where(d=>d.Fecha_Celebrado>=inicio && d.Fecha_Celebrado<=final)
                .ToListAsync();
            return Ok(actas);
        }

        /// <summary>
        /// Se obtiene el acta con sus créditos a partir de el id de detalle
        /// </summary>
        /// <param name="id_acta">int</param>
        /// <returns>Acta</returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Detalle_Acta(int id_acta)
        {
            Acta acta = await _context.Acta.AsNoTracking()
                .Include("Creditos")
                .Include("Creditos.Periodo")
                .Include("Creditos.Persona")
                .Include("Creditos.Linea_Credito")
                .SingleOrDefaultAsync(w=>w.Id_Acta==id_acta);
            return Ok(acta);
        }

        /// <summary>
        /// se obtiene el Acta con su créditos agregados
        /// </summary>
        /// <returns>Acta</returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Detalle_Acta_Actual()
        {
            DateTime fecha = DateTime.Now;
            Acta acta = await _context.Acta.AsNoTracking()
                .Include("Creditos")
                .Include("Creditos.Periodo")
                .Include("Creditos.Persona")
                .Include("Creditos.Linea_Credito")
                .SingleOrDefaultAsync(
                    w => w.Fecha_Celebrado.Day==fecha.Day && 
                    w.Fecha_Celebrado.Month==fecha.Month && 
                    w.Fecha_Celebrado.Year==fecha.Year);
            return Ok(acta);
        }

        /// <summary>
        /// Creación de Actas para varios créditos
        /// </summary>
        /// <param name="acta">Acta objeto json</param>
        /// <returns>id de acta guardada</returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> NuevaActa([FromBody] Acta acta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            DateTime fecha = DateTime.Now;
            Acta acta_existente = _context.Acta
                .SingleOrDefault(
                        d => d.Fecha_Celebrado.Day == fecha.Day &&
                        d.Fecha_Celebrado.Month == fecha.Month &&
                        d.Fecha_Celebrado.Year == fecha.Year);

            if (acta_existente != null)
            {
                return Ok(acta_existente.Id_Acta);
            }
            else {
                _context.Acta.Add(acta);
                await _context.SaveChangesAsync();
                return Ok(acta.Id_Acta);
            }
        }

        private bool ActaExists(int id)
        {
            return _context.Acta.Any(e => e.Id_Acta == id);
        }
    }
}