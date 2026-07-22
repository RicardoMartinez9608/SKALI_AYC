using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelNucleo.Model;
using ModelNucleo.Model.ModelView;
using NucleoAyC.Entities;
using Microsoft.AspNetCore.Identity;
using NucleoAyC.Migrations;

namespace AyCWeb.Controllers
{
    /// <summary>
    /// controlle de documentos
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = "Documentos")]
    public class DocumentosController : Controller
    {
        private readonly NegocioContexto _context;
        private readonly IHostingEnvironment _enviroment;
        private readonly UserManager<IdentityUser> _userManager;
        /// <summary>
        /// constructor injectado de documentos
        /// </summary>
        /// <param name="context"></param>
        /// <param name="enviroment"></param>
        public DocumentosController(NegocioContexto context,IHostingEnvironment enviroment, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _enviroment = enviroment;
            _userManager = userManager;
        }

        /// <summary>
        /// Get lista de documentos de una persona determinada
        /// </summary>
        /// <param name="id_persona">int id de la persona</param>
        /// <returns>Lista de documentos</returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public IEnumerable<Documento> Documentos(int id_persona)
        {
            return _context.Documento.AsNoTracking().Include(i=>i.Descripcion).Where(t=>t.Id_Persona==id_persona);
        }

        /// <summary>
        /// se tiene el expediente y su formación
        /// </summary>
        /// <param name="dui">documento único de identidad</param>
        /// <param name="id_persona"></param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Expediente(string dui, int id_persona=0)
        {
            Persona persona_expediente = await _context
                .Personas
                .AsNoTracking()
                .Include(d => d.Documentos)
                .Include("Documentos.Descripcion")
                .SingleAsync(s => s.Dui == dui || s.Id_Persona==id_persona);
            
            if (persona_expediente != null)
            {
                List<Documento_Clasificado> clasificados = new List<Documento_Clasificado>();
                List<Descripcion_Documento> descripciones = _context.Descripcion_Documento.ToList();//como catálogo
                foreach (Descripcion_Documento descripcion in descripciones)
                {
                    if (persona_expediente.Documentos.Count > 0)
                    {
                        if (persona_expediente.Documentos.Any(f => f.Id_Descripcion_Documento == descripcion.Id_Descripcion_Documento))
                        {
                            Documento doc = persona_expediente
                            .Documentos
                            .Single(f => f.Id_Descripcion_Documento == descripcion.Id_Descripcion_Documento);
                            clasificados.Add(new Documento_Clasificado
                            {
                                Id_Documento = doc.Id_Documento,
                                Fecha = doc.Fecha,
                                Agregado = true,
                                NombreArchivo = doc.NombreArchivo,
                                Descripcion = doc.Descripcion.Descripcion,
                                Archivo_Base64 = doc.Archivo_Base64,
                                Es_PDF = doc.Es_PDF,
                                Es_Imagen = doc.Es_Imagen
                            });
                        }
                        else
                            clasificados.Add(new Documento_Clasificado
                            {
                                Id_Documento = 0,
                                Fecha = null,
                                Agregado = false,
                                NombreArchivo = "--",
                                Descripcion = descripcion.Descripcion,
                                Archivo_Base64 = "",
                                Es_PDF = false,
                                Es_Imagen = false
                            });
                    }else
                        clasificados.Add(new Documento_Clasificado
                        {
                            Id_Documento = 0,
                            Fecha = null,
                            Agregado = false,
                            NombreArchivo = "--",
                            Descripcion = descripcion.Descripcion,
                            Archivo_Base64 = "",
                            Es_PDF = false,
                            Es_Imagen = false
                        });
                }

                Expediente expediente = new Expediente
                {
                    Socio=persona_expediente,
                    Documentos_Clasificados= clasificados
                };

                return Ok(expediente);
            }
            return BadRequest("No se encontró alguien con ese documento");
        }

        /// <summary>
        /// Get Detalle de un documento
        /// </summary>
        /// <param name="id_documento">int id de un documetno</param>
        /// <returns>Documento objeto json</returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Documento_Detalle(int id_documento)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var documento = await _context.Documento.SingleOrDefaultAsync(m => m.Id_Documento == id_documento);
            if (documento == null)
                return NotFound();

            return Ok(documento);
        }

        /// <summary>
        /// Put Modificar documento
        /// </summary>
        /// <param name="id_documento">int id de documento</param>
        /// <param name="documento">Documento objeto json</param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPut]
        public async Task<IActionResult> Modificar_Documento(int id_documento, [FromBody] Documento documento)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (id_documento != documento.Id_Documento)
                return BadRequest();
            _context.Entry(documento).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DocumentoExists(id_documento))
                    return NotFound();
                else
                    throw;
            }
            return NoContent();
        }

        /// <summary>
        /// Post Crea documento y guarda el físico
        /// </summary>
        /// <param name="documento">Documento objeto json</param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> Nuevo_Documento([FromBody] Documento documento)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (await Verificar_Documento(documento))
                return BadRequest("Ya existe ese tipo de documento para la persona consultada");
            else
            {
                documento.NombreArchivo = string.Format("{0}.{1}", Guid.NewGuid(), "JPEG");
                _context.Documento.Add(documento);
                await _context.SaveChangesAsync();
                return Ok("El documento ha sido guardado");
            }
        }

        /// <summary>
        /// para verificar si una persona ya tiene documentos existentes
        /// </summary>
        /// <param name="id_descripcion_documento"></param>
        /// <param name="id_persona"></param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Verificar_Documento(int id_descripcion_documento, int id_persona)
        {
            bool existe =await _context
                .Documento
                .AnyAsync(
                    a=>a.Id_Descripcion_Documento==id_descripcion_documento 
                    && a.Id_Persona==id_persona);
            if (existe)
                return Ok("Si Existe");
            else
                return BadRequest("No Existe");
        }


        private async Task<bool> Verificar_Documento(Documento documento)
        {
            bool existe=await _context
                .Documento
                .AnyAsync(
                    w=>w.Id_Persona==documento.Id_Persona && 
                    w.Id_Descripcion_Documento==documento.Id_Descripcion_Documento);
            return existe;
        }

        /// <summary>
        /// Delete Borra un documento el registro y físicamente
        /// </summary>
        /// <param name="id_documento">int id de un documento</param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpDelete]
        public async Task<IActionResult> Borrar_Documento(int id_documento)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var documento = await _context.Documento.SingleOrDefaultAsync(m => m.Id_Documento == id_documento);
            if (documento == null)
            {
                return NotFound();
            }
            _context.Documento.Remove(documento);
            await _context.SaveChangesAsync();
            return Ok();
        }

        private bool DocumentoExists(int id)
        {
            return _context.Documento.Any(e => e.Id_Documento == id);
        }

        /// <summary>
        /// dado el nombre del archivo se obtiene el binario para mostrar
        /// </summary>
        /// <param name="nombre_archivo">string nombre del archivo</param>
        /// <returns>Obtinen binario de imagen</returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<HttpResponseMessage> DescargarArchivo(string nombre_archivo)
        {
            var scanning = Path.Combine(_enviroment.WebRootPath, "scanning");
            string direccion_completa=Path.Combine(scanning, nombre_archivo);
            HttpResponseMessage hrm = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            Stream stream = new FileStream(direccion_completa,FileMode.Open,FileAccess.Read);
            hrm.Content = new StreamContent(stream);
            hrm.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
            return await Task.Run(()=> hrm);
        }

        /// <summary>
        /// Lista de descripción para la documentación
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Descripciones_Documento()
        {
            List<Descripcion_Documento> descripcion_documento = _context.Descripcion_Documento
                .AsNoTracking().ToList();
            return await Task.Run(()=>Ok(descripcion_documento));
        }

        /// <summary>
        /// Post Crea documento y guarda el físico
        /// </summary>
        /// <param name="documento">Documento objeto json</param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> Nuevo_Documento_Atm([FromBody] Fotografia_Atm documento)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            string userId = _userManager.GetUserId(HttpContext.User);
           
            documento.Creado_Por = userId;
            _context.Fotografia_Atms.Add(documento);
                await _context.SaveChangesAsync();
                return Ok("El Fotografía almaceada");
            
        }
        /// <summary>
        /// Post Crea documento y guarda el físico
        /// </summary>
        /// <param name="documento">Documento objeto json</param>
        /// <returns></returns>
        [Authorize(Roles = "Asistente,Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Listado_Verificaciones(DateTime fecha_inicio, DateTime Fecha_fin)
        {
            string userId = _userManager.GetUserId(HttpContext.User);
            Usuario_Agencia ua = _context.Usuario_Agencia.AsNoTracking().Single(a => a.Id_Usuario == userId);
            List<Usuario_Agencia> usuarios_agencia = _context.Usuario_Agencia
                .AsNoTracking().Where(a => a.Id_Agencia == ua.Id_Agencia).ToList();
            List<Fotografia_Atm> fotografia_Atms = new List<Fotografia_Atm>();
            if (User.IsInRole("Jefe") || User.IsInRole("Asistente"))
            {
                foreach (Usuario_Agencia user in usuarios_agencia)
                {
                    fotografia_Atms.AddRange(_context.Fotografia_Atms
                        .Include(a=>a.Banco)
                        .AsNoTracking()
                    .Where(a => a.Creado_En.Date >= fecha_inicio.Date
                    && a.Creado_En.Date <= Fecha_fin.Date && a.Creado_Por == user.Id_Usuario).ToList());
                }

            }
            else if (User.IsInRole("Administrador")) {
                foreach (Usuario_Agencia user in usuarios_agencia)
                {
                    fotografia_Atms = _context.Fotografia_Atms
                        .AsNoTracking()
                        .Include(a => a.Banco)
                    .Where(a => a.Creado_En.Date >= fecha_inicio.Date
                    && a.Creado_En.Date <= Fecha_fin.Date ).ToList();
                }
            }
            
            return Ok(fotografia_Atms);

        }
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Detalle_Atm(int id_fotografia_atm)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var foto_Atm = await _context.Fotografia_Atms.SingleOrDefaultAsync(m => m.Id_Fotografia_Atm == id_fotografia_atm);
            if (foto_Atm == null)
                return NotFound();

            return Ok(foto_Atm);

        }
        /// <summary>
        /// Validar colecta
        /// </summary>
        /// <param name="id_documento">int id de un documetno</param>
        /// <returns>Documento objeto json</returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPut]
        public async Task<IActionResult> Validar_Imagen_Atm(int id_fotografia_atm)
        {
            try
            {
                bool no_verificado = _context.Fotografia_Atms.Any(l => l.Id_Fotografia_Atm == id_fotografia_atm && !l.Verificado);
                if (no_verificado)
                {
                    Fotografia_Atm fotografia_Comprobante = _context.Fotografia_Atms
                         .SingleOrDefault(s => s.Id_Fotografia_Atm == id_fotografia_atm);
                    fotografia_Comprobante.Verificado = true;
                    _context.Entry(fotografia_Comprobante).State = EntityState.Modified;
                    _context.SaveChanges();
                    return Ok("Imagen Verificada");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest("Algo anda mal! Intentalo de Nuevo");
        }
    }
}