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
    [ApiExplorerSettings(IgnoreApi = false, GroupName = "Documentos_Legales")]
    public class Documentos_LegalesController : Controller
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
        public Documentos_LegalesController(
            NegocioContexto context,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        //Obtener documentos legales
        [Authorize(Roles = "Asistente,Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> GetDocumentosLegales()
        {
            try
            {
                List<Documentos_Legales> documentos = await _context.DocumentosLegales.Where(g => g.Tipo_Documento == Tipo_Documento.Credito).ToListAsync();
                return Ok(documentos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace);
            }
        }

        [Authorize(Roles = "Asistente,Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> GetDocumentosLegalesAhorro()
        {
            try
            {
                List<Documentos_Legales> documentos = await _context.DocumentosLegales.Where(g => g.Tipo_Documento == Tipo_Documento.Ahorro).ToListAsync();
                return Ok(documentos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace);
            }
        }

        /*manejo de los mutuos*/
        /// <summary>
        /// la lista de mutuos disponibles por una
        /// linea crédito
        /// </summary>
        /// <param name="id_linea_credito"></param>
        /// <returns></returns>
        public async Task<IActionResult> Mutuos(int id_linea_credito)
        {
            Linea_Credito linea = await _context
                .Linea_Credito
                .AsNoTracking()
                .Include(c => c.Tipo_Mutuos)
                .SingleAsync(c => c.Id_Linea == id_linea_credito);
            return Ok(linea);
        }

    }
}
