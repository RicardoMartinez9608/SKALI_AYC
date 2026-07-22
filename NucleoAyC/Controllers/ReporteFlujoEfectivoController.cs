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
using ModelNucleo.Model.ModelView;
using NucleoAyC.Entities;

namespace NucleoAyC.Controllers
{
    /// <summary>
    /// Reportes spara el flujo de efectivo
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = "ReporteFlujoEfectivo")]
    [ApiController]
    public class ReporteFlujoEfectivoController : Base_TransaccionController
    {

        /// <summary>
        /// constructor para Identity y connexión a base de datos
        /// </summary>
        /// <param name="context">contexto de la base de datos</param>
        /// <param name="userManager">manejo de usuarios</param>
        /// <param name="roleManager">manejo de roles</param>
        public ReporteFlujoEfectivoController(NegocioContexto context,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
            : base(context, userManager, roleManager)
        {
        }

        /// <summary>
        /// genera el objeto para reporte de generales
        /// </summary>
        /// <param name="id_agencia">identificador de la agencia</param>
        /// <param name="fecha_inicio">fecha inicia rango</param>
        /// <param name="fecha_final">fecha termina rango</param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Reporte_Flujo_Efectivo()
        {
            try
            {
                Reporte_Flujo_Efectivo_Vista reporteFlujoEfectivoVista = new Reporte_Flujo_Efectivo_Vista();
                var reporte = new List<Detalle_Efectivo_Vista>();
                var denominacion = await _context.Denominacion_Efectivo.ToListAsync();
                foreach (var tipo in denominacion)
                {
                    reporte.Add(new Detalle_Efectivo_Vista()
                    {
                        Denominacion = tipo
                    });
                }

                reporteFlujoEfectivoVista.Efectivo_Detallado = reporte;
                return Ok(reporteFlujoEfectivoVista);
            }
            catch (Exception e)
            {
                return NoContent();
            }
        }
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Vista_Flujo_Efectivo()
        {

            try
            {
                Reporte_Flujo_Efectivo_Vista reporteFlujoEfectivoVista = new Reporte_Flujo_Efectivo_Vista();
                var reporte = new List<Detalle_Efectivo_Vista>();
                var denominacion = await _context.Denominacion_Efectivo.ToListAsync();
                foreach (var tipo in denominacion)
                {
                    reporte.Add(new Detalle_Efectivo_Vista()
                    {
                        Denominacion = tipo
                    });
                }

                return Ok(reporte);
            }
            catch (Exception e)
            {
                return NoContent();
            }
        }

        /// <summary>
        /// Para mostrar las denominaciones de los billetes
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Reporte_Denominacion()
        {
            try
            {
                Reporte_Denominacion reporteFlujoEfectivoVista = new Reporte_Denominacion();
                var reporte = new List<Denominacion_Vista>();
                var denominacion = await _context.Denominacion_Efectivo.ToListAsync();
                foreach (var tipo in denominacion)
                {
                    reporte.Add(new Denominacion_Vista()
                    {
                        Denominacion_dinero = tipo
                    });
                }

                reporteFlujoEfectivoVista.detalle = reporte;
                return Ok(reporteFlujoEfectivoVista);
            }
            catch (Exception e)
            {
                return NoContent();
            }
        }
    }
}