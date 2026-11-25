using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelNucleo.CalculosYProceos;
using ModelNucleo.Model;
using NucleoAyC.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NucleoAyC.Controllers
{
    /// <summary>
    /// Manejo de línea de crédito
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = "Linea_Credito")]
 
        public class Linea_CreditoController : Base_TransaccionController
        {
            /// <summary>
            /// Constructor de Linea_Credito con inyección de base de datos
            /// </summary>
            /// <param name="context"></param>
            /// <param name="userManager"></param>
            /// <param name="roleManager"></param>
            public Linea_CreditoController(
                NegocioContexto context,
                UserManager<IdentityUser> userManager,
                RoleManager<IdentityRole> roleManager)
                : base(context, userManager, roleManager)
            {
            }

            /// <summary>
            /// Get Retorna la lista de Líneas de créditos
            /// </summary>
            /// <returns>Lista línea de crédito</returns>
            [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public IEnumerable<Linea_Credito> LineasCredito()
        {
            return _context.Linea_Credito;
        }
        [HttpGet]
        public IEnumerable<TipoGarantia> getTipoGarantias()
        {
            return _context.TipoGarantias;
        }  
        
        [HttpGet]
        public IEnumerable<TipoDestinoCredito> getTipoDestinoCredito()
        {
            return _context.TipoDestinoCreditos;
        }
        /// <summary>
        /// Get Retorna el detalle de línea crédito
        /// </summary>
        /// <param name="id_linea">int id de línea</param>
        /// <returns>LineaCredito</returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Linea_CreditoDetalle(int id_linea)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var linea_Credito = await _context.Linea_Credito.SingleOrDefaultAsync(m => m.Id_Linea == id_linea);

            if (linea_Credito == null)
            {
                return NotFound();
            }

            return Ok(linea_Credito);
        }

        /// <summary>
        /// Post Para actualizar la línea de crédito
        /// </summary>
        /// <param name="id_linea">int id de la línea</param>
        /// <param name="linea_Credito">Linea_Credito objeto json</param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> Modificar([FromBody] Linea_Credito linea_Credito)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            _context.Entry(linea_Credito).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!Linea_CreditoExists(linea_Credito.Id_Linea))
                {
                    return NotFound("No fue encontrado para poder ser editado");
                }
                else
                {
                    return BadRequest(ex.StackTrace);
                }
            }

            return Ok("Se ha editado correctamente");
        }

        /// <summary>
        /// Post Agregar línea de crédito
        /// </summary>
        /// <param name="linea_Credito">Linea_Credito objeto json</param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> NuevaLinea_Credito([FromBody] Linea_Credito linea_Credito)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Linea_Credito.Add(linea_Credito);
            await _context.SaveChangesAsync();

            return Ok("La nueva Línea de crédito se ha registrado");
        }

        // DELETE: api/Linea_Credito/5
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLinea_Credito([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var linea_Credito = await _context.Linea_Credito.SingleOrDefaultAsync(m => m.Id_Linea == id);
            if (linea_Credito == null)
            {
                return NotFound();
            }

            _context.Linea_Credito.Remove(linea_Credito);
            await _context.SaveChangesAsync();

            return Ok(linea_Credito);
        }

        #region Linea_Credito_Historial
        /// <summary>
        /// detalle de la linea credito historial
        /// </summary>
        /// <param name="id_credito"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Detalle_Linea_Credito_Historial(int id_credito)
        {
            Linea_Credito_Historial linea =await _context
                .Linea_Credito_Historial
                .SingleAsync(s=>s.Id_Credito==id_credito);
            return Ok(linea);
        }

        /// <summary>
        /// edicion de linea credito historial
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Editar_Linea_Credito_Historial(
        [FromBody] Linea_Credito_Historial linea)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                if (!User.IsInRole("Administrador"))
                    return BadRequest(
                        "Debe ser Administrador para poder modificar la configuración");

                Credito credito = await _context.Credito
                    .SingleAsync(d => d.Id_Credito == linea.Id_Credito);

                // Actualizar historial
                _context.Entry(linea).State = EntityState.Modified;

                // Actualizar tasas del crédito
                credito.Interes = linea.Interes;
                credito.Interes_Comision = linea.Interes_Comision;

                // Recalcular cuota con la nueva tasa
                decimal nuevaCuota = await RecalcularCuotaCredito(credito, linea);

                credito.Cuota_Total = nuevaCuota;
                credito.Valor_Cuota = nuevaCuota;

                _context.Entry(credito).State = EntityState.Modified;

                await _context.SaveChangesAsync();

                await Bitacora(
                    "Linea_Credito",
                    "Cambio Interes",
                    credito.Id_Credito,
                    false,
                    true
                );

                return Ok(
                    $"Se ha modificado la Línea y la nueva cuota es {credito.Cuota_Total}");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace);
            }
        }


        private async Task<decimal> RecalcularCuotaCredito(
         Credito credito,
         Linea_Credito_Historial lch)
        {
            Periodo periodo = await _context.Periodo
                .AsNoTracking()
                .SingleAsync(d => d.Id_Periodo == credito.Id_Periodo);

            Linea_Credito lc = await _context.Linea_Credito
                .AsNoTracking()
                .Include(f => f.Cobros_Credito)
                .SingleAsync(t => t.Id_Linea == credito.Id_Linea);

            decimal total = await _context.Cobro_Periodico_Historial
                .AsNoTracking()
                .Where(a =>
                    a.Id_Linea_Credito_Historial ==
                    lch.Id_Linea_Credito_Historial)
                .SumAsync(d => (decimal?)d.Valor) ?? 0.00M;

            var resultado = new CalculosYProcesoCredito(
                lch.Interes_Comision,
                credito.Monto_pago,
                periodo,
                IVA,
                credito.Plazo,
                lch.Interes,
                DateTime.Now,
                total,
                lch.PlanAyC_Cobros_Periodico_Dinero,
                0.00M,
                dias_movible: 0,
                generar_amortizacion: false
            ).Resultado;

            return resultado.CuotaTotal;
        }
        private async Task Bitacora(string Tabla_Operacion, string Proceso, int id_transaccion, bool es_credito, bool reversion_credito = false)
        {
            try
            {
                Persona persona = new Persona();
                if (es_credito)
                {
                    persona = (from p in _context.Personas
                               join c in _context.Credito on p.Id_Persona equals c.Id_Persona
                               join t in _context.Transaccion_Credito on c.Id_Credito equals t.Id_Credito
                               where t.Id_Transaccion_Credito == id_transaccion
                               select p).FirstOrDefault();
                }
                if (!es_credito)
                {
                    persona = (from p in _context.Personas
                               join a in _context.Ahorro on p.Id_Persona equals a.Id_Persona
                               join t in _context.Transaccion_Ahorro on a.Id_Ahorro equals t.Id_Ahorro
                               where t.Id_Transaccion_Ahorro == id_transaccion
                               select p).FirstOrDefault();
                }
                if (reversion_credito)
                {
                    persona = (from p in _context.Personas
                               join c in _context.Credito on p.Id_Persona equals c.Id_Persona
                               where c.Id_Credito == id_transaccion
                               select p).FirstOrDefault();
                }


                DateTime fecha = DateTime.Now;
                string id_usuario = _userManager.GetUserId(HttpContext.User); ;
                Bitacora Bitacora = new Bitacora();
                Bitacora.Tabla_Operacion = Tabla_Operacion;
                Bitacora.Proceso = Proceso;
                Bitacora.Fecha_Proceso = DateTime.Now;
                Bitacora.Id_Usuario = id_usuario;
                Bitacora.Id_Transaccion = id_transaccion.ToString();
                Bitacora.Cliente = persona.NombreCompleto;
                Bitacora.Codigo = persona.Codigo;
                _context.Bitacora.Add(Bitacora).State = EntityState.Added;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private async Task<bool> Verificar_Estado_Credito(int id_credito)
        {
            Credito credito =await _context
                .Credito
                .SingleAsync(d=>d.Id_Credito==id_credito);
            return credito.Liquidado;
        }
        #endregion


        private bool Linea_CreditoExists(int id)
        {
            return _context.Linea_Credito.Any(e => e.Id_Linea == id);
        }
    }
}