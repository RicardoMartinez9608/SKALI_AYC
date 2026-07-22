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

namespace NucleoAyC.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = "InteresLineaCreditoPeriodo")]
    public class InteresLineaCreditoPeriodoController : Controller
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
        public InteresLineaCreditoPeriodoController(
            NegocioContexto context,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        //[Authorize(Roles = "Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Intereses_Periodos(int id_linea_credito)
        {
            try
            {
                List<Interes_Periodo_Linea_Credito> lista = await _context
                    .InteresPeriodoLineaCreditos.Where
                        (g => g.Id_Linea_Credito == id_linea_credito)
                    .Include(b => b.Periodo).ToListAsync();
                return Ok(lista);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }
        [Authorize(Roles = "Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPut]
        public async Task<IActionResult> Editar_Interes_Periodos([FromBody] Interes_Periodo_Linea_Credito interesPeriodo)
        {
            if (_context.InteresPeriodoLineaCreditos.Any(y => y.Id_Interes_Periodo_Linea_Credito == interesPeriodo.Id_Interes_Periodo_Linea_Credito))
            //if (interesPeriodo != null)
            {
                
                _context.Entry(interesPeriodo).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok("El registro se actualizo correctamente");
            }
            return BadRequest("No existe el registro que intenta actualizar");
        }

        [Authorize(Roles = "Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> Crear_Editar_Interes([FromBody] Interes_Periodo_Linea_Credito interesPeriodo)
        {
            bool yaexiste = _context
                .InteresPeriodoLineaCreditos
                .Any(s => s.Id_Periodo == interesPeriodo.Id_Periodo && s.Id_Linea_Credito == interesPeriodo.Id_Linea_Credito);
            if (ModelState.IsValid && !yaexiste)
            {
                _context.Add(interesPeriodo);
                await _context.SaveChangesAsync();
                return Ok("El registro se creó correctamente");
            }
            return BadRequest("Ya existe un Periodo de Pago");
        }

        [Authorize(Roles = "Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> DetalleInteres(int idinteres)
        {
            Interes_Periodo_Linea_Credito interes = await _context.InteresPeriodoLineaCreditos.SingleAsync(g => g.Id_Interes_Periodo_Linea_Credito == idinteres);
            return Ok(interes);
        }

        [Authorize(Roles = "Administrador", AuthenticationSchemes = "Bearer")]
        [HttpDelete]
        public async Task<IActionResult> Borrar_Editar_Interes(int id_interes)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var interes = await _context.InteresPeriodoLineaCreditos.SingleOrDefaultAsync(m => m.Id_Interes_Periodo_Linea_Credito == id_interes);
            if (interes == null)
            {
                return NotFound();
            }
            try
            {
                _context.InteresPeriodoLineaCreditos.Remove(interes);
                await _context.SaveChangesAsync();
                return Ok("Interes Eliminado Exitosamente");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}