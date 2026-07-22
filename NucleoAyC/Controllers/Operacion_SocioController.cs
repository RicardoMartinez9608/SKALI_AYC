using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelNucleo.CalculosYProceos;
using ModelNucleo.Model;
using ModelNucleo.Model.Contabilidad;
using ModelNucleo.Model.ModelView;
using ModelNucleo.Util;
using NucleoAyC.Entities;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace NucleoAyC.Controllers
{
    /// <summary>
    /// Métodos de transacciones de credito
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = "Operacion_Socio")]
    [Authorize(Roles = "Socio", AuthenticationSchemes = "Bearer")]
    public class Operacion_SocioController : Base_TransaccionController
    {

        /// <summary>
        /// constructor e injección de las operaciones del socio
        /// </summary>
        /// <param name="context"></param>
        /// /// <param name="userManager"></param>
        /// <param name="roleManager"></param>
        public Operacion_SocioController(NegocioContexto context,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
            : base(context, userManager, roleManager)
        {
            
        }

        /// <summary>
        /// Estado de las cuenta de usuario socio
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Datos_Personales()
        {
            try
            {
                string Id_Usuario = _userManager.GetUserId(HttpContext.User);
                Persona persona = await _context
                    .Personas.SingleAsync(w=>w.Usuario==Id_Usuario);
                return Ok(persona);
            }
            catch (Exception ex) {
                return BadRequest("No se pudo obtener el socio, o el usuario no es el propietario");
            }
        }

        /// <summary>
        /// las cuentas de ahorro propietarias
        /// </summary>
        /// <param name="id_persona"></param>
        /// <returns></returns>
        public async Task<IActionResult> Cuentas_Ahorro(int id_persona)
        {
            bool escorrecto =await EsPropietario(id_persona);
            if (escorrecto)
            {
                var cuentas = _context.Ahorro.AsNoTracking().Where(d => d.Id_Persona == id_persona && d.Saldo_Actual>0);
                return Ok(cuentas);
            }
            return BadRequest("No es el usuario propietario");
        }

        /// <summary>
        /// las cuentas de credito del acreedor
        /// </summary>
        /// <param name="id_persona"></param>
        /// <returns></returns>
        public async Task<IActionResult> Cuentas_Credito(int id_persona)
        {
            bool escorrecto = await EsPropietario(id_persona);
            if (escorrecto)
            {
                var cuentas = _context.Credito.AsNoTracking().Where(d => d.Id_Persona == id_persona && !d.Cancelado);
                return Ok(cuentas);
            }
            return BadRequest("No es el usuario propietario");
        }

        /// <summary>
        /// transacciones de una cuenta de ahorro
        /// solo para el usuario identificado
        /// </summary>
        /// <param name="id_ahorro"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Transacciones_Ahorro(int id_ahorro)
        {
            DateTime desde =await LapsoDias(false, id_ahorro, 30);
            var transacciones = _context
                .Transaccion_Ahorro.AsNoTracking()
                .Where(d=>d.Id_Ahorro==id_ahorro && !d.Borrado_Logico && d.Fecha.Date>=desde.Date).ToList();

            return Ok(transacciones);
        }

        /// <summary>
        /// transacciones de una cuenta de ahorro
        /// solo para el usuario identificado
        /// </summary>
        /// <param name="id_credito"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Transacciones_Credito(int id_credito)
        {
            DateTime desde = await LapsoDias(true, id_credito, 30);
            var transacciones = _context
                .Transaccion_Credito.AsNoTracking()
                .Where(d => d.Id_Credito == id_credito && !d.Borrado_Logico && d.FechaPago.Date >= desde.Date).ToList();

            return Ok(transacciones);
        }

        /// <summary>
        /// verificar si es propetario
        /// </summary>
        /// <param name="id_persona"></param>
        /// <returns></returns>
        private async Task<bool> EsPropietario(int id_persona)
        {
            string Id_Usuario = _userManager.GetUserId(HttpContext.User);
            return await _context.Personas.AnyAsync(c => c.Usuario == Id_Usuario && c.Id_Persona == id_persona);
        }

        /// <summary>
        /// obtener el cliente a apartir del propietario
        /// </summary>
        /// <returns></returns>
        private async Task<Persona> Propietario()
        {
            string Id_Usuario = _userManager.GetUserId(HttpContext.User);
            return await _context.Personas.AsNoTracking().SingleAsync(s => s.Usuario == Id_Usuario);
        }
    }
}
