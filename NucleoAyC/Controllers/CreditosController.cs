using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.CompilerServices;
using ModelNucleo.CalculosYProceos;
using ModelNucleo.Model;
using ModelNucleo.Model.Contabilidad;
using ModelNucleo.Model.Interfaces;
using ModelNucleo.Model.ModelView;
using NucleoAyC.Entities;
using NucleoAyC.Helper;

namespace NucleoAyC.Controllers
{
    /// <summary>
    /// Manejo de los creditos
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = "Creditos")]
    public class CreditosController : Base_TransaccionController
    {
        /// <summary>
        /// Constructor de la clase de Crédito
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userManager"></param>
        /// <param name="roleManager"></param>
        public CreditosController(NegocioContexto context,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
            : base(context, userManager, roleManager)
        {
        }

        /// <summary>
        /// Get Se obtiene la lista de crédito por persona
        /// </summary>
        /// <returns>Lista Crédito</returns>
        // GET: api/Creditos
        [Authorize(Roles = "Asesor, Asistente,Jefe,Administrador,Gerente", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> GetCreditosPorPersona(int id_persona)
        {
            try
            {
                List<Credito> creditos = await _context.Credito
                    .Include(a=>a.Linea_Credito_Historial)
                    .Where(g => g.Id_Persona == id_persona).ToListAsync();
                return Ok(creditos);
            } catch (Exception ex)
            {
                return BadRequest(ex.StackTrace);
            }
        }

        /// <summary>
        /// componente solo los que no se han
        /// cancelado o liquidado
        /// </summary>
        /// <param name="id_persona"></param>
        /// <param name="cancelado"></param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Administrador,Gerente", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> ComponentesCreditosPorPersona(int id_persona,bool cancelado=false)
        {
            try
            {
                List<Credito_Componentes> creditos = await _context
                    .Credito
                    .AsNoTracking()
                    .Where(g => g.Id_Persona == id_persona && g.Cancelado==false && g.Liquidado)
                    .Select(d => new Credito_Componentes
                    {
                        Id_Credito = d.Id_Credito,
                        Codigo_Credito = d.Codigo_Credito,
                        Con_Ahorro = d.Credito_Con_Ahorro
                    }).ToListAsync();

                return Ok(creditos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace);
            }
        }
        /// <summary>
        /// Get Lista de créditos por acta dada
        /// </summary>
        /// <param name="id_acta">int id de acta</param>
        /// <returns>Lista de Credito</returns>
        [Authorize(Roles = "Asistente,Jefe,Administrador,Gerente", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public IEnumerable<Credito> CreditosPorActa(int id_acta)
        {
            return _context.Credito.Where(c => c.Id_Acta == id_acta);
        }

        /// <summary>
        /// muestra los créditos
        /// por rango de fecha y agencia
        /// </summary>
        /// <param name="inicio"></param>
        /// <param name="fin"></param>
        /// <param name="id_agencia"></param>
        /// <param name="id_comportamiento"></param>
        /// <returns></returns>
        [Authorize(Roles = "Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Creditos_Colocados(
            DateTime inicio,
            DateTime fin,
            int id_agencia,
            int id_comportamiento)
        {
               List<Creditos_Colocados> colocacion =await Obtener_Creditos_Colocados(
                   inicio,
                   fin,
                   id_agencia,
                   id_comportamiento,
                   DateTime.MinValue,
                   true);

               return Ok(colocacion);
        }

        /// <summary>
        /// lista de comportamiento
        /// del crédito
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Comportamiento_Credito()
        {
            List<Comportamiento_Credito> comportamientos = await _context
                .Comportamiento_Credito
                .AsNoTracking().ToListAsync();
            return Ok(comportamientos);
        }


        /// <summary>
        /// Muestra los creditos
        /// </summary>
        /// <param name="id_agencia">int</param>
        /// <returns>Creditos</returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Administrador,Gerente", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Creditos(int id_agencia = 0)
        {
            var userId = _userManager.GetUserId(HttpContext.User);

            List<Credito> creditos = null;
            if (User.IsInRole("Gerente") || User.IsInRole("Jefe"))
                creditos = await FiltrarCreditosPorAgencia(userId);

            if (User.IsInRole("Administrador"))
                creditos = await FiltrarCreditosPorAgencia(userId, id_agencia);/*solo la agencia*/

            if (creditos != null)
                return Ok(creditos);
            return NoContent();
        }

        private async Task<List<Credito>> FiltrarCreditosPorAgencia(string userId = null, int id_agencia = 0)
        {
            if (userId != null && id_agencia == 0)
                id_agencia = _context.Usuario_Agencia.AsNoTracking().SingleOrDefault(a => a.Id_Usuario == userId).Id_Agencia;
            List<string> id_usuarios = _context.Usuario_Agencia.AsNoTracking().Where(c => c.Id_Agencia == id_agencia).Select(q => q.Id_Usuario).ToList();
            List<Credito> creditosAgencia = new List<Credito>();

            foreach (string id_usuario in id_usuarios)
            {
                /*se verifica si es asesor para obtener su cartera*/
                IdentityUser identidad = await _userManager.FindByNameAsync(id_usuario);
                if (await _userManager.IsInRoleAsync(identidad, "Asesor"))
                {
                    List<Credito> creditos = await FiltrarCreditosPorCartera(id_usuario);
                    if (creditos != null)
                        creditosAgencia.AddRange(creditos);
                }
            }
            return creditosAgencia;
        }

        private async Task<List<Credito>> FiltrarCreditosPorCartera(string id_usuario)
        {
            if (_context.Cartera.AsNoTracking().Any(d => d.Id_Usuario == id_usuario))
            {
                Cartera cartera = await _context.Cartera.AsNoTracking().SingleAsync(d => d.Id_Usuario == id_usuario);/*se filtra por la cartera del usuario que lo está manejando*/
                List<Credito> creditos = _context.Credito
                    .AsNoTracking()
                    .Include(r => r.Persona)
                    .Where(b => b.Persona.Id_Cartera == cartera.Id_Cartera)
                    .ToList(); //creditos.Where(b => b.Persona.Id_Cartera == cartera.Id_Cartera).ToList();
                return creditos;
            }
            else {
                return null;
            }
        }

        /// <summary>
        /// Get Se obtiene detalle de un credito a partir de la persona
        /// y se obtienen todos los creditos paginados
        /// </summary>
        /// <param name="id_persona">int</param>
        /// <param name="id_credito">int id del crédito</param>
        /// <param name="paginar">bool</param>
        /// <param name="siguiente">bool</param>
        /// <returns>Detalle del crédito</returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Administrador,Gerente", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Detalle_Credito_Paginado(int id_persona, int id_credito = 0, bool paginar = false, bool siguiente = true)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            List<Credito> creditos = creditos = await _context.Credito
                    .Include(h => h.Transacciones_Creditos)//borrado logico no considerarlo ver en la aplicacion cliente
                    .Include(f => f.Persona)
                    .Include(a => a.Linea_Credito_Historial)
                    .Include(c => c.Comites)
                    .Include(a => a.Persona.Evaluaciones)
                    .AsNoTracking()
                    .Where(d => d.Id_Persona == id_persona).ToListAsync();
            
            Linea_Credito_Historial lineaCreditoHistorial = await _context.Linea_Credito_Historial.AsNoTracking().SingleAsync(a => a.Id_Credito == id_credito);
            Credito cr = await _context.Credito.AsNoTracking().SingleAsync(a => a.Id_Credito == id_credito);
            Cartera cartera = await _context.Cartera.AsNoTracking().SingleAsync(a => a.Id_Cartera == cr.Id_Cartera);

            if (creditos == null)
                return BadRequest("La persona consultada no posee registros");

            Credito credito_consultado = null;
            if (id_credito > 0)
                credito_consultado = creditos.FirstOrDefault(d => d.Id_Credito == id_credito);
            else
                credito_consultado = creditos.FirstOrDefault();

            Control_Navegacion control_Navegacion = new Control_Navegacion();
            Credito credito = control_Navegacion.Paginar(creditos, credito_consultado, paginar, siguiente);
            credito.Cartera = cartera;
            credito.Linea_Credito_Historial = lineaCreditoHistorial;
            return Ok(credito);
        }
        /// <summary>
        /// se obtiene el detalle por su id
        /// </summary>
        /// <param name="id_credito">int</param>
        /// <returns>Credito</returns>
       // [Authorize(Roles = "Asesor, Asistente,Jefe,Administrador,Gerente", AuthenticationSchemes = "Bearer")]

        [HttpGet]
        public async Task<IActionResult> Detalle_Credito(int id_credito, bool es_comite = false)
        {
            Credito credito = new Credito();
            if (es_comite)
            {
                credito = await _context.Credito
                                .Include(f => f.Persona)
                                .Include(a => a.Linea_Credito_Historial)
                                .Include(c => c.Comites)
                                .Include(a => a.Persona.Evaluaciones)
                                .Include(h => h.Transacciones_Creditos)
                                .Include(p => p.Periodo)
                                .Include(p => p.Comportamiento_Credito)
                                .Include(a=>a.Linea_Credito)
                                .AsNoTracking()
                                .SingleOrDefaultAsync(d => d.Id_Credito == id_credito && !d.Revertido && !d.Aprobado);
            }
            else
            {
                credito = await _context.Credito
                              .Include(f => f.Persona)
                              .Include(a => a.Linea_Credito_Historial)
                              .Include(c => c.Comites)
                              .Include(a => a.Persona.Evaluaciones)
                              .Include(h => h.Transacciones_Creditos)
                              .Include(p => p.Periodo)
                               .Include(a => a.Linea_Credito)
                              .Include(p => p.Comportamiento_Credito)
                              .AsNoTracking()
                              .SingleOrDefaultAsync(d => d.Id_Credito == id_credito);
            }
           
            if (credito != null)
                return Ok(credito);
            return NoContent();
        }

        /// <summary>
        /// Post Para modificar un crédito por su id
        /// </summary>
        /// <param name="credito">Credito objeto json</param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Administrador,Gerente", AuthenticationSchemes = "Bearer")]
        [HttpPut]
        public async Task<IActionResult> Modificar([FromBody] Credito credito)
        {
            /*se debe verificar que el credito no este activo*/
            Credito credito_verificado = await _context.Credito.
                AsNoTracking()
                .SingleAsync(f => f.Id_Credito == credito.Id_Credito);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                //se quito para mientras
                //bool verificar_codigo = _context.Credito.Any(d=>d.Codigo_Credito==credito.Codigo_Credito && !d.Revertido);
                //if (verificar_codigo)
                //    return BadRequest("Ya existe un código igual, ingrese uno diferente");
                if (!credito_verificado.Liquidado)
                {
                    _context.Update(credito).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    /*regenerar la amortización por si cambiaron datos*/
                    await BorrarAmortizacionPorCredito(credito.Id_Credito);/*borrar la amortización anterior*/
                    await GenerarAmortizacionPorCredito(credito)/*generarla de nuevo*/;
                    return Ok("El registro se ha guardado");
                }
                else
                    return BadRequest("No se puede editar un crédito ya liquidado");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace);
            }
        }

        /// <summary>
        /// Put Colocar id_acta para el id_credito seleccionado
        /// </summary>
        /// <param name="id_acta">int id de acta</param>
        /// <param name="id_credito">int id de crédito</param>
        /// <returns></returns>
        [Authorize(Roles = "Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPut]
        public async Task<IActionResult> AgregarAActa(int id_acta, int id_credito)
        {
            Credito credito = await _context.Credito.SingleAsync(f => f.Id_Credito == id_credito);
            if (
                    credito != null &&
                    credito.Id_Acta == null &&
                    id_acta > 0 &&
                    id_credito > 0
                )
            {
                credito.Id_Acta = id_acta;
                _context.Entry(credito).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok("Acta agregada correctamente");
            }
            else
                return BadRequest("El acta no se pudo crear");
        }

        private async Task Agregar_Acta_Unica(NegocioContexto contexto, int id_credito)
        {
            /*verificar si existe acta*/
            DateTime fecha = DateTime.Now;
            Acta acta = contexto.Acta.SingleOrDefault(
                    y => y.Fecha_Celebrado.Year == fecha.Year &&
                    y.Fecha_Celebrado.Month == fecha.Month &&
                    y.Fecha_Celebrado.Day == fecha.Day);
            Credito credito = contexto.Credito.SingleOrDefault(g => g.Id_Credito == id_credito);

            if (acta != null && credito != null)
            {
                credito.Id_Acta = acta.Id_Acta;
                contexto.Entry(credito).State = EntityState.Modified;
            }
            else {
                /*crear el acta correspondiente al dia actual*/
                Acta acta_nueva = new Acta {
                    Fecha_Celebrado = fecha
                };
                contexto.Acta.Add(acta_nueva).State = EntityState.Added;
            }
            await contexto.SaveChangesAsync();
        }

        /// <summary>
        /// método loco para generar amortización a todos
        /// </summary>
        /// <param name="id_agencia"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GenerarAmortizacionesPorAgencia(int id_agencia)
        {
            try
            {
                var creditos = (from c in _context.Credito
                                join p in _context.Personas on c.Id_Persona equals p.Id_Persona
                                join car in _context.Cartera on p.Id_Cartera equals car.Id_Cartera
                                join ua in _context.Usuario_Agencia on car.Id_Usuario equals ua.Id_Usuario
                                where ua.Id_Agencia == id_agencia
                                select c).ToList();
                int creditos_amortizados = 0;
                foreach (var credito in creditos)
                {
                    await GenerarAmortizacionPorCredito(credito.Id_Credito);
                    creditos_amortizados++;
                }
                /*mostrar la verificación*/
                int creditos_encontrados = creditos.Count();
                string respuesta = string.Format("Encontrados:{0}, Amortizados:{1}", creditos_encontrados, creditos_amortizados);
                return Ok(respuesta);
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// método loco para generar amortización a todos
        /// </summary>
        /// <param name="id_linea"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GenerarAmortizacionesPorLinea(int id_linea)
        {
            try
            {
                var creditos = (from c in _context.Credito
                                join l in _context.Linea_Credito on c.Id_Linea equals l.Id_Linea
                                where l.Id_Linea == id_linea
                                select c).ToList();
                int creditos_amortizados = 0;
                foreach (var credito in creditos)
                {
                    await GenerarAmortizacionPorCredito(credito.Id_Credito);
                    creditos_amortizados++;
                }
                /*mostrar la verificación*/
                int creditos_encontrados = creditos.Count();
                string respuesta = string.Format("Encontrados:{0}, Amortizados:{1}", creditos_encontrados, creditos_amortizados);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /*volver generar amortizacion individual*/
        /// <summary>
        /// generar amortización por el crédito
        /// </summary>
        /// <param name="id_credito"></param>
        /// <returns></returns>
       // [Authorize(Roles = "Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")] 12327
        [HttpGet]
        public async Task<IActionResult> GenerarAmortizacionPorCredito(int id_credito)
        {
            if (_context.Credito.Any(d => d.Id_Credito == id_credito))
            {
                Credito credito = _context.Credito.Single(d => d.Id_Credito == id_credito);
                bool borrado = await BorrarAmortizacionPorCredito(id_credito);
                if (borrado)
                    await GenerarAmortizacionPorCredito(credito);
                return Ok("Se volvió a generar la amortización");
            }
            else
                return BadRequest("No se encontró el el crédito que necesita");
        }

        /// <summary>
        /// Generar amortización individual por crédito
        /// </summary>
        /// <param name="credito"></param>
        /// <returns></returns>
        private async Task GenerarAmortizacionPorCredito(Credito credito)
        {
            if (!_context.Amortizacion.Any(d => d.Id_Credito == credito.Id_Credito))
            {
                /*tratar de guardar la amortización*/
                var resultado = CuotaPrimaria(credito.Id_Linea, credito.Monto_pago, credito.Id_Periodo, credito.Plazo, credito.Id_Credito, generar_amortizacion: true);
                List<Amortizacion> amortizaciones = resultado.Amortizacion.OrderBy(d => d.No).ToList();
                foreach (var amortizacion in amortizaciones)
                    amortizacion.Id_Credito = credito.Id_Credito;
                _context.Amortizacion.AddRange(amortizaciones);
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_credito"></param>
        /// <returns></returns>
        private async Task<bool> BorrarAmortizacionPorCredito(int id_credito)
        {
            try
            {
                var borrarse = _context.Amortizacion.Where(c => c.Id_Credito == id_credito);
                _context.RemoveRange(borrarse);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }



        /// <summary>
        /// Post Para crear un crédito para una persona
        /// </summary>
        /// <param name="credito">Credito objeto json</param>
        /// <returns>Si fue satisfactorio o no</returns>
      //  [Authorize(Roles = "Asistente,Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> NuevoCredito([FromBody] Credito credito)
        {
            using (var dbContextTransaccion = _context.Database.BeginTransaction())
            {
                if (credito.Plazo ==0 || credito.Numero_cuotas ==0 || credito.Cuota_Total ==0)
                {
                    return BadRequest("Calcule el monto del crédito");
                }
                else
                {
                    try
                    {
                        /*Nio encontré créditos que faltan que le pongan plazo y le dan clic a guardar sin darle clic a calcular*/
                        if (!ModelState.IsValid && credito.Plazo == 0 && credito.Numero_cuotas == 0 && credito.Cuota_Total == 0)
                        {
                            return BadRequest(ModelState);
                        }

                        /*armar el codigo para el crédito*/
                        int id_credito_ultimo = 0;
                        if (_context.Credito.Any())
                            id_credito_ultimo = _context.Credito.AsEnumerable().Last().Id_Credito;
#if RELEASE || DEBUG
                        credito.Codigo_Credito = GeneradorCodigo.Generar(credito.Id_Credito, credito.Id_Persona);
#endif
#if SOLOTEST
                    credito.Codigo_Credito = GenerarCodigo(credito.Id_Persona,credito.Id_Linea,true);
#endif
                        int id_cartera = _context.Personas.SingleOrDefault(y => y.Id_Persona == credito.Id_Persona).Id_Cartera;
                        credito.Id_Cartera = id_cartera;
                        credito.Id_Credito = id_credito_ultimo + 1;
                        credito.Id_Linea_Credito_Historial = 0;
                        _context.Credito.Add(credito);

                        Linea_Credito lc = _context
                            .Linea_Credito
                            .FirstOrDefault(k => k.Id_Linea == credito.Id_Linea);

                        /*se define el interes y plan ayc a partir del periodo que se eligio*/
                        Interes_Periodo_Linea_Credito tablero = await Fijar_Interes_Comision_Periodo(credito.Id_Linea, credito.Id_Periodo);
                        credito.Interes_Comision = tablero.Interes_Comision;
                        credito.Interes = tablero.Interes;

                        /*de la linea de credito al linea_credito_historial*/
                        AutoMapper.MapperConfiguration config = new AutoMapper.MapperConfiguration(cfg => cfg.CreateMap<Linea_Credito, Linea_Credito_Historial>());
                        AutoMapper.IMapper mapper = config.CreateMapper();
                        Linea_Credito_Historial linea_historial = mapper.Map<Linea_Credito_Historial>(lc);
                        linea_historial.Id_Credito = credito.Id_Credito;
                        linea_historial.Recargo = lc.Recargo;
                        _context.Linea_Credito_Historial.Add(linea_historial);
                        credito.Id_Linea_Credito_Historial = linea_historial.Id_Linea_Credito_Historial;

                        await _context.SaveChangesAsync();

                        Func<Cobro_Periodico, bool> filtro_cobro_periodico = new Func<Cobro_Periodico, bool>(d => d.Id_Linea_Credito == credito.Linea_Credito.Id_Linea);
                        Obtener_Valores_Historial<Cobro_Periodico, Cobro_Periodico_Historial>(filtro_cobro_periodico, linea_historial, _context);

                        Func<Deduccion_Desembolso, bool> filtro_deducciones_desembolso = new Func<Deduccion_Desembolso, bool>(f => f.Id_Linea_Credito == credito.Id_Linea);
                        Obtener_Valores_Historial<Deduccion_Desembolso, Deduccion_Historial>(filtro_deducciones_desembolso, linea_historial, _context, credito.Monto_pago);

                        /*tratar de guardar la amortización*/
                        var resultado = CuotaPrimaria(lc.Id_Linea,credito.Monto_pago,credito.Id_Periodo,credito.Plazo,generar_amortizacion: true);
                        List<Amortizacion> amortizaciones = resultado.Amortizacion.OrderBy(d => d.No).ToList();
                        foreach (var amortizacion in amortizaciones)
                        {
                            amortizacion.Id_Credito = credito.Id_Credito;
                            _context.Amortizacion.Add(amortizacion).State = EntityState.Added;
                        }
                        await _context.SaveChangesAsync();
                        await Agregar_Acta_Unica(_context, credito.Id_Credito);
                        if (credito.Credito_Con_Ahorro) {
                            await Credito_Con_Ahorro(credito.Id_Credito, credito.Id_Porcentaje_Ahorro,credito.Monto_Credito_Ahorro);
                        }
                        dbContextTransaccion.Commit();
                        //return CreatedAtAction("GetCredito", new { id = credito.Id_Credito }, credito);
                        return Ok("El crédito fue creado correctamente");
                    }
                    catch (DbUpdateException ex)
                    {
                        dbContextTransaccion.Rollback();
                        return BadRequest(ex.Message);
                    }
                }
             
            }
        }
        private async Task Credito_Con_Ahorro(int id_credito,int id_porcentaje, decimal monto_ahorro)
        {
          
                Credito_Con_Ahorro credito_Con_Ahorro = new Credito_Con_Ahorro
                {
                    Id_Credito = id_credito,
                    Monto_Ahorro = monto_ahorro,
                    Id_Porcentaje_Ahorro_Credito = id_porcentaje
                };
                _context.Credito_Con_Ahorros.Add(credito_Con_Ahorro).State = EntityState.Added;
                await _context.SaveChangesAsync();
           
        }
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Porcentaje_Ahorro()
        {
            List<Porcentaje_Ahorro_Credito> porcentajes = await _context.Porcentaje_Ahorro_Creditos.ToListAsync();
            return Ok(porcentajes);
        }
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Detalle_Credito_Con_Ahorro( int id_credito)
        {
            Credito cr = _context.Credito.AsNoTracking().Single(a => a.Id_Credito == id_credito);
            Credito_Con_Ahorro crh = _context.Credito_Con_Ahorros.AsNoTracking().Single(a=>a.Id_Credito == cr.Id_Credito);
            Persona pe = _context.Personas.AsNoTracking().Single(a=>a.Id_Persona == cr.Id_Persona);
            Detalle_Credito_Ahorro det = new Detalle_Credito_Ahorro
            {
                Codigo_Credito = cr.Codigo_Credito,
                Nombre_Cliente = pe.NombreCompleto,
                Porcentaje_Ahorro = crh.Monto_Ahorro / cr.Numero_cuotas
            };
            return Ok(det);
        }
        private async Task<Interes_Periodo_Linea_Credito> Fijar_Interes_Comision_Periodo(int id_linea_credito, int id_periodo)
        {
            Func<Interes_Periodo_Linea_Credito, bool> lamb = new Func<Interes_Periodo_Linea_Credito, bool>(f => f.Id_Linea_Credito == id_linea_credito && f.Id_Periodo == id_periodo);
            bool existe = _context
                .InteresPeriodoLineaCreditos
                .Any(lamb);
            if (existe)
            {
                Interes_Periodo_Linea_Credito tablero = _context
                    .InteresPeriodoLineaCreditos
                    .Single(lamb);
                return await Task.Run(() => tablero);
            }
            return null;
        }

        /*Obtiene la configuracion para guardarse al credito especifico solo para información*/
        private void Obtener_Valores_Historial<T, Y>(Func<T, bool> filtro_fuente, Linea_Credito_Historial linea_historial, NegocioContexto contexto, decimal monto = 0.00M) where T : class where Y : class, IIDentidad_Linea_Credito_Historial
        {
            bool hay_fuente = contexto.Set<T>().Any(filtro_fuente);
            if (hay_fuente)
            {
                List<T> fuente = contexto.Set<T>().Where(filtro_fuente).ToList();
                foreach (T f in fuente)
                {
                    AutoMapper.MapperConfiguration config = new AutoMapper.MapperConfiguration(cfg => cfg.CreateMap<T, Y>());
                    AutoMapper.IMapper mapper = config.CreateMapper();
                    Y destino = mapper.Map<Y>(f);

                    destino.Id_Linea_Credito_Historial = linea_historial.Id_Linea_Credito_Historial;
                    contexto.Add(destino);
                }
            }
        }

        /*calculo y procesamiento de información*/
        #region calculo y procesamiento de información
        /// <summary>
        /// Post Cálculo de la cuota antes de AyC plan e IVA se vale de
        /// la clase CalculosYProcesoCredito
        /// </summary>
        /// <param name="id_linea">id de la línea de crédito</param>
        /// <param name="monto">decimal monto del crédito</param>
        /// <param name="id_periodo">int periodo</param>
        /// /// <param name="plazo">int meses de plazo</param>
        /// <param name="id_credito">int id del crédito</param>
        /// <param name="generar_amortizacion">dice si necesita la amortización</param>
        /// <returns>decimal cuotaprimaria</returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Administrador,Gerente", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public ResultadoCalculoCreditio CuotaPrimaria(
            int id_linea, 
            decimal monto, 
            int id_periodo, 
            int plazo, 
            int id_credito = 0,
            bool generar_amortizacion=false)
        {
            Periodo periodo = _context.Periodo.AsNoTracking().SingleOrDefault(d => d.Id_Periodo == id_periodo);

            if (id_periodo == 1 && id_linea==17)
                periodo = new Periodo { Id_Periodo=0,Valor=20,dias=1,Descripcion="Ágil-Efectivo"};

            /*obtener el dato de la línea de credito*/
            Linea_Credito lc = _context.Linea_Credito.AsNoTracking().Include(f => f.Cobros_Credito).Single(t => t.Id_Linea == id_linea);
            decimal total = lc.Cobros_Credito.Count > 0 ? lc.Cobros_Credito.Sum(v => v.Valor) : 0.00M;
            DateTime fecha_inicio = DateTime.Now;
            //DateTime fecha ;
            bool agil_efectivo = !(id_linea!=17);
            /*traer el iva legislado de las configuraciones*/
            decimal iva = IVA;

            List<Amortizacion> amortizacion_tabla = _context
                    .Amortizacion
                    .AsNoTracking()
                    .OrderBy(d => d.No)
                    .Where(d => d.Id_Credito == id_credito)
                    .ToList();

            if (id_credito > 0)
            {
                Credito credito = _context
                    .Credito
                    .AsNoTracking()
                    .Include(t => t.Linea_Credito_Historial)
                    .Include(s=>s.Linea_Credito)
                    .Include(f => f.Persona)
                    .Include("Linea_Credito_Historial.Cobros_Periodicos_Historial")
                    .SingleOrDefault(t => t.Id_Credito == id_credito);
         
                Linea_Credito_Historial lch = _context
                    .Linea_Credito_Historial
                    .AsNoTracking()
                    .Single(s => s.Id_Credito == credito.Id_Credito);

                total = _context.Cobro_Periodico_Historial.AsNoTracking().Where(a => a.Id_Linea_Credito_Historial == lch.Id_Linea_Credito_Historial).Sum(d => d.Valor);
                DateTime fecha_movible = Verificar_Credito_Fecha_Movible(credito);
                TimeSpan diferencia_dias = fecha_movible - credito.Liquidado_En;
                var resultado = new CalculosYProcesoCredito(
                    lch.Interes_Comision,
                    monto,
                    periodo,
                    iva,
                    plazo,
                    lch.Interes,
                    Verificar_Credito_Fecha_Movible(credito),//se mueve fecha de liquidación
                    total,
                    lch.PlanAyC_Cobros_Periodico_Dinero,
                    0.00M,
                    dias_movible: diferencia_dias.Days,
                    generar_amortizacion
                ).Resultado;

                return new ResultadoCalculoCreditioExtendido
                {
                    CuotaPrimaria = resultado.CuotaPrimaria,
                    cuotaAyC = resultado.cuotaAyC,
                    IVACalculado = resultado.IVACalculado,
                    CuotaTotal = resultado.CuotaTotal,
                    Amortizacion = generar_amortizacion?resultado.Amortizacion:amortizacion_tabla,
                    Cliente = credito.Persona.NombreCompleto,
                    Codigo_Credito = credito.Codigo_Credito
                };

            }

            var otroresultado= new CalculosYProcesoCredito(
                lc.Interes_Comision,
                monto,
                periodo,
                iva,
                plazo,
                lc.Interes,
                fecha_inicio,
                total,
                lc.PlanAyC_Cobros_Periodico_Dinero, 
                0.00M,
                generar_amortizacion: generar_amortizacion).Resultado;
            if(!generar_amortizacion)
                otroresultado.Amortizacion = amortizacion_tabla;
            return otroresultado;
        }

        /// <summary>
        /// Post Retorna el número de cuotas
        /// </summary>
        /// <param name="plazo">int meses plazo</param>
        /// <param name="id_periodo">int frecuencia de pagos</param>
        /// <returns>int número de cuotas</returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Administrador,Gerente", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public decimal ObtenerNumeroCuotas(int plazo, int id_periodo) {
            int frecuencia = _context.Periodo.AsNoTracking().SingleOrDefault(p => p.Id_Periodo == id_periodo).Valor;
            CalculosYProcesoCredito cpc = new CalculosYProcesoCredito();

            return cpc.CalcularNoCuotas(plazo, frecuencia);
        }


        /// <summary>
        /// se ratifica que entrega del dinero por el credito acordado,
        /// para 
        /// </summary>
        /// <param name="id_credito">int id de crédito</param>
        /// <returns>Credito objeto json</returns>
        [Authorize(Roles = "Jefe,Administrador,Asistente", AuthenticationSchemes = "Bearer")]
        [HttpPut]
        public async Task<IActionResult> PantallaCredito(int id_credito)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    string Id_Usuario = _userManager.GetUserId(HttpContext.User);

                    DateTime fecha = DateTime.Now;
                    Credito credito = _context.Credito
                        .Include(p => p.Persona)
                        .Include(c => c.Cartera)
                        .Include(k => k.Linea_Credito_Historial)
                        .Include("Linea_Credito_Historial.Deducciones_Historial")
                        .Include(p => p.Periodo).SingleOrDefault(l => l.Id_Credito == id_credito);

                    Linea_Credito_Historial linea_historial = await _context
                            .Linea_Credito_Historial
                            .Include(c => c.Deducciones_Historial)
                            .SingleAsync(d => d.Id_Credito == credito.Id_Credito);

                    //verificar si tiene ahorro tambien si no está liquidado para hacerlo */
                    if (_context.Ahorro.Any(a => a.Id_Persona == credito.Persona.Id_Persona) && !credito.Liquidado)
                    {
                        await Procesar_Liquidacion(credito, linea_historial, Id_Usuario, fecha);
                        dbContextTransaction.Commit();
                    }
                    ///*termina liquidación del crédito

                    Agencia agencia = _context
                        .Usuario_Agencia
                        .AsNoTracking()
                        .Include(d => d.Agencia)
                        .SingleOrDefault(g => g.Id_Usuario == Id_Usuario)
                        .Agencia;
                    string dia = DateTime.Now.ToString("dd/MM/yyyy");
                    string hora = DateTime.Now.ToString("h:mm:ss tt");
                    string Nombre_Codeudor = "";

                    Func<Persona_Relaciones,bool> filtro_codeudores = new Func<Persona_Relaciones,bool>(s => s.Id_Sujeto == credito.Id_Persona && s.Id_Credito == credito.Id_Credito && s.Activo);

                    if (_context
                       .Persona_Relaciones
                       .Any(filtro_codeudores))
                    {
                        Persona_Relaciones codeudor = _context
                            .Persona_Relaciones
                            .Include(f => f.Predicado)
                            .Single(filtro_codeudores);

                        Nombre_Codeudor = codeudor.Predicado != null ? codeudor.Predicado.NombreCompleto : "--";
                    }
                    string Ciudad = "--";
                        Cartera cartera_asesor = _context.Cartera.AsNoTracking().Single(a=>a.Id_Cartera == credito.Id_Cartera);
                        Usuario_Agencia  usuario_asesor = _context.Usuario_Agencia.AsNoTracking()
                            .Single(b => b.Id_Usuario == cartera_asesor.Id_Usuario);
                        Agencia agencia_liquidacion = _context.Agencia.AsNoTracking()
                            .Single(c => c.Id_Agencia == usuario_asesor.Id_Agencia);
                    
                        Ciudad = agencia_liquidacion.Ciudad;
                        
                    Pantalla_Credito pantalla = new Pantalla_Credito(
                        credito, 
                        Nombre_Codeudor, 
                        Ciudad, 
                        linea_historial);
                    if (credito.Id_Comportamiento_Credito ==5)
                    {
                        pantalla.Liquido_Recibir = 0.00M;
                    }

                    var usuario_agencia = await _context.Usuario_Agencia.AsNoTracking().SingleAsync(d=>d.Id_Usuario==Id_Usuario);

                    pantalla.Creador = !string.IsNullOrEmpty(usuario_agencia.Nombre_Completo) ? usuario_agencia.Nombre_Completo : Id_Usuario;

                    var usuario_nombre =await _context.Usuario_Agencia.AsNoTracking().SingleAsync(d => d.Id_Usuario == credito.Cartera.Id_Usuario);
                    string responsable = !string.IsNullOrEmpty(usuario_nombre.Nombre_Completo) ? usuario_nombre.Nombre_Completo : credito.Cartera.Id_Usuario;
                    pantalla.Asesor = responsable;
                    pantalla.Colector = responsable;

                    pantalla.agencia = agencia;
                    pantalla.dia_ejecucion = dia;
                    pantalla.hora_ejecucion = hora;
                    return Ok(pantalla);
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    return BadRequest(ex.StackTrace);
                }
            }
        }


        /// <summary>
        /// se hace la liquidación de un crédito con todo los procesos
        /// que implican
        /// </summary>
        /// <param name="credito"></param>
        /// <param name="linea_historial"></param>
        /// <param name="id_usuario"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        private async Task Procesar_Liquidacion(
            Credito credito,
            Linea_Credito_Historial linea_historial,
            string id_usuario,
            DateTime fecha)
        {
            Caja caja = await Caja_Actual();
            string linea_ahorro_externo = _context
                .Configuracion_General
                .AsNoTracking()
                .Single(a => a.Nombre == "LINEA_AHORRO_CLIENTE").Valor;
            string linea_ahorro_empleado = _context
                .Configuracion_General
                .AsNoTracking()
                .Single(a => a.Nombre == "LINEA_AHORRO_EMPLEADO").Valor;
            /*si la línea es de empleado sacar otro*/
            int id_linea_ahorro = 0;
            if (credito.Id_Linea == 8)
                id_linea_ahorro = int.Parse(linea_ahorro_empleado);
            else
                id_linea_ahorro = int.Parse(linea_ahorro_externo);

            Ahorro ahorro = new Ahorro();

            var tieneCuentaEmpleado = _context
                .Ahorro
                .Any(
                    a => a.Id_Persona == credito.Persona.Id_Persona && a.Id_Linea_Ahorro == id_linea_ahorro);
            if (tieneCuentaEmpleado)
            {
                 ahorro = _context
                        .Ahorro
                        .FirstOrDefault(
                         a => a.Id_Persona == credito.Persona.Id_Persona && a.Id_Linea_Ahorro == id_linea_ahorro);
            } 
            else
            {
                ahorro = _context
                     .Ahorro
                     .FirstOrDefault(
                     a => a.Id_Persona == credito.Persona.Id_Persona && a.Id_Linea_Ahorro == 03);
            }
            //&& a.Id_Linea_Ahorro == 3);/*esta en duro la linea de ahorro*/
            /*conocer el saldo actual*/
            decimal saldo = 0.00M;
            if (_context.Transaccion_Ahorro.Any(w => w.Id_Ahorro == ahorro.Id_Ahorro))
                saldo = _context
                    .Transaccion_Ahorro
                    .Where(w => w.Id_Ahorro == ahorro.Id_Ahorro && !w.Borrado_Logico)
                    .OrderBy(d => d.Id_Ahorro).AsEnumerable().Last().Saldo;

            /*no se tenía el uso de deducciones si se tiene*/
            /*son deducciones en dinero*/
            Deducibles deducibles = await Armar_Deducciones(credito, linea_historial, caja, id_usuario, fecha);
            decimal capital = credito.Monto_pago;
            decimal capitalSinDeduccion = capital - deducibles.Refinanciamiento;

            /*termina el manejo correcto de las deducciones*/

            /*ver la transacción de ahorro si no es reestructura*/
            if (credito.Id_Comportamiento_Credito !=5)
            {
                await Transferir_Monto_Credito(ahorro.Id_Ahorro, caja.Id_Caja, saldo, capitalSinDeduccion, fecha);
            }
            await Registrar_Desembolso(capital, caja.Id_Caja, credito.Id_Credito, fecha, id_usuario);
            
            if (credito.Id_Linea == 5)
                credito.Fecha_ProntoPago = fecha;
            credito.Liquidado = true;
            credito.Liquidado_En = fecha;
            _context.Entry(credito).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            await GenerarAmortizacionPorCredito(credito.Id_Credito);
            List<Amortizacion> amortizacion =await _context
                .Amortizacion
                .AsNoTracking()
                .Where(d => d.Id_Credito == credito.Id_Credito).ToListAsync();   //Amortizacion(credito.Id_Linea, credito.Monto_pago, credito.Id_Periodo,credito.Plazo, credito.Id_Credito).Amortizacion;
            DateTime fecha_vencimiento = amortizacion.Last().FechaPago;
            credito.Vencido_En = fecha_vencimiento;
            _context.Entry(credito).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// registra desembolso en la caja
        /// </summary>
        /// <param name="resto_capital"></param>
        /// <param name="id_caja"></param>
        /// <param name="id_credito"></param>
        /// <param name="fecha"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        private async Task Registrar_Desembolso(
            decimal resto_capital,
            int id_caja,
            int id_credito,
            DateTime fecha,
            string id_usuario)
        {
            
            Transaccion_Simple transaccion_simple = new Transaccion_Simple
            {
                Id_Tipo_Operacion_Simple = 8,
                Monto = resto_capital,
                Id_Caja = id_caja,
                Id_Credito = id_credito,
                Creado_En = fecha,
                Creado_Por = id_usuario,
            };
            _context.Transaccion_Simple.Add(transaccion_simple).State = EntityState.Added;
            await _context.SaveChangesAsync();
            string no_comprobante_simple = await Generar_No_Comprobante();
            Transaccion_Caja trans_caja_simple = Registrar_Caja(transaccion_simple, id_caja, fecha, false, comprobante: no_comprobante_simple);
            _context.Transaccion_Caja.Add(trans_caja_simple).State = EntityState.Added;
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// muestra el detalle de la 
        /// operación para ser refinanciado
        /// </summary>
        /// <param name="id_credito">crédito nuevo</param>
        /// <param name="codigo_credito">crédito refinnaciable</param>
        /// <param name="movible"></param>
        /// <param name="monto_nuevo_refinanciamiento"></param>
        /// <returns></returns>
        [Authorize(Roles = "Jefe,Administrador,Asistente", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Refinanciable(int id_credito,string codigo_credito, bool movible = false,
            decimal? monto_nuevo_refinanciamiento = null)
        {
            decimal monto_refinanciable = 0.00M;
            DateTime fecha = DateTime.Now;
            Credito credito_nuevo =await _context.Credito.SingleAsync(t=>t.Id_Credito==id_credito);/*hay que poner que no se halla liquidado*/
            if (credito_nuevo != null)
            {
                Credito credito_refinanciable = await _context
                    .Credito
                    .Include(t => t.Persona)
                    .SingleAsync(t => t.Codigo_Credito == codigo_credito);
                if (credito_refinanciable != null && credito_nuevo.Id_Credito != credito_refinanciable.Id_Credito)
                {
                    /*verificar que no este en deducibles agregado ticket #1013*/
                    if (YaExisteDeducible(credito_refinanciable.Id_Credito))
                        return BadRequest("Este crédito ya fue refinanciado en este o en otro crédito");

                    Transaccion_Credito transaccion = await Task.Run(() => AmortizacionActualProc(credito_refinanciable.Id_Credito, fecha,monto_nuevo_refinanciamiento, para_refinanciar: true));
                    decimal monto_nuevo = credito_nuevo.Monto_pago;

                    monto_refinanciable = transaccion.Pago_para_cancelar;
                    decimal convertido = Convert.ToDecimal(monto_nuevo_refinanciamiento);
                    if (movible && convertido > 0.00M)
                        monto_refinanciable = convertido;
                        
                    decimal remanente = monto_nuevo - monto_refinanciable;
                    bool esrefinanciable = remanente >= 0;
                    Refinanciable refinanciable = new Refinanciable
                    {
                        Id_Credito_Nuevo = credito_nuevo.Id_Credito,
                        Id_Credito_Refinanciable = credito_refinanciable.Id_Credito,
                        Codigo_Refinanciable = credito_refinanciable.Codigo_Credito,
                        Socio_Refinanciado = credito_refinanciable.Persona.NombreCompleto,
                        Dui_Socio_Refinanciado = credito_refinanciable.Persona.Dui,
                        Monto_Nuevo = monto_nuevo,
                        Monto_Refinanciable = monto_refinanciable,
                        Remanente = remanente,
                        EsRefinanciable = esrefinanciable
                    };
                    return Ok(refinanciable);
                }
                else
                    return BadRequest("No se encontró el crédito que se desea refinanciar");
            }
            else
                return BadRequest("no se puede refinanciar con un crédito liquidado o que no exista");
        }


        private bool YaExisteDeducible(int id)
        {
            /*verificar que no este en deducibles agregado ticket #1013*/
            return  _context.Deduccion_Historial.AsNoTracking()
                .Any(t => t.Id_Credito_Financiado == id);
        }


        /// <summary>
        /// muestra el detalle de la 
        /// operación para ser re-estructurado
        /// </summary>
        /// <param name="id_credito">crédito nuevo</param>
        /// <param name="codigo_credito">crédito refinnaciable</param>
        /// <returns></returns>
        [Authorize(Roles = "Jefe,Administrador,Asistente", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Reestructurado(int id_credito, string codigo_credito)
        {
            DateTime fecha = DateTime.Now;
            Credito credito_nuevo = await _context.Credito.SingleAsync(t => t.Id_Credito == id_credito);/*hay que poner que no se halla liquidado*/
            if (credito_nuevo != null)
            {
                Credito credito_reestructurado = await _context
                    .Credito
                    .Include(t => t.Persona)
                    .SingleAsync(t => t.Codigo_Credito == codigo_credito);

                /*verificar que no este en deducibles agregado ticket #1013*/
                if (YaExisteDeducible(credito_reestructurado.Id_Credito))
                    return BadRequest("Este crédito ya fue refinanciado en este o en otro crédito");
                /*termina tarea de no estar dos veces el deducible ticket #1013*/
                if (credito_reestructurado != null && credito_nuevo.Id_Credito != credito_reestructurado.Id_Credito)
                {
                    Transaccion_Credito transaccion = await Task.Run(() => AmortizacionActualProc(credito_reestructurado.Id_Credito, fecha, null, true));
                    decimal monto_nuevo = credito_nuevo.Monto_pago;
                    decimal monto_refinanciable = transaccion.Pago_para_cancelar;
                    decimal remanente = 0.00M;
                    bool esrefinanciable = remanente >= 0;
                    Refinanciable refinanciable = new Refinanciable
                    {
                        Id_Credito_Nuevo = credito_nuevo.Id_Credito,
                        Id_Credito_Refinanciable = credito_reestructurado.Id_Credito,
                        Codigo_Refinanciable = credito_reestructurado.Codigo_Credito,
                        Socio_Refinanciado = credito_reestructurado.Persona.NombreCompleto,
                        Dui_Socio_Refinanciado = credito_reestructurado.Persona.Dui,
                        Monto_Nuevo = monto_nuevo,
                        Monto_Refinanciable = monto_refinanciable,
                        Remanente = remanente,
                        EsRefinanciable = esrefinanciable
                    };
                    return Ok(refinanciable);
                }
                else
                    return BadRequest("No se encontró el crédito que se desea refinanciar");
            }
            else
                return BadRequest("no se puede refinanciar con un crédito liquidado o que no exista");
        }

        /// <summary>
        /// se hace el refinanciamiento
        /// para que en la liquidación se pueda
        /// descontar del nuevo crédito
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Jefe,Administrador,Asistente", AuthenticationSchemes = "Bearer")]
        [HttpPut]
        public async Task<IActionResult> Procesar_Refinanciamiento(
            [FromBody]Refinanciable refinanciable, 
            bool reestructura = false,
            bool movible = false, 
            bool descuento = false)
        {
            try
            {
                NegocioContexto db = new NegocioContexto();
                string Id_Usuario = _userManager.GetUserId(HttpContext.User);
                bool existe = db.Deduccion_Historial.Any(t=>t.Id_Credito_Financiado == refinanciable.Id_Credito_Refinanciable);
                if (!existe)
                {
                    /*crédito nuevo*/
                    Linea_Credito_Historial linea_historial = _context
                        .Linea_Credito_Historial.Single(f => f.Id_Credito == refinanciable.Id_Credito_Nuevo);
                    DateTime fecha = DateTime.Now;
                    //crear la transacción simple
                    Deduccion_Historial deduccion = new Deduccion_Historial
                    {
                        Id_Linea_Credito_Historial = linea_historial.Id_Linea_Credito_Historial,
                        Id_Credito_Financiado = refinanciable.Id_Credito_Refinanciable,
                        Valor = refinanciable.Monto_Refinanciable,
                        Tipo_Valor = Tipo_Valor.Dinero,
                        Descripcion = "Refinanciado crédito anterior",
                        Nombre = "Refinanciamiento",
                        Codigo_Credito_Financiado = refinanciable.Codigo_Refinanciable
                    };
                    if (movible)
                    {
                            Manejo_Refinanciamiento Manejo = new Manejo_Refinanciamiento
                        {
                            Id_Credito_Refinanciado = refinanciable.Id_Credito_Refinanciable,
                            Id_Credito_Nuevo = refinanciable.Id_Credito_Nuevo,
                            Fecha_Refinanciamiento = fecha,
                            Usuario = Id_Usuario,
                            Descuento = descuento
                        };
                            _context.Manejo_Refinanciamiento.Add(Manejo).State = EntityState.Added;
                            await _context.SaveChangesAsync();
                    }
                        
                    _context.Deduccion_Historial.Add(deduccion).State = EntityState.Added;
                    await _context.SaveChangesAsync();
                    return Ok("EL refinanciamiento está listo para descontarse en la liquidación");
                    
                }
                else
                    return BadRequest("El crédito ya está refinanciado");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace);
            }
        }

        /// <summary>
        /// transfiere el monto del crédito a
        /// una cuenta de ahorro del cliente
        /// </summary>
        /// <param name="id_ahorro"></param>
        /// <param name="id_caja"></param>
        /// <param name="saldo"></param>
        /// <param name="resto_capital"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        private async Task Transferir_Monto_Credito(
            int id_ahorro,
            int id_caja,
            decimal saldo,
            decimal resto_capital,
            DateTime fecha)
        {
            /*ver la transacción de ahorro*/
            Transaccion_Ahorro transaccion = new Transaccion_Ahorro
            {
                Id_Ahorro = id_ahorro,
                Id_Tipo_Operacion_Ahorro = 1,/*es un deposito*/
                Saldo = saldo + resto_capital,
                Valor = resto_capital,
                Fecha = fecha
            };

            _context.Transaccion_Ahorro.Add(transaccion).State = EntityState.Added;
            await _context.SaveChangesAsync();
            string no_comprobante = await Generar_No_Comprobante();

            /*trabajo para la caja máquina*/
            string caja_maquina = _context.Configuracion_General.Single(x => x.Nombre == "CAJA_MAQUINA").Valor;
            id_caja = Convert.ToInt32(caja_maquina);

            Transaccion_Caja trans_caja = Registrar_Caja(transaccion, id_caja, fecha, false, comprobante: no_comprobante);
            _context.Transaccion_Caja.Add(trans_caja).State = EntityState.Added;
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// arma las deducciones si las tiene
        /// </summary>
        /// <param name="credito"></param>
        /// <param name="linea_historial"></param>
        /// <param name="caja"></param>
        /// <param name="Id_Usuario"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        private async Task<Deducibles> Armar_Deducciones(
            Credito credito,
            Linea_Credito_Historial linea_historial,
            Caja caja,
            string Id_Usuario,
            DateTime fecha)
        {
            Deducibles deducibles = new Deducibles();

            decimal deduccion = 0.00M;
            decimal refinanciado = 0.00M;
            if (linea_historial.Deducciones_Historial.Any())
            {
                List<Deduccion_Historial> deducciones_capital = linea_historial
                    .Deducciones_Historial.ToList();
                if (deducciones_capital != null)
                {
                    foreach (Deduccion_Historial dc in deducciones_capital)
                    {
                        decimal valor_transformado = 0.00M;
                        if (dc.Tipo_Valor == Tipo_Valor.Porcentaje)
                            valor_transformado = Math.Round((credito.Monto_pago * (dc.Valor / 100)), 2);
                        else
                            valor_transformado = dc.Valor;
                        if (dc.Id_Credito_Financiado == 0)
                            deduccion += valor_transformado;
                        Func<Manejo_Refinanciamiento, bool> es_manejo =
                            (new Func<Manejo_Refinanciamiento, bool>(d =>
                                d.Id_Credito_Refinanciado == dc.Id_Credito_Financiado));
                        Manejo_Refinanciamiento refinanciamiento = new Manejo_Refinanciamiento();

                        /*cambiar a caja máquina lo que es cobro de estructura*/
                        if (dc.Id_Credito_Financiado == 0)
                            caja = await obtener_Caja_Sistema();

                        Transaccion_Simple transaccion_deduccion = new Transaccion_Simple
                        {
                            Id_Tipo_Operacion_Simple = 7,
                            Monto = valor_transformado,
                            Id_Caja = caja.Id_Caja,
                            Id_Credito = credito.Id_Credito,
                            Id_Cobro_Acciones = 0,
                            Id_Deduccion = dc.Id_Deduccion_Historial,
                            Creado_En = fecha,
                            Creado_Por = caja.Id_Usuario,
                        };
                        /*efectuar el pago de refinanciamiento si se encuaentra*/
                        
                        _context.Transaccion_Simple.Add(transaccion_deduccion).State = EntityState.Added;
                        await _context.SaveChangesAsync();
                        if (dc.Id_Credito_Financiado > 0)
                        {
                            await Efectuar_Pago_Refinanciado(dc.Id_Credito_Financiado, fecha, true, transaccion_deduccion.Monto);
                            refinanciado += valor_transformado;/*aparto el refinanciamiento de las otras deducciones*/
                            if (_context.Manejo_Refinanciamiento.Any(es_manejo))
                            {
                                refinanciamiento = _context
                                .Manejo_Refinanciamiento.Single(c => c.Id_Credito_Refinanciado == dc.Id_Credito_Financiado);

                                string no_comprobante_deduccion = await Generar_No_Comprobante();
                                Transaccion_Caja trans_caja_deduccion = Registrar_Caja(transaccion_deduccion, caja.Id_Caja, fecha, true, comprobante: no_comprobante_deduccion);
                                _context.Transaccion_Caja.Add(trans_caja_deduccion).State = EntityState.Added;
                                await _context.SaveChangesAsync();

                                await Manejo_Contabilidad_Refinanciamientos(credito, transaccion_deduccion.Id_Transaccion_Simple, 1, refinanciamiento.Descuento);
                            }
                        }
                    }
                }
            }
            deducibles.Deducciones = deduccion;
            deducibles.Refinanciamiento = refinanciado;

            return deducibles;
        }

        /// <summary>
        /// se revierte la liquidación del crédito
        /// </summary>
        /// <param name="id_credito">el crédito que se va a revertir</param>
        /// <returns></returns>
        [Authorize(Roles = "Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPut]
        public async Task<IActionResult> Revertir_Liquidacion(int id_credito)
        {
            using (var dbContextTransaccion = _context.Database.BeginTransaction())
            {
                try
                {
                    /*verificar si se tiene al menos una transacción de pago no se puede revertir*/
                    if (_context.Transaccion_Credito.Any(d => d.Id_Credito == id_credito && !d.Borrado_Logico))
                        return BadRequest("No se puede revertir la liquidación de un crédito que ya tiene pagos");
                    //se consulta todas las inserciones de datos que se realizaron al liquidar crédito
                    var transacciones = (from c in _context.Credito
                                         join l in _context.Linea_Credito_Historial on c.Id_Credito equals l.Id_Credito
                                         join d in _context.Deduccion_Historial on l.Id_Linea_Credito_Historial equals d.Id_Linea_Credito_Historial
                                         join s in _context.Transaccion_Simple on d.Id_Deduccion_Historial equals s.Id_Deduccion
                                         //join a in _context.Transaccion_Caja on s.Id_Transaccion_Simple equals a.Id_Transaccion_Simple
                                         where c.Id_Credito == id_credito && /*s.Id_Tipo_Operacion_Simple == 8 &&*/ s.Id_Tipo_Operacion_Simple == 7
                                         select new
                                         {
                                             c.Id_Credito,
                                             l.Id_Linea_Credito_Historial,
                                             d.Id_Deduccion_Historial,
                                             s.Id_Transaccion_Simple,
                                             s.Id_Tipo_Operacion_Simple,
                                             d.Id_Credito_Financiado,
                                             s.Monto
                                         }).ToList();
                    // solo para creditos refinanciados o reestructurados que no se han liquidado
                    var transacciones_sin_liquidar = (from c in _context.Credito
                                         join l in _context.Linea_Credito_Historial on c.Id_Credito equals l.Id_Credito
                                         join d in _context.Deduccion_Historial on l.Id_Linea_Credito_Historial equals d.Id_Linea_Credito_Historial
                                         //join s in _context.Transaccion_Simple on d.Id_Deduccion_Historial equals s.Id_Deduccion
                                         //join a in _context.Transaccion_Caja on s.Id_Transaccion_Simple equals a.Id_Transaccion_Simple
                                         where c.Id_Credito == id_credito /*s.Id_Tipo_Operacion_Simple == 8 &&*/ 
                                         select new
                                         {
                                             c.Id_Credito,
                                             l.Id_Linea_Credito_Historial,
                                             d.Id_Deduccion_Historial,
                                             //s.Id_Transaccion_Simple,
                                             //s.Id_Tipo_Operacion_Simple,
                                             d.Id_Credito_Financiado,
                                             //s.Monto
                                         }).ToList();
                    //obtener el crédito 
                    var credito_persona = _context.Credito.Include(t => t.Persona).Single(g => g.Id_Credito == id_credito);
                    //obtener las cuentas de ahorro

                    //consulta transaccion simple y tipo de operacion desembolso
                    Func<Transaccion_Simple, bool> tiene_simple = new Func<Transaccion_Simple, bool>(t => t.Id_Credito == id_credito && t.Id_Tipo_Operacion_Simple == 8);
                    bool tiene_refinanciamiento = transacciones_sin_liquidar.Any(c => c.Id_Credito_Financiado > 0);
                    if (_context.Transaccion_Simple.Any(tiene_simple))
                    {
                        Transaccion_Simple transaccion = _context.Transaccion_Simple.Single(tiene_simple);

                        var ahorros = _context.Ahorro.Where(y => y.Id_Persona == credito_persona.Id_Persona && y.Id_Linea_Ahorro == 3).ToList();

                        List<Transaccion_Ahorro> taa = new List<Transaccion_Ahorro>();
                        List<Ahorro> ah = new List<Ahorro> { ahorros.FirstOrDefault() };
                        //validar si tiene deducciones o es refinanciamiento
                        bool tiene_deducciones = transacciones.Any(c => c.Id_Tipo_Operacion_Simple == 7 || c.Id_Credito_Financiado > 0);

                        foreach (var a in ah)
                        {
                            //obtener lista de transacciones ahorro
                            List<Transaccion_Ahorro> tr_ahorro = _context.Transaccion_Ahorro.Where(d => d.Id_Ahorro == a.Id_Ahorro
                            && !d.Borrado_Logico).ToList();
                            //obtener la ultima transaccion
                            var tr1 = tr_ahorro[^1];
                            //obtener la penultima transaccion (en caso de deducciones)
                            var tr2 = tr_ahorro[^2];
                            //obtener la penultima transaccion ahorro siempre y cuanto sea con deducciones
                            if (tiene_deducciones)
                            {
                                taa.Add(tr2);
                            }
                            //obtener la ultima transaccion para créditos sin deduccion
                            else if (tr1.Saldo >= transaccion.Monto)
                            {
                                taa.Add(tr1);
                            }
                            //obtener y agregar a la lista el retiro de las deducciones
                            if (tiene_deducciones)
                            {
                                taa.Add(tr1);
                            }
                        }
                        /*revertir deposito*/
                        if (taa != null)
                            foreach (var tran_aho in taa)
                            {
                                await Reversion(tran_aho.Id_Transaccion_Ahorro, false);
                            }

                        if (tiene_deducciones)
                        {
                            /*revertir las deducciones*/
                            var deduccion_simple = transacciones.Where(c => c.Id_Tipo_Operacion_Simple == 7 || c.Id_Credito_Financiado > 0).ToList();
                            foreach (var deduccion in deduccion_simple)
                            {
                                if (deduccion.Id_Credito_Financiado > 0)
                                {
                                    await Reversion_Refinanciamiento(deduccion.Id_Credito_Financiado);
                                    await Reversion_Deduccion_Historial(deduccion.Id_Credito_Financiado);
                                }
                                if (deduccion.Id_Credito_Financiado == 0)
                                {
                                    await Reversion_Honorarios_Gastos(deduccion.Id_Deduccion_Historial);
                                }
                            }
                            /*termina revertir deducción*/
                            List<int> id_deducciones = transacciones
                                .Where(c => c.Id_Credito_Financiado == 0)
                                .Select(f => f.Id_Transaccion_Simple).ToList();
                            /*reversión simple*/
                            await Reversion_Simple(id_deducciones);
                        }
                    }
                    if(tiene_refinanciamiento && !credito_persona.Liquidado)
                    {
                        var deduccion_simple = transacciones_sin_liquidar.Where(c =>  c.Id_Credito_Financiado > 0).ToList();
                        foreach (var deduccion in deduccion_simple)
                        {
                            if (deduccion.Id_Credito_Financiado > 0)
                            {
                                await Reversion_Refinanciamiento(deduccion.Id_Credito_Financiado);
                                await Reversion_Deduccion_Historial(deduccion.Id_Credito_Financiado);
                            }
                            if (deduccion.Id_Credito_Financiado == 0)
                            {
                                await Reversion_Honorarios_Gastos(deduccion.Id_Deduccion_Historial);
                            }
                        }
                    }

                    /*actualizar el crédito mismo*/
                    Credito credito_modi = _context.Credito.Single(s=>s.Id_Credito==credito_persona.Id_Credito);
                    credito_modi.Liquidado = false;
                    credito_modi.Liquidado_En = DateTime.MinValue;
                    credito_modi.Cancelado = true;
                    credito_modi.Aprobado = false;
                    credito_modi.Aprobado_En = DateTime.MinValue;
                    credito_modi.Cancelado_En = DateTime.Now;
                    credito_modi.Revertido = true;
                    credito_modi.Codigo_Credito = string.Format("---{0}---", credito_modi.Codigo_Credito);
                    
                    _context.Entry(credito_modi).State=EntityState.Modified;
                    await _context.SaveChangesAsync();

                    dbContextTransaccion.Commit();
                    await Bitacora("Crédito", "Reversión Crédito", credito_modi.Id_Credito,false,true,monto: credito_modi.Monto_pago);
                    /*llenar los datos para el documento*/
                    var ahorita = DateTime.Now;
                    string id_usuario = _userManager.GetUserId(HttpContext.User); 
                    Reversion_Credito reversion = new Reversion_Credito {
                        Emision = ahorita.Date,
                        Usuario = id_usuario,
                        Codigo_Credito = credito_persona.Codigo_Credito,
                        Cliente = credito_persona.Persona.NombreCompleto,
                        Monto = credito_persona.Monto_pago,
                        Revertido_En = ahorita,
                        Aperturado_En = credito_persona.Liquidado_En,
                        Concepto = "Reversión de crédito",
                        Revertido_Por = id_usuario
                    };

                    return Ok(reversion);
                }
                catch (Exception ex) 
                {
                    dbContextTransaccion.Rollback();
                    return BadRequest(ex.StackTrace);
                }
            }
        }

        /// <summary>
        /// hacer la reversion de crédito refinanciado
        /// a partir de las deducciones
        /// </summary>
        /// <returns></returns>
        public async Task Reversion_Refinanciamiento(int id_credito_refinanciado)
        {
            
            var transaccion_refinanciada = _context
                    .Transaccion_Credito
                    .Where(u => u.Id_Credito == id_credito_refinanciado)
                    .AsEnumerable().Last();
            await Reversion(transaccion_refinanciada.Id_Transaccion_Credito, true);
            /*hay que quitar el estado de cancelado*/
            Credito credito_refinanciado = await _context.Credito.SingleAsync(v => v.Id_Credito == id_credito_refinanciado);
            credito_refinanciado.Cancelado = false;
            credito_refinanciado.Cancelado_En = DateTime.MinValue;
            _context.Entry(credito_refinanciado).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task Reversion_Deduccion_Historial(int id_credito_refinanciado)
        {
            var deduccion = _context
                .Deduccion_Historial
                .Single(u => u.Id_Credito_Financiado == id_credito_refinanciado);
            _context.Deduccion_Historial.Remove(deduccion);
            await _context.SaveChangesAsync();
        }
        public async Task Reversion_Honorarios_Gastos(int id_deduccion)
        {
            var deduccion = _context
                .Deduccion_Historial
                .Single(u => u.Id_Deduccion_Historial == id_deduccion);
            _context.Deduccion_Historial.Remove(deduccion);
            await _context.SaveChangesAsync();
        }
        /// <summary>
        /// se efectua el pago refinanciado
        /// </summary>
        /// <returns></returns>
        private async Task Efectuar_Pago_Refinanciado(int id_credito,DateTime fecha,bool refinanciable= false, decimal? monto = 0.00M )
        {
            Transaccion_Credito transaccion = await Task.Run(() => AmortizacionActualProc(id_credito, fecha, monto));
            transaccion.Valor_Cuota =Convert.ToDecimal(monto);
            Credito credito_para_cancelar = await _context.Credito.Include(f => f.Persona)
                .Include(c => c.Comites)
                .Include(a => a.Persona.Evaluaciones)
                .Include(h => h.Transacciones_Creditos)
                .Include(p => p.Periodo)
                .Include(p => p.Comportamiento_Credito)
                .SingleOrDefaultAsync(d => d.Id_Credito == transaccion.Id_Credito);
            credito_para_cancelar.Cancelado = true;
            credito_para_cancelar.Cancelado_En = DateTime.Now;
            _context.Entry(credito_para_cancelar).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            string no_comprobante_simple = await Generar_No_Comprobante();
            await EfectuarPago(transaccion,no_comprobante_simple, refinanciable,false,true);
        }


        /// <summary>
        /// entrega acta con la lista de creditos aprobados
        /// </summary>
        /// <param name="id_acta">int id de acta</param>
        /// <returns>Lista de créditos</returns>
        [Authorize(Roles = "Asistente,Jefe,Administrador,Gerente", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> ActaPorId(int id_acta)
        {
            List<Credito> creditos=await _context.Credito
                .Include("Persona")
                .Include("Linea_Credito")
                .Include(p=>p.Periodo).Where(i=>i.Id_Acta==id_acta).ToListAsync();
            return Ok(creditos);
        }

#endregion

        /// <summary>
        /// borrar el crédito
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Administrador", AuthenticationSchemes = "Bearer")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCredito([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var credito = await _context.Credito.SingleOrDefaultAsync(m => m.Id_Credito == id);
            if (credito == null)
            {
                return NotFound();
            }

            _context.Credito.Remove(credito);
            await _context.SaveChangesAsync();

            return Ok(credito);
        }

        /// <summary>
        /// Consulta los datos de la persona para generar el mutuo
        /// </summary>
        /// <param name="id_credito"></param>
        /// <returns>200</returns>
        [Authorize(Roles = "Asistente,Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Mutuo(int id_credito)
        {
            try
            {
                Credito credito =await _context
                    .Credito
                    .Include(d=>d.Periodo)
                    .Include(l=>l.Linea_Credito)
                    .Include("Linea_Credito.Tipo_Mutuos")
                    .Include(p=>p.Persona)
                    .Include(a=>a.Persona.Ahorros)
                    .Include(r=>r.Persona.Persona_Referencias)
                    .AsNoTracking()
                    .SingleAsync(d => d.Id_Credito == id_credito);
                Amortizacion datos = new Amortizacion();
                Amortizacion datos_segunda_fecha = new Amortizacion();
                datos = CuotaPrimaria(
                        credito.Id_Linea,
                        credito.Monto_pago,
                        credito.Id_Periodo,
                        credito.Plazo,
                        credito.Id_Credito)
                    .Amortizacion
                    .Single(d => d.No == 1);
                credito.Primer_Pago = datos.Valor_Cuota;
                credito.Fecha_Primer_Pago = datos.FechaPago;
                decimal plan_ayc = datos.Comision;
                decimal iva_plan_ayc = 0.00M; //datos.IvaSobreComision;
                decimal plan_ayc_total = plan_ayc+iva_plan_ayc ;
                decimal cuota_menos_plan = datos.Valor_Cuota - plan_ayc_total;
                credito.Plan_AyC = plan_ayc_total;
                credito.Cuota_Menos_Plan = cuota_menos_plan;
                if (credito.Liquidado_En.Day >=25) {
                    bool tiene_mas_de_una_cuota = false;
                    if (credito.Numero_cuotas > 1)
                    {
                        datos_segunda_fecha = CuotaPrimaria(
                            credito.Id_Linea,
                            credito.Monto_pago,
                            credito.Id_Periodo,
                            credito.Plazo,
                            credito.Id_Credito)
                        .Amortizacion
                        .Single(d => d.No == 2);
                    }
                    else {
                        datos_segunda_fecha = CuotaPrimaria(
                            credito.Id_Linea,
                            credito.Monto_pago,
                            credito.Id_Periodo,
                            credito.Plazo,
                            credito.Id_Credito)
                        .Amortizacion
                        .Single(d => d.No == 1);
                    }
                    credito.Segundo_Pago = datos_segunda_fecha.Valor_Cuota;
                    credito.Fecha_Segundo_Pago = datos_segunda_fecha.FechaPago;
                    credito.Segunda_Cuota_Menos_Plan = datos_segunda_fecha.Valor_Cuota-plan_ayc_total;

                }
               
               
                Persona persona = credito.Persona;
                persona.Creditos.Add(credito);
                Cartera cartera_persona = _context.Cartera.AsNoTracking()
                    .Single(a=>a.Id_Cartera == persona.Id_Cartera);
                Usuario_Agencia agencia_asesor = _context.Usuario_Agencia.AsNoTracking()
                    .Single(a => a.Id_Usuario == cartera_persona.Id_Usuario);
                Agencia nombre_agencia = _context.Agencia.AsNoTracking()
                    .Single(a => a.Id_Agencia == agencia_asesor.Id_Agencia);
                credito.Nombre_Ciudad_Mutuo = nombre_agencia.Ciudad;
                Func<Persona_Relaciones,bool> func_relaciones = new Func<Persona_Relaciones, bool>(t=>t.Id_Sujeto == persona.Id_Persona);
                List<Persona_Relaciones> relaciones = new List<Persona_Relaciones>();
                if (_context.Persona_Relaciones.Any(func_relaciones))
                {
                   List< Persona_Relaciones> relacion_codeudor = 
                        await Codeudor_Mutuos(persona.Id_Persona, credito.Id_Credito);
                   
                    foreach (var r in relacion_codeudor)
                    {
                        relaciones.Add(r);
                    }
                    persona.Persona_Relaciones = relaciones;
                } //validacion de documentos
                if (credito.Liquidado_En.Day >= 25 || relaciones.Count() ==2)
                {
                    if (credito.Numero_cuotas > 1)
                    {
                        datos_segunda_fecha = CuotaPrimaria(
                        credito.Id_Linea,
                        credito.Monto_pago,
                        credito.Id_Periodo,
                        credito.Plazo,
                        credito.Id_Credito)
                    .Amortizacion
                    .Single(d => d.No == 2);
                    }
                    else {
                        datos_segunda_fecha = CuotaPrimaria(
                            credito.Id_Linea,
                            credito.Monto_pago,
                            credito.Id_Periodo,
                            credito.Plazo,
                            credito.Id_Credito)
                        .Amortizacion
                        .Single(d => d.No == 1);
                    }
                        
                    credito.Segundo_Pago = datos_segunda_fecha.Valor_Cuota;
                    credito.Fecha_Segundo_Pago = datos_segunda_fecha.FechaPago;
                    credito.Segunda_Cuota_Menos_Plan = datos_segunda_fecha.Valor_Cuota - plan_ayc_total;

                }
                if (credito.Liquidado_En.Day >= 25 && credito.Id_Linea !=5 && credito.Id_Linea != 12 
                    && credito.Id_Linea != 17)
                {
                    credito.mutuo_dos_fechas = true;
                } 
                if (relaciones.Count() ==2) {
                    credito.mutuo_dos_codeudores = true;
                }
                if (relaciones.Count() == 2 && credito.Liquidado_En.Day >= 25 && credito.Id_Linea != 5 && credito.Id_Linea != 12
                    && credito.Id_Linea != 17) { 

                    credito.mutuo_dos_codeudores = false;
                    credito.mutuo_dos_fechas = false;
                    credito.mutuo_dos_codeudores_dos_fechas = true;
                }

                return Ok(persona);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }

        /// <summary>
        /// saber si un crédito es grupal
        /// </summary>
        /// <param name="id_credito"></param>
        /// <returns></returns>
        [Authorize(Roles = "Asistente,Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Es_Grupal(int id_credito)
        {
            var grupal = await _context
                .Detalle_Credito_Grupal
                .Include(v=>v.Credito_Grupal)
                .SingleAsync(d=>d.Id_Credito==id_credito);
            if (grupal != null)
                return Ok(grupal.Credito_Grupal.Nombre_grupo);
            return BadRequest("No es Grupal");
        }


        /// <summary>
        /// se obtendrá el mutuo grupal
        /// </summary>
        /// <param name="id_credito"></param>
        /// <returns></returns>
        [Authorize(Roles = "Asistente,Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Mutuo_Grupal(int id_credito)/*el id del credito lider*/
        {
            Func<Detalle_Credito_Grupal, bool> filtro_grupal = new Func<Detalle_Credito_Grupal, bool>(f=>f.Id_Credito==id_credito);
            bool es_grupal = _context.Detalle_Credito_Grupal.Any(filtro_grupal);
            if (!es_grupal)
                BadRequest("El crédito que intenta procesar no es grupal");
            int id_grupo = _context.Detalle_Credito_Grupal.Single(filtro_grupal).Id_Credito_Grupal;
            var grupo_credito = await _context.Detalle_Credito_Grupal
                .Include(d=>d.Credito)
                .Include(d=>d.Credito.Persona)
                .Include(d=>d.Credito.Periodo)
                .Include(d=>d.Credito.Linea_Credito)
                .Include(d=>d.Credito.Linea_Credito.Tipo_Mutuos)
                .Include(d=>d.Credito.Persona.Ahorros)
                .Include(d=>d.Credito.Persona.Persona_Referencias)
                .AsNoTracking()
                .Where(c=>c.Id_Credito_Grupal==id_grupo).ToListAsync();
            List<Persona> personas = new List<Persona>();
            foreach (var item in grupo_credito)
                personas.Add(item.Credito.Persona);
            foreach (var persona in personas)
            {
                Credito credito = grupo_credito.Single(d=>d.Credito.Id_Persona==persona.Id_Persona).Credito;
                persona.Creditos.Add(credito);
            }
            return Ok(personas);
        }

        /// <summary>
        /// información de un codeudor
        /// </summary>
        /// <param name="id_persona"></param>
        /// <returns></returns>
        private async Task<Persona_Relaciones> Codeudor_Activo_Mutuo(int id_persona)
        {
            Tipo_Relacion tipo = await _context.Tipo_Relacion.AsNoTracking().SingleAsync(a=>a.Tipo_Relacion_Definicion== "Codeudora");
            Persona_Relaciones codeudor = _context
                .Persona_Relaciones
                .AsNoTracking()
                .Include(x=>x.Predicado)
                .FirstOrDefault(m=>m.Id_Tipo_Relacion==tipo.Id_Tipo_Relacion && m.Id_Sujeto==id_persona);
            return codeudor;
        }
        private async Task<List<Persona_Relaciones>> Codeudor_Mutuos(int id_persona, int id_credito)
        {
            Tipo_Relacion tipo = await _context.Tipo_Relacion.AsNoTracking().SingleAsync(a => a.Tipo_Relacion_Definicion == "Codeudora");
            List<Persona_Relaciones> codeudor = _context
                .Persona_Relaciones
                .AsNoTracking()
                .Include(x => x.Predicado)
                .Where(m => m.Id_Tipo_Relacion == tipo.Id_Tipo_Relacion && m.Id_Sujeto == id_persona
                && m.Id_Credito == id_credito).ToList();
            return codeudor;
        }
        /// <summary>
        /// solo la lista de periodos
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Administrador,Gerente", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Periodos()
        {
            return Ok(await _context.Periodo.AsNoTracking().ToListAsync());
        }

        /// <summary>
        /// obtener el id_ de la linea_credito_historial
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Administrador,Gerente", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Linea_Credito_Historial(int id_credito)
        {
            Linea_Credito_Historial linea =await _context
                .Linea_Credito_Historial
                .SingleAsync(c=>c.Id_Credito==id_credito);
            return Ok(linea);
        }

        private bool CreditoExists(int id)
        {
            return _context.Credito.Any(e => e.Id_Credito == id);
        }


        //para sacar reporte de rendimiento
        /// <summary>
        /// retorna el rendimiento de una agencia específica
        /// </summary>
        /// <param name="fecha_inicio"></param>
        /// <param name="fecha_final"></param>
        /// <param name="id_agencia"></param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Rendimiento(DateTime fecha_inicio, DateTime fecha_final,int id_agencia)
        {
            List<Usuario_Agencia> usuarios =await _context
                .Usuario_Agencia
                .AsNoTracking()
                .Where(a=>a.Id_Agencia==id_agencia)
                .ToListAsync();
            List<Rendimiento_Vista> rendimientos = new List<Rendimiento_Vista>();

            foreach (Usuario_Agencia usuario in usuarios)
            {
                List<Credito> creditos = _context
                    .Credito
                    .Include(r=>r.Linea_Credito_Historial)
                    .Include("Linea_Credito_Historial.Deducciones_Historial")
                    .AsNoTracking()
                    .Include(f=>f.Cartera)
                    .Where(t => t.Cartera.Id_Usuario ==usuario.Id_Usuario).ToList();

                decimal Recuperacion_Capital = 0.00M;
                decimal Cobertura_Deuda = 0.00M;
                decimal Recargos = 0.00M;
                decimal Intereses = 0.00M;
                decimal Ingresos_Percibidos = 0.00M;
                decimal Comision_Estructural = 0.00M;

                foreach (Credito crecito in creditos)
                {
                    List<Transaccion_Credito> transacciones = _context.
                    Transaccion_Credito.
                    AsNoTracking().
                    Where(
                        d => d.FechaPago.Date >= fecha_inicio.Date &&
                        d.FechaPago.Date <= fecha_final.Date &&
                        d.Id_Credito==crecito.Id_Credito
                    ).ToList();

                    //agregar en un primer intento el rendimiento
                    Recuperacion_Capital += transacciones.Sum(d => d.Abono_capital);
                    Cobertura_Deuda += transacciones.Sum(t => t.Interes);
                    Recargos += transacciones.Sum(s => s.Recargo);
                    Intereses += transacciones.Sum(t => t.Interes_Comision);
                    Ingresos_Percibidos += Cobertura_Deuda + Recargos + Intereses;
                }

                Comision_Estructural = creditos
                    .SelectMany(
                        f=>f.Linea_Credito_Historial.Deducciones_Historial)
                    .Sum(d=>d.Valor);
                rendimientos.Add(new Rendimiento_Vista
                {
                    Cartera=usuario.Id_Usuario,
                    Recuperacion_Capital = Math.Round(Recuperacion_Capital,2),
                    Cobertura_Deuda = Math.Round(Cobertura_Deuda, 2),
                    Recargos = Math.Round(Recargos, 2),
                    Intereses = Math.Round(Intereses, 2),
                    Ingresos_Percibidos = Math.Round(Ingresos_Percibidos, 2),
                    IVA = Math.Round(Ingresos_Percibidos * IVA, 2),
                    Reinversion_Colocacion = 0.00M,
                    Efectivo_No_Rotativo = 0.00M,
                    Comision_Estructural = Math.Round(Comision_Estructural, 2),
                    Ingresos_Devengados= Math.Round(Ingresos_Percibidos + Comision_Estructural, 2)
                });
            }
            return Ok(rendimientos);
        }

        /// <summary>
        /// solo para obtener la lista de comportamientos de créditos
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Comportamientos_Credito()
        {
            List<Comportamiento_Credito> comportamiento =await _context.Comportamiento_Credito.ToListAsync();
            return Ok(comportamiento);
        }

        /// <summary>
        /// estado de cuenta de un crédito
        /// </summary>
        /// <param name="id_credito"></param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Administrador,Gerente", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Estado_Cuenta_Creditos(int id_credito)
        {
            string id_usuario = _userManager.GetUserId(HttpContext.User);
            Credito credito = await _context.Credito
                .Include(f => f.Persona)
                .Include(a => a.Linea_Credito_Historial)
                .Include(c => c.Comites)
                .Include(a => a.Persona.Evaluaciones)
                .Include(h => h.Transacciones_Creditos)
                .Include(p => p.Periodo)
                .Include(p => p.Comportamiento_Credito)
                .AsNoTracking()
                .SingleOrDefaultAsync(d => d.Id_Credito == id_credito);
            Cartera cartera = await _context.Cartera.AsNoTracking().SingleAsync(a=>a.Id_Cartera == credito.Id_Cartera);
            if (credito != null)
            {
                Estado_Cuenta_Prestamos estado = new Estado_Cuenta_Prestamos
                {
                    nombre_generado = id_usuario,
                    Fecha = DateTime.Today,
                    Nombre = credito.Persona.NombreCompleto,
                    Codigo = credito.Codigo_Credito,
                    direccion = credito.Persona.Domicilio,
                    Lugar_Trabajo = credito.Persona.Lugar_Trabajo,
                    Detalle_Transacciones = credito.Transacciones_Creditos.ToList(),
                };
                Detalle_Credito detalle = new Detalle_Credito
                {
                    Apertura = credito.Aprobado_En,
                    Vencimiento = credito.Vencido_En,
                    Ultimo_Pago = credito.Vencido_En,
                    Plazo = credito.Plazo,
                    Cuotas = credito.Numero_cuotas,
                    F_Pago = null,
                    Telefonos = credito.Persona.Telefono,
                    Monto = credito.Monto_pago,
                    Saldo = credito.Transacciones_Creditos.Max(a=>a.Saldo_Capital),
                    Cuota = credito.Cuota_Total,
                    Dias = "",
                    Acu_Mora = (decimal) 0.00,
                    Cuot_Mora = (decimal) 0.00,
                    Asesor = cartera.Id_Usuario

                };
                Totales_Estado_Cuenta totalesEstado = new Totales_Estado_Cuenta
                {
                    Saldo = credito.Transacciones_Creditos.Sum(a=>a.Saldo_Capital),
                    Normales = credito.Transacciones_Creditos.Sum(a=>a.Interes),
                    Pendientes = (decimal) 0.00,
                    Recargos = (decimal) 0.00,
                    Cobertura_Deuda = credito.Transacciones_Creditos.Sum(c=>c.Interes)+ credito.Transacciones_Creditos.Sum(a=>a.IvaSobreComision),
                    Moratorios = (decimal) 0.00,
                    Recargo_Vencimiento = (decimal) 0.00
                };
                estado.Detalle_Credito = detalle;
                estado.Totales_Estado_Cuenta = totalesEstado;
                
                return Ok(estado);
            }
            else
            {
                return NoContent();
            } 
           
        }

        /// <summary>
        /// genera la información para presentar un pagaré
        /// </summary>
        /// <param name="id_persona"></param>
        /// <returns></returns>
        [Authorize(Roles = "Asistente,Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Pagare(int id_credito)
        {
            try
            {
                Credito credito = await _context
                    .Credito
                    .Include(d => d.Periodo)
                    .Include(l => l.Linea_Credito)
                    .Include("Linea_Credito.Tipo_Mutuos")
                    .Include(p => p.Persona)
                    .Include(a => a.Persona.Ahorros)
                    .Include(r => r.Persona.Persona_Referencias)
                    .AsNoTracking()
                    .SingleAsync(d => d.Id_Credito == id_credito);

                credito.Primer_Pago = CuotaPrimaria(
                        credito.Id_Linea,
                        credito.Monto_pago,
                        credito.Id_Periodo,
                        credito.Plazo,
                        credito.Id_Credito)
                    .Amortizacion
                    .Single(d => d.No == 1).Valor_Cuota;
                Persona persona = credito.Persona;
                Cartera cartera_persona = _context.Cartera.AsNoTracking()
                  .Single(a => a.Id_Cartera == persona.Id_Cartera);
                Usuario_Agencia agencia_asesor = _context.Usuario_Agencia.AsNoTracking()
                    .Single(a => a.Id_Usuario == cartera_persona.Id_Usuario);
                Agencia nombre_agencia = _context.Agencia.AsNoTracking()
                    .Single(a => a.Id_Agencia == agencia_asesor.Id_Agencia);
                credito.Nombre_Ciudad_Mutuo = nombre_agencia.Ciudad;
                persona.Creditos.Add(credito);
                Func<Persona_Relaciones, bool> func_relaciones = new Func<Persona_Relaciones, bool>(t => t.Id_Sujeto == persona.Id_Persona);
                if (_context.Persona_Relaciones.Any(func_relaciones))
                {
                    Persona_Relaciones relacion_codeudor = await Codeudor_Activo_Mutuo(persona.Id_Persona);
                    List<Persona_Relaciones> relaciones = new List<Persona_Relaciones>();
                    relaciones.Add(relacion_codeudor);
                    persona.Persona_Relaciones = relaciones;
                }
                return Ok(persona);
            }
            catch (Exception e)
            {
               return BadRequest("Algo anda mal, intentalo de nuevo");
            }
        }

     //   [Authorize(Roles = "Asistente,Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        #region  AQUI ESTA EL METODO PAGARE CODEUDOR
        public async Task<IActionResult> PagareCodeudor(int id_credito)
        {
            try
            {
                bool tieneCodeudor = _context.Persona_Relaciones.Any(c => c.Id_Credito == id_credito);
                if (tieneCodeudor)
                {
                    List<Persona_Relaciones> persona_Relaciones =
                   _context.Persona_Relaciones.Where(a => a.Id_Credito == id_credito).ToList();

                    var id_persona = persona_Relaciones.Where(a=>a.Activo).LastOrDefault().Id_Predicado;

                    Persona informacionCodeudor = _context.Personas
                        .Single(a => a.Id_Persona == id_persona);
                    Credito credito = _context.Credito.First(a => a.Id_Credito == id_credito);
                    Cartera carteraUsuario = _context.Cartera.First(a => a.Id_Cartera == informacionCodeudor.Id_Cartera);
                    Usuario_Agencia usuarioCartera = _context.Usuario_Agencia
                        .Include(c => c.Agencia)
                        .First(a => a.Id_Usuario == carteraUsuario.Id_Usuario);

                    PagarCodeudor Codeudor = new PagarCodeudor
                    {
                        fecha = DateTime.Now,
                        nombreFiador = informacionCodeudor.NombreCompleto,
                        edad = informacionCodeudor.Edad,
                        direccion = informacionCodeudor.Domicilio,
                        Dui = informacionCodeudor.Dui,
                        monto = credito.Monto_pago,
                        ciudad = usuarioCartera.Agencia.Ciudad,
                        departamento = usuarioCartera.Agencia.Ciudad
                    };
                    return Ok(Codeudor);
                }
                else
                {
                    return BadRequest("el credito no tiene codeudor o no ha sido asignado");
                }

            }
            catch (Exception e)
            {
                return BadRequest("Algo anda mal, intentalo de nuevo");
            }
        }
        [HttpGet]
        public async Task<IActionResult> ordenDescuentoDeudor(int id_credito)
        {
            try
            {
                    Credito credito = _context.Credito
                    .Include(b => b.Persona)
                    .Single(a => a.Id_Credito == id_credito);
                    Cartera carteraUsuario = _context.Cartera.First(a => a.Id_Cartera == credito.Id_Cartera);
                    Usuario_Agencia usuarioCartera = _context.Usuario_Agencia
                        .Include(c => c.Agencia)
                        .First(a => a.Id_Usuario == carteraUsuario.Id_Usuario);
                    ordenDescuento ordenDescuento = new ordenDescuento
                    {
                        fecha = DateTime.Now.Date,
                        nombreCliente = credito.Persona.NombreCompleto,
                        edad = credito.Persona.Edad != null ? credito.Persona.Edad : 0 ,
                        dui = credito.Persona.Dui != null ? credito.Persona.Dui : "DUI NO INGRESADO",
                        expedicionDui = usuarioCartera.Agencia.Ciudad != null ? usuarioCartera.Agencia.Ciudad : "DATO NO ALMACENADO EN SISTEMA",
                        monto = credito.Monto_pago,
                        numCuotas = credito.Numero_cuotas,
                        valCuota = credito.Valor_Cuota,
                        codigoCredito = credito.Codigo_Credito,
                        ciudad = usuarioCartera.Agencia.Ciudad,
                        departamento = usuarioCartera.Agencia.Ciudad
                    };
                    return Ok(ordenDescuento);

            }
            catch (Exception e)
            {
                return BadRequest("Algo anda mal, intentalo de nuevo");
            }
        }
        [HttpGet]
        public async Task<IActionResult> calendarioPagos(int id_credito)
        {
            try
            {
                Credito credito = _context.Credito
                .Include(b => b.Persona)
                .Single(a => a.Id_Credito == id_credito);
                Cartera carteraUsuario = _context.Cartera.First(a => a.Id_Cartera == credito.Id_Cartera);
              
                var tieneAmortizacion = _context.Amortizacion.Any(a => a.Id_Credito == id_credito);
                List<FechasPagoCalendario> fechas = new List<FechasPagoCalendario>();

                if (tieneAmortizacion)
                {
                    var fechasPago = _context.Amortizacion.Where(a => a.Id_Credito == id_credito).ToList();
                    foreach(var data in fechasPago)
                    {
                        fechas.Add(new FechasPagoCalendario
                        {
                            fechaPago = data.FechaPago.ToString("dd/MM/yyyy"),
                            descripcion = "PAGO DE CUOTA"
                        });
                    }
                }
                else
                {
                    return BadRequest("El credito no tiene amortizacion");
                }
                CalendarioPago calendario = new CalendarioPago
                {
                    codigoCredito = credito.Codigo_Credito,
                    nombreCliente = credito.Persona.NombreCompleto,
                    valorCuota = credito.Valor_Cuota,
                    asesor = carteraUsuario.Nombre,
                    fechas = fechas
                };

                return Ok(calendario);

            }
            catch (Exception e)
            {
                return BadRequest("Algo anda mal, intentalo de nuevo");
            }
        }
        [HttpGet]
        public async Task<IActionResult> ordenDescuentoFiador(int id_credito)
        {
            try
            {
                bool tieneCodeudor = _context.Persona_Relaciones.Any(c => c.Id_Credito == id_credito);
                if (tieneCodeudor)
                {
                    List<Persona_Relaciones> persona_Relaciones =
                   _context.Persona_Relaciones.Where(a => a.Id_Credito == id_credito).ToList();

                    var idPersona = persona_Relaciones.FirstOrDefault().Id_Predicado;
                    Credito credito = _context.Credito
                    .Single(a => a.Id_Credito == id_credito );
                    Persona fiador = _context.Personas.Single(n => n.Id_Persona == idPersona);
                    Cartera carteraUsuario = _context.Cartera.First(a => a.Id_Cartera == credito.Id_Cartera);
                    Usuario_Agencia usuarioCartera = _context.Usuario_Agencia
                        .Include(c => c.Agencia)
                        .First(a => a.Id_Usuario == carteraUsuario.Id_Usuario);
                    ordenDescuento ordenDescuento = new ordenDescuento
                    {
                        fecha = DateTime.Now.Date,
                        nombreCliente = fiador.NombreCompleto,
                        edad = fiador.Edad,
                        dui = fiador.Dui,
                        expedicionDui = fiador.ExpedicionDocumentoIdentifcacion,
                        monto = credito.Monto_pago,
                        numCuotas = credito.Numero_cuotas,
                        valCuota = credito.Valor_Cuota,
                        codigoCredito = credito.Codigo_Credito,
                        ciudad = usuarioCartera.Agencia.Ciudad,
                        departamento = usuarioCartera.Agencia.Ciudad
                    };
                    return Ok(ordenDescuento);
                }
                else
                {
                    return BadRequest("el credito no tiene codeudor o no ha sido asignado");
                }



            }
            catch (Exception e)
            {
                return BadRequest("Algo anda mal, intentalo de nuevo");
            }
        }
        #endregion

        /// <summary>
        /// documento de condiciones generales
        /// </summary>
        /// <param name="id_persona"></param>
        /// <returns></returns>
        [Authorize(Roles = "Asistente,Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Condiciones_Generales(int id_persona)
        {
            try
            {
                Persona persona = await _context.Personas.AsNoTracking()
                    .Include(d => d.Creditos)
                    .Include(d => d.Persona_Referencias)
                    .SingleOrDefaultAsync(w => w.Id_Persona == id_persona);
                Persona_Relaciones relacion_codeudor = await Codeudor_Activo_Mutuo(id_persona);
                List<Persona_Relaciones> relaciones = new List<Persona_Relaciones>();
                relaciones.Add(relacion_codeudor);
                persona.Persona_Relaciones = relaciones;
                return Ok(persona);
            }
            catch (Exception e)
            {
                return BadRequest("Algo anda mal, intentalo de nuevo");
            }
        }

        /// <summary>
        /// se obtiene el documento de carpeta azul
        /// </summary>
        /// <param name="id_persona"></param>
        /// <param name="id_linea"></param>
        /// <param name="monto"></param>
        /// <param name="id_periodo"></param>
        /// <param name="plazo"></param>
        /// <param name="id_credito"></param>
        /// <returns></returns>
        [Authorize(Roles = "Asistente,Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Carpeta_Azul(int id_persona, int id_linea, decimal monto, int id_periodo, int plazo, int id_credito = 0)
        {
            try
            {
                string id_usuario = _userManager.GetUserId(HttpContext.User);
                var amortizacion =  CuotaPrimaria(id_linea ,monto,id_periodo,plazo,id_credito);
                List<Amortizacion> datos= amortizacion.Amortizacion;
                Persona persona = await _context.Personas.AsNoTracking()
                    .Include(d => d.Creditos)
                    .Include(d => d.Persona_Referencias)
                    .SingleOrDefaultAsync(w => w.Id_Persona == id_persona);
                Persona_Relaciones relacion_codeudor = await Codeudor_Activo_Mutuo(id_persona);
                List<Persona_Relaciones> relaciones = new List<Persona_Relaciones>();
                relaciones.Add(relacion_codeudor);
                persona.Persona_Relaciones = relaciones;
                Usuario_Agencia agencia = await _context.Usuario_Agencia.AsNoTracking()
                    .SingleAsync(a => a.Id_Usuario == id_usuario);
                Agencia ubicacion = await _context.Agencia.AsNoTracking()
                    .SingleAsync(a => a.Id_Agencia == agencia.Id_Agencia);
                persona.Comentario = ubicacion.Ciudad;
                Credito credito = await _context.Credito.AsNoTracking().SingleAsync(a => a.Id_Credito == id_credito);
                Periodo periodo = await _context.Periodo.AsNoTracking().SingleAsync(a=>a.Id_Periodo == credito.Id_Periodo);
                Carpeta_Azul carpetaAzul = new Carpeta_Azul
                {
                    Persona = persona,
                    Amortizacion = datos,
                    Periodo = periodo
                };
                return Ok(carpetaAzul);
            }
            catch (Exception e)
            {
                return BadRequest("Algo anda mal, intentalo de nuevo");
            }
        }

        /// <summary>
        /// documento de plan ayc
        /// </summary>
        /// <param name="id_persona"></param>
        /// <param name="id_credito"></param>
        /// <returns></returns>
      //  [Authorize(Roles = "Asistente,Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Plan_AyC(int id_credito)
        {
            try
            {
                Credito credito = await _context
                    .Credito
                    .Include(d => d.Periodo)
                    .Include(p => p.Persona)
                    .AsNoTracking()
                    .SingleAsync(d => d.Id_Credito == id_credito);

                credito.Primer_Pago = CuotaPrimaria(
                        credito.Id_Linea,
                        credito.Monto_pago,
                        credito.Id_Periodo,
                        credito.Plazo,
                        credito.Id_Credito)
                    .Amortizacion
                    .Single(d => d.No == 1).Comision; /*+ 
                                      CuotaPrimaria(
                                              credito.Id_Linea,
                                              credito.Monto_pago,
                                              credito.Id_Periodo,
                                              credito.Plazo,
                                              credito.Id_Credito)
                                          .Amortizacion
                                          .Single(d => d.No == 1).IvaSobreComision;*/
                Persona persona = credito.Persona;
                persona.Creditos.Add(credito);
                return Ok(persona);
            }
            catch (Exception e)
            {
                return BadRequest(e.StackTrace);
            }
        }
        /// <summary>
        /// documento de plan ayc
        /// </summary>
        /// <param name="id_persona"></param>
        /// <param name="id_credito"></param>
        /// <returns></returns>
        [Authorize(Roles = "Asistente,Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPut]
        public async Task<IActionResult> Cambio_Fecha_Credito(int id_credito, DateTime fecha_nueva)
        {
            try
            {
                Credito credito_fecha = _context.Credito.AsNoTracking().Single(a=>a.Id_Credito == id_credito);
                if (fecha_nueva.GetHashCode() !=0 )
                {
                    credito_fecha.Fecha_ProntoPago = fecha_nueva;
                    _context.Entry(credito_fecha).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return Ok("Fecha Editada Correctamente");
                }
                else
                {
                    return BadRequest("Ingresa la fecha nueva");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.StackTrace);
            }
        }

        /// <summary>
        /// entrega acta con la lista de creditos aprobados
        /// </summary>
        /// <param name="id_acta">int id de acta</param>
        /// <returns>Lista de créditos</returns>
        [Authorize(Roles = "Jefe,Administrador,Asistente", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Creditos_Pendientes_Liquidar(DateTime fecha, int id_agencia = 0)
        {
            string userId = _userManager.GetUserId(HttpContext.User);
            List<Credito> creditos = new List<Credito>();
            NegocioContexto db = new NegocioContexto();
            Usuario_Agencia agencia_usuario;
            var asesores = new List<Cartera>();
            if (User.IsInRole("Jefe") || User.IsInRole("Asistente"))
            {
                agencia_usuario = await _context.Usuario_Agencia.AsNoTracking()
                    .SingleAsync(a => a.Id_Usuario == userId);
                asesores = (from c in _context.Cartera
                    join ua in _context.Usuario_Agencia on c.Id_Usuario equals ua.Id_Usuario
                    where ua.Id_Agencia == agencia_usuario.Id_Agencia
                    select c).ToList();
            }
            else if (User.IsInRole("Administrador") && id_agencia > 0)
            {
                asesores = (from c in _context.Cartera
                    join ua in _context.Usuario_Agencia on c.Id_Usuario equals ua.Id_Usuario
                    where ua.Id_Agencia == id_agencia
                    select c).ToList();
            }
            foreach (var credito in asesores)
            {
                bool existe = db.Credito.AsNoTracking().Any(t => t.Id_Cartera == credito.Id_Cartera && t.Aprobado_En.Date == fecha.Date && t.Liquidado == false);
                if (existe)
                {
                    creditos.AddRange(_context.Credito.AsNoTracking()
                        .Include(b=>b.Persona)
                        .Include(c=>c.Linea_Credito)
                        .Where(a=>a.Aprobado_En.Date == fecha.Date && a.Liquidado == false && a.Id_Cartera == credito.Id_Cartera).ToList());
                }
            }
            return Ok(creditos);
        }
        /*Manejo Mantenimiento Creditos Grupales*/

        /// <summary>
        /// Put Colocar id_acta para el id_credito seleccionado
        /// </summary>
        /// <param name="nombre_grupo"></param>
        /// <param name="id_agencia"></param>
        /// <returns></returns>
        [Authorize(Roles = "Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPut]
        public async Task<IActionResult> Agregar_Grupo(string nombre_grupo)
        {
            /*obtener agencia del usuario*/
            int id_agencia = Agencia_Usuario();
            if (ModelState.IsValid)
            {
                bool existe = await _context
                    .Credito_Grupal
                    .AnyAsync(
                        d => d.Nombre_grupo.ToUpper() == nombre_grupo.ToUpper() &&
                        d.Id_Agencia==id_agencia);
                if (existe)
                    return BadRequest("Ya existe un grupo con el mismo nombre en la misma agencia");

                Credito_Grupal grupal = new Credito_Grupal
                {
                    Nombre_grupo = nombre_grupo.ToLower(),
                    Id_Agencia=id_agencia
                };
                _context.Credito_Grupal.Add(grupal).State = EntityState.Added;
                await _context.SaveChangesAsync();
                return Ok("El grupo fue creado exitosamente");
            }
            else
            {
                return BadRequest(ModelState.ValidationState.ToString());
            }
        }

        [Authorize(Roles = "Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPut]
        public async Task<IActionResult> Agregar_Personas_Grupo(int id_credito,int id_grupo, bool es_lider = false)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool existe = await
                        _context.Detalle_Credito_Grupal.AnyAsync(d => d.Id_Credito == id_credito );
                    if (existe)
                    {
                        return BadRequest("Ya existe esta persona en otro grupo");
                    }

                    Detalle_Credito_Grupal Detalle_Credito_Grupal = new Detalle_Credito_Grupal
                    {
                       Id_Credito = id_credito,
                       Id_Credito_Grupal = id_grupo,
                       Lider = es_lider
                    };
                    _context.Detalle_Credito_Grupal.Add(Detalle_Credito_Grupal).State = EntityState.Added;
                    await _context.SaveChangesAsync();
                    return Ok("Persona Agregada Exitosamente");
                }
                else
                {
                    return BadRequest(ModelState.ValidationState.ToString());
                }
            }
            catch (Exception e)
            {
                return await Task.Run(() => BadRequest());
            }
        }


        [Authorize(Roles = "Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Consultar_Grupo(string nombre_grupo)
        {
            try
            {
                using (NegocioContexto db = new NegocioContexto())
                {
                    int id_agencia = Agencia_Usuario();

                    if (db.Credito_Grupal.Any(a => a.Nombre_grupo == nombre_grupo.ToLower() && a.Id_Agencia == id_agencia))
                    {
                        Credito_Grupal cr = _context.Credito_Grupal.Single(a => a.Nombre_grupo == nombre_grupo.ToLower() && a.Id_Agencia == id_agencia);
                        List<Detalle_Credito_Grupal> creditos = await _context
                            .Detalle_Credito_Grupal
                            .AsNoTracking()
                            .Include(c=>c.Credito)
                            .Include("Credito.Persona")
                            .Where(g => g.Id_Credito_Grupal == cr.Id_Credito_Grupal)
                            .ToListAsync();
                        return Ok(creditos);
                    }
                    else
                    {
                        return BadRequest("El grupo no existe");
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace);
            }
        }
        [Authorize(Roles = "Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Consulta_Grupo(string nombre_grupo)
        {
            try
            {
                using (NegocioContexto db = new NegocioContexto())
                {
                    if (db.Credito_Grupal.Any(a => a.Nombre_grupo == nombre_grupo.ToLower()))
                    {
                        Credito_Grupal grupo = _context.Credito_Grupal.Single(a => a.Nombre_grupo == nombre_grupo.ToLower());
                        return Ok(grupo);
                    }
                    else
                    {
                        return BadRequest("El grupo no existe");
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace);
            }
        }
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpDelete]
        public async Task<IActionResult> Borrar_Integrante_Grupo(int id_detalle_grupo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var detalle = await _context.Detalle_Credito_Grupal
                .SingleOrDefaultAsync(m => m.Id_Detalle_Credito_Grupal == id_detalle_grupo);
            if (detalle == null)
            {
                return NotFound();
            }
            try
            {
                _context.Detalle_Credito_Grupal.Remove(detalle);
                await _context.SaveChangesAsync();
                return Ok("Integrante del grupo eliminado exitosamente");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Roles = "Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPut]
        public async Task<IActionResult> Cambiar_Lider(int id_credito, int id_grupo, bool es_lider = false)
        {
            using (NegocioContexto db = new NegocioContexto())
            {
                try
                {
                    bool ya_es_lider = _context.Detalle_Credito_Grupal.Any(a => a.Id_Credito == id_credito && a.Lider);
                    if (!ya_es_lider)
                    {
                        List<Detalle_Credito_Grupal> integrantes = _context.Detalle_Credito_Grupal.AsNoTracking()
                                .Where(a => a.Id_Credito_Grupal == id_grupo).ToList();
                            Detalle_Credito_Grupal Detalle_Credito_Grupal = new Detalle_Credito_Grupal();

                            foreach (var detalleCreditoGrupal in integrantes)
                            {
                                if (detalleCreditoGrupal.Lider)
                                {
                                    Detalle_Credito_Grupal = new Detalle_Credito_Grupal
                                    {
                                        Id_Credito = detalleCreditoGrupal.Id_Credito,
                                        Id_Detalle_Credito_Grupal = detalleCreditoGrupal.Id_Detalle_Credito_Grupal,
                                        Id_Credito_Grupal = id_grupo,
                                        Lider = false
                                    };
                                    _context.Detalle_Credito_Grupal.Add(Detalle_Credito_Grupal).State = EntityState.Modified;
                                    await _context.SaveChangesAsync();
                                }
                            }
                            if (db.Detalle_Credito_Grupal.Any(a=>a.Id_Credito == id_credito))
                            {
                                foreach (var detalleCreditoGrupal in integrantes)
                                {
                                    if (detalleCreditoGrupal.Id_Credito == id_credito)
                                    {
                                        Detalle_Credito_Grupal = new Detalle_Credito_Grupal
                                        {
                                            Id_Credito = detalleCreditoGrupal.Id_Credito,
                                            Id_Detalle_Credito_Grupal = detalleCreditoGrupal.Id_Detalle_Credito_Grupal,
                                            Id_Credito_Grupal = id_grupo,
                                            Lider = true
                                        };
                                        _context.Detalle_Credito_Grupal.Add(Detalle_Credito_Grupal).State = EntityState.Modified;
                                        await _context.SaveChangesAsync();
                                        return Ok("Persona Agregada Exitosamente");
                                    }
                                }
                            }
                            else
                            {
                                Detalle_Credito_Grupal = new Detalle_Credito_Grupal
                                {
                                    Id_Credito = id_credito,
                                    Id_Credito_Grupal = id_grupo,
                                    Lider = es_lider
                                };
                                _context.Detalle_Credito_Grupal.Add(Detalle_Credito_Grupal).State = EntityState.Added;
                                await _context.SaveChangesAsync();
                                return Ok("Persona Agregada Exitosamente");
                        }
                            return Ok();
                    }
                    else
                    {
                        return Ok();
                    }
                }
                catch (Exception e)
                {
                    return await Task.Run(() => BadRequest());
                }
            }

           
        }
        [Authorize(Roles = "Asistente,Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Obtener_Fecha_Diferente(int id_credito)
        {
            try
            {
                Credito credito_fecha = _context.Credito.AsNoTracking().Single(a => a.Id_Credito == id_credito);
                if (credito_fecha.Fecha_ProntoPago.Date != DateTime.MinValue.Date)
                {
                    return Ok(credito_fecha.Fecha_ProntoPago.Date.ToString("yyyy-MM-dd"));
                }
                else
                {
                    return Ok();
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.StackTrace);
            }
        }

        /// <summary>
        /// para efectuar la congelación de un crédito
        /// </summary>
        /// <param name="id_credito"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [Authorize(Roles = "Asistente,Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPut]
        public async Task<IActionResult> Procesar_Congelar(
            int id_credito,
            DateTime fecha)
        {
            try
            {
                Credito credito = await _context.Credito.SingleAsync(c => c.Id_Credito == id_credito);
                credito.Congelado_En = fecha;
                credito.Congelado = true;
                _context.Entry(credito).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok("Se ha congelado el crédito a la fecha");
            }
            catch (Exception ex) {
                return BadRequest("No se pudo efectuar la congelación del crédito solicitado");
            }
        }

        /// <summary>
        /// disminuir capital mágicamente
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPut]
        public async Task<IActionResult> Cambiar_Capital(int id_credito,decimal monto)
        {
            if (_context
                .Transaccion_Credito
                .Any(d => d.Id_Credito == id_credito && !d.Borrado_Logico))
            {
                Transaccion_Credito ultima_transaccion = _context
                  .Transaccion_Credito
                  .Where(d => d.Id_Credito == id_credito && !d.Borrado_Logico)
                  .OrderBy(d => d.Id_Transaccion_Credito).Last();
                if (monto < 0)

                    return BadRequest("No se puede utilizar números negativos");
                ultima_transaccion.Saldo_Capital -= monto;
                _context.Entry(ultima_transaccion).State=EntityState.Modified;
                await _context.SaveChangesAsync();

                /*guardar el dato*/
                await Bitacora(
                    "Transacción Crédito",
                    "Disminución de Capital",
                    ultima_transaccion.Id_Transaccion_Credito,
                    true,
                    monto:monto);

                await Contabilidad_Cambiar_Capital(monto);
                return Ok("Se ha disminuído el capital");
            }
            else
                return BadRequest("No se puede hacer disminución de saldo a un crédito que no tiene pagos");
        }

        /// <summary>
        /// guardar la contabilidad de el cambio de capital
        /// </summary>
        /// <param name="monto"></param>
        /// <returns></returns>
        private async Task Contabilidad_Cambiar_Capital(decimal monto)
        {
            using (var dbContextTransaccion = _context.Database.BeginTransaction())
            { 
                try
                {
                    int id_agencia = Agencia_Usuario();
                    Conf_Partida_Contable conf = _context
                        .Conf_Partida_Contable
                        .Include(d => d.Conf_Detalles_Partidas_Contables)
                        .AsNoTracking()
                        .Single(
                            d => d.Id_Agencia == id_agencia
                            && d.Id_Otras_Operaciones == 4
                            && d.Identificador == "APROXIMACION_SALDO");
                    /*crear el asiento*/
                    Asiento_Contable asiento = new Asiento_Contable
                    {
                        Id_Transaccion_Caja = 0,
                        Fecha = DateTime.Now,
                        Concepto = "Aproximación de Saldo"
                    };
                    _context.Asiento_Contable.Add(asiento).State = EntityState.Added;
                    await _context.SaveChangesAsync();
                    List<Detalle_Asiento_Contable> detalle_asientos = new List<Detalle_Asiento_Contable>();
                    foreach (var detalle in conf.Conf_Detalles_Partidas_Contables)
                    {
                        Detalle_Asiento_Contable detalle_asiento = new Detalle_Asiento_Contable
                        {
                            Id_Agencia = id_agencia,
                            Id_Asiento_Contable = asiento.Id_Asiento_Contable,
                            Cargo = detalle.Cargo ? monto : 0.00M,
                            Abono = detalle.Abono ? monto : 0.00M,
                            Concepto = "Aproximación de Saldo",
                            EsCaja = false,
                            Cuenta_Contable = detalle.Cuenta_Contable
                        };
                        detalle_asientos.Add(detalle_asiento);
                    }
                    await _context.SaveChangesAsync();
                    dbContextTransaccion.Commit();
                }
                catch (Exception ex)
                {
                    dbContextTransaccion.Rollback();
                }
            }
        }
    }
}