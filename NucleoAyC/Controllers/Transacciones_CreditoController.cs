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
using ModelNucleo.Util;
using Newtonsoft.Json;
using NucleoAyC.Entities;
using NucleoAyC.Migrations;
using Org.BouncyCastle.Utilities;


namespace NucleoAyC.Controllers
{
    /// <summary>
    /// Métodos de transacciones de credito
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = "Transacciones_Credito")]
    public class Transacciones_CreditoController : Base_TransaccionController
    {
        private readonly CertificacionService _certificacionService;
        public Transacciones_CreditoController(
         NegocioContexto context,
         UserManager<IdentityUser> userManager,
         RoleManager<IdentityRole> roleManager,
         CertificacionService certificacionService) // Agregar el servicio aquí
         : base(context, userManager, roleManager)
            {
            _certificacionService = certificacionService ?? throw new ArgumentNullException(nameof(certificacionService));
        }

        /// <summary>
        /// Get Lista de la amortizacón ya efectuadas
        /// por el cliente
        /// </summary>
        /// <param name="id_credito">int id de un crédito</param>
        /// <returns>Lista de Transaccion_Credito</returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente, Supervisor,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<List<Transacciones_para_Credito>> Transacciones(int id_credito)
        {
            try
            {
                List<Transaccion_Credito> transacciones = await _context
                    .Transaccion_Credito
                    .Where(tc => tc.Id_Credito == id_credito && !tc.Borrado_Logico)
                    .OrderBy(w => w.Id_Transaccion_Credito).ToListAsync();
                List<Transacciones_para_Credito> vistas = new List<Transacciones_para_Credito>();
                foreach (var trans in transacciones)
                {
                    //Func<Transaccion_Caja, bool> filtrar_caja = new Func<Transaccion_Caja, bool>(t => t.Id_Transaccion_Credito == trans.Id_Transaccion_Credito);
                    if (_context.Transaccion_Caja.Any(t => t.Id_Transaccion_Credito == trans.Id_Transaccion_Credito))
                    {
                        Transaccion_Caja trans_caja = _context.
                            Transaccion_Caja
                            .Include(x => x.Caja).Single(t => t.Id_Transaccion_Credito == trans.Id_Transaccion_Credito);

                        vistas.Add(new Transacciones_para_Credito
                        {
                            Transacciones = trans,
                            Caja = trans_caja.Caja,
                            Transaccion_Caja = trans_caja
                        });
                    }
                    else
                    {
                        vistas.Add(new Transacciones_para_Credito
                        {
                            Transacciones = trans
                        });
                    }

                }
                vistas.OrderByDescending(w => w.Transacciones.Id_Transaccion_Credito);
                return vistas
                             .OrderByDescending(w => w.Transacciones.FechaPago)
                             .ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        private async Task<List<Transaccion_Credito>> Transaccion_Credito(int id_credito)
        {
                if (_context.Transaccion_Credito.AsNoTracking().Any(a => a.Id_Credito == id_credito))
                    return await _context.Transaccion_Credito
                    .AsNoTracking()
                    .Where(tc => tc.Id_Credito == id_credito && !tc.Borrado_Logico)
                    .OrderByDescending(w => w.Id_Transaccion_Credito)
                    .ToListAsync();
                return null;
        }
        /// <summary>
        /// Get Se obtendrá una transacción por su id
        /// </summary>
        /// <param name="id_transaccion_credito">int id de transacción de crédito</param>
        /// <returns>Transaccion_Credito objeto json</returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente, Supervisor,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<Transaccion_Credito> TransaccionPorId(int id_transaccion_credito)
        {
            return await _context.Transaccion_Credito.FirstOrDefaultAsync(y => y.Id_Transaccion_Credito == id_transaccion_credito);
        }

        /// <summary>
        /// Post Se guarda un pago del crédito
        /// </summary>
        /// <param name="transaccion">Transaccion_Credito objeto json</param>
        /// <returns></returns>
        [Authorize(Roles = "Asistente,Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> GuardarPago([FromBody] Transaccion_Credito transaccion, string comprobante, bool recargo = false)
        {
            var errores = new List<string>();
            var credito = _context.Credito
                        .Include(d => d.Persona)
                        .Include(d => d.Cartera)
                        .FirstOrDefault(d => d.Id_Credito == transaccion.Id_Credito);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (string.IsNullOrWhiteSpace(credito.Persona.Telefono))
            {
                errores.Add("El teléfono de la persona es obligatorio.");
            }

            if (string.IsNullOrWhiteSpace(credito.Persona.Domicilio))
            {
                errores.Add("El domicilio de la persona es obligatorio.");
            }

            if (string.IsNullOrWhiteSpace(credito.Persona.Correo))
            {
                errores.Add("El correo de la persona es obligatorio.");
            }

            if (string.IsNullOrWhiteSpace(credito.Persona.Dui) || credito.Persona.Dui.Length < 10)
            {
                errores.Add("El DUI de la persona debe tener al menos 10 dígitos.");
            }
            if (errores.Any())
            {
                return BadRequest(string.Join(" ", errores));
            }
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                if (transaccion.Recargo >= transaccion.Valor_Cuota)
                {
                    transaccion.Recargo = transaccion.Valor_Cuota;
                }
                try
                {
                      await EfectuarPago(transaccion, comprobante,false, recargo);
                
                    dbContextTransaction.Commit();
                    return Ok("Pago efectuado Exitosamente");
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    return BadRequest(ex.StackTrace);
                }
            }
        }
        [HttpPost]
        public async Task<IActionResult> GenerarDTEPagoCredito(int IDtransaccion)
        {
            try
            {
                var transaccion = _context.Transaccion_Credito.FirstOrDefault(a => a.Id_Transaccion_Credito == IDtransaccion);
                var credito = _context.Credito
                      .Include(d => d.Persona)
                      .Include(d => d.Cartera)
                      .FirstOrDefault(d => d.Id_Credito == transaccion.Id_Credito);
                //obtener informacion para DTE
                string establecimiento = "";
                string PuntoVenta = "";
                string Departamento = "";
                string Municipio = "";
                if (transaccion.Interes > 0 || transaccion.Interes_Comision > 0 || transaccion.Recargo > 0)
                {
                    if (credito?.Cartera != null)
                    {
                        var agencia = _context.Usuario_Agencia
                            .Include(u => u.Agencia)
                            .FirstOrDefault(u => u.Id_Usuario == credito.Cartera.Id_Usuario);

                        if (agencia?.Agencia?.Nombre != null)
                        {
                            switch (agencia.Agencia.Nombre)
                            {
                                case "Administrativa (01)":
                                    establecimiento = "M001";
                                    PuntoVenta = "P001";
                                    Departamento = "02";
                                    Municipio = "15";
                                    break;
                                case "Ahuachapán (02)":
                                    establecimiento = "S001";
                                    PuntoVenta = "P002";
                                    Departamento = "01";
                                    Municipio = "14";

                                    break;
                                case "Chalchuapa (03)":
                                    establecimiento = "S002";
                                    PuntoVenta = "P003";
                                    Departamento = "02";
                                    Municipio = "17";
                                    break;
                                case "Santa Ana (04)":
                                    establecimiento = "S003";
                                    PuntoVenta = "P004";
                                    Departamento = "02";
                                    Municipio = "15";
                                    break;
                                case "Sonsonate (05)":
                                    establecimiento = "S004";
                                    PuntoVenta = "P005";
                                    Departamento = "03";
                                    Municipio = "18";
                                    break;
                                case "Santa Tecla (07)":
                                    establecimiento = "S005";
                                    PuntoVenta = "P006";
                                    Departamento = "05";
                                    Municipio = "28";
                                    break;

                            }
                        }
                    }
                    var configuracion = _context.Configuracion_General.FirstOrDefault(a => a.Nombre == "DTE_Factura");
                    var email = "";
                    if (credito.Persona.Correo == null || credito.Persona.Correo == "")
                    {
                        email = "clientessincorreoayc@gmail.com";
                    }
                    else
                    {
                        email = credito.Persona.Correo;
                    }
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
                            receptor = new CertificacionRequest.Receptor
                            {
                                tipo_documento = "13",
                                numero_documento = credito.Persona.Dui.Trim(),
                                correo = email,
                                correos_cc = "backupcreditosayc@gmail.com",
                                nombre = credito.Persona.NombreCompleto,
                                telefono = credito.Persona.Telefono.Trim(),
                                direccion = new CertificacionRequest.Direccion
                                {
                                    departamento = Departamento,
                                    municipio = Municipio,
                                    complemento = credito.Persona.Domicilio.Trim()
                                }
                            },
                            items = new List<CertificacionRequest.Item>(),
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
                                    monto = Math.Round(
                                                Math.Abs((double)transaccion.Interes_Comision) +
                                                Math.Abs((double)transaccion.Recargo) +
                                                Math.Abs((double)transaccion.Interes),2)
                                }
                            }
                        }
                    };

                    // Validar y agregar el ítem "Comisiones"
                    double precioComisiones = Math.Round(Math.Abs((double)transaccion.Recargo) + Math.Abs((double)transaccion.Interes), 2);
                    if (precioComisiones > 0)
                    {
                        request.documento.items.Add(new CertificacionRequest.Item
                        {
                            tipo = 1,
                            cantidad = 1,
                            unidad_medida = 59,
                            descripcion = "Comisiones",
                            precio_unitario = precioComisiones,
                            tipo_venta = "1",
                        });
                    }

                    // Agregar el ítem "Intereses"
                    request.documento.items.Add(new CertificacionRequest.Item
                    {
                        tipo = 1,
                        cantidad = 1,
                        unidad_medida = 59,
                        descripcion = "Intereses",
                        precio_unitario = Math.Round((double)Math.Abs(transaccion.Interes_Comision), 2),
                        tipo_venta = "3",
                    });
                    var response = _certificacionService.CertificarDocumento(request);
                    var result = JsonConvert.DeserializeObject<dynamic>(response);

                    if (result.ok == false)
                    {
                        string erroresJson = JsonConvert.SerializeObject(result.errores);
                        var erroresDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(erroresJson);

                        string erroresDte = string.Join(", ", erroresDict.Select(e => $"{e.Key}: {e.Value}"));
                        await Bitacora(
                            "DTE",
                            "Generación DTE, error: " + erroresDte,
                            credito.Id_Credito,
                            false,
                            true,
                            monto: transaccion.Interes + transaccion.Interes_Comision + transaccion.Recargo
                        );
                        return BadRequest("Error al generar DTE: " + erroresDte);
                    }
                    await Bitacora(
                                "DTE",
                                "Generación DTE exitosa. Número control: " + request.documento.numero_control,
                                credito.Id_Credito,
                                false,
                                true,
                                monto: transaccion.Interes + transaccion.Interes_Comision + transaccion.Recargo
                            );
                                            }
                return Ok("Documento autenticado y generado exitosamente");

            }
            catch (Exception ex)
            {
                return BadRequest("Error al generar DTE");
            }
        }
        private async Task<IActionResult> GenerarDTE(decimal valor, int IdAgencia)
        {
            string establecimiento = "";
            string PuntoVenta = "";
            string Departamento = "";
            string Municipio = "";

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
                            break;
                        case "Ahuachapán (02)":
                            establecimiento = "S001";
                            PuntoVenta = "P002";
                            Departamento = "01";
                            Municipio = "14";

                            break;
                        case "Chalchuapa (03)":
                            establecimiento = "S002";
                            PuntoVenta = "P003";
                            Departamento = "02";
                            Municipio = "17";
                            break;
                        case "Santa Ana (04)":
                            establecimiento = "S003";
                            PuntoVenta = "P004";
                            Departamento = "02";
                            Municipio = "15";
                            break;
                        case "Sonsonate (05)":
                            establecimiento = "S004";
                            PuntoVenta = "P005";
                            Departamento = "03";
                            Municipio = "18";
                            break;
                        case "Santa Tecla (07)":
                            establecimiento = "S005";
                            PuntoVenta = "P006";
                            Departamento = "05";
                            Municipio = "28";
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
      //  [Authorize(Roles = "Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> GenerarDteRefinanciamientos(decimal totalGravados, decimal totalExcentos, int IdAgencia)
        {
            var dte = await FacturarTrasladosRefinanciamientos(totalGravados, totalExcentos, IdAgencia);
            if (dte.Equals("Error"))
            {
                return BadRequest("Error al generar el DTE");
            }
            else
            {
                return Ok(dte);
            }
        }
        private async Task<string> FacturarTrasladosRefinanciamientos( decimal totalGravados,decimal totalExcentos, int IdAgencia)
        {
            string establecimiento = "";
            string PuntoVenta = "";
            string Departamento = "";
            string Municipio = "";

            if(IdAgencia != 0)
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
                            break;
                        case "Ahuachapán (02)":
                            establecimiento = "S001";
                            PuntoVenta = "P002";
                            Departamento = "01";
                            Municipio = "14";

                            break;
                        case "Chalchuapa (03)":
                            establecimiento = "S002";
                            PuntoVenta = "P003";
                            Departamento = "02";
                            Municipio = "17";
                            break;
                        case "Santa Ana (04)":
                            establecimiento = "S003";
                            PuntoVenta = "P004";
                            Departamento = "02";
                            Municipio = "15";
                            break;
                        case "Sonsonate (05)":
                            establecimiento = "S004";
                            PuntoVenta = "P005";
                            Departamento = "03";
                            Municipio = "18";
                            break;
                        case "Santa Tecla (07)":
                            establecimiento = "S005";
                            PuntoVenta = "P006";
                            Departamento = "05";
                            Municipio = "28";
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
                           
                            items = new List<CertificacionRequest.Item>(),
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
                                    monto = Math.Round(
                                                Math.Abs((double)totalGravados) +
                                                Math.Abs((double)totalExcentos), 2)
                                }
                            }
                        }
                    };
                    
                    if (totalGravados > 0)
                    {
                        request.documento.items.Add(new CertificacionRequest.Item
                        {
                            tipo = 1,
                            cantidad = 1,
                            unidad_medida = 59,
                            descripcion = "Comisiones",
                            precio_unitario = Math.Round((double)Math.Abs(totalGravados), 2),
                            tipo_venta = "1",
                        });
                    }
                    // Agregar el ítem "Intereses"
                    request.documento.items.Add(new CertificacionRequest.Item
                    {
                        tipo = 1,
                        cantidad = 1,
                        unidad_medida = 59,
                        descripcion = "Intereses",
                        precio_unitario = Math.Round((double)Math.Abs(totalExcentos), 2),
                        tipo_venta = "3",
                    });
                    var response = _certificacionService.CertificarDocumento(request);

                    return "Dte generado y procesado exitosamente";

                }
                return "No se encontró la agencia";
            }
            else
            {
                return "Error";
            }
        }
            /// <summary>
            /// efectua el procesamiento de colecta específica
            /// </summary>
            /// <param name="id_colecta_diaria">int</param>
            /// <returns></returns>
            [Authorize(Roles = "Asistente,Jefe,Administrador", AuthenticationSchemes = "Bearer")]
            [HttpPut]
            public async Task<IActionResult> ProcesarColectaCredito(int id_colecta_diaria)
            {
                try
                {
                    //DateTime fecha =DateTime.Now;
                    bool verificado = _context.Colecta_Diaria.Any(l => l.Id_Colecta_Diaria == id_colecta_diaria && l.Verificado);
                    if (verificado)
                    {
                        Colecta_Diaria colecta_diaria = _context.Colecta_Diaria
                            .Include(d => d.Colectas)
                            .SingleOrDefault(s => s.Id_Colecta_Diaria == id_colecta_diaria);

                        /*sentinelas*/
                        List<Colecta> colectas = colecta_diaria.Colectas.Where(d => d.Id_Credito > 0 && !d.Procesado && !d.Borrado_Logico).ToList();
                        if (colectas != null)
                        {
                            int son_creditos = colectas.Count;
                            int ejecutados = 0;
                            foreach (Colecta colecta in colectas)
                            {
                                /*se efectua el pago al crédito*/
                                Transaccion_Credito transaccion = AmortizacionActualProc(colecta.Id_Credito, colecta.Creado_En.Date, colecta.Monto);
                                /*ahora efectuo el pago*/
                                if (transaccion.Recargo >= transaccion.Valor_Cuota)
                                {
                                    transaccion.Recargo = transaccion.Valor_Cuota;
                                }
                                Transaccion_Credito transaccion_guradada = await EfectuarPago(transaccion, colecta.No_Comprobante);
                                colecta.Id_Transaccion = transaccion_guradada.Id_Transaccion_Credito;
                                colecta.Procesado = true;
                                _context.Entry(colecta).State = EntityState.Modified;
                                ejecutados += 1;
                            }
                            await _context.SaveChangesAsync();
                            if (ejecutados == son_creditos)
                            {
                                colecta_diaria.Procesado_Total = true;
                                _context.Entry(colecta_diaria).State = EntityState.Modified;
                                await _context.SaveChangesAsync();
                                return Ok("Colectas para créditos procesadas en su totalidad");
                            }
                            return Ok("Colectas para créditos procesadas parcialmente");
                        }else
                            return Ok("No se registraron colectas de crédito");
                    }
                    else
                        return Ok("No se registraron colectas de crédito");

                } catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

        /// <summary>
        /// Muestra en factura los totales de una colecta que ya haya sido procesada
        /// </summary>
        /// <param name="id_colecta_diaria">int</param>
        /// <returns>Factura</returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente, Supervisor,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Mostrar_Factura_Colecta(int id_colecta_diaria)
        {
            try
            {
                string id_usuario = _userManager.GetUserId(HttpContext.User);
                Colecta cliente = _context.Colecta.AsNoTracking().FirstOrDefault(a => a.Id_Colecta_Diaria == id_colecta_diaria && a.Id_Credito>0);
                Transaccion_Caja caja = await _context.Transaccion_Caja.AsNoTracking().SingleAsync(d => d.Id_Transaccion_Credito == cliente.Id_Transaccion);
                Caja usuario = await _context.Caja.SingleAsync(c => c.Id_Caja == caja.Id_Caja);
                Colecta_Diaria colecta_diaria = await _context.Colecta_Diaria
               .Include(v => v.Colectas)
               .SingleOrDefaultAsync(v => v.Id_Colecta_Diaria == id_colecta_diaria &&
               v.Procesado_Total);

                if (colecta_diaria != null)
                {
                    List<Transaccion_Credito> transaccion_creditos = new List<Transaccion_Credito>();
                    foreach (var c in colecta_diaria.Colectas.Where(a => a.Id_Credito > 0))
                    {
                        Transaccion_Credito transaccion = _context
                            .Transaccion_Credito.AsNoTracking()
                            .SingleOrDefault(k => k.Id_Transaccion_Credito == c.Id_Transaccion);
                        transaccion_creditos.Add(transaccion);
                    }
                    Agencia agencia = _context
                        .Usuario_Agencia
                        .AsNoTracking()
                        .Include(f => f.Agencia)
                        .SingleOrDefault(g => g.Id_Usuario == id_usuario)
                        .Agencia;
                    Impresion impresion = new Impresion();
                    Transaccion_Credito tc = await _context.Transaccion_Credito
                        .AsNoTracking()
                        .Include(c => c.Credito)
                        .Include(p => p.Credito.Periodo)
                        //.Include(q => q.Credito.Linea_Credito_Historial) 
                        .Include(x => x.Credito.Persona)
                        .Include(h => h.Credito.Persona.Cartera)
                        .SingleOrDefaultAsync(i => i.Id_Transaccion_Credito == cliente.Id_Transaccion);
                    Detalle_Factura_Credito detalle = new Detalle_Factura_Credito
                    {
                        Abono_Capital = transaccion_creditos.Sum(f => f.Abono_capital),
                        Interes = transaccion_creditos.Sum(s => s.Interes_Comision),
                        Plan_AyC = transaccion_creditos.Sum(c => c.Interes) + transaccion_creditos.Sum(c => c.IvaSobreComision),/*se va a revisar si existe la iva adicional el una parte*/
                        Recargos = transaccion_creditos.Sum(x => x.Recargo),
                        Total_Pagar = transaccion_creditos.Sum(d => d.Valor_Cuota)
                    };
                    //generar DTE
                    var dte = await GenerarDTE(transaccion_creditos.Sum(d => d.Valor_Cuota),agencia.Id_Agencia);
                    Factura factura = new Factura
                    {
                        Detalle_Factura_Credito = detalle,
                        Total = detalle.Total_Pagar,
                        Fecha = colecta_diaria.Fecha,
                        Agencia = agencia != null ? agencia.Nombre : "",
                        Cliente = tc.Credito.Persona.NombreCompleto,
                        No_Ahorro = tc.Credito.Codigo_Credito,
                        Concepto = "Colecta",
                        Cajero = usuario.Nombre_PC,
                        id_cajero = usuario.Id_Caja,
                        Linea = "",
                        Saldo_Capital = 0.00M,
                        Tasa = 0.00M
                    };
                    string no_comprobante = await Generar_No_Comprobante();
                    Comprobante comprobante = new Comprobante
                    {
                        NoComprobante = no_comprobante

                    };
                    impresion.Factura = factura;
                    impresion.Comprobante = comprobante;
                    return Ok(impresion);
                }
                else
                {
                    return BadRequest("el registro que busca no se ha encontrado o no ha sido procesado en su totalidad");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
           
        }

        /// <summary>
        /// Se muestra la factura para el abono de un crédito
        /// </summary>
        /// <param name="id_transaccion_credito">int</param>
        /// <returns>Factura</returns>
        [Authorize(Roles = "Asistente,Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet] 
        public async Task<IActionResult> Mostrar_Factura_Transaccion(int id_transaccion_credito)
        {
            try
            {
                string id_usuario = _userManager.GetUserId(HttpContext.User);
                Transaccion_Caja caja = await _context.Transaccion_Caja.AsNoTracking().SingleAsync(d => d.Id_Transaccion_Credito == id_transaccion_credito);
                Transaccion_Credito credito = await _context.Transaccion_Credito.AsNoTracking().SingleAsync(a => a.Id_Transaccion_Credito == caja.Id_Transaccion_Credito);
                Tipo_Operacion_Credito tipo = await _context.Tipo_Operacion_Credito.SingleAsync(b => b.Id_Tipo_Operacion_Credito == credito.Id_Tipo_Operecion_Credito);
                Caja usuario = await _context.Caja.SingleAsync(c => c.Id_Caja == caja.Id_Caja);
                DateTime fecha = DateTime.Now;
                DateTime fecha_cercana = Fecha_Cercana_Simple(credito.Id_Credito, fecha.Date);

                Transaccion_Credito transaccion = await Task.Run(() => AmortizacionActualProc(credito.Id_Credito, fecha_cercana,0));
             
                Transaccion_Credito tc = await _context.Transaccion_Credito
                    .AsNoTracking()
                    .Include(c => c.Credito)
                    .Include(p=>p.Credito.Periodo)
                    //.Include(q => q.Credito.Linea_Credito_Historial)
                    .Include(x => x.Credito.Persona)
                    .Include(h => h.Credito.Persona.Cartera)
                    .SingleOrDefaultAsync(i => i.Id_Transaccion_Credito == id_transaccion_credito);
                Linea_Credito_Historial linea = await _context.Linea_Credito_Historial.AsNoTracking().SingleAsync(a => a.Id_Credito == tc.Id_Credito);
                Agencia agencia = _context
                    .Usuario_Agencia
                    .AsNoTracking()
                    .Include(f=>f.Agencia)
                    .SingleOrDefault(g => g.Id_Usuario == id_usuario)
                    .Agencia;

                if (tc != null)
                {
                    Impresion impresion = new Impresion();
                    Detalle_Factura_Credito dfc = new Detalle_Factura_Credito
                    {
                        Proximo_Pago = transaccion.FechaPago,
                        Abono_Capital = tc.Abono_capital,
                        Interes = tc.Interes_Comision,
                        Plan_AyC = tc.Interes + tc.IvaSobreComision,
                        Recargos = tc.Recargo,
                        Total_Pagar = tc.Valor_Cuota
                    };
                    Factura factura = new Factura
                    {
                        Detalle_Factura_Credito = dfc,
                        Total = dfc.Total_Pagar,
                        Fecha = tc.Creado_En,
                        Agencia = agencia != null ? agencia.Nombre : "",
                        No_Ahorro = tc.Credito.Codigo_Credito,
                        Cliente = tc.Credito.Persona.NombreCompleto,
                        Concepto = tipo.Descripcion,
                        Cajero = usuario.Nombre_PC,
                        id_cajero = usuario.Id_Caja,
                        Linea = linea.Descripcion_linea,
                        Saldo_Capital = tc.Saldo_Capital,
                        Tasa = linea.Interes_Comision
                    };
                    Comprobante comprobante = new Comprobante
                    {
                        NoComprobante = caja.No_Comprobante

                    };
                    impresion.Factura = factura;
                    impresion.Comprobante = comprobante;
                    return Ok(impresion);
                }
                return NoContent();
            } catch (NullReferenceException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /*trabajo sobre los traslados*/
        private async Task<List<Credito>> Creditos_Requieren_Pago_Agencia(
            int id_agencia,
            DateTime ahora,
            bool incluir_juridico = false, 
            bool solo_juridico = false)
        {
            try
            {
                List<Credito> creditos = new List<Credito>();
                List<Traslado_Abono> pagos_requeridos = new List<Traslado_Abono>();
                List<Usuario_Agencia> usuarios = _context
                    .Usuario_Agencia
                    .Where(f => f.Id_Agencia == id_agencia).ToList();
                foreach (Usuario_Agencia ua in usuarios)
                {
                    if (_context.Cartera.Any(d => d.Id_Usuario == ua.Id_Usuario))
                    {
                        var cartera = _context.Cartera
                        .Single(d => d.Id_Usuario == ua.Id_Usuario);
                        List<Credito> creditos_cartera = _context
                            .Credito
                            .Include(d=>d.Cartera)
                            .Include(d=>d.Periodo)
                            .Include(p=>p.Persona)
                            .Include(c=>c.Linea_Credito)
                            .Include(c=>c.Amortizaciones)
                            .Where(
                                    n =>n.Id_Cartera==cartera.Id_Cartera && 
                                    !n.Cancelado &&
                                    n.Liquidado && 
                                    !n.Revertido &&
                                    n.Amortizaciones.Any(d=>d.FechaPago.Date==ahora.Date))
                            .ToList();
                        if (creditos_cartera.Count > 0)
                        {
                            if (!incluir_juridico)
                                creditos_cartera = creditos_cartera.Where(d => d.Cartera.Id_Usuario != "juridico").ToList();
                            if (solo_juridico)
                                creditos_cartera = creditos_cartera.Where(d => d.Cartera.Id_Usuario == "juridico").ToList();
                            if (creditos_cartera.Count() > 0)
                                creditos.AddRange(creditos_cartera);
                        }
                    }
                }
                return await Task.Run(() => creditos);
            }
            catch (Exception ex) {
                return null;
            }
        }

        private async Task<List<Traslado_Abono>> Pagos_Requeridos_Agencia(int id_agencia,bool exe=false,bool especificar_fecha=false,DateTime? fecha_especifica=null,bool incluir_juridico=false,bool solo_juridico=false)
        {
            try
            {
                DateTime ahora = DateTime.Now;
                if (especificar_fecha)
                    ahora = (DateTime)fecha_especifica;
                List<Credito> creditos = await Creditos_Requieren_Pago_Agencia(id_agencia, ahora, incluir_juridico);
                List<Traslado_Abono> pagos_requeridos = new List<Traslado_Abono>();
                if (creditos.Count > 0)
                {
                    foreach (Credito credito in creditos)
                    {
                        var transaccion_ahorro_encontrado = await MaximoTransaccionEnSaldo(credito.Id_Persona);
                        if (transaccion_ahorro_encontrado != null)
                        {
                            //decimal maximo_saldo = _context.Ahorro.AsNoTracking().
                            //        Where(y => y.Id_Persona == credito.Id_Persona).Max(t => t.Saldo_Actual);

                         //   DateTime fecha_amortizacion = Fecha_Cercana_Simple(credito.Id_Credito, ahora);
                            bool probar_ahorro = transaccion_ahorro_encontrado != null;
                            if (exe)
                                probar_ahorro = transaccion_ahorro_encontrado != null && transaccion_ahorro_encontrado.Saldo > 0; //new Func<Ahorro, bool>(v => v.Saldo_Actual == maximo_saldo && v.Saldo_Actual > 0 && v.Id_Persona == credito.Id_Persona);

                            if (probar_ahorro)
                            {
                                Transaccion_Credito transaccion = AmortizacionActualProc(credito.Id_Credito, ahora.Date,null);

                                bool tiene_transacciones = _context.Transaccion_Credito.AsNoTracking().Any(x => x.Id_Credito == credito.Id_Credito && !x.Borrado_Logico);
                                Transaccion_Credito ultima_transaccion = new Transaccion_Credito();
                                if(tiene_transacciones)
                                    ultima_transaccion = _context
                                                        .Transaccion_Credito
                                                        .AsNoTracking()
                                                        .Where(x => x.Id_Credito == credito.Id_Credito && !x.Borrado_Logico)
                                                        .OrderByDescending(f => f.FechaPago)
                                                        .FirstOrDefault();

                                decimal valor_al_dia = transaccion.Valor_Al_Dia;
                                if (valor_al_dia > 0 && transaccion.FechaPago.Date==ahora.Date && transaccion_ahorro_encontrado.Saldo >0)
                                {
                                        Traslado_Abono pago_requerido = new Traslado_Abono
                                        {
                                            Id_Persona = credito.Id_Persona,
                                            Nombre_Completo = credito.Persona.NombreCompleto,
                                            Fecha = transaccion.FechaPago,
                                            Fecha_Amortizacion = ahora,
                                            Id_Usuario = credito.Cartera.Id_Usuario,
                                            Id_Credito = credito.Id_Credito,
                                            Codigo_Credito = credito.Codigo_Credito,
                                            Pago_Requerido = valor_al_dia,
                                            Id_Ahorro = transaccion_ahorro_encontrado.Id_Ahorro,
                                            Codigo_Ahorro = transaccion_ahorro_encontrado.Ahorro.No_Ahorro,
                                            Saldo_Ahorro = transaccion_ahorro_encontrado.Saldo
                                        };
                                        pagos_requeridos.Add(pago_requerido);
                                    
                                }
                            }
                        }
                    }
                }
                return pagos_requeridos;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /*sacar el máximo saldo de una cuenta de ahorro dada*/
        private async Task<Transaccion_Ahorro> MaximoTransaccionEnSaldo(int id_persona)
        {
            try
            {
                // Obtener ahorros filtrados por id_persona y id_linea_ahorro
                var ahorros = await _context.Ahorro
                    .AsNoTracking()
                    .Where(t => t.Id_Persona == id_persona && t.Id_Linea_Ahorro == 3)
                    .ToListAsync();

                if (!ahorros.Any())
                    return null;

                var ahorroIds = ahorros.Select(a => a.Id_Ahorro).ToList();

                // Obtener todas las transacciones relevantes
                var transacciones = await _context.Transaccion_Ahorro
                    .Include(a => a.Ahorro)
                    .AsNoTracking()
                    .Where(t => ahorroIds.Contains(t.Id_Ahorro))
                    .ToListAsync();

                if (!transacciones.Any())
                    return null;

                // Obtener las últimas transacciones para cada ahorro
                var ultimasTransacciones = transacciones
                                            .GroupBy(t => t.Id_Ahorro)
                                            .Select(g => g
                                                .OrderByDescending(t => t.Fecha)
                                                .ThenByDescending(t => t.Id_Transaccion_Ahorro)
                                                .FirstOrDefault())
                                            .ToList();

                if (!ultimasTransacciones.Any())
                    return null;

                // Obtener la transacción con el saldo máximo
                var transaccionConMaxSaldo = ultimasTransacciones
                    .OrderByDescending(t => t.Saldo)
                    .FirstOrDefault();

                return transaccionConMaxSaldo;
            }
            catch (Exception ex)
            {
                // Registrar el error (log) antes de retornar null
                // Log.Error(ex, "Error obteniendo la transacción con el saldo máximo para la persona {IdPersona}", id_persona);
                return null;
            }
        }



        /// <summary>
        /// se obtiene la lista de pagos requeridos y lo que tiene disponible para pagar
        /// </summary>
        /// <param name="id_agencia">int</param>
        /// <param name="especificar_fecha">bool</param>
        /// <param name="fecha_especifica">DateTime?</param>
        /// <returns></returns>
    //    [Authorize(Roles = "Jefe,Administrador", AuthenticationSchemes = "Bearer")]

        [HttpGet]
        public async Task<IActionResult> Traslados_Requeridos_Agencia(int id_agencia,bool especificar_fecha,DateTime? fecha_especifica=null)
        {
            List<Traslado_Abono> pagos_requeridos =await Pagos_Requeridos_Agencia(id_agencia,false,especificar_fecha,fecha_especifica);
            return Ok(pagos_requeridos);
        }


        /// <summary>
        /// Se obtiene los pagos al día por la fecha de hoy
        /// y por la agencia de donde es el usuario
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Pagos_Requerido()
        {
            int id_agencia = Agencia_Usuario();
            List<Traslado_Abono> pagos_requeridos = await Pagos_Requeridos_Agencia(id_agencia, false, false);
            return Ok(pagos_requeridos);
        }

        /// <summary>
        /// Efectuar traslados por agencia seleccionada
        /// de saldos disponibles a abonar créditos.
        /// </summary>
        /// <param name="pagos">lista de los pagos para procesar</param>
        /// <returns></returns>
        [Authorize(Roles = "Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> Ejecutar_Pagos_Requeridos_Agencia([FromBody]List<Traslado_Abono> pagos)
        {
            try
            {
                List<Traslado_Abono> pagos_requeridos = pagos;
                if(pagos_requeridos.Count>0)
                    foreach (Traslado_Abono pago in pagos_requeridos)
                        if(pago.Saldo_Ahorro >0.00M)
                            await TrasladoPorCredito(pago);

                return Ok("Los traslados se han efectuado correctamente");
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        private async Task TrasladoPorCredito(Traslado_Abono traslado)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    decimal valor_traslado = 0.00M;
                    if (traslado.Pago_Requerido < traslado.Saldo_Ahorro)
                    {
                        valor_traslado = traslado.Pago_Requerido;
                    }
                    else if (traslado.Pago_Requerido > traslado.Saldo_Ahorro)
                    {
                        valor_traslado = traslado.Saldo_Ahorro;
                    }
                    else if(traslado.Pago_Requerido == traslado.Saldo_Ahorro)
                    {
                        valor_traslado = traslado.Pago_Requerido;
                    }
                    
                    Transaccion_Credito transaccion = AmortizacionActualProc(traslado.Id_Credito, traslado.Fecha, valor_traslado);
                    _context.Transaccion_Credito.Add(transaccion);
                    string comprobante=await Generar_No_Comprobante();
                    await _context.SaveChangesAsync();
                    Transaccion_Caja trans_caja = Registrar_Caja(transaccion,26, traslado.Fecha, true,comprobante: comprobante);
                    _context.Transaccion_Caja.Add(trans_caja).State = EntityState.Added;
                    await _context.SaveChangesAsync();
                    string comprobante_ahorro = await Generar_No_Comprobante();
                    await GuardarSimple(traslado.Id_Ahorro,valor_traslado,3,true,false, comprobante_ahorro,true,false,traslado.Fecha,true);
                    dbContextTransaction.Commit();
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    throw ex;
                }
            }
        }
        /// <summary>
        /// Efectuar traslados por agencia seleccionada
        /// de saldos disponibles a abonar créditos.
        /// </summary>
        /// <param name="pagos">lista de los pagos para procesar</param>
        /// <returns></returns>
        [Authorize(Roles = "Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> GenerarDtetraslados([FromBody] List<Traslado_Abono> pagos, bool isDte)
        {
            try
            {
                List<Traslado_Abono> pagos_requeridos = pagos;
                if (pagos_requeridos.Count > 0)
                {
                    var idsCredito = pagos_requeridos.Select(p => p.Id_Credito).Distinct().ToList();

                    var creditos = await _context.Credito
                        .Include(c=> c.Persona)
                        .Include(c => c.Transacciones_Creditos)
                        .Where(c => idsCredito.Contains(c.Id_Credito))
                        .ToListAsync();

                    // Obtener la última TransaccionCredito por crédito
                    var ultimasTransacciones = creditos
                        .Select(c => c.Transacciones_Creditos
                            .OrderByDescending(tc => tc.FechaPago) 
                            .FirstOrDefault())
                        .Where(tc => tc != null)
                        .ToList();

                    // Sumar campos directamente desde la transacción
                    var totalRecargo = ultimasTransacciones.Sum(tc => tc.Recargo);
                    var totalInteres = ultimasTransacciones.Sum(tc => tc.Interes);
                    var totalInteresComision = ultimasTransacciones.Sum(tc => tc.Interes_Comision);
                    var totalAbonoCapital = ultimasTransacciones.Sum(tc => tc.Abono_capital);

                    // Verificar si son mayores a cero
                    decimal? resultadoRecargo = totalRecargo > 0 ? (decimal?)totalRecargo : 0.00M;
                    decimal? resultadoInteres = totalInteres > 0 ? (decimal?)totalInteres : 0.00M;
                    decimal? resultadoInteresComision = totalInteresComision > 0 ? (decimal?)totalInteresComision : 0.00M;
                    decimal? resultadoAbonoCapital = totalAbonoCapital > 0 ? (decimal?)totalAbonoCapital : 0.00M;
                    //generar factura 
                    string id_usuario = _userManager.GetUserId(HttpContext.User);
                   
                   
                    DateTime fecha = DateTime.Now;
                    Agencia agencia = _context
                        .Usuario_Agencia
                        .AsNoTracking()
                        .Include(f => f.Agencia)
                        .SingleOrDefault(g => g.Id_Usuario == id_usuario)
                        .Agencia;
                    if (isDte)
                    {
                        decimal totalGravados = (decimal)resultadoInteres + (decimal)resultadoRecargo;
                        decimal totalExcentos = (decimal)resultadoInteresComision;
                        var dte = await FacturarTrasladosRefinanciamientos(totalGravados,totalExcentos,agencia.Id_Agencia);
                        if(dte.Equals("Error"))
                        {
                            return BadRequest("Error al generar el DTE");
                        }
                        else
                        {
                            return Ok(dte);
                        }
                    }
                    
                        Impresion impresion = new Impresion();
                        Detalle_Factura_Credito dfc = new Detalle_Factura_Credito
                        {
                            Interes = (decimal) resultadoInteresComision,
                            Plan_AyC = (decimal) resultadoInteres,
                            Recargos = (decimal) resultadoRecargo,
                            Total_Pagar = (decimal)resultadoInteresComision + (decimal)resultadoInteres + (decimal)resultadoRecargo
                        };
                        Factura factura = new Factura
                        {
                            Detalle_Factura_Credito = dfc,
                            Total = (decimal) resultadoAbonoCapital,
                            Fecha = fecha,
                            Agencia = agencia != null ? agencia.Nombre : "",
                            No_Ahorro = creditos.FirstOrDefault().Codigo_Credito,
                            Cliente = creditos.FirstOrDefault().Persona.NombreCompleto,
                            Concepto = "Abono a Credito",
                            Linea = "",
                            Saldo_Capital = 0.00M,
                            Tasa =0.00M
                        };
                        Comprobante comprobante = new Comprobante
                        {
                            NoComprobante = "9999999"
                        };
                        impresion.Factura = factura;
                        impresion.Comprobante = comprobante;
                        return Ok(impresion);
                    
                }

                return Ok("Los traslados se han efectuado correctamente");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /*termina el manejo de traslados*/


        /// <summary>
        /// Get Calcular la amortización considerando el momento actual
        /// </summary>
        /// <param name="id_credito">int id del crédito</param>
        /// <param name="fecha">DateTime fecha en que empieza</param>
        /// <param name="valor_cuota">decimal valor del pago</param>
        /// <param name="para_refinanciar"></param>
        /// <param name="condonar_recargo"></param>
        /// <returns>Transaccion_Credito objeto json</returns>
      //[Authorize(Roles = "Asesor, Asistente,Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> AmortizacionActual(int id_credito, DateTime fecha, decimal? valor_cuota=null,bool para_refinanciar=false, bool condonar_recargo = false)
        {
            try
            {
                DateTime ahora = DateTime.Now;

                /*cambiar la fecha de la transacción*/
                if (User.IsInRole("Asesor") || User.IsInRole("Asistente"))
                {
                    var diferente = ahora.Date - fecha.Date;
                    var dias = diferente.Days;
                    if (dias != 0)
                        fecha = ahora;
                }

                Transaccion_Credito transaccion=await Task.Run(() => AmortizacionActualProc(id_credito, fecha, valor_cuota, para_refinanciar, recargo: condonar_recargo));
                Credito cr = _context.Credito.AsNoTracking().Single(a=>a.Id_Credito == transaccion.Id_Credito);
                transaccion.Credito = cr;
                return Ok(transaccion);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Saber el próximo pago en los comprobantes
        /// </summary>
        /// <param name="id_credito"></param>
        /// <param name="fecha"></param>
        /// <param name="valor_cuota"></param>
        /// <param name="saldo_capital"></param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Proximo_Pago(int id_transaccion_credito, DateTime fecha, decimal? valor_cuota = null, bool para_refinanciar=false)
        {
            try
            {
                Transaccion_Credito credito =await _context.Transaccion_Credito.AsNoTracking().SingleAsync(a=>a.Id_Transaccion_Credito == id_transaccion_credito);

                DateTime fecha_cercana = Fecha_Cercana_Simple(credito.Id_Credito, fecha.Date);

                Transaccion_Credito transaccion = await Task.Run(() => AmortizacionActualProc(credito.Id_Credito, fecha_cercana, valor_cuota, para_refinanciar));
                return Ok(transaccion);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /*reversión*/
        /// <summary>
        /// se revierte un abono a un credito
        /// </summary>
        /// <param name="ids">List int</param>
        /// <returns></returns>
        [Authorize(Roles = "Jefe,Administrador", AuthenticationSchemes = "Bearer")]

        [HttpDelete]
        public async Task<IActionResult> Reversion_Credito([FromBody]int ids)
        {
            using (var dbContextTransaccion = _context.Database.BeginTransaction())
            {
                try
                {
                    await Reversion(ids, true);
                    dbContextTransaccion.Commit();
                    return Ok("Se ha revertido transacciones de pagos seleccionadas");
                }
                catch (Exception ex)
                {
                    dbContextTransaccion.Rollback();
                    return BadRequest(ex.StackTrace);
                }
            }
        }

        /// <summary>
        /// descubrir los pagos realizados
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Pagos_Realizados()
        {
            string Id_Usuario = _userManager.GetUserId(HttpContext.User);
            DateTime ahora = DateTime.Now;
            Cartera cartera = await _context
                .Cartera
                .AsNoTracking()
                .SingleAsync(c=>c.Id_Usuario==Id_Usuario);
            
            List<Transaccion_Credito> transacciones = _context
                .Credito
                .AsNoTracking()
                .Include(a => a.Transacciones_Creditos)
                .Where(d => d.Id_Cartera == cartera.Id_Cartera)
                .SelectMany(f => f.Transacciones_Creditos)
                .Where(
                    r => r.Creado_En.Day == ahora.Day && 
                    r.Creado_En.Month == ahora.Month && 
                    r.Creado_En.Year ==ahora.Year).ToList();
            if (transacciones.Count > 0)
            {
                List<Pago_Corriente> pagos = new List<Pago_Corriente>();
                foreach (Transaccion_Credito trans in transacciones)
                {
                    Credito credito = _context.Credito.Include(k => k.Persona).Single(a => a.Id_Credito == trans.Id_Credito);
                    Persona persona = credito.Persona;
                    pagos.Add(new Pago_Corriente //falta hacer cambios para mejorar los datos en la aplicacion movil
                    {
                        Transaccion = trans,
                        Id_Persona = persona.Id_Persona,
                        Id_Usuario = Id_Usuario,
                        Codigo_Credito=credito.Codigo_Credito,
                        Nombre_Completo = persona.NombreCompleto
                    });
                }
                return Ok(pagos);
            }
            else
                return NoContent();
        }

        /*modificación de saldo como cola*/
        /// <summary>
        /// Se reajusta el saldo
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPut]
        public async Task<IActionResult> Reajuste_Saldo(int id_credito)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                DateTime fecha = DateTime.Now;
                int id_transaccion = _context
                    .Transaccion_Credito
                    .Where(f => f.Id_Credito == id_credito)
                    .Max(t => t.Id_Transaccion_Credito);
                Transaccion_Credito transaccion = await _context
                    .Transaccion_Credito
                    .SingleAsync(k => k.Id_Transaccion_Credito == id_transaccion);
                decimal monto = transaccion.Saldo_Capital;
                if (monto > 0)
                {
                    transaccion.Saldo_Capital = 0.00M;
                    await _context.SaveChangesAsync();
                    /*reportarlo como cancelado*/
                    Credito credito = await _context.Credito.SingleAsync(f => f.Id_Credito == id_credito);
                    credito.Cancelado = true;
                    credito.Cancelado_En = fecha;
                    await _context.SaveChangesAsync();
                    string Id_Usuario = _userManager.GetUserId(HttpContext.User);
                    Caja caja = await Caja_Actual();
                    //poder tener una lista de transacciones
                    Transaccion_Simple transaccion_simple = new Transaccion_Simple
                    {
                        Id_Tipo_Operacion_Simple = 9,
                        Monto = monto,
                        Id_Caja = caja.Id_Caja,
                        Id_Credito = credito.Id_Credito,
                        //Id_Caja_Boveda=boveda_actual.Id_Caja,/*se va a eliminar*/
                        //Id_Banco = banco != null ? banco.Id_Banco : 0,
                        //No_Cuenta = banco != null ? banco.No_Cuenta : "--",
                        //Nombre_Banco = banco != null ? banco.Nombre : "--",
                        Creado_En = fecha,
                        Creado_Por = Id_Usuario,
                    };
                    _context.Transaccion_Simple.Add(transaccion_simple).State = EntityState.Added;
                    await _context.SaveChangesAsync();
                    dbContextTransaction.Commit();
                    return Ok("Se ha gestionado un reajuste del saldo capital");
                }
                else
                    return BadRequest("No es necesarios reajustar saldo capital que se reporta a cero");
            }
        }

      //  [Authorize(Roles = "Asesor, Asistente,Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> PagoAhora(int id_credito, int id_linea, decimal monto, int id_periodo, int plazo)
        {
            try
            {   DateTime fecha = DateTime.Now;
                Transaccion_Credito transaccion = AmortizacionActualProc(id_credito, fecha,null);
                List<Amortizacion> amortizaciones =await _context
                    .Amortizacion
                    .AsNoTracking().
                    Where(d => d.Id_Credito == id_credito).ToListAsync();  

                Credito cr = _context.Credito
                    .Include(a=>a.Linea_Credito)
                    .Include(b=>b.Periodo)
                    .Include(d=>d.Transacciones_Creditos)
                    .AsNoTracking().Single(a => a.Id_Credito == id_credito && !a.Cancelado);

                Datos_Credito datos = new Datos_Credito();

                if (cr != null)
                {
                    //var transacciones = cr.Transacciones_Creditos.Where(d => !d.Borrado_Logico).ToList();
                    decimal interes_diario = transaccion.Interes_Diario;
                    var interes_pendiente = transaccion.Interes_Pendiente; 
                    var cobertura_pendiente = transaccion.Comsion_Pendiente;
                    //  decimal total_intereses = interes_pendiente > 0
                    //? interes_diario + (interes_diario - interes_pendiente) + cobertura_pendiente
                    //: interes_diario + interes_pendiente + cobertura_pendiente;
                    var total_intereses = interes_diario;
                    var capital_mora = transaccion.Capital_Mora;
                    var pago_dia = transaccion.Valor_Al_Dia;
                    datos = new Datos_Credito
                    {
                        Interes_Diario = interes_diario,
                        Interes_Pendiente = interes_pendiente,
                        Cobertura_Pendiente = cobertura_pendiente,
                        Total_Intereses = total_intereses,
                        Pago_al_Dia = pago_dia,
                        Capital_Mora = capital_mora,
                        Recargos_Acumulados=transaccion.Recargos_Acumulados,
                        Recargos_Pendientes=transaccion.Recargo,
                        Transacciones = transaccion
                    };
                    return Ok(datos);
                }
                return Ok(datos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
  
        private ResultadoCalculoCreditio Amortizacion(int id_linea, decimal monto, int id_periodo, int plazo, int id_credito = 0)
        {
            Periodo periodo = _context.Periodo.AsNoTracking().SingleOrDefault(d => d.Id_Periodo == id_periodo);
            /*obtener el dato de la línea de credito*/
            Linea_Credito lc = _context.Linea_Credito.AsNoTracking().Include(f => f.Cobros_Credito).Single(t => t.Id_Linea == id_linea);
            decimal total = lc.Cobros_Credito.Count > 0 ? lc.Cobros_Credito.Sum(v => v.Valor) : 0.00M;
            DateTime fecha_inicio = DateTime.Now;
            /*traer el iva legislado de las configuraciones*/
            decimal iva = IVA;
            if (id_credito > 0)
            {
                Credito credito = _context
                    .Credito
                    .AsNoTracking()
                    .Include(t => t.Linea_Credito_Historial)
                    .Include("Linea_Credito_Historial.Cobros_Periodicos_Historial")
                    .SingleOrDefault(t => t.Id_Credito == id_credito);
                if (credito.Liquidado)
                    fecha_inicio = credito.Liquidado_En;
                Linea_Credito_Historial lch = _context
                    .Linea_Credito_Historial
                    .AsNoTracking()
                    .Single(s => s.Id_Credito == credito.Id_Credito);

                total = _context.Cobro_Periodico_Historial.AsNoTracking().Where(a => a.Id_Linea_Credito_Historial == lch.Id_Linea_Credito_Historial).Sum(d => d.Valor);
                return new CalculosYProcesoCredito(lch.Interes_Comision,
                    monto,
                    periodo,
                    iva,
                    plazo,
                    lch.Interes,
                    fecha_inicio,
                    total,
                    lch.PlanAyC_Cobros_Periodico_Dinero).Resultado;
            }
            return new CalculosYProcesoCredito(lc.Interes_Comision,
                monto,
                periodo,
                iva,
                plazo,
                lc.Interes,
                fecha_inicio,
                total,
                lc.PlanAyC_Cobros_Periodico_Dinero).Resultado;
        }

        [Authorize(Roles = "Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Mostrar_Revertir_Traslado(string codigo_credito, DateTime fecha)
        {
            try
            {
                bool credito_persona = _context.Credito.AsNoTracking().Any(a => a.Codigo_Credito == codigo_credito);
                if (credito_persona)
                {
                    var tiene_transaccion_linea = (from cr in _context.Credito
                                                             join pe in _context.Personas on cr.Id_Persona equals pe.Id_Persona
                                                             join tr in _context.Transaccion_Credito on cr.Id_Credito equals tr.Id_Credito
                                                             join ta in _context.Transaccion_Caja on tr.Id_Transaccion_Credito equals ta.Id_Transaccion_Credito
                                                             join ah in _context.Ahorro on cr.Id_Persona equals ah.Id_Persona
                                                             join th in _context.Transaccion_Ahorro on ah.Id_Ahorro equals th.Id_Ahorro
                                                             where ta.Fecha == fecha.Date
                                                             && !ta.Borrado_Logico && cr.Codigo_Credito == codigo_credito
                                                             && ta.Id_Caja ==26 && th.Id_Tipo_Operacion_Ahorro ==3
                                                             && th.Valor == tr.Valor_Cuota
                                                             select new Transacciones_Reversion_Traslado{
                                                                 id_Transaccion_Credito= tr.Id_Transaccion_Credito,
                                                                 id_Transaccion_Caja = ta.Id_Transaccion_Caja,
                                                                 id_Transaccion_Ahorro = th.Id_Transaccion_Ahorro,
                                                                 fecha_Traslado = ta.Fecha.Date,
                                                                 nombre_persona = pe.NombreCompleto,
                                                                 numero_caja = ta.Id_Caja,
                                                                 codigo_credito = cr.Codigo_Credito,
                                                                 valor_traslado = ta.Monto,
                                                                 codigo_ahorro = ah.No_Ahorro,
                                                                 valor_retiro = th.Valor
                                                             }).ToList();
                    return Ok(tiene_transaccion_linea);
                }
                else
                {
                    return BadRequest("El codigo no existe, vuelva a ingresarlo");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
     
        [Authorize(Roles = "Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> Revertir_Traslado(int id_transaccion_credito, int id_transaccion_ahorro)
        {
            try
            {
                //reversión transacción credito
                await Reversion(id_transaccion_credito, true);
                //reversión transacción ahorro
                await Reversion(id_transaccion_ahorro, false);
                return Ok("Reversión realizada exitosamente");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// procesar las condonaciones en las amortizaciones
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [Authorize(Roles = "Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> Calcular_Pago_Anticipado(int id_credito, bool penalizacion)
        {
            try
            {
                DateTime fecha = DateTime.Now;
                Transaccion_Credito transaccion = AmortizacionActualProc(id_credito, fecha, null);
                List<Amortizacion> amortizaciones = await _context
                    .Amortizacion
                    .AsNoTracking().
                    Where(d => d.Id_Credito == id_credito).ToListAsync();

                Credito cr = _context.Credito
                    .Include(a => a.Linea_Credito)
                    .Include(b => b.Periodo)
                    .Include(d => d.Transacciones_Creditos)
                    .AsNoTracking().Single(a => a.Id_Credito == id_credito && !a.Cancelado);

                Datos_Credito datos = new Datos_Credito();
                Pago_Anticipado pago_Anticipado = new Pago_Anticipado();

                if (cr != null)
                {
                    decimal interes_diario = transaccion.Interes_Diario;
                    var interes_pendiente = transaccion.Interes_Pendiente;
                    var cobertura_pendiente = transaccion.Comsion_Pendiente;
                    var total_intereses = interes_diario + interes_pendiente + cobertura_pendiente;
                    var capital_mora = transaccion.Capital_Mora;
                    var pago_dia = transaccion.Valor_Al_Dia;
                    var saldo_capital = 0.00M ;
                    if (cr.Transacciones_Creditos.Count>0)
                    {
                        saldo_capital = cr.Transacciones_Creditos.LastOrDefault().Saldo_Capital;
                    }
                    else
                    {
                        saldo_capital = cr.Monto_pago;
                    }
                    decimal pago_anticipado = 0.00M;
                    if (penalizacion)
                    {
                      
                        pago_anticipado = Math.Round((total_intereses + saldo_capital
                      + (cobertura_pendiente + transaccion.Interes)), 2);
                        pago_Anticipado = new Pago_Anticipado
                        {
                            Intereses_Dia = total_intereses,
                            Saldo_Capital = saldo_capital,
                            Penalizacion = 0.0M,
                            Cobertura_Transito = cobertura_pendiente + transaccion.Interes,
                            Pago_Para_Cancelar = transaccion.Pago_para_cancelar,
                            Valor_Pago_Anticipado = pago_anticipado,
                            Descuento = transaccion.Pago_para_cancelar - pago_anticipado
                        };
                    }
                    else {
                        pago_anticipado = Math.Round((total_intereses + saldo_capital
                     + saldo_capital * 0.05M + (cobertura_pendiente + transaccion.Interes)), 2);
                        pago_Anticipado = new Pago_Anticipado
                        {
                            Intereses_Dia = total_intereses,
                            Saldo_Capital = saldo_capital,
                            Penalizacion = saldo_capital * 0.05M,
                            Cobertura_Transito = cobertura_pendiente + transaccion.Interes,
                            Pago_Para_Cancelar = transaccion.Pago_para_cancelar,
                            Valor_Pago_Anticipado = pago_anticipado,
                            Descuento = transaccion.Pago_para_cancelar - pago_anticipado
                        };
                    }

                    
                    return Ok(pago_Anticipado);
                }
                return Ok(pago_Anticipado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}