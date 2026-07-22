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
    /// <summary>
    /// maneja los cobros periodicos configurados en las líneas de crédito
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = "Cobro_Periodico")]
    public class Cobro_PeriodicoController : Base_CreditoController
    {
        /// <summary>
        /// Constructor de la clase de Crédito
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userManager"></param>
        /// <param name="roleManager"></param>
        public Cobro_PeriodicoController(NegocioContexto context,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
            : base(context, userManager, roleManager)
        {
        }

        /// <summary>
        /// guardar un nuevo cobro periodico
        /// </summary>
        /// <param name="cobro">Cobro_Periodico</param>
        /// <returns>200</returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> Nuevo_Cobro_Periodico([FromBody] Cobro_Periodico cobro)
        {
            if (!_context.Cobro_Periodico.Any(c => c.Nombre == cobro.Nombre && c.Id_Linea_Credito == cobro.Id_Linea_Credito))
            {
                _context.Add(cobro);
                await _context.SaveChangesAsync();
                return Ok("El registro se agregó correctamente");
            }
            return BadRequest("Ya existe un registro igual");
        }

        /// <summary>
        /// Lista de Cobro_Periodico
        /// </summary>
        /// <param name="id_linea_credito">int</param>
        /// <returns>Lista de Cobro_Periodico</returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Cobros_Periodicos(int id_linea_credito)
        {
            List<Cobro_Periodico> cobros= await _context.Cobro_Periodico.Where(u=>u.Id_Linea_Credito==id_linea_credito).ToListAsync();
            return Ok(cobros);
        }

        /// <summary>
        /// Detalle de un cobro periódico
        /// </summary>
        /// <param name="id_cobro_periodico">int</param>
        /// <returns>Cobro_Periodico</returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Cobro_Periodico(int id_cobro_periodico)
        {
            if (_context.Cobro_Periodico.Any(f => f.Id_Cobro_Periodico == id_cobro_periodico))
            {
                Cobro_Periodico cobro = await _context.Cobro_Periodico.FirstOrDefaultAsync(t => t.Id_Cobro_Periodico == id_cobro_periodico);
                return Ok(cobro);
            }
            return BadRequest("No se encontró el registro específico");
        }

        /// <summary>
        /// Editar un cobre periodico determinado
        /// </summary>
        /// <param name="cobro">Cobro_Periodico</param>
        /// <returns>200</returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPut]
        public async Task<IActionResult> Editar_Cobro_Periodico([FromBody] Cobro_Periodico cobro)
        {
            if (_context.Cobro_Periodico.Any(v => v.Id_Cobro_Periodico == cobro.Id_Cobro_Periodico && v.Id_Linea_Credito == cobro.Id_Linea_Credito))
            {
                _context.Entry(cobro).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok("El regostro fue edita correctamente");
            }
            return BadRequest("El registro no fue encontrado o no se pudo editar");
        }

        /// <summary>
        /// Elimina un Cobro Periodico
        /// </summary>
        /// <param name="id_cobro_periodico">int</param>
        /// <returns>200</returns>
        public async Task<IActionResult> Eliminar_Cobro_Periodico(int id_cobro_periodico)
        {
            if (_context.Cobro_Periodico.Any(q => q.Id_Cobro_Periodico == id_cobro_periodico))
            {
                Cobro_Periodico cobro =await _context.Cobro_Periodico.FirstOrDefaultAsync(a=>a.Id_Cobro_Periodico==id_cobro_periodico);
                _context.Cobro_Periodico.Remove(cobro);
                await _context.SaveChangesAsync();
                return Ok("El registro se eliminó correctamente");
            }
            return BadRequest("El registro no se encontró o no se pudo eliminar");
        }

        #region Cobro Periodico Historial
        //manejo de cobros periodicos historial
        /// <summary>
        /// se obtienen todos los cobros de un 
        /// </summary>
        /// <param name="id_credito"></param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Cobros_Peridodicos_Credito(int id_credito)
        {
            List<Cobro_Periodico_Historial> cobro_credito =await _context
                .Cobro_Periodico_Historial
                .Include(p=>p.Linea_Credito_Historial)
                .Where(d=>d.Linea_Credito_Historial.Id_Credito== id_credito)
                .ToListAsync();

            return Ok(cobro_credito);
        }

        /// <summary>
        /// detalle de un cobro periodico
        /// </summary>
        /// <param name="id_cobro_periodico_historial"></param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Cobro_Periodico_Historial(int id_cobro_periodico_historial)
        {
            Cobro_Periodico_Historial cobro_credito = await _context
                .Cobro_Periodico_Historial
                .SingleAsync(d => d.Id_Cobro_Periodico_Historial == id_cobro_periodico_historial);
            return Ok(cobro_credito);
        }

        /// <summary>
        /// guarda un nuevo cobro periódico
        /// </summary>
        /// <param name="cobro"></param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> Nuevo_Cobro_Periodico_Historial([FromBody] Cobro_Periodico_Historial cobro)
        {
            if (!ModelState.IsValid)
                return BadRequest("Los datos no están correctos");
            try
            {
                if (await Credito_Aprobado(id_linea_credito_historial: cobro.Id_Linea_Credito_Historial))
                    return BadRequest("No se puede agregar más cobros a un crédito aprobado");
                _context.Cobro_Periodico_Historial.Add(cobro);
                await _context.SaveChangesAsync();
                return Ok("El cobro ha sido registrado");
            }
            catch (Exception ex) {
                return BadRequest(ex.StackTrace);
            }
        }
        /// <summary>
        /// para editar el cobro periódico historico de un crédito termenido
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPut]
        public async Task<IActionResult> Editar_Cobro_Periodico_Historial([FromBody] Cobro_Periodico_Historial cobro)
        {
            if (!ModelState.IsValid)
                return BadRequest("Los datos no están correctos");
            try
            {
                if (await Credito_Aprobado(id_linea_credito_historial: cobro.Id_Linea_Credito_Historial))
                    return BadRequest("No se puede editar cobros a un crédito aprobado");
                _context.Cobro_Periodico_Historial.Update(cobro).State=EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok("El cobro ha sido registrado");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace);
            }
        }
        #endregion
    }
}