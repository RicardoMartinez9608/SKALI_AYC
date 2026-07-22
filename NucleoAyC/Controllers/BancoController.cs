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
    [ApiExplorerSettings(IgnoreApi = false, GroupName = "Banco")]
    public class BancoController : Controller
    {
        private readonly NegocioContexto _context;
        private readonly UserManager<IdentityUser> _userManager;

        /// <summary>
        /// Constructor de la clase de crud de banco
        /// no como una cuenta contable sino como una identificaci'on del destino
        /// de una remesa efectuada por una boveda
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userManager"></param>
        public BancoController(NegocioContexto context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        /// <summary>
        /// crear un banco
        /// </summary>
        /// <param name="banco">Banco</param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> Crear_Banco([FromBody]Banco banco)
        {
            if (!Existe(banco))
            {
                try
                {
                    _context.Banco.Add(banco).State = EntityState.Added;
                    await _context.SaveChangesAsync();
                    return Ok("El banco fue registrado correctamente");
                } catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return BadRequest("El banco que intenta registrar ya existe");
        }

        /// <summary>
        /// Edite un banco dando el objeto completo
        /// </summary>
        /// <param name="banco">Banco</param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPut]
        public async Task<IActionResult> Editar_Banco([FromBody] Banco banco)
        {
            if (Existe(banco))
            {
                try
                {
                    _context.Entry(banco).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return Ok("El registro fue editado correctamente");
                } catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return BadRequest("El banco que intenta editar no existe");
        }

        /// <summary>
        /// Muestra la lista de bancos disponible
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Bancos()
        {
            List<Banco> bancos= await _context.Banco.Where(b=>!b.Borrado_Logico).ToListAsync();
            return Ok(bancos);
        }

        /// <summary>
        /// muestra el registro de un banco específico
        /// </summary>
        /// <param name="id_banco">int</param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Detalle_Banco(int id_banco)
        {
            Banco banco =await _context.Banco.SingleOrDefaultAsync(b=>b.Id_Banco==id_banco);
            return Ok(banco);
        }

        /// <summary>
        /// borra un banco específico
        /// </summary>
        /// <param name="id_banco">int</param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpDelete]
        public async Task<IActionResult> Eliminar_Banco(int id_banco)
        {
            Banco banco =await _context.Banco.SingleOrDefaultAsync(b=>b.Id_Banco==id_banco);
            string Id_Usuario = _userManager.GetUserId(HttpContext.User);
            banco.Borrado_Logico = true;
            banco.Eliminado_En = DateTime.Now;
            banco.Eliminado_Por = Id_Usuario;

            _context.Entry(banco).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok();
        }

        /// <summary>
        /// verifica si un banco existe por su número de cuenta
        /// </summary>
        /// <param name="banco">Banco</param>
        /// <returns></returns>
        public bool Existe(Banco banco)
        {
            return _context.Banco.Any(b => b.No_Cuenta == banco.No_Cuenta);
        }
    }
}