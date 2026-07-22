using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelNucleo.CalculosYProceos;
using ModelNucleo.Model;
using ModelNucleo.Model.Evaluacion_Tecnica;
using ModelNucleo.Model.ModelView;
using NucleoAyC.Entities;

namespace NucleoAyC.Controllers
{
    /// <summary>
    /// Manejo de evaluaci'on t'ecnica
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = "Evaluacion_Tecnica")]
    public class Evaluacion_TecnicaController : Controller
    {
        private readonly NegocioContexto _context;
        private readonly UserManager<IdentityUser> _userManager;

        /// <summary>
        /// Constructor de la clase de Crédito
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userManager"></param>
        public Evaluacion_TecnicaController(NegocioContexto context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        /// <summary>
        /// Crear una nueva evaluaci'on t'ecnica 
        /// </summary>
        /// <param name="evaluacion">Todo el objeto de evaluaci'on completo</param>
        /// <returns>Ok</returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> Nueva_Evaluacion([FromBody]Evaluacion evaluacion)
        {
            if (!_context.Evaluacion
                .Any(e => e.Id_Persona == evaluacion.Id_Persona && 
                        e.No_Dias_Buenos==evaluacion.No_Dias_Buenos &&
                        e.No_Dias_Malos==evaluacion.No_Dias_Malos &&
                        e.No_Dias_Descanso==evaluacion.No_Dias_Descanso &&
                        e.Venta_Mensual_Bueno==evaluacion.Venta_Mensual_Bueno &&
                        e.Venta_Mensual_Malo==evaluacion.Venta_Mensual_Malo &&
                        e.Venta_Semanal_Bueno==evaluacion.Venta_Semanal_Malo))
            {
                /*hacer el cálculo*/
                evaluacion.Venta_Semanal_Bueno = evaluacion.No_Dias_Buenos * evaluacion.Venta_Diaria_Bueno;
                evaluacion.Venta_Semanal_Malo = evaluacion.No_Dias_Malos * evaluacion.Venta_Diaria_Mala;

                evaluacion.Venta_Mensual_Bueno = evaluacion.Venta_Semanal_Bueno * 4;
                evaluacion.Venta_Mensual_Malo = evaluacion.Venta_Semanal_Malo * 4;

                _context.Add(evaluacion);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return NoContent();
        }

        /// <summary>
        /// lista de evaluaciones tecnicas por un cliente o persona
        /// es d[onde se pueden hacer todas las evaluaciones necesarias
        /// </summary>
        /// <param name="id_persona">identificador de una persona</param>
        /// <returns>Lista Evaluacion</returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Evaluaciones_Persona(int id_persona)
        {
            List<Evaluacion> evaluaciones= await _context.Evaluacion.Where(e => e.Id_Persona == id_persona).ToListAsync();
            return Ok(evaluaciones);
        }

        /// <summary>
        /// es la evaluación con la que se aprobó el crédito en cuestión
        /// </summary>
        /// <param name="id_credito">identificador de un credito</param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Evaluacion_Credito(int id_credito)
        {
            List<Evaluacion> evaluaciones = await _context.Evaluacion.Where(e => e.Id_Credito == id_credito).ToListAsync();
            return Ok(evaluaciones);
        }

        /// <summary>
        /// editar la evaluacion tecnica
        /// </summary>
        /// <param name="evaluacion">todo el objeto de evaluación</param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPut]
        public async Task<IActionResult> Modificar_Evaluacion([FromBody]Evaluacion evaluacion)
        {
            if (ModelState.IsValid)
            {
                evaluacion.Venta_Semanal_Bueno = evaluacion.No_Dias_Buenos * evaluacion.Venta_Diaria_Bueno;
                evaluacion.Venta_Semanal_Malo = evaluacion.No_Dias_Malos * evaluacion.Venta_Diaria_Mala;

                evaluacion.Venta_Mensual_Bueno = evaluacion.Venta_Semanal_Bueno * 4;
                evaluacion.Venta_Mensual_Malo = evaluacion.Venta_Semanal_Malo * 4;

                _context.Entry(evaluacion).State=EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok();
            }
            return NoContent();
        }

        /// <summary>
        /// Detalle de la evaluación por su identificador
        /// </summary>
        /// <param name="id_evaluacion"></param>
        /// <returns>Evaluacion_Tecnica_Vista</returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Detalle_Evaluacion_Tecnica(int id_evaluacion)
        {
            Evaluacion evaluacion = await Evaluacion(id_evaluacion);
            Evaluacion_Tecnica_Vista vista = new Evaluacion_Tecnica_Vista(evaluacion);
            return Ok(vista);
        }

        /// <summary>
        /// solo muestra la evaluación sin sus tablas hijas
        /// </summary>
        /// <param name="id_evaluacion"></param>
        /// <returns>Evaluacion</returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Detalle_Evaluacion(int id_evaluacion)
        {
            Evaluacion evaluacion = await _context.Evaluacion.FirstOrDefaultAsync(t=>t.Id_Evaluacion==id_evaluacion);
            return Ok(evaluacion);
        }

        private async Task<Evaluacion> Evaluacion(int id_evaluacion)
        {
            return await _context.Evaluacion
                .Include(e => e.Persona)
                .Include(g => g.Gastos)
                .Include(o => o.Otros_Ingresos)
                .Include(p => p.Prestamos)
                .Include(c => c.Cuentas_Cobrar)
                .Include(i => i.Inventarios_Negocio)
                .Include(c => c.Costos_Ventas)
                .Include(a => a.Activos_Negocio)
                .Include("Gastos.Tipo_Gasto.Categoria_Gasto")
                .Include("Gastos.Tipo_Gasto")
                .Include("Otros_Ingresos.Tipo_Ingreso")

                .FirstOrDefaultAsync(e => e.Id_Evaluacion == id_evaluacion);
        }

        private async Task<List<Historial_Crediticio_Interno>> Creditos_Historial(int id_persona)
        {
            List<Credito> creditos = await _context.Credito.AsNoTracking()
                .Include(p=>p.Periodo)
                .Include(q => q.Transacciones_Creditos).Where(t => t.Id_Persona == id_persona && t.Liquidado).ToListAsync();
            List<Historial_Crediticio_Interno> historial = new List<Historial_Crediticio_Interno>();
            foreach (Credito credito in creditos)
            {
                decimal saldo = 0.00M;
                int dias_mora = 0;
                int cuota_mora = 0;

                if (credito.Transacciones_Creditos.Any())
                {
                    saldo = credito.Transacciones_Creditos.Where(f => !f.Borrado_Logico).Min(t => t.Saldo_Capital);
                    dias_mora = credito.Transacciones_Creditos.Sum(g => g.Dias);
                    cuota_mora = credito.Transacciones_Creditos.Where(t => t.Vencido == true).Count();
                }

                historial.Add( new Historial_Crediticio_Interno
                {
                    Codigo=credito.Codigo_Credito,
                    Fecha_Apertura=credito.Liquidado_En,
                    Monto=credito.Monto_pago,
                    Saldo= saldo,/*información de las transacciones*/
                    Plazo=credito.Plazo,
                    Forma_Pago=credito.Periodo.Descripcion,
                    Cuota=credito.Valor_Cuota,
                    Dias_Mora=dias_mora,/*información de las transacciones*/
                    Cuota_Mora= cuota_mora,/*información de las transacciones*/
                    Cuota_Dia=0,/*información de las transacciones*/
                    Cancelado=credito.Cancelado,
                    Cancelado_En=credito.Cancelado_En,
                    Vencido=credito.Vencido,
                    Vencido_En=credito.Vencido_En
                });
            }
            return historial;
        }

        /// <summary>
        /// muestra la vista de la calificación
        /// </summary>
        /// <returns>Vista de la calificación</returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Calificacion(int id_evaluacion)
        {
            Evaluacion evaluacion =await Evaluacion(id_evaluacion);
            List<Historial_Crediticio_Interno> historial=await Creditos_Historial(evaluacion.Id_Persona);
            Evaluacion_Tecnica_Vista evaluacion_tecnica = new Evaluacion_Tecnica_Vista(evaluacion);
            Calificacion_Evaluacion calificacion =new Calificacion_Evaluacion();
            Calificacion_Vista vista = calificacion.Calificar(evaluacion_tecnica);
            vista.Historial = historial;
            vista.Persona = evaluacion.Persona;
            return Ok(vista);
        }


        /*manejo de los detalles de la evaluación técnica*/

        #region Gastos
        /// <summary>
        /// Guarda los gatos de una evaluación técnica
        /// </summary>
        /// <param name="gasto">Objeto completo de gastos</param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> Nuevo_Gasto([FromBody]Gasto gasto)
        {
            if (!_context.Gastos
                .Any(w => w.Id_Evaluacion == gasto.Id_Evaluacion &&
                        w.Id_Tipo_Gasto == gasto.Id_Tipo_Gasto &&
                        w.Valor==gasto.Valor))
            {
                _context.Gastos.Add(gasto);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return NoContent();
        }

        /// <summary>
        /// Modificar un gasto especifico
        /// </summary>
        /// <param name="gasto">Todo el objeto de gasto</param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPut]
        public async Task<IActionResult> Modificar_Gasto([FromBody]Gasto gasto)
        {
            if (_context.Gastos.Any(e =>e.Id_Gasto==gasto.Id_Gasto))
            {
                _context.Entry(gasto).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok();
            }
            return NoContent();
        }
        /// <summary>
        /// detalle del gasto
        /// </summary>
        /// <param name="id_gasto">int</param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Detalle_Gasto(int id_gasto)
        {
            Gasto gasto = await _context.Gastos.FirstOrDefaultAsync(r=>r.Id_Gasto==id_gasto);
            return Ok(gasto);
        }
        #endregion

        #region Otros_Ingresos

        /// <summary>
        /// Guarda los gastos de una evaluación técnica
        /// </summary>
        /// <param name="ingreso">Objeto completo de otro ingreso</param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> Nuevo_Otro_Ingreso([FromBody]Otro_Ingreso ingreso)
        {
            if (!_context.Otro_Ingreso
                .Any(w => w.Id_Evaluacion == ingreso.Id_Evaluacion &&
                        w.Id_Tipo_Ingreso == ingreso.Id_Tipo_Ingreso &&
                        w.Valor == ingreso.Valor))
            {
                _context.Otro_Ingreso.Add(ingreso);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return NoContent();
        }

        /// <summary>
        /// Modificar un ingreso especifico
        /// </summary>
        /// <param name="ingreso">Todo el objeto de ingreso</param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPut]
        public async Task<IActionResult> Modificar_Otro_Ingreso([FromBody]Otro_Ingreso ingreso)
        {
            if (_context.Otro_Ingreso.Any(e => e.Id_Otro_Ingreso == ingreso.Id_Otro_Ingreso))
            {
                _context.Entry(ingreso).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok();
            }
            return NoContent();
        }
        /// <summary>
        /// detalle del otro ingreso
        /// </summary>
        /// <param name="id_otro_ingreso">int</param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Detalle_Otro_Ingreso(int id_otro_ingreso)
        {
            Otro_Ingreso ingreso = await _context.Otro_Ingreso.FirstOrDefaultAsync(r => r.Id_Otro_Ingreso == id_otro_ingreso);
            return Ok(ingreso);
        }
        #endregion

        #region Prestamos

        /// <summary>
        /// Guarda los prestamos de una evaluación técnica
        /// </summary>
        /// <param name="prestamo">Objeto completo de los prestamos</param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> Nuevo_Prestamo([FromBody]Prestamo prestamo)
        {
            if (!_context.Prestamo
                .Any(w => w.Id_Evaluacion == prestamo.Id_Evaluacion &&
                        w.Id_Prestamo == prestamo.Id_Prestamo &&
                        w.Monto_Otorgado == prestamo.Monto_Otorgado &&
                        w.Saldo==prestamo.Saldo &&
                        w.Frecuencia==prestamo.Frecuencia &&
                        w.Cuota==prestamo.Cuota))
            {
                _context.Prestamo.Add(prestamo);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return NoContent();
        }

        /// <summary>
        /// Modificar un prestamo especifico
        /// </summary>
        /// <param name="prestamo">Todo el objeto de prestamo</param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPut]
        public async Task<IActionResult> Modificar_Prestamo([FromBody]Prestamo prestamo)
        {
            if (_context.Prestamo.Any(e => e.Id_Prestamo == prestamo.Id_Prestamo))
            {
                _context.Entry(prestamo).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok();
            }
            return NoContent();
        }

        /// <summary>
        /// detalle del prestamo
        /// </summary>
        /// <param name="id_prestamo">int</param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Detalle_Prestamo(int id_prestamo)
        {
            Prestamo prestamo = await _context.Prestamo.FirstOrDefaultAsync(r => r.Id_Prestamo == id_prestamo);
            return Ok(prestamo);
        }
        #endregion

        #region Cuentas_cobrar
        /// <summary>
        /// Guarda las cuentas por cobrar de una evaluación técnica
        /// </summary>
        /// <param name="cxc">Objeto completo de cuentas por cobrar</param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> Nueva_Cuenta_Cobrar([FromBody]Cuenta_Cobrar cxc)
        {
            if (!_context.Cuenta_Cobrar
                .Any(w => w.Id_Evaluacion == cxc.Id_Evaluacion &&
                        w.Recuperado == cxc.Recuperado &&
                        w.Completo == cxc.Completo))
            {
                _context.Cuenta_Cobrar.Add(cxc);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return NoContent();
        }

        /// <summary>
        /// Modificar un cuentas por cobrar especifico
        /// </summary>
        /// <param name="cxc">Todo el objeto de cuentas por cobrar</param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPut]
        public async Task<IActionResult> Modificar_Cuenta_Cobrar([FromBody]Cuenta_Cobrar cxc)
        {
            if (_context.Cuenta_Cobrar.Any(e => e.Id_Cuenta_Cobrar == cxc.Id_Cuenta_Cobrar))
            {
                _context.Entry(cxc).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok();
            }
            return NoContent();
        }
        /// <summary>
        /// detalle del cuenta_cobrar
        /// </summary>
        /// <param name="id_cuenta_cobrar">int</param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Detalle_Cuenta_Cobrar(int id_cuenta_cobrar)
        {
            Cuenta_Cobrar cuenta_cobrar = await _context.Cuenta_Cobrar.FirstOrDefaultAsync(r => r.Id_Cuenta_Cobrar == id_cuenta_cobrar);
            return Ok(cuenta_cobrar);
        }
        #endregion

        #region Inventarios
        /// <summary>
        /// Guarda los inventarios de una evaluación técnica
        /// </summary>
        /// <param name="inventario">Objeto completo de inventario</param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> Nuevo_Inventario([FromBody]Inventario_Negocio inventario)
        {
            if (!_context.Inventario_Negocio
                .Any(w => w.Id_Evaluacion == inventario.Id_Evaluacion &&
                        w.Unidad_Compra == inventario.Unidad_Compra &&
                        w.Precio_Compra == inventario.Precio_Compra &&
                        w.Cantidad==inventario.Cantidad &&
                        w.Detalle==inventario.Detalle &&
                        w.Total==inventario.Total))
            {
                _context.Inventario_Negocio.Add(inventario);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return NoContent();
        }

        /// <summary>
        /// Modificar un inventario especifico
        /// </summary>
        /// <param name="inventario">Todo el objeto de inventario</param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPut]
        public async Task<IActionResult> Modificar_Inventario([FromBody]Inventario_Negocio inventario)
        {
            if (_context.Inventario_Negocio.Any(e => e.Id_Inventario_Negocio == inventario.Id_Inventario_Negocio))
            {
                _context.Entry(inventario).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok();
            }
            return NoContent();
        }
        /// <summary>
        /// detalle del inventario
        /// </summary>
        /// <param name="id_inventario">int</param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Detalle_Inventario(int id_inventario)
        {
            Inventario_Negocio inventario = await _context.Inventario_Negocio.FirstOrDefaultAsync(r => r.Id_Inventario_Negocio == id_inventario);
            return Ok(inventario);
        }
        #endregion

        #region Costos_Ventas
        /// <summary>
        /// Guarda los costos de ventas por cobrar de una evaluación técnica
        /// </summary>
        /// <param name="costo_venta">Objeto completo de costo de venta</param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> Nuevo_Costo_Venta([FromBody]Costo_Venta costo_venta)
        {
            if (!_context.Costo_Venta
                .Any(w => w.Id_Evaluacion == costo_venta.Id_Evaluacion &&
                        w.Precio_Compra == costo_venta.Precio_Compra &&
                        w.Precio_Venta == costo_venta.Precio_Venta &&
                        w.Unidad_Medida == costo_venta.Unidad_Medida))
            {

                costo_venta.Margen_Bruto = costo_venta.Precio_Venta - costo_venta.Precio_Compra;
                costo_venta.Margen_Bruto_Porcentaje = costo_venta.Margen_Bruto/100;
                
                _context.Costo_Venta.Add(costo_venta);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return NoContent();
        }

        /// <summary>
        /// Modificar un costo_venta especifico
        /// </summary>
        /// <param name="costo_venta">Todo el objeto de costo de venta</param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPut]
        public async Task<IActionResult> Modificar_Costo_venta([FromBody]Costo_Venta costo_venta)
        {
            if (_context.Costo_Venta.Any(e => e.Id_Costo_Venta == costo_venta.Id_Costo_Venta))
            {
                _context.Entry(costo_venta).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok();
            }
            return NoContent();
        }
        /// <summary>
        /// detalle del costo_venta
        /// </summary>
        /// <param name="id_costo_venta">int</param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Detalle_Costo_Venta(int id_costo_venta)
        {
            Costo_Venta costo_venta = await _context.Costo_Venta.FirstOrDefaultAsync(r => r.Id_Costo_Venta == id_costo_venta);
            return Ok(costo_venta);
        }
        #endregion

        #region Activos_Negocio
        /// <summary>
        /// Guarda los activos de negocio de una evaluación técnica
        /// </summary>
        /// <param name="activo_negocio">Objeto completo de activo</param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> Nuevo_Activo_Negocio([FromBody]Activo_Negocio activo_negocio)
        {
            if (!_context.Activo_Negocio
                .Any(w => w.Id_Evaluacion == activo_negocio.Id_Evaluacion &&
                        w.Nombre == activo_negocio.Nombre &&
                        w.Descripcion == activo_negocio.Descripcion &&
                        w.Estado == activo_negocio.Estado &&
                        w.Precio_Compra == activo_negocio.Precio_Compra &&
                        w.Precio_Peritado == activo_negocio.Precio_Peritado))
            {
                _context.Activo_Negocio.Add(activo_negocio);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return NoContent();
        }

        /// <summary>
        /// Modificar un activo de negocio especifico
        /// </summary>
        /// <param name="activo_negocio">Todo el objeto de activo de negocio</param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPut]
        public async Task<IActionResult> Modificar_Activo_Negocio([FromBody]Activo_Negocio activo_negocio)
        {
            if (_context.Activo_Negocio.Any(e => e.Id_Activo_Negocio == activo_negocio.Id_Activo_Negocio))
            {
                _context.Entry(activo_negocio).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok();
            }
            return NoContent();
        }
        /// <summary>
        /// detalle del activo_negocio
        /// </summary>
        /// <param name="id_activo_negocio">int</param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Detalle_Activo_Negocio(int id_activo_negocio)
        {
            Activo_Negocio activo_negocio = await _context.Activo_Negocio.FirstOrDefaultAsync(r => r.Id_Activo_Negocio == id_activo_negocio);
            return Ok(activo_negocio);
        }
        #endregion

        #region Tipo_Gasto
        /// <summary>
        /// para catalogo de tipos de gastos
        /// sean de negocio o familiar
        /// </summary>
        /// <param name="id_categoria_gasto">int</param>
        /// <returns>List de Tipo_Gasto</returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Tipos_Gastos(int id_categoria_gasto)
        {
            List<Tipo_Gasto> tipos_gastos =await _context.Tipo_Gasto.Where(t=>t.Id_Categoria_Gasto== id_categoria_gasto).ToListAsync();
            return Ok(tipos_gastos);
        }
        #endregion
        #region Tipo_Otro_Ingreso
        /// <summary>
        /// para catalogo de tipos de otro ingresos
        /// </summary>
        /// <returns>List de Tipo_Ingreso</returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Tipos_Otro_Ingreso()
        {
            List<Tipo_Ingreso> tipos_otro_ingreso = await _context.Tipo_Ingreso.ToListAsync();
            return Ok(tipos_otro_ingreso);
        }
        #endregion
    }
}