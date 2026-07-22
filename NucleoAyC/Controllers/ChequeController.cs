using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelNucleo.Model;
using NucleoAyC.Entities;

namespace NucleoAyC.Controllers
{
    /// <summary>
    /// Manejo de cheques
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = "Cheque")]
    public class ChequeController : Controller
    {
        private readonly NegocioContexto _context;
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="context"></param>
        public ChequeController(NegocioContexto context)
        {
            _context = context;
        }

        /// <summary>
        /// la lista de cheques por fecha
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Cheques(DateTime fecha)
        {
            List<Ingreso_Cheque> cheques =await _context
                .Ingreso_Cheque
                .AsNoTracking()
                .Where(v=>v.Fecha.Date==fecha.Date).ToListAsync();
            return Ok(cheques);
        }

        /// <summary>
        /// nuevo cheque
        /// </summary>
        /// <param name="cheque"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> NuevoCheque([FromBody] Ingreso_Cheque cheque)
        {
            if (!Existe(cheque))
            {
                _context.Ingreso_Cheque.Add(cheque).State = EntityState.Added;
                await _context.SaveChangesAsync();
                return Ok("Se ha guardado el cheque");
            }
            return BadRequest("ya existe un cheque con el mismo número");
        }

        /// <summary>
        /// verificar si el número que cheque
        /// existe
        /// </summary>
        /// <param name="cheque"></param>
        /// <returns></returns>
        private bool Existe(Ingreso_Cheque cheque)
        {
            return _context.Ingreso_Cheque.Any(v=>v.No_Cheque==cheque.No_Cheque);
        }
    }
}
