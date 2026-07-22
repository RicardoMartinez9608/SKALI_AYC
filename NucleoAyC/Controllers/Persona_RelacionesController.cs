using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelNucleo.Model;
using ModelNucleo.Model.ModelView;
using NucleoAyC.Entities;

namespace NucleoAyC.Controllers
{
    /// <summary>
    /// Persona relaciones
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = "Persona_Relaciones")]
    public class Persona_RelacionesController : Controller
    {
        private readonly NegocioContexto _context;

        /// <summary>
        /// Constructor con injección
        /// </summary>
        /// <param name="context"></param>
        public Persona_RelacionesController(NegocioContexto context)
        {
            _context = context;
        }

        /// <summary>
        /// devuelve todas las relaciones creadas a la persona
        /// </summary>
        /// <param name="id_persona">int</param>
        /// <param name="id_credito"></param>
        /// <returns>List Persona_Relaciones</returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Relaciones(int id_persona,int id_credito=0)
        {
            Func<Persona_Relaciones, bool> buscado_relacion = new Func<Persona_Relaciones, bool>(t => t.Id_Sujeto == id_persona);
            if (id_credito > 0)
                buscado_relacion = new Func<Persona_Relaciones, bool>(t=>t.Id_Sujeto==id_persona && t.Id_Credito==id_credito);

            var relaciones = _context.Persona_Relaciones
                .Include(t => t.Sujeto)
                .Include(e => e.Predicado)
                .Include(c => c.Tipo_Relacion)
                .AsNoTracking()
                .Where(buscado_relacion);

            return Ok(await Task.Run(()=> relaciones.ToList()));
        }

        /// <summary>
        /// se crea una relación
        /// </summary>
        /// <param name="relaciones">Persona_Relaciones</param>
        /// <returns>200</returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> Crear_Relacion([FromBody] Persona_Relaciones relaciones)
        {
            if (!Persona_Relacion_Existe(relaciones))
            {
                _context.Add(relaciones);
                await _context.SaveChangesAsync();
                //await Activar_Relacion(relaciones.Id_Sujeto,relaciones.Id_Predicado,relaciones.Id_Tipo_Relacion);
                return Ok("Se guardó correctamente la relación");
            }
            return BadRequest("Ya existe una configuración igual");
        }

        /// <summary>
        /// para editar la realación
        /// </summary>
        /// <param name="relaciones">Persona_Relaciones</param>
        /// <returns>200</returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPut]
        public async Task<IActionResult> Editar_Relacion([FromBody] Persona_Relaciones relaciones)
        {
            if (Persona_Relacion_Existe(relaciones))
            {
                _context.Entry(relaciones).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest();
        }

        /// <summary>
        /// se desactivan todos y se activa el que se solicita
        /// </summary>
        /// <param name="id_relacion"></param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPut]
        public async Task<IActionResult> Activar_Relacion(int id_relacion)
        {
            using (var dbContextTransaccion = _context.Database.BeginTransaction())
            {
                try
                {
                    Persona_Relaciones relacion = await _context.Persona_Relaciones.SingleAsync(d=>d.Id_Persona_Relaciones==id_relacion);
                    if (relacion != null)
                    {
                        List<Persona_Relaciones> relaciones = _context
                        .Persona_Relaciones
                        .Where(f => f.Id_Sujeto == relacion.Id_Sujeto && f.Id_Tipo_Relacion == relacion.Id_Tipo_Relacion)
                        .ToList();
                        foreach (Persona_Relaciones pr in relaciones)
                        {
                            pr.Activo = false;
                            _context.Entry(pr).State = EntityState.Modified;
                            await _context.SaveChangesAsync();
                        }
                        relacion.Activo = true;
                        _context.Entry(relacion).State = EntityState.Modified;
                        await _context.SaveChangesAsync();

                        dbContextTransaccion.Commit();
                        return Ok("Se ha activado correctamente");
                    }
                    else
                        return BadRequest("No se ha encontrado la relación que busca activar");
                }
                catch (Exception ex) {
                    dbContextTransaccion.Rollback();
                    return BadRequest("Algo ocurrió y no se pudo actualizar la activación");
                }
            }  
        }

        private bool Persona_Relacion_Existe(Persona_Relaciones relaciones)
        {
            bool encontrado= _context.Persona_Relaciones.
                Any(t => t.Id_Sujeto == relaciones.Id_Sujeto && 
                    t.Id_Predicado == relaciones.Id_Predicado &&
                    t.Id_Tipo_Relacion == relaciones.Id_Tipo_Relacion);
            if(relaciones.Id_Tipo_Relacion==5)
                encontrado= _context.Persona_Relaciones.
                Any(t => t.Id_Sujeto == relaciones.Id_Sujeto &&
                    t.Id_Predicado == relaciones.Id_Predicado &&
                    t.Id_Credito==relaciones.Id_Credito &&
                    t.Id_Tipo_Relacion == relaciones.Id_Tipo_Relacion);

            return encontrado;
        }
    }
}