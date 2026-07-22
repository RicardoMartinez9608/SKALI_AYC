using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelNucleo.Model;
using ModelNucleo.Model.ModelView;
using NucleoAyC.Entities;

namespace NucleoAyC.Controllers
{
    /// <summary>
    /// para manejar las referencias de la persona
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = "Referencias")]
    public class ReferenciasController : Controller
    {
        private readonly NegocioContexto _context;

        public ReferenciasController(NegocioContexto context)
        {
            _context = context;
        }

        /// <summary>
        /// Get Obtiene todas las referencias
        /// por ahora no se utiliza
        /// </summary>
        /// <returns>Lista de Referencia</returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public IEnumerable<Referencia> Referencias()
        {
            return _context.Referencia;
        }
        /// <summary>
        /// Get Se obtiene la referencia especifica de una persona para ver
        /// o editar
        /// </summary>
        /// <param name="id_referencia">int id de referencia</param>
        /// <param name="id_persona">int id de persona</param>
        /// <returns>Obtiene la referencia</returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Referencia(int id_referencia,int id_persona)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var referencia = await _context.Persona_Referencia.Include("Referencia").SingleOrDefaultAsync(m => m.Id_Referencia == id_referencia && m.Id_Persona==id_persona);
            ReferenciaVista rv = new ReferenciaVista();
            rv.Id_Referencia = referencia.Id_Referencia;
            rv.FechaNacimiento = referencia.Referencia.FechaNacimiento;
            rv.Nombre = referencia.Referencia.Nombre;
            rv.Parentesco = referencia.Parentesco;
            rv.Residencia = referencia.Referencia.Residencia;
            rv.Telefono = referencia.Referencia.Telefono;
            rv.Tipo_Referencia = referencia.Tipo_Referencia;
            rv.Beneficiario = referencia.Referencia.Beneficiario;
            rv.Porcentaje = referencia.Referencia.Porcentaje;

            /*rv.Ponderacion = referencia.Referencia.Ponderacion;
            rv.Esta_Verificado = referencia.Referencia.Esta_Verificado;
            rv.No_Contesta = referencia.Referencia.No_Contesta;
            rv.Telefono_Equivocado = referencia.Referencia.Telefono_Equivocado;*/

            rv.Edad = referencia.Referencia.Edad;
            rv.Esta_Estudiando = referencia.Referencia.Esta_Estudiando;
            rv.Esta_Trabajando = referencia.Referencia.Esta_Trabajando;
            rv.Direccion_Estudio = referencia.Referencia.Direccion_Estudio;
            rv.Direccion_Empleo = referencia.Referencia.Direccion_Empleo;
            rv.Cargo_Empleo = referencia.Referencia.Cargo_Empleo;
            rv.Salario_Empleo = referencia.Referencia.Salario_Empleo;
            rv.Nombre_Jefe_Inmediato = referencia.Referencia.Nombre_Jefe_Inmediato;
            rv.Telefono_Empleo = referencia.Referencia.Telefono_Empleo;

            if (referencia == null)
            {
                return NotFound();
            }

            return Ok(rv);
        }

        /// <summary>
        /// Post Edita una referencia dada de una persona
        /// </summary>
        /// <param name="referencia">ReferenciaVista objeto en json</param>
        /// <param name="id_persona">int id de persona</param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> EditarReferencia([FromBody]ReferenciaVista referencia, int id_persona)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (referencia.Porcentaje > 100)
            {
                return BadRequest("el porcentaje no puede ser mayor al 100%");
            }
            else {
                Referencia refi = new Referencia
                {
                    Id_Referencia = referencia.Id_Referencia,
                    FechaNacimiento = referencia.FechaNacimiento,
                    Nombre = referencia.Nombre,
                    Telefono = referencia.Telefono,
                    Residencia = referencia.Residencia,
                    Porcentaje = referencia.Porcentaje,
                    Beneficiario = referencia.Beneficiario,

                    /*Ponderacion = referencia.Ponderacion,
                    Esta_Verificado = referencia.Esta_Verificado,
                    No_Contesta = referencia.No_Contesta,
                    Telefono_Equivocado = referencia.Telefono_Equivocado,*/


                    Edad = referencia.Edad,
                    Esta_Estudiando = referencia.Esta_Estudiando,
                    Esta_Trabajando = referencia.Esta_Trabajando,
                    Direccion_Estudio = referencia.Direccion_Estudio,
                    Direccion_Empleo = referencia.Direccion_Empleo,
                    Cargo_Empleo = referencia.Cargo_Empleo,
                    Salario_Empleo = referencia.Salario_Empleo,
                    Nombre_Jefe_Inmediato = referencia.Nombre_Jefe_Inmediato,
                    Telefono_Empleo = referencia.Telefono_Empleo
                };

                _context.Entry<Referencia>(refi).State = EntityState.Modified;

                Persona_Referencia pr = _context.Persona_Referencia.Single(d => d.Id_Referencia == referencia.Id_Referencia && d.Id_Persona == id_persona);
                pr.Parentesco = referencia.Parentesco;
                pr.Tipo_Referencia = referencia.Tipo_Referencia;
                _context.Entry(pr).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                //try
                //{
                    
                //}
                //catch (DbUpdateConcurrencyException)
                //{
                //    if (!ReferenciaExists(referencia.Id_Referencia))
                //    {
                //        return NotFound();
                //    }
                //    else
                //    {
                //        throw;
                //    }
                //}

                return Ok("Referencia Editada Correctamente");
            }
           
        }

        /// <summary>
        /// Post Para crear una referencia a una persona
        /// </summary>
        /// <param name="referencia">ReferenciaVista objeto json</param>
        /// <param name="id_persona">int id de persona</param>
        /// <returns></returns>
        // POST: api/Referencias
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> CrearReferencia([FromBody] ReferenciaVista referencia,int id_persona)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (referencia.Porcentaje>100) {
                return BadRequest("el porcentaje no puede ser mayor al 100%");
            }
            else
            {
                Referencia refi = new Referencia
                {
                    FechaNacimiento = referencia.FechaNacimiento,
                    Nombre = referencia.Nombre,
                    Telefono = referencia.Telefono,
                    Residencia = referencia.Residencia,
                    Porcentaje = referencia.Porcentaje,
                    Beneficiario = referencia.Beneficiario,

                    /*Ponderacion = referencia.Ponderacion,
                    Esta_Verificado = referencia.Esta_Verificado,
                    No_Contesta = referencia.No_Contesta,
                    Telefono_Equivocado = referencia.Telefono_Equivocado,*/


                    Edad = referencia.Edad,
                    Esta_Estudiando = referencia.Esta_Estudiando,
                    Esta_Trabajando = referencia.Esta_Trabajando,
                    Direccion_Estudio = referencia.Direccion_Estudio,
                    Direccion_Empleo = referencia.Direccion_Empleo,
                    Cargo_Empleo = referencia.Cargo_Empleo,
                    Salario_Empleo = referencia.Salario_Empleo,
                    Nombre_Jefe_Inmediato = referencia.Nombre_Jefe_Inmediato,
                    Telefono_Empleo = referencia.Telefono_Empleo
                };
                _context.Referencia.Add(refi);

                await _context.SaveChangesAsync();
                /*me arriesgo a que venga el id_referencia ya lleno como identity*/
                Persona_Referencia pr = new Persona_Referencia
                {
                    Id_Persona = id_persona,
                    Id_Referencia = refi.Id_Referencia,
                    Parentesco = referencia.Parentesco,
                    Tipo_Referencia = referencia.Tipo_Referencia
                };
                _context.Persona_Referencia.Add(pr);
                await _context.SaveChangesAsync();
                return Ok("La referencia fue registrada correctamente");
            } 
               
        }

        /*manejo de referencias bancarias e instituciones de credito y ahorro*/
        /// <summary>
        /// Lista de las referencias para saber si tienes otras cuentas en otras instituciones bancarias
        /// </summary>
        /// <param name="id_persona">int</param>
        /// <returns>200 y Lista Referencia_Bancaria</returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Referencias_Bancarias(int id_persona)
        {
            List<Referencia_Bancaria> referencia= await _context
                .Referencia_Bancaria
                .AsNoTracking()
                .Where(l=>l.Id_Persona==id_persona).ToListAsync();
            return Ok(referencia);
        }

        /// <summary>
        /// muestra el detalle de la referencia bancaria
        /// </summary>
        /// <param name="id_referencia_bancaria">int</param>
        /// <returns>200</returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Detalle_Referencia_Bancaria(int id_referencia_bancaria)
        {
            Referencia_Bancaria referencia =await _context
                .Referencia_Bancaria.
                FirstOrDefaultAsync(f=>f.Id_Referencia_Bancaria== id_referencia_bancaria);
            return Ok(referencia);
        }

        /// <summary>
        /// Guardar una refernecia bancaria
        /// </summary>
        /// <param name="referencia">Referencia_Bancaria</param>
        /// <returns>200</returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> Nueva_Referencia_Bancaria([FromBody] Referencia_Bancaria referencia)
        {
            if (ModelState.IsValid)
            {
                _context.Referencia_Bancaria.Add(referencia).State = EntityState.Added;
                await _context.SaveChangesAsync();
                return Ok("La referencia ha sido guardada");
            }  
            return BadRequest("guardar la referencia ha fallado");
        }


        /// <summary>
        /// Editar una refernecia bancaria
        /// </summary>
        /// <param name="referencia">Referencia_Bancaria</param>
        /// <returns>200</returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPut]
        public async Task<IActionResult> Editar_Referencia_Bancaria([FromBody] Referencia_Bancaria referencia)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("editar la referencia ha fallado");
            }
            _context.Entry(referencia).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok("La referencia ha sido editada");
          
        }


        /*referencia adicionales agregada a las referencias*/
        /// <summary>
        /// obtener la lista de referencias de una referencia
        /// </summary>
        /// <param name="id_referencia"></param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Referencias_Adicionales(int id_referencia)
        {
            List<Referencia_Adicional> referencias_conyuge =await _context
                .Referencia_Adicional
                .AsNoTracking()
                .Where(l=>l.Id_Referencia==id_referencia).ToListAsync();
            return Ok(referencias_conyuge);
        }

        /// <summary>
        /// detalle de una referencia
        /// </summary>
        /// <param name="id_referencia_adicional"></param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Referencia_Adicional_Detalle(int id_referencia_adicional)
        {
            Referencia_Adicional referencias_conyuge = await _context
                .Referencia_Adicional
                .AsNoTracking()
                .SingleOrDefaultAsync(l => l.Id_Referencia_Adicional == id_referencia_adicional);

            ReferenciaVista rv = new ReferenciaVista();
            rv.Id_Referencia_Adicional = referencias_conyuge.Id_Referencia_Adicional;
            rv.Id_Referencia = referencias_conyuge.Id_Referencia;
            rv.Parentesco = referencias_conyuge.Parentesco;
            rv.Tipo_Referencia = referencias_conyuge.Tipo_Referencia;
            rv.Nombre = referencias_conyuge.Nombre;
            rv.Telefono = referencias_conyuge.Telefono;
            rv.Residencia = referencias_conyuge.Residencia;

            return Ok(rv);
        }
        /// <summary>
        /// nueva referencia adicional
        /// </summary>
        /// <param name="referencia_accion"></param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> Crear_Referencia_Adicional([FromBody] ReferenciaVista referencia_accion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Referencia_Adicional refi = new Referencia_Adicional
            {
                Id_Referencia=referencia_accion.Id_Referencia,
                Parentesco= referencia_accion.Parentesco,
                Telefono = referencia_accion.Telefono,
                Residencia = referencia_accion.Residencia,
                Tipo_Referencia=referencia_accion.Tipo_Referencia,
                Nombre=referencia_accion.Nombre
            };
            _context.Referencia_Adicional.Add(refi);

            await _context.SaveChangesAsync();
            return Ok("La referencia adicional fue registrada correctamente");
        }

        /// <summary>
        /// Editar la referencia adicional mostrado
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPut]
        public async Task<IActionResult> Editar_Referencia_Adicional([FromBody] ReferenciaVista referencia_accion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Referencia_Adicional ra =await _context
                .Referencia_Adicional
                .SingleOrDefaultAsync(d=>d.Id_Referencia_Adicional==referencia_accion.Id_Referencia_Adicional);
            
            ra.Id_Referencia = referencia_accion.Id_Referencia;
            ra.Parentesco = referencia_accion.Parentesco;
            ra.Telefono = referencia_accion.Telefono;
            ra.Residencia = referencia_accion.Residencia;
            ra.Tipo_Referencia = referencia_accion.Tipo_Referencia;
            ra.Nombre = referencia_accion.Nombre;

            _context.Entry(ra).State = EntityState.Modified;

            await _context.SaveChangesAsync();
            return Ok("La referencia adicional fue registrada correctamente");
        }
        
        private bool ReferenciaExists(int id)
        {
            return _context.Referencia.Any(e => e.Id_Referencia == id);
        }

        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpDelete]
        public async Task<IActionResult> BorrarReferencia(int idreferencia)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var referencia = await _context.Referencia.SingleOrDefaultAsync(m => m.Id_Referencia == idreferencia);
            if (referencia == null)
            {
                return NotFound();
            }
            try
            {
                _context.Referencia.Remove(referencia);
                await _context.SaveChangesAsync();
                return Ok("Referencia Eliminada Exitosamente");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}