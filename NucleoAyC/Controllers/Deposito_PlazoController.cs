using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelNucleo.CalculosYProceos;
using ModelNucleo.Model;
using NucleoAyC.Entities;

namespace NucleoAyC.Controllers
{
    /// <summary>
    /// Depositos a Plazo
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = "Deposito_Plazo")]
    public class Deposito_PlazoController : Base_TransaccionController
    {
        /// <summary>
        /// constructor e injección de base de datos
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userManager"></param>
        /// <param name="roleManager"></param>
        public Deposito_PlazoController(NegocioContexto context,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
            : base(context, userManager, roleManager)
        {
        }
        /// <summary>
        /// Post Crear un registro de ahorro a plazo fijo
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Asistente,Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> Nuevo_Deposito_Plazo([FromBody] Deposito_Plazo ahorro)
        {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                try
                {
                    ahorro.Id_Deposito_Plazo = 0;
                    int id_ahorro_ultimo = 0;
                    if (_context.Deposito_Plazo.Any())
                        id_ahorro_ultimo = _context.Deposito_Plazo.AsEnumerable().Last().Id_Deposito_Plazo;
                    ahorro.No_Ahorro = GeneradorCodigo.Generar(id_ahorro_ultimo, ahorro.Id_Persona);
                    _context.Deposito_Plazo.Add(ahorro).State = EntityState.Added;
                    await _context.SaveChangesAsync();
                    return Ok("Deposito a Plazo creado correctamente");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
        }

        /// <summary>
        /// Get Obtener la lista de deposito a plazo por persona
        /// </summary>
        /// <returns>Lista de Ahorro</returns>
        [Authorize(Roles = "Asistente,Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Deposito_Plazo_Por_Persona(int id_persona)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            List<Deposito_Plazo> ahorros = await _context.Deposito_Plazo.AsNoTracking()
                .Where(h => h.Id_Persona == id_persona).ToListAsync();
            return Ok(ahorros);
        }
        /// <summary>
        /// Get Se obtiene un detalle de un deposito a plazo
        /// </summary>
        /// <returns>Ahorro</returns>
        [Authorize(Roles = "Asistente,Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> GetDepositoDetalle(int id_deposito)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ahorro = await _context.Deposito_Plazo.SingleOrDefaultAsync(m => m.Id_Deposito_Plazo == id_deposito);

            if (ahorro == null)
            {
                return NotFound();
            }

            return Ok(ahorro);
        }
        /// <summary>
        /// Put Para modificar el deposito a plazo
        /// </summary>
    
        /// <returns></returns>
        [Authorize(Roles = "Asistente,Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPut]
        public async Task<IActionResult> Modificar([FromBody] Deposito_Plazo ahorro)
        {
            Deposito_Plazo Ahorro_verificado = await _context.Deposito_Plazo.
                AsNoTracking()
                .SingleAsync(f => f.Id_Deposito_Plazo == ahorro.Id_Deposito_Plazo);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                ahorro.Fecha_Apertura = DateTime.Now;
                _context.Update(ahorro).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok("El registro se ha guardado");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace);
            }
        }

        //----------------------------------------------------manejo beneficiarios---------------------------------------------
        /// <summary>
        /// Post Crear un registro de ahorro a plazo fijo
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> Agregar_Beneficiario_Deposito([FromBody] Beneficiarios_Deposito_Plazo beneficiario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                _context.Beneficiarios_Deposito_Plazo.Add(beneficiario).State = EntityState.Added;
                await _context.SaveChangesAsync();
                return Ok("Beneficiario Agregado Correctamente");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Post Crear un registro de ahorro a plazo fijo
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Beneficiarios_Deposito(int id_deposito)
        {
            List<Beneficiarios_Deposito_Plazo> beneficiarios = await _context.Beneficiarios_Deposito_Plazo.AsNoTracking()
                .Where(h => h.Id_Deposito_Plazo == id_deposito).ToListAsync();
            return Ok(beneficiarios);
        }
        /// <summary>
        /// Put Para modificar el deposito a plazo
        /// </summary>

        /// <returns></returns>
        [Authorize(Roles = "Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPut]
        public async Task<IActionResult> Modificar_Beneficiario([FromBody] Beneficiarios_Deposito_Plazo deposito)
        {
            Beneficiarios_Deposito_Plazo deposito_verificado = await _context.Beneficiarios_Deposito_Plazo.
                AsNoTracking()
                .SingleAsync(f => f.Id_Beneficiario_Deposito_Plazo == deposito.Id_Beneficiario_Deposito_Plazo);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
             
                _context.Update(deposito).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok("El registro se ha guardado");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace);
            }
        }
        [Authorize(Roles = "Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpDelete]
        public async Task<IActionResult> BorrarBeneficiario(int id_beneficiario)
        {
            
            var beneficiario = await _context.Beneficiarios_Deposito_Plazo.
                SingleOrDefaultAsync(m => m.Id_Beneficiario_Deposito_Plazo == id_beneficiario);
            if (beneficiario == null)
            {
                return NotFound();
            }
            try
            {
                _context.Beneficiarios_Deposito_Plazo.Remove(beneficiario);
                await _context.SaveChangesAsync();
                return Ok("Beneficiario Eliminado Exitosamente");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Get Se obtiene un detalle de un deposito a plazo
        /// </summary>
        /// <returns>Ahorro</returns>
        [Authorize(Roles = "Asistente,Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> GetBeneficiarioDetalle(int id_deposito)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var beneficiario = await _context.Beneficiarios_Deposito_Plazo
                .SingleOrDefaultAsync(m => m.Id_Beneficiario_Deposito_Plazo == id_deposito);

            if (beneficiario == null)
            {
                return NotFound();
            }

            return Ok(beneficiario);
        }

        //-----------------------------------------------------manejo autorizados-------------------------------------------------------
        /// <summary>
        /// Post Crear un registro de ahorro a plazo fijo
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> Agregar_Autorizado([FromBody] Autorizado autorizado)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                _context.Autorizado.Add(autorizado).State = EntityState.Added;
                await _context.SaveChangesAsync();
                return Ok("Persona Agregada Correctamente");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Post Crear un registro de ahorro a plazo fijo
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Autorizados_Deposito(int id_deposito)
        {
            List<Autorizado> autorizados = await _context.Autorizado.AsNoTracking()
                .Where(h => h.Id_Deposito_Plazo == id_deposito).ToListAsync();
            return Ok(autorizados);
        }
        /// <summary>
        /// Put Para modificar el deposito a plazo
        /// </summary>

        /// <returns></returns>
        [Authorize(Roles = "Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPut]
        public async Task<IActionResult> Modificar_Autorizado([FromBody] Autorizado autorizado)
        {
            Autorizado autorizado_verificado = await _context.Autorizado.
                AsNoTracking()
                .SingleAsync(f => f.Id_Autorizado == autorizado.Id_Autorizado);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {

                _context.Update(autorizado).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok("El registro se ha guardado");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace);
            }
        }
        [Authorize(Roles = "Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpDelete]
        public async Task<IActionResult> BorrarAutorizado(int id_autorizado)
        {

            var autorizado = await _context.Autorizado.
                SingleOrDefaultAsync(m => m.Id_Autorizado == id_autorizado);
            if (autorizado == null)
            {
                return NotFound();
            }
            try
            {
                _context.Autorizado.Remove(autorizado);
                await _context.SaveChangesAsync();
                return Ok("Beneficiario Eliminado Exitosamente");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Get Se obtiene un detalle de un deposito a plazo
        /// </summary>
        /// <returns>Ahorro</returns>
        [Authorize(Roles = "Asistente,Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> GetAutorizadoDetalle(int id_autorizado)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var autorizado = await _context.Autorizado
                .SingleOrDefaultAsync(m => m.Id_Autorizado == id_autorizado);

            if (autorizado == null)
            {
                return NotFound();
            }

            return Ok(autorizado);
        }
        /// <summary>
        /// Get La lista de las agencias existentes
        /// </summary>
        /// <returns>muestra todas las agencias existentes</returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public List<Parentesco_Persona> Parestesco()
        {
            using (NegocioContexto db = new NegocioContexto())
            {
                return db.Parentesco.ToList();
            }
        }
        /// <summary>
        /// Get La lista de las agencias existentes
        /// </summary>
        /// <returns>muestra todas las agencias existentes</returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public List<Forma_Pago_DP> Forma_Pago()
        {
            using (NegocioContexto db = new NegocioContexto())
            {
                return db.Forma_Pago.ToList();
            }
        }

        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> DepositosPorPersona(int id_persona)
        {
           
            List<Deposito_Plazo> depositos = await _context.Deposito_Plazo.AsNoTracking()
                .Where(h => h.Id_Persona == id_persona).ToListAsync();
            return Ok(depositos);
        }
    }
}
