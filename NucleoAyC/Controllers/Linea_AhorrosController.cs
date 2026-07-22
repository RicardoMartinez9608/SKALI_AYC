using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelNucleo.Model;
using NucleoAyC.Entities;

namespace NucleoAyC.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = "Linea_Ahorros")]
    public class Linea_AhorrosController : Controller
    {
        private readonly NegocioContexto _context;

        public Linea_AhorrosController(NegocioContexto context)
        {
            _context = context;
        }

        // GET: api/Linea_Ahorro
        /// <summary>
        /// Get Lista de las lineas de ahorro
        /// </summary>
        /// <returns>Lista de Linea_Ahorro</returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public IEnumerable<Linea_Ahorro> LineasAhorro()
        {
            var lineas= _context.Linea_Ahorro.ToList();
            return lineas;
        }
        /// <summary>
        /// Get Lista de los tipos de ahorro
        /// </summary>
        /// <returns>Lista de Tipo_Linea_Ahorro</returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public IEnumerable<Tipo_Linea_Ahorro> TipoLineaAhorro()
        {
            return _context.Tipo_Linea_Ahorro;
        }

        // GET: api/Linea_Ahorro/5
        /// <summary>
        /// Get Detalle de una línea de ahorro dado su id
        /// </summary>
        /// <param name="id_linea_ahorro">int id de la línea de ahorro</param>
        /// <returns>Linea_Ahorro objeto json</returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> LineaAhorroDetalle(int id_linea_ahorro)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var linea_Ahorro = await _context.Linea_Ahorro.SingleOrDefaultAsync(m => m.Id_Linea_Ahorro == id_linea_ahorro);

            if (linea_Ahorro == null)
            {
                return NotFound();
            }

            return Ok(linea_Ahorro);
        }

        // PUT: api/Linea_Ahorro/5
        /// <summary>
        /// Put Modifica una linea de ahorro dada
        /// </summary>
        /// <param name="id_linea_ahorro">int id línea de ahorro</param>
        /// <param name="linea_Ahorro">Linea_Ahorro objeto json</param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPut]
        public async Task<IActionResult> Modificar(int id_linea_ahorro, [FromBody] Linea_Ahorro linea_Ahorro)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id_linea_ahorro != linea_Ahorro.Id_Linea_Ahorro)
            {
                return BadRequest();
            }

            _context.Entry(linea_Ahorro).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Linea_AhorroExists(id_linea_ahorro))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        // POST: api/Linea_Ahorro
        /// <summary>
        /// Post Para crear nueva línea de ahorro
        /// </summary>
        /// <param name="linea_Ahorro">Linea_Ahorro objeto de json</param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> NuevaLineaAhorro([FromBody] Linea_Ahorro linea_Ahorro)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Linea_Ahorro.Add(linea_Ahorro);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLinea_Ahorro", new { id = linea_Ahorro.Id_Linea_Ahorro }, linea_Ahorro);
        }

        // DELETE: api/Linea_Ahorro/5
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLinea_Ahorro([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var linea_Ahorro = await _context.Linea_Ahorro.SingleOrDefaultAsync(m => m.Id_Linea_Ahorro == id);
            if (linea_Ahorro == null)
            {
                return NotFound();
            }

            _context.Linea_Ahorro.Remove(linea_Ahorro);
            await _context.SaveChangesAsync();

            return Ok(linea_Ahorro);
        }

        private bool Linea_AhorroExists(int id)
        {
            return _context.Linea_Ahorro.Any(e => e.Id_Linea_Ahorro == id);
        }
    }
}