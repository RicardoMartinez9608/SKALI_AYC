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
    /// maneja las deducciones aplicados a los desembolso configurados en las líneas de crédito
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = "Deduccion_Desembolso")]
    public class Deduccion_DesembolsoController : Base_CreditoController
    {
        /// <summary>
        /// Constructor de la clase de Crédito
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userManager"></param>
        /// <param name="roleManager"></param>
        public Deduccion_DesembolsoController(NegocioContexto context,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
            : base(context, userManager, roleManager)
        {
        }

        /// <summary>
        /// las deducciones configuradas para una linea de crédito
        /// </summary>
        /// <param name="id_linea_credito"></param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Deducciones_Desembolso(int id_linea_credito)
        {
            List<Deduccion_Desembolso> deducciones =await _context
                .Deduccion_Desembolso
                .Where(e=>e.Id_Linea_Credito==id_linea_credito).ToListAsync();
            return Ok(deducciones);
        }

        /// <summary>
        /// detalle de una dedducción para una línea de crédito
        /// </summary>
        /// <param name="id_deduccion_desembolso"></param>
        /// <returns></returns>

        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Deduccion_Desembolso(int id_deduccion_desembolso)
        {
            Deduccion_Desembolso deduccion = await _context
                .Deduccion_Desembolso
                .SingleAsync(s=>s.Id_Deduccion_Desembolso==id_deduccion_desembolso);
            return Ok(deduccion);
        }

        /// <summary>
        /// nueva deduccion desembolso
        /// </summary>
        /// <returns></returns>

        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> Nueva_Deduccion_Desembolso([FromBody] Deduccion_Desembolso deduccion)
        {
            if (!ModelState.IsValid)
                return BadRequest("La información no es correcta");
            try {
                _context.Deduccion_Desembolso.Add(deduccion);
                await _context.SaveChangesAsync();
                return Ok("La deducción se ha guardado correctamente");
            }
            catch (Exception ex) {
                return BadRequest(ex.StackTrace);
            }
        }

        /// <summary>
        /// editar deduccion desembolso
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPut]
        public async Task<IActionResult> Editar_Deduccion_Desembolso([FromBody] Deduccion_Desembolso deduccion)
        {
            if (!ModelState.IsValid)
                return BadRequest("La información no es correcta");
            try
            {
                _context.Entry(deduccion).State=EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok("La deducción se ha modificado correctamente");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace);
            }
        }

        #region Deducciones Desembolso Historial
        /// <summary>
        /// historial de deducciones para línea credito historial
        /// </summary>
        /// <param name="id_credito"></param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Deducciones_Desembolso_Historial(int id_credito)
        {
            List<Deduccion_Historial> deducciones = await _context
                .Deduccion_Historial
                .Include(a=>a.Linea_Credito_Historial)
                .Where(a=>a.Linea_Credito_Historial.Id_Credito==id_credito)
                .ToListAsync();
            return Ok(deducciones);
        }

        /// <summary>
        /// detalle de una deduccion historial de un crédito
        /// </summary>
        /// <param name="id_deduccion_desembolso_historial"></param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Deduccion_Desembolso_Historial(int id_deduccion_desembolso_historial)
        {
            Deduccion_Historial deduccion = await _context
                .Deduccion_Historial
                .SingleAsync(a => a.Id_Deduccion_Historial == id_deduccion_desembolso_historial);
            return Ok(deduccion);
        }

        /// <summary>
        /// guardar una nueva deducción para un créditos
        /// </summary>
        /// <param name="deduccion"></param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> Nueva_Deduccion_Desembolso_Historial([FromBody] Deduccion_Historial deduccion)
        {
            if (!ModelState.IsValid)
                return BadRequest("La información no es correcta");
            try
            {
                Linea_Credito_Historial historial = await _context.Linea_Credito_Historial.SingleAsync(s => s.Id_Linea_Credito_Historial == deduccion.Id_Linea_Credito_Historial);

                if (_context.Credito.Single(d => d.Id_Credito == historial.Id_Credito).Aprobado)
                    return BadRequest("No se puede agregar más deducciones a un crédito aprobado");
                _context.Deduccion_Historial.Add(deduccion);
                await _context.SaveChangesAsync();
                return Ok("Se guardó correctamente la deducción");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace);
            }
        }

        /// <summary>
        /// guardar una deducción editado para un créditos
        /// </summary>
        /// <param name="deduccion"></param>
        /// <returns></returns>
        [Authorize(Roles = "Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPut]
        public async Task<IActionResult> Editar_Deduccion_Desembolso_Historial([FromBody] Deduccion_Historial deduccion)
        {
            if (!ModelState.IsValid)
                return BadRequest("La información no es correcta");
            try
            {
                Linea_Credito_Historial historial = await _context
                    .Linea_Credito_Historial
                    .SingleAsync(s => s.Id_Linea_Credito_Historial == deduccion.Id_Linea_Credito_Historial);

                if (_context
                    .Deduccion_Historial
                    .Any(
                        d => d.Id_Deduccion_Historial == deduccion.Id_Deduccion_Historial &&
                        d.Id_Credito_Financiado > 0))
                    return BadRequest("No se puede editar un refinanciamiento");

                if (_context
                    .Credito
                    .Single(d => d.Id_Credito == historial.Id_Credito)
                    .Aprobado)
                    return BadRequest("No se puede editar deducciones a un crédito aprobado");

                _context.Deduccion_Historial.Update(deduccion).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok("Se guardó correctamente la deducción modificada");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace);
            }
        }

        /// <summary>
        /// Deducciones historiales, para cobrar
        /// </summary>
        /// <param name="id_persona"></param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Deducciones_Historiales(int id_persona)
        {
            List<Deduccion_Historial> deducciones = await _context
                .Deduccion_Historial
                .Include("Linea_Credito_Historial.Credito")
                .Where(y=>y.Linea_Credito_Historial.Credito.Id_Persona==id_persona && !y.Cancelado).ToListAsync();
            
            return Ok(deducciones);
        }

        #endregion
    }
}