using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelNucleo.Model;
using ModelNucleo.Model.Contabilidad;
using ModelNucleo.Model.ModelView;
using NucleoAyC.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NucleoAyC.Controllers
{
    /// <summary>
    /// la configuración para conectar contabilidad con las transacciones generales
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = "Configuracion_Contabilidad")]
    public class Configuracion_ContabilidadController : Controller
    {
        private NegocioContexto _context;

        /// <summary>
        /// constructor simplemente
        /// </summary>
        /// <param name="contexto"></param>
        public Configuracion_ContabilidadController(NegocioContexto contexto)
        {
            _context = contexto;
        }

        #region solo cabeceras de cofiguración
        /// <summary>
        /// lista de configuraciones 
        /// contables solo cabeceras
        /// </summary>
        /// <param name="id_agencia"></param>
        /// <param name="id_linea_credito"></param>
        /// <param name="id_linea_ahorro"></param>
        /// <param name="id_factura_servicio"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Configuraciones(
            int id_agencia,
            int id_linea_credito = 0,
            int id_linea_ahorro = 0,
            int id_factura_servicio=0,
            int id_otras_operaciones=0)
        {
            Func<Conf_Partida_Contable, bool> filtracion = new Func<Conf_Partida_Contable, bool>(t => t.Id_Agencia == id_agencia);
            if (id_linea_credito > 0 && id_linea_ahorro == 0 && id_factura_servicio==0 && id_otras_operaciones ==0)
                filtracion = new Func<Conf_Partida_Contable, bool>(t => t.Id_Agencia == id_agencia && t.Id_Linea_Credito == id_linea_credito);
            if (id_linea_ahorro > 0 && id_linea_credito == 0 && id_factura_servicio == 0 && id_otras_operaciones ==0)
                filtracion = new Func<Conf_Partida_Contable, bool>(t => t.Id_Agencia == id_agencia && t.Id_Linea_Ahorro == id_linea_ahorro);
            if (id_factura_servicio > 0 && id_linea_ahorro == 0 && id_linea_credito == 0 && id_otras_operaciones ==0)
                filtracion = new Func<Conf_Partida_Contable, bool>(t => t.Id_Agencia == id_agencia && t.Id_Factura_Servicio == id_factura_servicio);
            if (id_otras_operaciones > 0 && id_linea_ahorro == 0 && id_linea_credito == 0 && id_factura_servicio == 0)
                filtracion = new Func<Conf_Partida_Contable, bool>(t => t.Id_Agencia == id_agencia && t.Id_Otras_Operaciones == id_otras_operaciones);
            IEnumerable<Conf_Partida_Contable> temp = _context
                .Set<Conf_Partida_Contable>()
                .Include(d => d.Agencia)
                .AsEnumerable();
            List<Conf_Partida_Contable> configuraciones = await Task.FromResult(temp.Where(filtracion).ToList());
            return Ok(configuraciones);
        }

        /// <summary>
        /// guardar una configuración nueva
        /// </summary>
        /// <param name="configuracion"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Nuevo_Configuracion([FromBody] Conf_Partida_Contable configuracion)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(configuracion).State = EntityState.Added;
                await _context.SaveChangesAsync();
                return Ok("La configuración se ha guardado correctamente");
            }
            else {
                return BadRequest("los datos no son correctos o no están completos");
            }
        }

        /// <summary>
        /// editar configuración
        /// contable
        /// </summary>
        /// <param name="configuracion"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Editar_Configuracion([FromBody] Conf_Partida_Contable configuracion)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(configuracion).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok("La configuración se ha modificado correctamente");
            }
            else
            {
                return BadRequest("los datos no son correctos o no están completos");
            }
        }

        /// <summary>
        /// borrar la configuracion completa
        /// </summary>
        /// <param name="id_configuracion"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> Borrar_Configuracion(int id_configuracion)
        {
            try
            {
                Conf_Partida_Contable conf = _context.Conf_Partida_Contable.Find(id_configuracion);
                _context.Remove(conf).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
                return Ok("Se ha borrado la configuración y su cuentas contables");
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// detalle de la configuración
        /// con lista de los detalles de
        /// cuentas contables relacionadas
        /// </summary>
        /// <param name="id_configuracion"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Detalle_Configuracion(int id_configuracion)
        {
            Conf_Partida_Contable configuracion = await _context
                .Conf_Partida_Contable.AsNoTracking()
                .Include(d=>d.Agencia)
                .Include(d=>d.Conf_Detalles_Partidas_Contables)
                .SingleAsync(i=>i.Id_Conf_Partida_Contable==id_configuracion);

            Linea_Credito linea_credito = new Linea_Credito();
            if (configuracion.Id_Linea_Credito > 0)
                linea_credito =await _context
                    .Linea_Credito
                    .AsNoTracking()
                    .SingleAsync(f=>f.Id_Linea==configuracion.Id_Linea_Credito);
            Linea_Ahorro linea_ahorro = new Linea_Ahorro();
            if (configuracion.Id_Linea_Ahorro > 0)
                linea_ahorro = await _context.Linea_Ahorro.SingleAsync(t=>t.Id_Linea_Ahorro==configuracion.Id_Linea_Ahorro);

            Detalle_Partida_Contable_Configuracion_Vista vista = new Detalle_Partida_Contable_Configuracion_Vista {
                Conf_Partida_Contable = configuracion,
                Conf_Detalle_Partida_Contables= configuracion.Conf_Detalles_Partidas_Contables.ToList(),
                Agencia=configuracion.Agencia,
                Linea_Credito=linea_credito,
                Linea_Ahorro=linea_ahorro
            };
            return Ok(vista);
        }
        #endregion

        #region manejo de detalle de las configuraciones
        /// <summary>
        /// guardar detalle de la partida
        /// son cuentas contables
        /// </summary>
        /// <param name="detalle"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Nuevo_Detalle_Conf([FromBody] Conf_Detalle_Partida_Contable detalle)
        {
            if (!ModelState.IsValid)
                return BadRequest("los datos no son correctos o no están completos");
            
            _context.Conf_Detalle_Partida_Contable.Add(detalle).State = EntityState.Added;
            await _context.SaveChangesAsync();
            return Ok("El detalle de configuración se ha guardado correctamente");
        }

        /// <summary>
        /// modificar detalle de la partida
        /// son cuentas contables
        /// </summary>
        /// <param name="detalle"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Editar_Detalle_Conf([FromBody] Conf_Detalle_Partida_Contable detalle)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(detalle).State = EntityState.Added;
                await _context.SaveChangesAsync();
                return Ok("El detalle de configuración se ha modificado correctamente");
            }
            else
            {
                return BadRequest("los datos no son correctos o no están completos");
            }
        }

        /// <summary>
        /// detalle de una cuenta contable configurado
        /// </summary>
        /// <param name="id_conf_detalle_partida"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Detalle_Conf_Partida(int id_conf_detalle_partida)
        {
            Conf_Detalle_Partida_Contable detalle =await _context
                .Conf_Detalle_Partida_Contable
                .SingleAsync(c=>c.Id_Conf_Detalle_Partida_Contable==id_conf_detalle_partida);
            return Ok(detalle);
        }

        /// <summary>
        /// se edita la cuenta contable de una configuración
        /// </summary>
        /// <param name="detalle"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Editar_Detalle_Conf_Partida([FromBody] Conf_Detalle_Partida_Contable detalle)
        {
            try {
                _context.Update(detalle).State=EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok("la cuenta contable de configuración fue modifica");
            }
            catch (Exception ex) {
                return BadRequest(ex.InnerException.StackTrace);
            }
        }

        /// <summary>
        /// borrar detalle 
        /// </summary>
        /// <param name="id_conf_detalle_partida"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> Borrar_Detalle_Conf_Partida(int id_conf_detalle_partida)
        {
            try
            {
                Conf_Detalle_Partida_Contable detalle= await _context
                    .Conf_Detalle_Partida_Contable
                    .SingleAsync(m=>m.Id_Conf_Detalle_Partida_Contable==id_conf_detalle_partida);
                _context.Remove(detalle).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
                return Ok("la cuenta contable de configuración fue borrada");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// solo para ejecutar una vez
        /// </summary>
        [Authorize(Roles = "Asesor,Administrador,Gerente,Jefe", AuthenticationSchemes = "Bearer")]
        [HttpPut]
        public async Task<IActionResult> Verificar_Partida_Contable(int id_asiento_contable)
        {
            try
            {
                Asiento_Contable asiento_contable = _context.Asiento_Contable
                    .AsNoTracking().Single(a=>a.Id_Asiento_Contable == id_asiento_contable);
                if (asiento_contable.Verificada)
                {
                    return BadRequest("La partida contable ya ha sido verificada");
                }
                else {
                    asiento_contable.Verificada = true;
                    _context.Entry(asiento_contable).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return Ok("Se ha verificado la partida contable");
                }
             
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace);
            }


        }
        #endregion
    }
}
