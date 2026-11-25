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
using ModelNucleo.Model.Contabilidad;
using ModelNucleo.Model.ModelView;
using NucleoAyC.Entities;

namespace NucleoAyC.Controllers
{
    /// <summary>
    /// Transacciones simples
    /// propias de una caja en manera de bóveda
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = "Transaccion_Simple")]
    public class Transaccion_SimpleController : Base_TransaccionController
    {
        /// <summary>
        /// constructor e injección de credito
        /// </summary>
        /// <param name="context"></param>
        /// /// <param name="userManager"></param>
        /// <param name="roleManager"></param>
        private readonly CertificacionService _certificacionService;
        public Transaccion_SimpleController(NegocioContexto context,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
             CertificacionService certificacionService)
            : base(context, userManager, roleManager)
        {
            _certificacionService = certificacionService ?? throw new ArgumentNullException(nameof(certificacionService));

        }

        /// <summary>
        /// Es solo para asignar fondos a una caja específica
        /// </summary>
        /// <param name="id_caja">int receptor del dinero</param>
        /// <param name="monto">decimal</param>
        /// <returns></returns>
        [Authorize(Roles = "Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPut]
        public async Task<IActionResult> Asignar_Fondos(int id_caja, decimal monto)
        {
            try
            {
                Caja boveda_actual = await Boveda_Actual();
                /*salida de boveda*/
                await ManejarBoveda(monto, 1, false, boveda_actual.Id_Caja);
                /*receptor del dinero*/
                await ManejarBoveda(monto, 2, true, id_caja);
                return Ok("Los fondos se han asignado correctamente");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        private async Task<IActionResult> GenerarDTE(decimal valor, int IdAgencia)
        {
            string establecimiento = "";
            string PuntoVenta = "";
            string Departamento = "";
            string Municipio = "";
            string Distrito = "";

            if (IdAgencia != 0)
            {
                var agencia = _context.Agencia
                           .FirstOrDefault(u => u.Id_Agencia == IdAgencia);
                if (agencia?.Nombre != null)
                {
                    switch (agencia.Nombre)
                    {
                        case "Administrativa (01)":
                            establecimiento = "M001";
                            PuntoVenta = "P001";
                            Departamento = "02";
                            Municipio = "15";
                            Distrito = "10";
                            break;
                        case "Ahuachapán (02)":
                            establecimiento = "S001";
                            PuntoVenta = "P002";
                            Departamento = "01";
                            Municipio = "14";
                            Distrito = "01";
                            break;
                        case "Chalchuapa (03)":
                            establecimiento = "S002";
                            PuntoVenta = "P003";
                            Departamento = "02";
                            Municipio = "17";
                            Distrito = "03";
                            break;
                        case "Santa Ana (04)":
                            establecimiento = "S003";
                            PuntoVenta = "P004";
                            Departamento = "02";
                            Municipio = "15";
                            Distrito = "10";
                            break;
                        case "Sonsonate (05)":
                            establecimiento = "S004";
                            PuntoVenta = "P005";
                            Departamento = "03";
                            Municipio = "18";
                            Distrito = "15";
                            break;
                        case "Santa Tecla (07)":
                            establecimiento = "S005";
                            PuntoVenta = "P006";
                            Departamento = "05";
                            Municipio = "28";
                            Distrito = "09";
                            break;

                    }
                    var configuracion = _context.Configuracion_General.FirstOrDefault(a => a.Nombre == "DTE_Factura");

                    if (configuracion != null)
                    {
                        int valorActual = int.Parse(configuracion.Valor);

                        valorActual++;

                        configuracion.Valor = valorActual.ToString("D15");

                        _context.SaveChanges();
                    }
                    CertificacionRequest request = new CertificacionRequest
                    {
                        documento = new CertificacionRequest.DocumentoFactura
                        {
                            tipo_dte = "01",
                            establecimiento = establecimiento,
                            punto_venta = PuntoVenta,
                            condicion_pago = 1,
                            numero_control = "DTE-01-" + establecimiento + PuntoVenta + "-" + configuracion.Valor,
                            items = new List<CertificacionRequest.Item>
                        {
                        new CertificacionRequest.Item
                        {
                            tipo = 1,
                            cantidad = 1,
                            unidad_medida = 59,
                            descripcion = "Traslados de credito",
                            precio_unitario = (double)valor,
                            tipo_venta = "1",
                        },

                        },
                            retener_iva = false,
                            tipo_contingencia = null,
                            motivo_contingencia = null,
                            pagos = new List<CertificacionRequest.Pago>
                        {
                            new CertificacionRequest.Pago
                            {
                                tipo = "01",
                                plazo = "01",
                                periodo = 30,
                                monto = (double)valor
                            }
                        }
                        }
                    };

                    var response = _certificacionService.CertificarDocumento(request);

                    return Ok("Factura procesada exitosamente");

                }
                return Ok("Factura procesada exitosamente");
            }
            else
            {
                return BadRequest("Algo anda mal, contacte a soporte");
            }
        }

        /*Cuidado asignación de Fondo Inicial (por implementación)*/

        /// <summary>
        /// Fondo Inicial para abrir una agencia
        /// </summary>
        /// <param name="monto"></param>
        /// <param name="id_agencia"></param>
        /// <returns></returns>
        [Authorize(Roles = "Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPut]
        public async Task<IActionResult> Fondo_Inicial(decimal monto, int id_agencia)
        {
            Agencia agencia = await _context.Agencia.Include(d => d.Cajas)
                    .SingleAsync(c => c.Id_Agencia == id_agencia);
            Caja boveda = agencia.Cajas.FirstOrDefault(f => f.Boveda && f.Activa);
            await ManejarBoveda(monto, 2, true, boveda.Id_Caja);
            return Ok("Los fondos han sido asignados");
        }


        /// <summary>
        /// es la asignación de fondos a otra agencia
        /// </summary>
        /// <param name="monto"></param>
        /// <param name="id_agencia"></param>
        /// <returns></returns>
        [Authorize(Roles = "Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPut]
        public async Task<IActionResult> Remesar_Fondos(decimal monto, int id_agencia)
        {
            try
            {
                Caja boveda_actual = await Boveda_Actual();
                Agencia agencia = await _context.Agencia.Include(d => d.Cajas)
                    .SingleAsync(c => c.Id_Agencia == id_agencia);
                Caja caja = agencia.Cajas.FirstOrDefault(f => f.Boveda && f.Activa);
                await ManejarBoveda(monto, 1, false, boveda_actual.Id_Caja); //entrega el dinero
                if (caja != null) await ManejarBoveda(monto, 2, true, caja.Id_Caja); //recibe el dinero
                return Ok("La Transacción ha sido Realizada");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Solo para recibir fondos al cierre de una caja en el día
        /// como una bóveda
        /// </summary>
        /// <param name="id_caja">int</param>
        /// <param name="monto">decimal</param>
        /// <returns></returns>
        [Authorize(Roles = "Asistente,Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPut]
        public async Task<IActionResult> Recibir_Fondos_Cierre(int id_caja, decimal monto)
        {
            try
            {
                /*caja que entrega los fondos*/
                await ManejarBoveda(monto, 1, false, id_caja);
                Caja boveda_actual = await Boveda_Actual();
                /*salida de boveda*/
                await ManejarBoveda(monto, 2, true, boveda_actual.Id_Caja);

                return Ok("Los fondos han sido recibidos correctamente");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// es para que como bóveda se pueda remesar el dinero físico
        /// </summary>
        /// <param name="id_banco">int</param>
        /// <param name="monto">decimal</param>
        /// <param name="referencia"></param>
        /// <returns></returns>
        [Authorize(Roles = "Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPut]
        public async Task<IActionResult> Remesar(int id_banco, decimal monto, string referencia, string razon_movimiento)
        {
            try
            {
                Banco banco = _context.Banco.SingleOrDefault(i => i.Id_Banco == id_banco);
                await ManejarBoveda(monto, 3, false, 0, banco, no_comprobante: referencia,razon_movimiento:razon_movimiento);
                return Ok("Remesa registrado correctamente");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Colecta de terceros
        /// </summary>
        /// <param name="monto"></param>
        /// <returns></returns>
        [Authorize(Roles = "Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> Colectar_Bolsa_Tercero(decimal monto)
        {
            Caja boveda_actual = await Boveda_Actual();
            await ManejarBoveda(monto, 11, true, boveda_actual.Id_Caja);
            return Ok("Se ha registrado la bolsa de terceros");
        }

        /// <summary>
        /// ha sido pagada una acción
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Asistente,Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPut]
        public async Task<IActionResult> Cobrar_Accion(int id_accion)
        {
            string id_usuario = _userManager.GetUserId(HttpContext.User);
            Func<Accion,bool> cancelada = new Func<Accion, bool>(t => t.Id_Accion == id_accion && t.Cancelado);
            if (_context.Accion.AsNoTracking().Any(cancelada))
            {
                return BadRequest("La acción ya fue cobrada");
            }
            else {
                Accion accion = _context.Accion.Include(d => d.Persona).Single(d => d.Id_Accion == id_accion);
                accion.Cancelado = true;
                try
                {
                    Impresion impresion = new Impresion();
                    int id_persona = accion.Id_Persona;
                    _context.Entry(accion).State = EntityState.Modified;
                    Caja caja = await Caja_Actual();
                    await ManejarBoveda(accion.Valor_Total, 6, true, caja.Id_Caja, null, id_accion, 0, id_persona);
                    //si se cobra la accion es que guarda el perfil como socio
                    Persona persona = await _context.Personas.SingleAsync(p => p.Id_Persona == accion.Id_Persona);
                    persona.Estado_Socio = Estado_Socio.Activo;
                    _context.Entry(persona).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    Transaccion_Simple transaccion_accion = _context.Transaccion_Simple.AsNoTracking()
                        .Include(a=>a.Tipo_Operacion_Simple)
                        .Single(a => a.Id_Cobro_Acciones == id_accion);
                    Ahorro Linea_Ahorro =
                        _context.Ahorro.AsNoTracking().FirstOrDefault(a => a.Id_Persona == persona.Id_Persona);
                     Agencia agencia = _context
                       .Usuario_Agencia
                       .AsNoTracking()
                       .Include(d => d.Agencia)
                       .SingleOrDefault(g => g.Id_Usuario == id_usuario)
                       .Agencia;

                    await Manejo_Contabilidad_Otros_Cobros(
                            transaccion_accion.Id_Transaccion_Simple,
                            Otro_Cobro.Accion,
                            0,
                            Linea_Ahorro.Id_Linea_Ahorro);
                    string nombre_completo_cliente = "--";
                    string codigo = "--";
                    if (transaccion_accion.Id_Cobro_Acciones >0) {
                        nombre_completo_cliente = persona.NombreCompleto;
                        codigo = Linea_Ahorro.No_Ahorro;
                    }
                    string concepto = transaccion_accion.Tipo_Operacion_Simple.Descripcion;
                    //generar DTE
                    var dte = await GenerarDTE(accion.Valor_Total, agencia.Id_Agencia);
                    Factura factura = new Factura
                    {
                        Total = transaccion_accion.Monto,
                        Fecha = transaccion_accion.Creado_En,
                        No_Ahorro = codigo,
                        Agencia = agencia != null ? agencia.Nombre : "",
                        Cliente = nombre_completo_cliente,
                        Concepto = concepto,
                        Cajero = id_usuario,
                        Linea = "--",
                        Saldo_Capital = 0.00M,
                        Tasa = 0.00M
                    };
                    impresion.Factura = factura;
                    return Ok(impresion);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.StackTrace);
                }

           }
          
        }
        
        private async Task ManejarBoveda(
            decimal monto, 
            int id_tipo_operacion, 
            bool ingreso, 
            int id_caja = 0,
            Banco banco=null,
            int id_cobro_accion = 0,
            int id_deduccion = 0,
            int id_persona = 0,
            string no_comprobante="",
            string razon_movimiento = ""
        )
        {
            using (var dbContextTransaccion = _context.Database.BeginTransaction())
            {
                try
                {
                    DateTime fecha = DateTime.Now;
                    string Id_Usuario = _userManager.GetUserId(HttpContext.User);

                    if (_context.Caja.Any(d => d.Id_Usuario == Id_Usuario))//se verifica la caja antes de hacer todo
                    {
                        if (id_caja == 0)
                        {
                            Caja boveda = await Boveda_Actual();
                            if (boveda != null) id_caja = boveda.Id_Caja;
                            if (id_caja == 0) id_caja = Caja_Actual().Id;
                        }
                    }
                    else
                        throw new Exception("Este proceso no se puede realizar sin una caja o una bóveda");

                    Transaccion_Simple transaccion = new Transaccion_Simple
                    {
                        Id_Tipo_Operacion_Simple = id_tipo_operacion,
                        Monto = monto,
                        Id_Caja = id_caja,
                        //Id_Caja_Boveda=boveda_actual.Id_Caja,/*se va a eliminar*/
                        Id_Banco =banco!=null?banco.Id_Banco:0,
                        No_Cuenta= banco != null ? banco.No_Cuenta:"--",
                        Nombre_Banco= banco != null ? banco.Nombre:"--",
                        Creado_En = fecha,
                        Creado_Por = Id_Usuario,
                        Detalle_Factura_Servicio= razon_movimiento
                    };
                    //verificar datos si para deducciones o cobro de acciones
                    if (id_tipo_operacion == 6)
                        transaccion.Id_Cobro_Acciones = id_cobro_accion;
                    if (id_tipo_operacion == 7)
                        transaccion.Id_Deduccion = id_deduccion;
                    if (id_tipo_operacion == 6 || id_tipo_operacion == 7)
                        transaccion.Id_Persona = id_persona;
                    _context.Transaccion_Simple.Add(transaccion).State = EntityState.Added;
                    await _context.SaveChangesAsync();
                    //generador de comprobante
                    if(string.IsNullOrWhiteSpace(no_comprobante))
                        no_comprobante=await Generar_No_Comprobante();

                    //termina generador de comprobante
                    Transaccion_Caja trans_caja = Registrar_Caja(transaccion, id_caja, fecha, ingreso,comprobante:no_comprobante);
                    _context.Transaccion_Caja.Add(trans_caja).State = EntityState.Added;
                    await _context.SaveChangesAsync();
                    dbContextTransaccion.Commit();
                }
                catch (Exception ex)
                {
                    dbContextTransaccion.Rollback();
                    throw ex;
                }
            }
        }

        //manejo de transacciones simples manejables por la cajera
        /// <summary>
        /// transacciones por personas en la caja
        /// </summary>
        /// <param name="id_persona"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [Authorize(Roles = "Asistente,Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Estructuraciones(int id_persona, DateTime fecha)
        {
            try
            {
                List<Deduccion_Estructuriacion> simples =await (from t in _context.Transaccion_Simple
                            join c in _context.Credito on t.Id_Credito equals c.Id_Credito
                            join d in _context.Deduccion_Historial on t.Id_Deduccion equals d.Id_Deduccion_Historial
                            where 
                            t.Id_Tipo_Operacion_Simple == 7 &&
                            t.Id_Credito > 0 && t.Creado_En.Date == fecha.Date &&
                            c.Id_Persona==id_persona
                            select new Deduccion_Estructuriacion {
                                Transaciccion = t,
                                Concepto = d != null ? d.Descripcion : "Deducción"
                            }).ToListAsync();
                return Ok(simples);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace);
            }
        }


        /// <summary>
        /// se mostrará información de los comprobantes
        /// </summary>
        /// <param name="id_transaccion_simple"></param>
        /// <returns></returns>
        [Authorize(Roles = "Asistente,Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Mostrar_Comprobante_Factura(int id_transaccion_simple)
        {
            string id_usuario = _userManager.GetUserId(HttpContext.User);
            Transaccion_Simple transaccion = await _context
                .Transaccion_Simple
                .Include(t=>t.Tipo_Operacion_Simple)
                .SingleAsync(c=>c.Id_Transaccion_Simple==id_transaccion_simple);
            bool deduccion =  _context.Deduccion_Historial
                .AsNoTracking().Any(a => a.Id_Deduccion_Historial == transaccion.Id_Deduccion 
                && a.Id_Credito_Financiado>0);
            Impresion impresion = new Impresion();
            Agencia agencia = _context
                   .Usuario_Agencia
                   .AsNoTracking()
                   .Include(d => d.Agencia)
                   .SingleOrDefault(g => g.Id_Usuario == id_usuario)
                   .Agencia;
            if (transaccion != null)
            {
                await ActualizarCajaCobroEstructuracion(id_transaccion_simple, deduccion);
                string nombre_completo_cliente = "--";
                string codigo = "--";
                if (transaccion.Id_Credito > 0) {
                
                    Credito credito_persona = _context.Credito.AsNoTracking()
                        .Single(a => a.Id_Credito == transaccion.Id_Credito);
                    Persona persona = _context.Personas.AsNoTracking()
                        .Single(a => a.Id_Persona == credito_persona.Id_Persona);
                    nombre_completo_cliente = persona.NombreCompleto;
                    codigo = credito_persona.Codigo_Credito;
                }
                    
                string concepto = transaccion.Tipo_Operacion_Simple.Descripcion;
                //generar DTE
                var DTE = await GenerarDTE(transaccion.Monto, agencia.Id_Agencia);
                Factura factura = new Factura
                {
                    Total = transaccion.Monto,
                    Fecha = transaccion.Creado_En,
                    No_Ahorro= codigo,
                    Agencia = agencia != null ? agencia.Nombre : "",
                    Cliente = nombre_completo_cliente,
                    Concepto = concepto,
                    Cajero = id_usuario,
                    Linea = "--",
                    Saldo_Capital = 0.00M,
                    Tasa = 0.00M
                };
                impresion.Factura = factura;
                return Ok(impresion);
            }
            else
                return NoContent();
        }

        private async Task ActualizarCajaCobroEstructuracion(int id_transaccion_simple, bool es_refinanciamiento = false)
        {
            using (var abrirConexion = _context.Database.BeginTransaction())
            {
                try
                {
                    Caja caja = await Caja_Actual();
                    Func<Transaccion_Simple, bool> filtro_transaccion = new Func<Transaccion_Simple, bool>
                        (d => d.Id_Transaccion_Simple == id_transaccion_simple &&
                         d.Id_Tipo_Operacion_Simple == 7 &&
                         d.Id_Deduccion > 0);
                    if (_context.Transaccion_Simple.Any(filtro_transaccion))//se cambia las cajas
                    {
                        Transaccion_Simple transaccion = _context.Transaccion_Simple.Single(filtro_transaccion);
                        Credito credito = _context
                               .Credito
                               .Include(c => c.Persona)
                               .Single(d => d.Id_Credito == transaccion.Id_Credito);
                        if (transaccion != null && !es_refinanciamiento)
                        {
                            transaccion.Id_Caja = caja.Id_Caja;
                            _context.Transaccion_Simple.Update(transaccion).State = EntityState.Modified;
                            _context.SaveChanges();

                            /*conseguir la cuenta de ahorro*/
                            string id_linea_ahorro = _context.Configuracion_General.AsNoTracking().Single(a => a.Nombre == "LINEA_AHORRO_CLIENTE").Valor;

                            Ahorro ahorro = _context
                                .Ahorro
                                .FirstOrDefault(
                                    a => a.Id_Persona == credito.Persona.Id_Persona && a.Id_Linea_Ahorro == int.Parse(id_linea_ahorro));

                            string no_comprobante_simple = await Generar_No_Comprobante();
                            await GuardarSimple(ahorro.Id_Ahorro, transaccion.Monto, 3, true, false, no_comprobante_simple);

                            /*falta tener el ingreso*/
                            string no_comprobante_deduccion = await Generar_No_Comprobante();
                            DateTime fecha = DateTime.Now;
                            Transaccion_Caja trans_caja_deduccion = Registrar_Caja(transaccion, caja.Id_Caja, fecha, true, comprobante: no_comprobante_deduccion);
                            _context.Transaccion_Caja.Add(trans_caja_deduccion).State = EntityState.Added;
                            await _context.SaveChangesAsync();

                            await Manejo_Contabilidad_Otros_Cobros(
                                trans_caja_deduccion.Id_Transaccion_Simple,
                                Otro_Cobro.Deduccion,
                                id_linea_credito: credito.Id_Linea);
                        }
                        else {
                            /*falta tener el ingreso*/
                            string no_comprobante_deduccion = await Generar_No_Comprobante();
                            DateTime fecha = DateTime.Now;
                            Transaccion_Caja trans_caja_deduccion = Registrar_Caja(transaccion, 26, fecha, true, comprobante: no_comprobante_deduccion);
                            _context.Transaccion_Caja.Add(trans_caja_deduccion).State = EntityState.Added;
                            await _context.SaveChangesAsync();

                            await Manejo_Contabilidad_Otros_Cobros(
                                trans_caja_deduccion.Id_Transaccion_Simple,
                                Otro_Cobro.Deduccion,
                                id_linea_credito: credito.Id_Linea);
                        }
                        abrirConexion.Commit();
                    }
                    else
                        throw new Exception("No se encontró la transacción para poder actualizarla");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// se guarda la factura de servicio por la agencia
        /// </summary>
        /// <param name="id_factura_servicio"></param>
        /// <param name="no_comprobante"></param>
        /// <param name="monto"></param>
        /// <param name="monto_telefonia"></param>
        /// <param name="es_credito_fiscal"></param>
        /// <param name="fecha"></param>
        /// <param name="detalle_factura_servicio"></param>
        /// <returns></returns>
        [Authorize(Roles = "Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> Guardar_Factura_Servicio(
            int id_factura_servicio, 
            string no_comprobante,
            decimal monto,
            decimal monto_telefonia,
            bool es_credito_fiscal,
            DateTime fecha, 
            string detalle_factura_servicio)
        {
            using (var dbContextTransaccion = _context.Database.BeginTransaction())
            {
                try
                {
                    decimal valor_monto = monto + monto_telefonia;
                    int id_agencia = Agencia_Usuario();
                    Caja caja = await Caja_Actual();

                    decimal valor_iva = 0.00M;
                    if (es_credito_fiscal)
                        valor_iva = monto / (IVA * 1);

                    string Id_Usuario = _userManager.GetUserId(HttpContext.User);

                    Transaccion_Simple transaccion = new Transaccion_Simple
                    {
                        Id_Factura_Servicio = id_factura_servicio,
                        Id_Tipo_Operacion_Simple = 5,
                        No_Cuenta = no_comprobante,
                        Monto = valor_monto,
                        Iva_Monto = valor_iva,
                        Id_Caja = caja.Id_Caja,
                        Creado_En=fecha,
                        Creado_Por= Id_Usuario,
                        Detalle_Factura_Servicio = detalle_factura_servicio

                    };
                    _context.Entry(transaccion).State = EntityState.Added;
                    await _context.SaveChangesAsync();

                    Transaccion_Caja trans_caja = Registrar_Caja(transaccion, caja.Id_Caja, fecha, false, comprobante: no_comprobante);
                    _context.Transaccion_Caja.Add(trans_caja).State = EntityState.Added;
                    await _context.SaveChangesAsync();
                    await Manejo_Contabilidad_Otros_Cobros(transaccion.Id_Transaccion_Simple,Otro_Cobro.Servicio,0,0, id_factura_servicio );
                    dbContextTransaccion.Commit();
                    return Ok("Se ha procesado la factura de servicio");
                }
                catch (Exception ex)
                {
                    dbContextTransaccion.Rollback();
                    return BadRequest(ex.InnerException.Message);
                }
            }
        }

        /// <summary>
        /// es catálogo de las lista de factura de servicios
        /// que se pueden efectuar
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Conceptos_Facturas_Servicios()
        {
            List<Factura_Servicio> conceptos =await _context
                .Factura_Servicios
                .AsNoTracking()
                .Where(a=>!a.Es_Servicio_Basico)
                .ToListAsync();
            return Ok(conceptos);
        }
        /// <summary>
        /// es catálogo de las lista de factura de servicios básicos 
        /// que se pueden efectuar
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Conceptos_Facturas_Servicios_Basicos()
        {
            List<Factura_Servicio> conceptos = await _context
                .Factura_Servicios
                .AsNoTracking()
                .Where(a => a.Es_Servicio_Basico)
                .ToListAsync();
            return Ok(conceptos);
        }

        /// <summary>
        /// es catálogo de las lista de factura de servicios
        /// que se pueden efectuar
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Otras_Operaciones()
        {
            List<Otras_Operaciones> Operaciones = await _context
                .Otras_Operaciones
                .AsNoTracking()
                .ToListAsync();
            return Ok(Operaciones);
        }

        [Authorize(Roles = "Asistente,Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> Pago_Libreta(int id_ahorro,bool retiro_cuenta = false)
        {
            using (var dbContextTransaccion = _context.Database.BeginTransaction())
            {
                try
                {
                    string no_comprobante = "";
                    bool ingreso =false;
                    Ahorro ahorro = _context.Ahorro.AsNoTracking().FirstOrDefault(a => a.Id_Ahorro == id_ahorro);
                    Libreta_Transaccion libreta = await _context.LibretaTransacciones.AsNoTracking()
                        .SingleAsync(b => b.Id_Ahorro == ahorro.Id_Ahorro && b.Activo == true);
                    // verificar saldo en cuenta de ahorro
                    decimal saldo = 0.00M;
                    if (_context.Transaccion_Ahorro.Any(w => w.Id_Ahorro == ahorro.Id_Ahorro))
                        saldo = _context
                            .Transaccion_Ahorro
                            .Where(w => w.Id_Ahorro == ahorro.Id_Ahorro && !w.Borrado_Logico)
                            .OrderBy(d => d.Id_Ahorro).AsEnumerable().Last().Saldo;
                    var monto = _context.Configuracion_General.AsNoTracking().Single(d => d.Nombre == "Pago_Libreta");
                    decimal valor = Convert.ToDecimal(monto.Valor);/*VARIABLE DE SISTEMA*/
                    string comprobante = await Generar_No_Comprobante();

                    if (retiro_cuenta)
                    { 
                        if(saldo >= valor)
                            await GuardarSimple(ahorro.Id_Ahorro, valor, 3, true, false, comprobante);
                        else
                            return BadRequest("No tiene suficiente fondo para retirar la cantidad requerida");
                    }
                    
                    string Id_Usuario = _userManager.GetUserId(HttpContext.User);
                    Caja caja = await _context.Caja.AsNoTracking().SingleAsync(c => c.Id_Usuario == Id_Usuario & c.Boveda == false);
                    
                    if (_context.Caja.Any(d => d.Id_Usuario == Id_Usuario))
                    {
                        if (caja.Id_Caja == 0)
                        {
                            Caja boveda = await Boveda_Actual();
                            if (boveda != null) caja.Id_Caja = boveda.Id_Caja;
                            if (caja.Id_Caja == 0) caja.Id_Caja = Caja_Actual().Id;
                        }
                        DateTime fecha = DateTime.Now;
                        Transaccion_Simple transaccion = new Transaccion_Simple
                        {
                            Id_Tipo_Operacion_Simple = 10,
                            Monto = decimal.Parse(monto.Valor),
                            Id_Caja = caja.Id_Caja,
                            Creado_En = fecha,
                            Creado_Por = Id_Usuario,
                            Id_Libreta = libreta.Id_Libreta

                        };
                        _context.Transaccion_Simple.Add(transaccion).State = EntityState.Added;
                        await _context.SaveChangesAsync();
                        if (string.IsNullOrWhiteSpace(no_comprobante))
                            no_comprobante = await Generar_No_Comprobante();

                        Transaccion_Caja trans_caja = Registrar_Caja(transaccion, caja.Id_Caja, fecha, true, comprobante: no_comprobante);
                        _context.Transaccion_Caja.Add(trans_caja).State = EntityState.Added;
                        await _context.SaveChangesAsync();

                        await Manejo_Contabilidad_Otros_Cobros(
                            transaccion.Id_Transaccion_Simple,
                            Otro_Cobro.Libreta, 0, 0,0,2);
                        dbContextTransaccion.Commit();
                        
                    }
                    //generar DTE
                    var dte = GenerarDTE(valor, caja.Id_Agencia);
                    Transaccion_Simple factura = await _context.Transaccion_Simple
                        .AsNoTracking().SingleAsync(a=>a.Id_Libreta == libreta.Id_Libreta);
                    var mostrar_factura = await Mostrar_Factura(factura.Id_Transaccion_Simple);
                    return Ok(mostrar_factura);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.StackTrace);
                }
            }
        }

        [Authorize(Roles = "Asistente,Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Lista_Libretas(int id_persona)
        {
            List<Ahorro> Ahorro =  _context.Ahorro.AsNoTracking().Where(a => a.Id_Persona == id_persona).ToList();
            foreach (var Libretas_Ahorro in Ahorro)
            {
                Libreta_Transaccion libreta = await _context.LibretaTransacciones.AsNoTracking()
                    .SingleAsync(c => c.Id_Ahorro == Libretas_Ahorro.Id_Ahorro && c.Activo == true);
                Func<Transaccion_Simple, bool> func_relaciones = new Func<Transaccion_Simple, bool>(t => t.Id_Libreta == libreta.Id_Libreta);
                bool esta_pagada = _context.Transaccion_Simple.Any(func_relaciones);
                if (esta_pagada != true)
                {
                    List<Libreta_Transaccion> libretas = await _context.LibretaTransacciones.AsNoTracking()
                        .Where(b => b.Id_Ahorro == Libretas_Ahorro.Id_Ahorro && b.Activo == true).ToListAsync();
                    return Ok(libretas);
                }
                return BadRequest("Las libretas estan pagadas");
            }
            return Ok("");
        }

        private async Task<Impresion> Mostrar_Factura(int id_transaccion_simple)
        {
            string id_usuario = _userManager.GetUserId(HttpContext.User);
            Transaccion_Simple transaccion = await _context
                .Transaccion_Simple
                .Include(t => t.Tipo_Operacion_Simple)
                .SingleAsync(c => c.Id_Transaccion_Simple == id_transaccion_simple);
            Impresion impresion = new Impresion();
            Agencia agencia = _context
                .Usuario_Agencia
                .AsNoTracking()
                .Include(d => d.Agencia)
                .SingleOrDefault(g => g.Id_Usuario == id_usuario)
                .Agencia;
            //if (transaccion != null)
            //{
                string nombre_completo_cliente = "--";
                string No_Ahorro = "--";
            if (transaccion.Id_Libreta > 0) {
                Libreta_Transaccion libreta = _context.LibretaTransacciones.AsNoTracking()
                    .Single(a=>a.Id_Libreta == transaccion.Id_Libreta);
                Ahorro ahorro_libreta = _context.Ahorro.AsNoTracking()
                    .Single(a=>a.Id_Ahorro == libreta.Id_Ahorro);
                Persona persona_libreta = _context.Personas.AsNoTracking()
                    .Single(a=>a.Id_Persona == ahorro_libreta.Id_Persona);
                nombre_completo_cliente = persona_libreta.NombreCompleto;
                No_Ahorro = ahorro_libreta.No_Ahorro;
               }
                    
                string concepto = transaccion.Tipo_Operacion_Simple.Descripcion;

                Factura factura = new Factura
                {
                    Total = transaccion.Monto,
                    Fecha = transaccion.Creado_En,
                    No_Ahorro = No_Ahorro,
                    Agencia = agencia != null ? agencia.Nombre : "",
                    Cliente = nombre_completo_cliente,
                    Concepto = concepto,
                    Cajero = id_usuario,
                    Linea = "--",
                    Saldo_Capital = 0.00M,
                    Tasa = 0.00M
                };
                impresion.Factura = factura;
                return impresion;
           // }
           
        }

        /// <summary>
        /// solo para ejecutar una vez
        /// </summary>
        [Authorize(Roles = "Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public void ReemplazandoCodigos(int id_agencia,bool es_credito=true)
        {
            if (es_credito)
                GenerarCreditoAgenciaCodigo(id_agencia);
            else
                GenerarAhorroAgenciaCodigo(id_agencia);
        }
        /// <summary>
        /// solo para ejecutar una vez
        /// </summary>
        [Authorize(Roles = "Asesor,Administrador,Gerente,Jefe", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Movimiento_Diario_Prestamos(DateTime fecha,
            int id_cartera = 0, int id_agencia = 0)
        {
            var asesores = new List<Cartera>();
            string userId = _userManager.GetUserId(HttpContext.User);
            int id_cartera_correcto = 0;

            try
            {
                if (User.IsInRole("Asesor"))
                {
                    Cartera ua = _context.Cartera.AsNoTracking().Single(a => a.Id_Usuario == userId);
                    id_cartera_correcto = ua.Id_Cartera;
                }
                else if (User.IsInRole("Gerente") || User.IsInRole("Jefe"))
                {
                    string Id_Usuario = _userManager.GetUserId(HttpContext.User);
                    Usuario_Agencia ua = _context.Usuario_Agencia.AsNoTracking().Single(a => a.Id_Usuario == Id_Usuario);
                    id_agencia = ua.Id_Agencia;
                    id_cartera_correcto = id_cartera;
                }
                else
                {
                    id_cartera_correcto = id_cartera;
                }
                if (User.IsInRole("Administrador") | User.IsInRole("Gerente") | User.IsInRole("Jefe") && id_agencia > 0 && id_cartera == 0)
                {
                    asesores = (from c in _context.Cartera
                                join ua in _context.Usuario_Agencia on c.Id_Usuario equals ua.Id_Usuario
                                where ua.Id_Agencia == id_agencia
                                select c).ToList();


                    List<Movimiento_Diario_Prestamos> rama = new List<Movimiento_Diario_Prestamos>();
                    foreach (var asesor in asesores)
                    {
                        var lineas = _context.Credito
                            .Include(d => d.Linea_Credito)
                            .Where(d => d.Id_Cartera == asesor.Id_Cartera)
                            .Select(d => d.Linea_Credito).Distinct().ToList();

                        List<Linea_Credito_Movimiento> lienasCartera = new List<Linea_Credito_Movimiento>();

                        foreach (var linea in lineas)
                        {
                            var creditos = _context.Credito.AsNoTracking()
                                .Where(v => v.Id_Linea == linea.Id_Linea && v.Id_Cartera == asesor.Id_Cartera).ToList();

                            List<Datos_Movimientos_Diarios> movimientos = new List<Datos_Movimientos_Diarios>();

                            foreach (var credito in creditos)
                            {
                                var tiene_transaccion = _context
                                    .Transaccion_Credito
                                    .Any(
                                        d => d.FechaPago.Date == fecha.Date &&
                                        d.Id_Credito == credito.Id_Credito);

                                if (tiene_transaccion)
                                {
                                    var transaccion = _context
                                    .Transaccion_Credito
                                    .Include(a=>a.Credito)
                                    .Include(a => a.Credito.Persona)
                                    .Where(
                                        d => d.FechaPago.Date == fecha.Date &&
                                        d.Id_Credito == credito.Id_Credito).ToList();
                                    foreach (var tr in transaccion)
                                    {
                                        var datos = new Datos_Movimientos_Diarios
                                        {
                                            valor = tr.Valor_Cuota,
                                            codigo = tr.Credito.Codigo_Credito,
                                            persona = tr.Credito.Persona.NombreCompleto
                                        };
                                        movimientos.Add(datos);
                                    }
                                }
                            }
                            List<Linea_Credito> tiene_tran = (from tc in _context.Transaccion_Caja
                                                                     join tr in _context.Transaccion_Credito on tc.Id_Transaccion_Credito equals tr.Id_Transaccion_Credito
                                                                     join cr in _context.Credito on tr.Id_Credito equals cr.Id_Credito
                                                                     join li in _context.Linea_Credito on cr.Id_Linea equals li.Id_Linea
                                                                     where tc.Fecha == fecha.Date
                                                                     && !tc.Borrado_Logico && li.Id_Linea == linea.Id_Linea
                                                                     select li).ToList();
                            if(movimientos.Count>0 && tiene_tran.Count >0)
                                lienasCartera.Add(new Linea_Credito_Movimiento
                                {
                                    Linea_Credito = linea.Descripcion_linea,
                                    datos = movimientos
                                });
                        }
                        List<Cartera> tiene_transaccion_linea = (from tc in _context.Transaccion_Caja
                                                       join tr in _context.Transaccion_Credito on tc.Id_Transaccion_Credito equals tr.Id_Transaccion_Credito
                                                       join cr in _context.Credito on tr.Id_Credito equals cr.Id_Credito
                                                       join pe in _context.Personas on cr.Id_Persona equals pe.Id_Persona
                                                       join ca in _context.Cartera on pe.Id_Cartera equals ca.Id_Cartera
                                                       where tc.Fecha == fecha.Date 
                                                       && !tc.Borrado_Logico && ca.Id_Cartera == asesor.Id_Cartera
                                                       select ca).ToList();
                        if (tiene_transaccion_linea.Count>0) {
                            rama.Add(new Movimiento_Diario_Prestamos
                            {
                                Cartera = asesor.Id_Usuario,
                                Generado_Por= userId,
                                datos_linea = lienasCartera
                            });
                        }
                       
                    }
                    
                    return Ok(rama);
                }
                else
                {
                    return BadRequest("Algo anda mal");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace);
            }
           
            
        }
        [Authorize(Roles = "Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Bitacora(int id_agencia, DateTime fecha_inicio, DateTime fecha_final)
        {
            string nombre_jefa = "";
            List<Bitacora> bitacora = new List<Bitacora>();
            string userId = _userManager.GetUserId(HttpContext.User);
            if (id_agencia > 0)
            {

                List<Usuario_Agencia> usuario = _context.Usuario_Agencia.AsNoTracking().Where(a => a.Id_Agencia == id_agencia).ToList();

                foreach (var es_jefa in usuario)
                {
                    IdentityUser usere = await _userManager.FindByNameAsync(es_jefa.Id_Usuario);
                    var user = await _userManager.FindByEmailAsync(usere.Email);
                    var usuario_jefa = await _userManager.GetRolesAsync(user);
                    if (usuario_jefa.FirstOrDefault() == "Jefe")
                    {
                        nombre_jefa = es_jefa.Id_Usuario;
                    }
                }
                bitacora = _context.Bitacora.AsNoTracking().Where(a => a.Id_Usuario == nombre_jefa
                && a.Fecha_Proceso.Date >= fecha_inicio.Date && a.Fecha_Proceso.Date <= fecha_final.Date).ToList();
               
            }
            else {
                bitacora = _context.Bitacora.AsNoTracking().Where(a => a.Fecha_Proceso.Date >= fecha_inicio.Date
                && a.Fecha_Proceso.Date <= fecha_final.Date).ToList();
              
            }
           
            return Ok(bitacora);
        }
    }
}