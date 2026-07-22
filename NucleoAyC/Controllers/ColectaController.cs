using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelNucleo.Model;
using ModelNucleo.Model.ModelView;
using NucleoAyC.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NucleoAyC.Controllers
{
    /// <summary>
    /// manejador de colectas
    /// </summary>
   // [Authorize(Roles = "Administrador,Asistente,Asesor,Gerente,Jefe", AuthenticationSchemes = "Bearer")]
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = "Colecta")]
    public class ColectaController : Controller
    {
        private readonly NegocioContexto _context;
        private readonly UserManager<IdentityUser> _userManager;

        /// <summary>
        /// constructor e injección de base de datos
        /// </summary>
        /// <param name="context"></param>
        /// 
        /// <param name="userManager"></param>
        public ColectaController(NegocioContexto context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        /// <summary>
        /// Lista de colectas diarias filtrada por mes y usuario
        /// </summary>
        /// <param name="fecha">DateTime</param>
        /// <param name="id_agencia"></param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor,Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Colectas_Diarias(DateTime fecha, int id_agencia=0)
        {
            string userId = _userManager.GetUserId(HttpContext.User);
            List<Colecta_Diaria> colecta = new List<Colecta_Diaria>();
            NegocioContexto db = new NegocioContexto();
            Usuario_Agencia agencia_usuario;
            var asesores = new List<Cartera>();
            if (User.IsInRole("Jefe") || User.IsInRole("Asistente") || User.IsInRole("Gerente"))
            {
                agencia_usuario = await _context.Usuario_Agencia.AsNoTracking()
                    .SingleAsync(a => a.Id_Usuario == userId);
                 asesores = (from c in _context.Cartera
                    join ua in _context.Usuario_Agencia on c.Id_Usuario equals ua.Id_Usuario
                    where ua.Id_Agencia == agencia_usuario.Id_Agencia
                    select c).ToList();
            }
            else if (User.IsInRole("Administrador") && id_agencia >0)
            {
                asesores = (from c in _context.Cartera
                                join  ua in _context.Usuario_Agencia on c.Id_Usuario equals ua.Id_Usuario
                                where ua.Id_Agencia == id_agencia
                                select c).ToList();
            }
            foreach (var asesor in asesores)
            {
                bool existe = db.Colecta_Diaria.AsNoTracking().Any(t => t.Id_Usuario == asesor.Id_Usuario);
                if (existe)
                {
                    colecta.AddRange(_context.Colecta_Diaria
                        .AsNoTracking()
                        .Include(i => i.Colectas)
                        .Where(w => w.Fecha.Date == fecha.Date && !w.Borrado_Logico && w.Id_Usuario == asesor.Id_Usuario).ToList());
                }
            }
            return Ok(colecta);
  

        }

        /// <summary>
        /// crear nueva colecta de cabecera
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> Nueva_Colecta_Diaria()
        {
            Colecta_Diaria colecta = await Nueva_Colecta_Diaria_Unico();
            if (colecta != null)
                return Ok(colecta.Id_Colecta_Diaria);
            else
                return BadRequest("ya existe el registro que se intenta guardar");
        }

        private async Task<Colecta_Diaria> Nueva_Colecta_Diaria_Unico()
        {
            Colecta_Diaria colecta = new Colecta_Diaria();
            DateTime fecha = DateTime.Now;
            string userId = _userManager.GetUserId(HttpContext.User);

            colecta.Fecha = fecha;
            colecta.Id_Usuario = userId;

            bool existente = _context.Colecta_Diaria
                .Any(c =>
                            c.Id_Usuario == colecta.Id_Usuario &&
                            c.Fecha.Day == colecta.Fecha.Day &&
                            c.Fecha.Month == colecta.Fecha.Month &&
                            c.Fecha.Year == colecta.Fecha.Year);
            if (!existente)
            {
                colecta.Creado_Por = userId;
                colecta.Creado_En = fecha;
                _context.Colecta_Diaria.Add(colecta).State = EntityState.Added;
                await _context.SaveChangesAsync();
                return colecta;
            }
            return null;
        }

        /// <summary>
        /// Se puede solo editar los número de comprobantes inicio y final
        /// para que salga en el reporte que se firma
        /// </summary>
        /// <param name="no_comprobante_inicial">string</param>
        /// <param name="no_comprobante_final">string</param>
        /// <param name="id_colecta_diaria">int</param>
        /// <returns>200</returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPut]
        public async Task<IActionResult> Editar_Colecta_Diaria(string no_comprobante_inicial, string no_comprobante_final,int id_colecta_diaria)
        {
            Colecta_Diaria colecta_diaria = _context.Colecta_Diaria.SingleOrDefault(d=>d.Id_Colecta_Diaria==id_colecta_diaria);
            colecta_diaria.No_Comprobante_Inicio = no_comprobante_inicial;
            colecta_diaria.No_Comprobante_Final = no_comprobante_final;
            if (ModelState.IsValid)
            {
                _context.Entry(colecta_diaria).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok("El Registro fue editado correctamente");
            }
            return BadRequest("No se pudo editar el registro");
        }

        /// <summary>
        /// Detalle de la colecta diaria
        /// puede ser por id o cargar la actual del dia como tal
        /// </summary>
        /// <param name="id_colecta_diaria">int</param>
        /// <returns>Colecta_Diaria</returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Detalle_Colecta_Diaria(int id_colecta_diaria)
        {
            DateTime fecha = DateTime.Now;
            Colecta_Diaria colecta_diaria = null;
            if (id_colecta_diaria > 0)
                colecta_diaria = await _context.Colecta_Diaria.AsNoTracking()
                .Include(d => d.Colectas)
                .SingleOrDefaultAsync(d => d.Id_Colecta_Diaria == id_colecta_diaria && !d.Borrado_Logico);
            else
                colecta_diaria = await _context.Colecta_Diaria
                    .AsNoTracking()
                    .Include(d => d.Colectas)
                    .SingleOrDefaultAsync(t=>t.Fecha.Day==fecha.Day && t.Fecha.Month==fecha.Month && t.Fecha.Year==fecha.Year);

            return Ok(colecta_diaria);
        }

        /// <summary>
        /// Retorna la Colecta_diaria actual si ya existe sino lo crea
        /// </summary>
        /// <returns>Colecta_Diaria</returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Detalle_Colecta_Diaria_Actual()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            DateTime fecha = DateTime.Now;
            Colecta_Diaria colecta_diaria = null;
       
            if (_context.Colecta_Diaria.AsNoTracking().Any(t =>t.Id_Usuario==userId && t.Fecha.Day == fecha.Day && t.Fecha.Month == fecha.Month && t.Fecha.Year == fecha.Year))
                colecta_diaria = await _context.Colecta_Diaria
                .AsNoTracking()
                .Include(d => d.Colectas)
                .SingleOrDefaultAsync(t => t.Id_Usuario == userId && t.Fecha.Day == fecha.Day && t.Fecha.Month == fecha.Month && t.Fecha.Year == fecha.Year);
                else {
                colecta_diaria = await Nueva_Colecta_Diaria_Unico();
                colecta_diaria = await _context.Colecta_Diaria
               .AsNoTracking()
               .Include(d => d.Colectas)
               .SingleOrDefaultAsync(t => t.Id_Usuario == userId && t.Fecha.Day == fecha.Day && t.Fecha.Month == fecha.Month && t.Fecha.Year == fecha.Year);
            }
               

            if (colecta_diaria != null)
                return Ok(colecta_diaria);
            else
                return NoContent();
        }


        /// <summary>
        /// elimina la colecta diaria de forma lógica
        /// </summary>
        /// <param name="id_colecta_diaria">int</param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpDelete]
        public async Task<IActionResult> Borrar_Colecta_Diaria(int id_colecta_diaria)
        {
            Colecta_Diaria colecta_diaria = _context.Colecta_Diaria
                .SingleOrDefault(d=>d.Id_Colecta_Diaria==id_colecta_diaria && !d.Procesado_Total);
            string userId = _userManager.GetUserId(HttpContext.User);
            if (colecta_diaria != null)
            {
                colecta_diaria.Borrado_Logico = true;
                colecta_diaria.Eliminado_En = DateTime.Now;
                colecta_diaria.Eliminado_Por = userId;
                _context.Entry(colecta_diaria).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok("el registro fue eliminado correctamente");
            }
            return BadRequest("No se pudo eliminar el registro");
        }

        /*para el maestro detalle*/
        /// <summary>
        /// lista de colectas dado una colecta diaria
        /// </summary>
        /// <param name="id_colecta_diaria">int</param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Colectas(int id_colecta_diaria)
        {
            List<Colecta> colectas =await _context.Colecta.AsNoTracking()
                .Where(w => w.Id_Colecta_Diaria == id_colecta_diaria && !w.Borrado_Logico).ToListAsync();
            return Ok(colectas);
        }

        /// <summary>
        /// guardar una colecta dada con su colecta diaria
        /// </summary>
        /// <param name="colecta">Colecta</param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> Nueva_Colecta([FromBody] Colecta colecta)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        DateTime fecha_creacion=DateTime.Now;

                        string userId = _userManager.GetUserId(HttpContext.User);

                        Persona_Cuentas cuentas = ObtenerDatosPersona(colecta);
                        Persona persona = cuentas.persona;
                        string codigo_credito = cuentas.codigo_credito;
                        string codigo_ahorro = cuentas.codigo_ahorro;
                        bool comprobante = _context.Colecta.Any(a=>a.No_Comprobante == colecta.No_Comprobante && a.Creado_En.Date== fecha_creacion.Date);
                        if (comprobante)
                            return BadRequest("Ya existe un comprobante ingresado en la misma fecha");
                        if (colecta.Id_Credito.Equals(0) && colecta.Id_Ahorro.Equals(0))
                            return BadRequest("Ingrese el código de la cuenta destino");
                        colecta.Nombre_Completo = persona.NombreCompleto;
                        colecta.DUI = persona.Dui;
                        colecta.Codigo_AHorro = codigo_ahorro;
                        colecta.Codigo_Credito = codigo_credito;
                        colecta.Creado_Por = userId;
                        colecta.Creado_En = fecha_creacion;
                        _context.Colecta.Add(colecta).State = EntityState.Added;
                        await _context.SaveChangesAsync();
                        await Actualizar_Comprobantes_Colecta_Diaria(_context,colecta.Id_Colecta_Diaria);
                        dbContextTransaction.Commit();
                        return Ok(colecta.Id_Colecta);
                    }
                    catch (DbUpdateException ex)
                    {
                        dbContextTransaction.Rollback();
                        return BadRequest(ex.Message);
                    }
                }
            }
            return BadRequest("El modelo no es correcto y no se puedo guardar");
        }

        private async Task Actualizar_Comprobantes_Colecta_Diaria(NegocioContexto context,int id_colecta_diaria)
        {
            Colecta_Diaria colecta_diaria = context.Colecta_Diaria.Include(d=>d.Colectas).SingleOrDefault(d => d.Id_Colecta_Diaria == id_colecta_diaria);
            int count = colecta_diaria.Colectas.Count();

            if (string.IsNullOrEmpty(colecta_diaria.No_Comprobante_Inicio) && count==1)
            {
                int id_colecta_menor = colecta_diaria.Colectas.Min(d => d.Id_Colecta);
                string no_comprobante_inicial = colecta_diaria.Colectas.SingleOrDefault(d => d.Id_Colecta == id_colecta_menor).No_Comprobante;
                colecta_diaria.No_Comprobante_Inicio = no_comprobante_inicial;
            }

            if (count > 1)
            {
                int id_colecta_mayor = colecta_diaria.Colectas.Max(d => d.Id_Colecta);
                string no_comprobante_final = colecta_diaria.Colectas.SingleOrDefault(d => d.Id_Colecta == id_colecta_mayor).No_Comprobante;
                colecta_diaria.No_Comprobante_Final = no_comprobante_final;
            }

            context.Entry(colecta_diaria).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        private Persona_Cuentas ObtenerDatosPersona(Colecta colecta)
        {
            Persona persona = _context.Personas
                                     .Include(t => t.Ahorros)
                                     .Include(e => e.Creditos)
                                     .SingleOrDefault(f => f.Id_Persona == colecta.Id_Cliente);

            string codigo_credito = colecta.Id_Credito > 0 ? persona.Creditos.SingleOrDefault(j => j.Id_Credito == colecta.Id_Credito).Codigo_Credito : "--";
            string codigo_ahorro = colecta.Id_Ahorro > 0 ? persona.Ahorros.SingleOrDefault(j => j.Id_Ahorro == colecta.Id_Ahorro).No_Ahorro : "--";
            return new Persona_Cuentas {
                persona=persona,
                codigo_ahorro=codigo_ahorro,
                codigo_credito=codigo_credito
            };
        }

        /// <summary>
        /// editar un detalle de colecta
        /// </summary>
        /// <param name="colecta">Colecta</param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPut]
        public async Task<IActionResult> Editar_Colecta([FromBody] Colecta colecta)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    bool existente = _context.Colecta.Any(c =>
                            c.Id_Colecta_Diaria == colecta.Id_Colecta_Diaria &&
                            c.Id_Colecta == colecta.Id_Colecta &&
                            c.Id_Cliente == colecta.Id_Cliente &&
                            !c.Procesado &&
                            !c.Anulado);
                    Persona_Cuentas cuentas = ObtenerDatosPersona(colecta);

                    string userId = _userManager.GetUserId(HttpContext.User);
                    if (existente)
                    {
                        colecta.Nombre_Completo = cuentas.persona.NombreCompleto;
                        colecta.DUI = cuentas.persona.Dui;
                        colecta.Codigo_AHorro = cuentas.codigo_ahorro;
                        colecta.Codigo_Credito = cuentas.codigo_credito;

                        colecta.Creado_En = DateTime.Now;
                        colecta.Creado_Por = userId;
                        colecta.Modificado_En = DateTime.Now;
                        colecta.Modificado_Por = userId;
                        _context.Entry(colecta).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                        await Actualizar_Comprobantes_Colecta_Diaria(_context,colecta.Id_Colecta_Diaria);
                        dbContextTransaction.Commit();
                        return Ok("la colecta fue modificado correctamente");
                    }
                    return BadRequest("La colecta diaria no existe");
                } catch (DbUpdateException ex)
                {
                    dbContextTransaction.Rollback();
                    return BadRequest(ex.Message);
                }
            }
        }

        /// <summary>
        /// detalle de colecta
        /// </summary>
        /// <param name="id_colecta">int</param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Detalle_Colecta(int id_colecta)
        {
            Colecta colecta = await _context.Colecta.SingleOrDefaultAsync(d => d.Id_Colecta == id_colecta);
            return Ok(colecta);
        }

        /// <summary>
        /// Borrado lógico de una colecta detalle
        /// </summary>
        /// <param name="id_colecta">int</param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpDelete]
        public async Task<IActionResult> Borrar_Colecta(int id_colecta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var colecta = await _context.Colecta.SingleOrDefaultAsync(m => m.Id_Colecta == id_colecta);
            if (colecta == null)
            {
                return NotFound();
            }
            try
            {
                _context.Colecta.Remove(colecta);
                await _context.SaveChangesAsync();
                return Ok("Colecta eliminada exitosamente");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        /// <summary>
        /// aplicar bandera de verificación a una colecta dada
        /// </summary>
        /// <param name="id_colecta_diaria"></param>
        /// <returns></returns>
        [Authorize(Roles = "Asistente,Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPut]
        public async Task<IActionResult> Verificar_Colecta(int id_colecta_diaria)
        {
            try
            {
                bool no_verificado = _context.Colecta_Diaria.Any(l => l.Id_Colecta_Diaria == id_colecta_diaria && !l.Verificado);
                if (no_verificado)
                {
                    Colecta_Diaria colecta_diaria = _context.Colecta_Diaria
                         .SingleOrDefault(s => s.Id_Colecta_Diaria == id_colecta_diaria);
                    colecta_diaria.Verificado = true;
                    _context.Entry(colecta_diaria).State = EntityState.Modified;
                    _context.SaveChanges();
                    return Ok("Colectas Verificadas Exitosamente");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest("Algo anda mal! Intentalo de Nuevo");
        }
        [Authorize(Roles = "Asesor, Asistente,Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> Nuevo_Imagen_Colecta([FromBody] Fotografia_Comprobante fotografia)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Fotografia_Comprobantes.Add(fotografia);
            await _context.SaveChangesAsync();
            return Ok("El la fotografia se ha cargado exitosamente");
            
        }

        /// <summary>
        /// Obtener Detalle Foto Colecta
        /// </summary>
        /// <param name="id_documento">int id de un documetno</param>
        /// <returns>Documento objeto json</returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Imagen_Colecta_Detalle(int id_colecta)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var colecta = await _context.Fotografia_Comprobantes.SingleOrDefaultAsync(m => m.Id_Colecta == id_colecta);
            if (colecta == null)
                return NotFound();

            return Ok(colecta);
        }
        /// <summary>
        /// Validar colecta
        /// </summary>
        /// <param name="id_documento">int id de un documetno</param>
        /// <returns>Documento objeto json</returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPut]
        public async Task<IActionResult> Validar_Imagen_Colecta(int id_fotografia_imagen)
        {
            try
            {
                bool no_verificado = _context.Fotografia_Comprobantes.Any(l => l.Id_Fotografia_Comprobante == id_fotografia_imagen && !l.Verificado);
                if (no_verificado)
                {
                    Fotografia_Comprobante fotografia_Comprobante = _context.Fotografia_Comprobantes
                         .SingleOrDefault(s => s.Id_Fotografia_Comprobante == id_fotografia_imagen);
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
        /// <summary>
        /// Validar colecta
        /// </summary>
        /// <param name="id_documento">int id de un documetno</param>
        /// <returns>Documento objeto json</returns>
        [Authorize(Roles = "Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Obtener_Fotografias_Legales(string asesor, DateTime fecha_inicio, DateTime fecha_fin)
        {
            try
            {
                List<Colecta> colectas = _context.Colecta.AsNoTracking().Where(a=>a.Creado_Por ==asesor 
                && a.Creado_En.Date >= fecha_inicio.Date && a.Creado_En.Date <= fecha_fin.Date).ToList();  
                
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest("Algo anda mal! Intentalo de Nuevo");
        }
    }
}