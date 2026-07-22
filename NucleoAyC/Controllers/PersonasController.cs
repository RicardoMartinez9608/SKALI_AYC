using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ModelNucleo.Model;
using ModelNucleo.Model.ModelView;
using NucleoAyC.Entities;

namespace NucleoAyC.Controllers
{
    /// <summary>
    /// Operaciones para personas
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = "Personas")]
    public class PersonasController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly NegocioContexto _context;
        /// <summary>
        /// injección de roles y usuarios
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userManager"></param>
        /// <param name="roleManager"></param>
        public PersonasController(
            NegocioContexto context,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }


        /// <summary>
        /// Post Para crear una persona
        /// </summary>
        /// <param name="persona">Persona como json para crear una persona</param>
        /// <param name="tipo_persona">int tipo de persona</param>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> CrearPersona([FromBody] Persona persona,int tipo_persona /*,bool esReferencia= false, int id_sujeto = 0,int tipo_relacion=0*/)
        {
            using (var dbContextTransaccion = _context.Database.BeginTransaction())
            {
                try
                {
                    if (!_context.Personas.Any(c => c.Dui == persona.Dui))
                    {
                        /*asignar esta persona al usuario si es un Asesor*/
                        if (User.IsInRole("Asesor"))
                        {
                            /*agregar la cartera*/
                            var userId = _userManager.GetUserId(HttpContext.User);
                            if (!_context.Cartera.Any(s => s.Id_Usuario == userId))
                            {
                                Cartera cartera = new Cartera
                                {
                                    Id_Usuario = userId,
                                };
                                _context.Cartera.Add(cartera);
                                await _context.SaveChangesAsync();
                            }
                            persona.Id_Cartera = _context.Cartera.Single(d => d.Id_Usuario == userId).Id_Cartera;
                        }

                        //manejar el estado por defecto del socio
                        persona.Estado_Socio = Estado_Socio.En_Suscripcion;

                        _context.Personas.Add(persona);
                        await _context.SaveChangesAsync();

                        if (_context.Personas.Any(c => c.Dui == persona.Dui))
                        {
                            int id_persona = _context.Personas.SingleOrDefault(c => c.Dui == persona.Dui).Id_Persona;
                            /*guardar el tipo inicial*/
                            _context.Persona_Tipo_Persona.Add(new Persona_Tipo_Persona { Id_Tipo_Persona = tipo_persona, Id_Persona = id_persona });
                            //if (esReferencia && id_sujeto > 0 && tipo_relacion > 0)
                            //    _context.Persona_Relaciones.Add(
                            //        new Persona_Relaciones
                            //        {
                            //            Id_Sujeto = id_sujeto,
                            //            Id_Tipo_Relacion = tipo_relacion,
                            //            Id_Predicado = id_persona
                            //        });
                            await _context.SaveChangesAsync();
                        }
                        dbContextTransaccion.Commit();
                        return Ok(persona.Id_Persona);
                    }
                    else
                        return BadRequest("El registro con el documento dado ya existe");
                } catch (Exception ex)
                {
                    dbContextTransaccion.Rollback();
                    return BadRequest(ex.StackTrace);
                }
            }
        }

        /// <summary>
        /// Post Para editar la persona
        /// </summary>
        /// <param name="persona">Persona objeto que se editará</param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPut]
        public async Task<IActionResult> EditarPersona([FromBody] Persona persona)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Traer persona anterior
            Persona persona_anterior = await _context.Personas
                                                     .AsNoTracking()
                                                     .SingleAsync(p => p.Id_Persona == persona.Id_Persona);

            if (persona_anterior == null)
                return NotFound();

            persona.Estado_Socio = persona_anterior.Estado_Socio;

            // Verificar si cambió el Id_Cartera
            bool cambioCartera = persona.Id_Cartera != persona_anterior.Id_Cartera;

            // Marcar persona como modificada
            _context.Entry(persona).State = EntityState.Modified;

            try
            {
                // Si cambió la cartera, actualizar los créditos asociados
                if (cambioCartera)
                {
                    var creditos = await _context.Credito
                                                 .Where(c => c.Id_Persona == persona.Id_Persona)
                                                 .ToListAsync();

                    foreach (var credito in creditos)
                    {
                        credito.Id_Cartera = persona.Id_Cartera;
                        _context.Entry(credito).State = EntityState.Modified;
                    }
                }

                await _context.SaveChangesAsync();
                return Ok("El registro se editó correctamente");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonaExists(persona.Id_Persona))
                {
                    return NotFound();
                }
                else
                {
                    return BadRequest("El registro no se pudo editar");
                }
            }
        }

        /// <summary>
        /// Get Para hacer busqueda de personas
        /// </summary>
        /// <param name="busqueda">string texto para buscar</param>
        /// <param name="esReferencia">bool es referencia</param>
        /// <param name="id_sujeto">int id de usuario como sujeto</param>
        /// <param name="tipo_relacion">int tipo de relación</param>
        /// <param name="id_predicado">int id usuario como predicado</param>
        /// <returns>Lista de Persona</returns>
        [Authorize(Roles = "Administrador,Asesor,Gerente,Jefe,Asistente", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> BuscarPersonas(string busqueda,bool esReferencia=false,int id_sujeto=0,int tipo_relacion=0,int id_predicado=0)
        {
            try
             {
                /*ver si se busca por referencia*/
                if (!esReferencia)
                {
                    var userId = _userManager.GetUserId(HttpContext.User);
                    
                    if (User.IsInRole("Gerente"))
                        return Ok(await PersonasPorAgencia(userId, busqueda));
                    if (User.IsInRole("Asesor"))
                        return Ok(await FiltrarPersonasPorCartera(userId, busqueda));

                    List<Persona> personas = _context.Personas.
                        AsNoTracking()
                        .Where(
                                d=>d.Dui.Contains(busqueda) || 
                                d.Codigo.Contains(busqueda) || 
                                d.Nombres.ToUpper().Contains(busqueda.ToUpper()) ||
                                d.Apellidos.ToUpper().Contains(busqueda.ToUpper())
                                ).Take(25).ToList();

                    return Ok(personas);/*si es eadministrador sin restricciones*/
                }
                var personas_relaciones= _context.Persona_Relaciones.AsNoTracking().Where(d => d.Id_Sujeto == id_sujeto && d.Id_Tipo_Relacion == tipo_relacion).Select(s => s.Predicado);
                return Ok(personas_relaciones);
            }
            catch (Exception ex) {
                return BadRequest(ex.InnerException.StackTrace);
            }
        }

        private async Task<List<Persona>> PersonasPorAgencia(string userId,string busqueda)
        {
            if (_context.Usuario_Agencia.Any(a => a.Id_Usuario == userId))
            {
                var id_agencia = _context.Usuario_Agencia.AsNoTracking().SingleOrDefault(a => a.Id_Usuario == userId).Id_Agencia;
                var id_usuarios = _context.Usuario_Agencia.AsNoTracking().Where(c => c.Id_Agencia == id_agencia)
                    .Select(q => q.Id_Usuario).ToList();
                var personasAgencia = new List<Persona>();
                foreach (var id_usuario in id_usuarios)
                {
                    /*se verifica si es asesor para obtener su cartera*/
                    var identidad = await _userManager.FindByNameAsync(id_usuario);
                    if (identidad != null && await _userManager.IsInRoleAsync(identidad, "Asesor"))
                    {
                        var personas_cartera = await FiltrarPersonasPorCartera(id_usuario,busqueda);
                        if(personas_cartera!=null)
                            personasAgencia.AddRange(personas_cartera);
                    }
                }
                return personasAgencia;
            }
            return null;
        }

        private async Task<List<Persona>> FiltrarPersonasPorCartera(string id_usuario,string busqueda)
        {
            if (!_context.Cartera.Any(a => a.Id_Usuario == id_usuario)) return null;
            Cartera cartera = await _context.Cartera.AsNoTracking().SingleAsync(d => d.Id_Usuario == id_usuario);
            return _context.
                Personas.
                AsNoTracking().
                Where(d=>d.Id_Cartera==cartera.Id_Cartera)
                .Where(d => d.Dui.Contains(busqueda) ||
                                d.Codigo.Contains(busqueda) ||
                                d.Nombres.ToUpper().Contains(busqueda.ToUpper()) ||
                                d.Apellidos.ToUpper().Contains(busqueda.ToUpper())).ToList();
        }

        /// <summary>
        /// Post Para buscar una persona espefifica por su DUI por ahora
        /// el flitro solo aceptara por el momento el DUI
        /// </summary>
        /// <param name="filtro">string filtro por ahora dui</param>
        /// <returns>Persona</returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> PersonaEspecifica(string filtro)
        {
            /*por ahora solo se buscara por dui*/
            Persona persona=await _context.Personas.SingleOrDefaultAsync(d=>d.Dui==filtro || d.Codigo==filtro);
            if (persona != null)
                return Ok(persona);
            return BadRequest("No se encontró al cliente solicitado");
        }

        /// <summary>
        /// Get Obtener persona a partir de el id
        /// </summary>
        /// <param name="id_persona">int id de la persona</param>
        /// <returns>Persona</returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public Persona PersonaDetalle(int id_persona)
        {
            return _context.Personas.AsNoTracking().Include(d=>d.Cartera).SingleOrDefault(a=>a.Id_Persona==id_persona);
        }

        /// <summary>
        /// Get Listado de personas por cartera o asesor
        /// </summary>
        /// <param name="id_usuario">identificador del usuario</param>
        /// <returns>Lista de personas</returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> PersonasPorUsuario(string id_usuario)
        {
            int id_cartera = _context.Cartera.SingleOrDefaultAsync(f=>f.Id_Usuario== id_usuario).Result.Id_Cartera;
            List<Credito> creditos = await _context.Credito.AsNoTracking()
                .Include(a=>a.Persona)
                .Where(s => s.Id_Cartera == id_cartera).ToListAsync();
            //List<Persona> personas=await _context.Personas
            //    .AsNoTracking()
            //    .Where(p=>p.Id_Cartera==id_cartera).Take(400).ToListAsync();/*este se debe modificar o paginarlo*/
            if (creditos != null)
                return Ok(creditos);
            else
            return BadRequest("no posee clientes asignados");
        }

        /// <summary>
        /// Lista de departamentos de El Salvador
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Departamentos()
        {
            List<Departamento_Pais> departamentos =await _context.Departamento_Pais.AsNoTracking().ToListAsync();
            departamentos.Add(new Departamento_Pais { Id_Departamento_Pais=0,Nombre_Departamento="Seleccione un departamento"});
            return Ok(departamentos.OrderBy(a=>a.Id_Departamento_Pais));
        }
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> DepartamentosPorID(int id_departamento)
        {
            Departamento_Pais depto = new Departamento_Pais();
            depto =  _context
                .Departamento_Pais
                .AsNoTracking()
                .Single(t => t.Id_Departamento_Pais == id_departamento);
            return Ok(depto); ;
        }
        /// <summary>
        /// lista de municipios por su departamento
        /// </summary>
        /// <param name="id_departamento"></param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Municipios(int id_departamento)
        {
            List<Municipio_Departamento> municipios=new List<Municipio_Departamento>();
            if (id_departamento == 0)
            {
                municipios.Add(new Municipio_Departamento { Id_Municipio_Departamento = 0, Nombre_Municipio = "Seleccione un municipio" });
                return Ok(municipios);
            }
            municipios =await _context
                .Municipio_Departamento
                .AsNoTracking()
                .Where(t => t.Id_Departamento_Pais == id_departamento).ToListAsync();
            return Ok(municipios);
        }

        /// <summary>
        /// se verifica si ya existe un cliente con ese dui
        /// </summary>
        /// <param name="dui"></param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Persona_Existe(string dui)
        {
            bool existe=await _context.Personas.AsNoTracking().AnyAsync(d=>d.Dui==dui);
            return Ok(existe);
        }


        /// <summary>
        /// Consulta los datos de la persona para generar extenso documentos sobre un socio
        /// </summary>
        /// <param name="id_persona">int</param>
        /// <returns>200</returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Persona_Raiz(int id_persona)
        {
            //Tipo_Relacion codeudor = _context.Tipo_Relacion.SingleOrDefault(k=>k.Tipo_Relacion_Definicion=="Codeudora");
            Persona persona = await _context.Personas.AsNoTracking()
                .Include(d => d.Creditos)
                .Include("Creditos.Periodo")
                .Include("Creditos.Linea_Credito")
                .Include(d => d.Ahorros)
                .Include(d => d.Persona_Referencias)
                .Include(d => d.Referencia_Bancaria)
                .Include("Persona_Referencias.Referencia")
                .Include("Persona_Referencias.Referencia.Referencias_Adicionales")
                .SingleOrDefaultAsync(w => w.Id_Persona == id_persona);
            return Ok(persona);
        }

        /// <summary>
        /// cambiar el estado de una persona
        /// </summary>
        /// <param name="id_persona"></param>
        /// <param name="estado"></param>
        /// <returns></returns>
        [Authorize(Roles = "Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPut]
        public async Task<IActionResult> Cambiar_Estado_Persona(int id_persona, Estado_Socio estado)// solo un administrador puede cambiar el estado
        {
            if (!User.IsInRole("Administrador"))
                return BadRequest("Solo un administrador puede cambiar el estado de un perfil");
            
            Persona persona =await _context.Personas.SingleAsync(c=>c.Id_Persona==id_persona);
            if (persona != null)
            {
                persona.Estado_Socio = estado;
                _context.Entry(persona).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok("Se ha cambiado el estado correctamente para el perfil dado");
            }else
                return BadRequest("No se encontró el perfil para actualizar el estado");
        }

        /// <summary>
        /// actualizar las ubicaciones de las personas
        /// </summary>
        /// <param name="ubicacion"></param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPut]
        public async Task<IActionResult> Actualizar_Ubicacion_Persona([FromBody] Ubicacion_Persona ubicacion)
        {
            Persona persona =await _context.Personas.SingleAsync(d=>d.Id_Persona==ubicacion.Id_Persona);
            if (ubicacion.Tipo_Ubicacion == "Domicilio")
            {
                persona.Latitud_Domicilio = ubicacion.Lat;
                persona.Longitud_Domicilio = ubicacion.Longi;
            }
            if (ubicacion.Tipo_Ubicacion == "Negocio")
            {
                persona.Latitud_Direccion_Negocio = ubicacion.Lat;
                persona.Longitud_Direccion_Negocio = ubicacion.Longi;
            }
            if (ubicacion.Tipo_Ubicacion == "Trabajo")
            {
                persona.Latitud_Direccion_Trabajo = ubicacion.Lat;
                persona.Longitud_Direccion_Trabajo = ubicacion.Longi;
            }
            _context.Entry(persona).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok("Ha sido modificado uno de las ubicaciones del cliente");
        }

        private bool PersonaExists(int id)
        {
            return _context.Tipo_Relacion.Any(e => e.Id_Tipo_Relacion == id);
        }
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Detalle_Codeudor(int id_persona)
        {
           
            Persona_Relaciones relaciones =  _context.Persona_Relaciones.AsNoTracking().FirstOrDefault(a => a.Id_Sujeto == id_persona && a.Id_Tipo_Relacion ==5);
            if (relaciones != null)
            {
                Persona persona = _context.Personas.AsNoTracking()
                    .SingleOrDefault(b => b.Id_Persona == relaciones.Id_Predicado);
                return Ok(persona);
            }
            else
            {
                return BadRequest("La persona no tiene codeudores");
            }
        
        }
    }
}
