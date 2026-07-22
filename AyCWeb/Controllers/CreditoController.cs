using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AyCWeb.Helper;
using AyCWeb.Models;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using ModelNucleo.CalculosYProceos;
using ModelNucleo.Model;
using ModelNucleo.Model.ModelView;
using ModelNucleo.Model.Tablero;
using Newtonsoft.Json;
using Rotativa.AspNetCore;
using Rotativa.AspNetCore.Options;
using Orientation = Rotativa.AspNetCore.Options.Orientation;
using JsonSerializer = System.Text.Json.JsonSerializer;
using System.Data;

namespace AyCWeb.Controllers
{
    public class CreditoController : BaseController
    {
        [HttpGet]
        public ActionResult Index(CreditoFiltro filtro, string nombre, string dui)
        {
            if (filtro.id_persona != 0)
                filtro.porCliente = true;
            ViewBag.Id_Persona = filtro.id_persona;
            ViewBag.nombre = nombre;
            ViewBag.dui = dui;
            return View(filtro);
        }

        [HttpPost]
        public async Task<IActionResult> CrearActa(int noacta)
        {
            Acta acta = new Acta {NoActa = noacta, Fecha_Celebrado = DateTime.Now};
            string jsonstring = JsonConvert.SerializeObject(acta).Replace("\"", "'");

            string recuroyparametro = string.Format("Actas/NuevaActa");
            ResponseApi responseApi = await OperarApi(HttpMethod.Post, recuroyparametro, jsonstring);
            /*convertir el json por ahora*/
            string contenido = responseApi.ContenidofromJson;
            int id_acta = JsonConvert.DeserializeObject<int>(contenido);
            return Ok(id_acta);
        }

        [HttpGet]
        public async Task<IActionResult> Creditos(DataSourceLoadOptions loadOptions, CreditoFiltro filtro)
        {
            ResponseApi responseApi = await Creditos_Principal(filtro);
            /*convertir el json por ahora*/
            string contenido = responseApi.ContenidofromJson;
            List<Credito> creditos = JsonConvert.DeserializeObject<List<Credito>>(contenido);

            var result = DataSourceLoader.Load(creditos, loadOptions);
            var resultJson = JsonConvert.SerializeObject(result);
            return Content(resultJson, "application/json");
        }
        [HttpPost]
        public async Task<IActionResult> GenerarDTECredito(int IDtransaccion)
        {
            string recuroyparametro = string.Format("Transacciones_Credito/GenerarDTEPagoCredito?IDtransaccion={0}", IDtransaccion);
            ResponseApi responseApi = await OperarApi(HttpMethod.Post, recuroyparametro, null);
            if (responseApi.Satisfactorio)
                return Ok(responseApi.ContenidofromJson);
            else
                return BadRequest(responseApi.ContenidofromJson);
        }
        private async Task<ResponseApi> Creditos_Principal(CreditoFiltro filtro)
        {
            string recuroyparametro = null;
            if (filtro.porCliente)
                recuroyparametro = string.Format("Creditos/GetCreditosPorPersona?id_persona={0}", filtro.id_persona);
            else
            {
                recuroyparametro = string.Format("Creditos/Creditos");
            }

            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            return responseApi;
        }

        private async Task<ResponseApi> Nuevo_Credito_Principal(Credito credito)
        {
            string jsonstring = JsonConvert.SerializeObject(credito).Replace("\"", "'");
            ResponseApi resultado = await OperarApi(HttpMethod.Post, "Creditos/NuevoCredito", jsonstring);
            return resultado;
        }

        private async Task<ResponseApi> Editar_Credito_Principal(Credito credito)
        {
            string url = string.Format("Creditos/Modificar");
            string jsonstring = JsonConvert.SerializeObject(credito).Replace("\"", "'");
            ResponseApi resultado = await OperarApi(HttpMethod.Put, url, jsonstring);
            return resultado;
        }

        public ActionResult FormActas(FechasFiltro filtro)
        {
            return View(filtro);
        }

        public async Task<IActionResult> ActasYCreditos(DataSourceLoadOptions loadOptions, FechasFiltro filtro)
        {
            string inicio = filtro.inicio.Value.ToString("MM/dd/yyyy");
            string final = filtro.final.Value.ToString("MM/dd/yyyy");
            string recuroyparametro = string.Format("Actas/Actas?inicio={0}&final={1}", inicio, final);

            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            /*convertir el json por ahora*/
            string contenido = responseApi.ContenidofromJson;
            List<Acta> actas = JsonConvert.DeserializeObject<List<Acta>>(contenido);

            var result = DataSourceLoader.Load(actas, loadOptions);
            var resultJson = JsonConvert.SerializeObject(result);
            return Content(resultJson, "application/json");
        }

        public async Task<IActionResult> CreditosPorActa(DataSourceLoadOptions loadOptions, int id_acta)
        {
            string recuroyparametro = string.Format("Creditos/CreditosPorActa?id_acta={0}", id_acta);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            /*convertir el json por ahora*/
            string contenido = responseApi.ContenidofromJson;
            List<Credito> creditos = JsonConvert.DeserializeObject<List<Credito>>(contenido);

            var result = DataSourceLoader.Load(creditos, loadOptions);
            var resultJson = JsonConvert.SerializeObject(result);
            return Content(resultJson, "application/json");
        }

        [HttpPost]
        public async Task<IActionResult> Create(Credito credito)
        {
            /*hacer casting a su objeto por usar devexpress*/
            //Credito credito = JsonConvert.DeserializeObject<Credito>(values);
            credito.Id_Credito = 0;
            /*conectarse al api*/
            ResponseApi resultado = await Nuevo_Credito_Principal(credito);
            if (resultado.Satisfactorio)
                return Ok(resultado.ContenidofromJson);
            else
                return BadRequest(resultado.ContenidofromJson);
        }

        // POST: Credito/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Edit(Credito credito)
        {
            /*conectarse al api*/
            ResponseApi resultado = await Editar_Credito_Principal(credito);
            if (resultado.Satisfactorio)
                return Ok(resultado.ContenidofromJson);
            else
                return BadRequest(resultado.ContenidofromJson);
        }
        [HttpPost]
        public async Task<IActionResult> EditCreditoAmpliado( int linea,int periodo, int comportamiento, DetalleComite detalle = null)
        {
            Credito credito = null;
            if (detalle != null)
            {
                credito = detalle.Credito;
            }
            credito.Id_Linea = linea;
            credito.Id_Periodo = periodo;
            credito.Id_Comportamiento_Credito = comportamiento;
            /*conectarse al api*/
            ResponseApi resultado = await Editar_Credito_Principal(credito);
            if (resultado.Satisfactorio)
                return Ok(resultado.ContenidofromJson);
            else
                return BadRequest(resultado.ContenidofromJson);
        }
        [HttpPost]
        public async Task<IActionResult> AgregarActa(int id_acta, int id_credito)
        {
            string url = string.Format("Creditos/AgregarAActa?id_acta={0}&id_credito={1}", id_acta, id_credito);
            ResponseApi resultado = await OperarApi(HttpMethod.Put, url, null);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Credito_Manejo(bool esNuevo, int id_credito = 0)
        {
            var periodos = await CargarPeriodos();
            var lineas = await CargarLineas();
            var comportamientos = await Comportamientos_Credito();
            ViewBag.esNuevo = esNuevo;
            if (esNuevo)
            {
                ViewBag.Id_Periodo = new SelectList(periodos, "Value", "Text");
                ViewBag.Id_Linea = new SelectList(lineas, "Value", "Text");
                ViewBag.Id_Comportamiento_Credito = new SelectList(comportamientos, "Value", "Text");
                return PartialView();
            }

            Credito credito = await Detalle(id_credito);
            ViewBag.Id_Periodo = new SelectList(periodos, "Value", "Text", credito.Id_Periodo);
            ViewBag.Id_Linea = new SelectList(lineas, "Value", "Text", credito.Id_Linea);
            ViewBag.Id_Comportamiento_Credito =
                new SelectList(comportamientos, "Value", "Text", credito.Id_Comportamiento_Credito);
          
            return PartialView(credito);
        }

        public async Task<Credito> Detalle(int id_credito)
        {
            string recuroyparametro = string.Format("Creditos/Detalle_Credito?id_credito={0}", id_credito);

            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            /*convertir el json por ahora*/
            string contenido = responseApi.ContenidofromJson;
            return JsonConvert.DeserializeObject<Credito>(contenido);
        }

        /*el verdadero detalle de credito con el detalle de todos los datos relacionados*/
        public async Task<IActionResult> Detalle_Credito(int id_persona, int id_credito = 0, bool paginar = false,
            bool siguiente = true)
        {
            string recuroyparametro = string
                .Format(
                    "Creditos/Detalle_Credito_Paginado?id_persona={0}&id_credito={1}&paginar={2}&siguiente={3}",
                    id_persona,
                    id_credito,
                    paginar,
                    siguiente);

            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            Credito credito = JsonConvert.DeserializeObject<Credito>(contenido);

            string recuroyparametro1 = string.Format("Personas/Detalle_Codeudor?id_persona={0}", id_persona);

            ResponseApi responseApi1 = await OperarApi(HttpMethod.Get, recuroyparametro1, null);

            string recuroyparametro2 = string.Format("Transacciones_Credito/Transacciones?id_credito={0}", id_credito);
            ResponseApi responseApi2 = await OperarApi(HttpMethod.Get, recuroyparametro2, null);

            DetalleCreditoVista vista = new DetalleCreditoVista
            {
                Credito = credito
            };

            string contenido2 = responseApi2.ContenidofromJson;
            List<Transacciones_para_Credito> transacciones =
                JsonConvert.DeserializeObject<List<Transacciones_para_Credito>>(contenido2);
            vista.Transacciones = transacciones;

            if (responseApi1.Satisfactorio)
            {
                string contenido1 = responseApi1.ContenidofromJson;
                Persona persona = JsonConvert.DeserializeObject<Persona>(contenido1);
                vista.Codeudor = persona;
                return View(vista);
            }
            else
            {
                return View(vista);

            }

        }

        [HttpGet]
        public async Task<IActionResult> RegenerarAmortizacion(int id_credito)
        {
            string recuroyparametro = string.Format("Creditos/GenerarAmortizacionPorCredito?id_credito={0}", id_credito);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            if (responseApi.Satisfactorio)
                return Ok(responseApi.ContenidofromJson);
            else
                return BadRequest(responseApi.ContenidofromJson);
        }

        private async Task<Credito> DetalleCreditoInterno(int id_credito)
        {
            string recuroyparametro = string.Format("Creditos/Detalle_Credito?id_credito={0}", id_credito);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            /*convertir el json por ahora*/
            string contenido = responseApi.ContenidofromJson;
            Credito credito = JsonConvert.DeserializeObject<Credito>(contenido);
            return credito;
        }


        [HttpGet]
        public async Task<IActionResult> DetalleCreditoAmortizacion(int id_credito)
        {
            Credito cred = await DetalleCreditoInterno(id_credito);

            /*obtener los datos del cliente*/
            int id_persona = cred.Id_Persona;
            string recursopersona = string.Format("Personas/PersonaDetalle?id_persona={0}", id_persona);
            ResponseApi respuestaPersona = await OperarApi(HttpMethod.Get, recursopersona, null);
            string contenidoPersona = respuestaPersona.ContenidofromJson;
            Persona persona = JsonConvert.DeserializeObject<Persona>(contenidoPersona);

            ResultadoCalculoCreditio am = await CuotaPrimariaInterna(cred.Id_Linea, cred.Monto_pago,
                cred.Periodo.Id_Periodo, cred.Plazo, id_credito);
            return View(new Credito_Amortizacion {Credito = cred, Amortizacion = am.Amortizacion, Cliente = persona});
        }

        /*cálculos y procesos para los creditos*/
        [HttpGet]
        public async Task<ResultadoCalculoCreditio> CuotaPrimaria(int id_linea, decimal monto, int id_periodo,
            int plazo, int id_credito = 0)
        {
            return await CuotaPrimariaInterna(id_linea, monto, id_periodo, plazo, id_credito);
        }

        private async Task<ResultadoCalculoCreditio> CuotaPrimariaInterna(int id_linea, decimal monto, int id_periodo,
            int plazo, int id_credito = 0)
        {
            string recuroyparametro =
                string.Format("Creditos/CuotaPrimaria?id_linea={0}&monto={1}&id_periodo={2}&plazo={3}&id_credito={4}",
                    id_linea, monto, id_periodo, plazo, id_credito);
            ResponseApi responseApi = await OperarApi(HttpMethod.Post, recuroyparametro, null);
            /*convertir el json por ahora*/
            string contenido = responseApi.ContenidofromJson;
            return JsonConvert.DeserializeObject<ResultadoCalculoCreditio>(contenido);
        }

        [HttpGet]
        public async Task<decimal> ObtenerNumeroCuotas(int plazo, int id_periodo)
        {
            string recuroyparametro =
                string.Format("Creditos/ObtenerNumeroCuotas?plazo={0}&id_periodo={1}", plazo, id_periodo);
            ResponseApi responseApi = await OperarApi(HttpMethod.Post, recuroyparametro, null);
            /*convertir el json por ahora*/
            string contenido = responseApi.ContenidofromJson;
            return JsonConvert.DeserializeObject<decimal>(contenido);
        }


        [HttpGet]
        public async Task<IActionResult> PantallaCredito(int id_credito)
        {
            string recuroyparametro = string.Format("Creditos/PantallaCredito?id_credito={0}", id_credito);
            ResponseApi responseApi = await OperarApi(HttpMethod.Put, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            Pantalla_Credito liquidacion = JsonConvert.DeserializeObject<Pantalla_Credito>(contenido);

            /*crear el pdf*/
            return new ViewAsPdf("Pantalla_Credito", liquidacion)
            {
                PageSize = Size.Letter,
                PageOrientation = Orientation.Portrait,
                PageMargins = {Left = 10, Right = 10}
            };
        }
        public async Task<IActionResult> Revertir_Credito(int id_credito)
        {
            string recuroyparametro = string.Format("Creditos/Revertir_Liquidacion?id_credito={0}", id_credito);
            ResponseApi responseApi = await OperarApi(HttpMethod.Put, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            Reversion_Credito reversion = JsonConvert.DeserializeObject<Reversion_Credito>(contenido);

            /*crear el pdf*/
            return new ViewAsPdf("Revertir_Credito", reversion)
            {
                PageSize = Size.Letter,
                PageOrientation = Orientation.Portrait,
                PageMargins = { Left = 10, Right = 10 }
            };
        }

        public async Task<IActionResult> ActaPorId(int id_acta)
        {
            string recuroyparametro = string.Format("Creditos/ActaPorId?id_acta={0}", id_acta);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            List<Credito> creditos = JsonConvert.DeserializeObject<List<Credito>>(contenido);

            return new ViewAsPdf("ActaPorId", creditos)
            {
                PageSize = Size.Letter,
                PageOrientation = Orientation.Landscape,
                PageMargins = {Left = 10, Right = 10}
            };
            //return View(creditos);
        }

        [HttpGet]
        public async Task<IActionResult> ConsultaParaPago(
              int id_credito,
            DateTime? fecha,
            decimal? valor_cuota,
            bool aldia = true,
            bool condonar_recargo = false)
        {
            Credito informCredito = await DetalleCreditoInterno(id_credito);
            ResponseApi responseApi1 = await ConsultaPagoAlDia(id_credito, 0, 0.00M, 0, 0);
            Datos_Credito datos_aldia = JsonConvert.DeserializeObject<Datos_Credito>(responseApi1.ContenidofromJson);

            Decimal monto_defecto = valor_cuota != null ? Convert.ToDecimal(valor_cuota) : datos_aldia.Pago_al_Dia;

            ResponseApi responseApi = await ConsultaGeneralPagoAlDia(id_credito, fecha, monto_defecto, condonar_recargo);
            string contenido = responseApi.ContenidofromJson;
            Transaccion_Credito transaccion = JsonConvert.DeserializeObject<Transaccion_Credito>(contenido);
            ViewBag.Respuesta =
                new Mensaje_Respuesta(responseApi.Satisfactorio, transaccion != null ? 1 : 0, responseApi.Error);
            ViewBag.AlDia = aldia;

            ViewBag.Cuota = Math.Round(informCredito.Cuota_Total, 2);
            ViewBag.Valor = monto_defecto;
            ViewBag.Recargo = condonar_recargo;
            return PartialView(transaccion);
        }

        [HttpGet]
        public async Task<IActionResult> ConsultaPagoAhora(
            int id_credito, 
            int id_linea, 
            decimal monto, 
            int id_periodo,
            int plazo)
        {
            ResponseApi responseApi = await ConsultaPagoAlDia(id_credito, id_linea, monto, id_periodo, plazo);
            string contenido = responseApi.ContenidofromJson;
            Datos_Credito transaccion = JsonConvert.DeserializeObject<Datos_Credito>(contenido);
            return PartialView(transaccion);
        }

        /*manejo del refinanciamiento de un crédito*/
        [HttpGet]
        public IActionResult Buscar_Refinanciables()
        {
            return PartialView();
        }

        [HttpGet]
        public IActionResult Cambiar_Valor_Refinanciamiento()
        {
            return PartialView();
        }

        [HttpGet]
        public IActionResult Buscar_Reestructura()
        {
            return PartialView();
        }

        [HttpGet]
        public async Task<IActionResult> Refinanciable(int id_credito, string codigo_credito, bool movible = false,
            decimal monto_nuevo_refinanciamiento = 0.00M)
        {
            string recuroyparametro = string
                .Format(
                    "Creditos/Refinanciable?id_credito={0}&codigo_credito={1}&movible={2}&monto_nuevo_refinanciamiento={3}",
                    id_credito,
                    codigo_credito,
                    movible,
                    monto_nuevo_refinanciamiento);

            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            if (responseApi.Satisfactorio)
            {
                string contenido = responseApi.ContenidofromJson;
                Refinanciable refinanciables = JsonConvert.DeserializeObject<Refinanciable>(contenido);
                return PartialView(refinanciables);
            }
            else
            {
                return BadRequest(responseApi.ContenidofromJson);
            }
        }

        public async Task<IActionResult> Reestructura(int id_credito, string codigo_credito, bool movible = false,
            decimal monto_nuevo_reestructura = 0.00M)
        {
            string recuroyparametro = string
                .Format("Creditos/Reestructurado?id_credito={0}&codigo_credito={1}",
                    id_credito,
                    codigo_credito);

            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            Refinanciable refinanciables = JsonConvert.DeserializeObject<Refinanciable>(contenido);
            if (movible)
            {
                refinanciables.Monto_Refinanciable =monto_nuevo_reestructura;
            }
            return PartialView(refinanciables);
        }

        [HttpPost]
        public async Task<IActionResult> Agregar_Refinanciamiento(
            Refinanciable refinanciable,
            bool reestructura = false,
            bool movible = false, bool descuento = false)
        {
            string jsonstring = JsonConvert.SerializeObject(refinanciable).Replace("\"", "'");
            string recuroyparametro = string
                .Format("Creditos/Procesar_Refinanciamiento?reestructura={0}&movible={1}&descuento={2}", reestructura,
                    movible, descuento);
            ResponseApi responseApi = await OperarApi(HttpMethod.Put, recuroyparametro, jsonstring);
            if (responseApi.Satisfactorio)
                return Ok(responseApi.ContenidofromJson);
            else
                return BadRequest(responseApi.ContenidofromJson);
        }

        private async Task<ResponseApi> ConsultaPagoAlDia(int id_credito, int id_linea, decimal monto, int id_periodo,
            int plazo)
        {
            string recuroyparametro =
                string.Format(
                    "Transacciones_Credito/PagoAhora?id_credito={0}&id_linea={1}&monto={2}&id_periodo={3}&plazo={4}",
                    id_credito, id_linea, monto, id_periodo, plazo);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            return responseApi;
        }

        private async Task<ResponseApi> ConsultaGeneralPagoAlDia(int id_credito, DateTime? fecha, decimal? valor_cuota,
            bool condonar_recargo)
        {
            string fecha_procesada = DateTime.Now.ToString("MM/dd/yyyy");
            if (fecha != null)
                fecha_procesada = fecha.Value.ToString("MM/dd/yyyy");

            string recuroyparametro = string.Format(
                "Transacciones_Credito/AmortizacionActual?id_credito={0}&fecha={1}&valor_cuota={2}&condonar_recargo={3}",
                id_credito,
                fecha_procesada,
                valor_cuota,
                condonar_recargo
            );
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            return responseApi;
        }

        [HttpPost]
        public async Task<IActionResult> GuardarPago(
            Transaccion_Credito transaccion, 
            [FromQuery] bool recargo = false,
            string comprobante = "")
        {
            /*conectarse al api*/
            string recuroyparametro = string.Format("Transacciones_Credito/GuardarPago?comprobante={0}&recargo={1}",
                comprobante, recargo);
            string jsonstring = JsonConvert.SerializeObject(transaccion).Replace("\"", "'");
            ResponseApi resultado = await OperarApi(HttpMethod.Post, recuroyparametro, jsonstring);
            Mensaje_Respuesta mr = new Mensaje_Respuesta(resultado.Satisfactorio, 0, resultado.Error,
                resultado.ContenidofromJson);
            return PartialView(MensajeAsync, mr);
        }

        [HttpGet]
        public async Task<IActionResult> Transacciones_Efectuadas(int id_credito)
        {
            string recuroyparametro = string.Format("Transacciones_Credito/Transacciones?id_credito={0}", id_credito);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            List<Transacciones_para_Credito> transaccion =
                JsonConvert.DeserializeObject<List<Transacciones_para_Credito>>(contenido);
            ViewBag.Respuesta = new Mensaje_Respuesta(responseApi.Satisfactorio, transaccion.Count, responseApi.Error);
            return PartialView(transaccion);
        }

        /*postear transacción*/
        [HttpGet]
        public async Task<IActionResult> Mostrar_Factura_Abono(int id_transaccion)
        {
            string direccion_credito =
                string.Format("Transacciones_Credito/Mostrar_Factura_Transaccion?id_transaccion_credito={0}",
                    id_transaccion);
            ResponseApi respuesta_factura = await OperarApi(HttpMethod.Get, direccion_credito, null);
            if (respuesta_factura.Satisfactorio)
            {
                Impresion factura = JsonConvert.DeserializeObject<Impresion>(respuesta_factura.ContenidofromJson);
                factura.Factura.Fecha = DateTime.Now;
                return new ViewAsPdf("Mostrar_Factura_Abono", factura)
                {
                    PageSize = Size.Letter,
                    PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                    PageMargins = {Left = 80, Right = 10}
                };
            }
            else
            {
                Mensaje_Respuesta mensaje = new Mensaje_Respuesta(respuesta_factura.Satisfactorio, 0,
                    respuesta_factura.Error, respuesta_factura.ContenidofromJson);
                return BadRequest(mensaje);
            }
        }


        [HttpGet]
        public async Task<IActionResult> Generar_Mutuos(int id_credito)
        {
            string parametrogrupal = string.Format("Creditos/Es_Grupal?id_credito={0}", id_credito);
            ResponseApi responseGrupal = await OperarApi(HttpMethod.Get, parametrogrupal, null);
            string contenidoGrupal = responseGrupal.ContenidofromJson;
            string nombreGrupal = JsonConvert.DeserializeObject<string>(contenidoGrupal);

            bool es_grupal =responseGrupal.Satisfactorio;
            Municipio_Departamento municipio = new Municipio_Departamento();
            Departamento_Pais departamento = new Departamento_Pais();

            string recuroyparametro = "";
            if (es_grupal)
                recuroyparametro = string.Format("Creditos/Mutuo_Grupal?id_credito={0}", id_credito);
            else
                recuroyparametro = string.Format("Creditos/Mutuo?id_credito={0}", id_credito);

            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            List<Persona> personas = new List<Persona>();
            if (es_grupal)
                personas = JsonConvert.DeserializeObject<List<Persona>>(contenido);
            else
            {
                Persona persona = JsonConvert.DeserializeObject<Persona>(contenido);
                personas.Add(persona);
            }
            foreach (var persona in personas)
            {
                if (persona.Id_Municipio_Departamento_Domicilio != null)
                {
                    List<Municipio_Departamento> municipio_persona = await Municipios_Comun(persona.Id_Departamento_Pais_Domicilio);
                    Departamento_Pais departamento_persona = await Departamentos(persona.Id_Departamento_Pais_Domicilio);
                    foreach (var m_persona in municipio_persona)
                    {
                        if (persona.Id_Municipio_Departamento_Domicilio == m_persona.Id_Municipio_Departamento)
                        {
                            municipio = municipio_persona.Single(c => c.Id_Municipio_Departamento == m_persona.Id_Municipio_Departamento);
                            departamento = departamento_persona;
                        }
                    }
                }
            }

            Generar_Documento documento = new Generar_Documento();
            MemoryStream stream = documento.Generar_Mutuo(personas, municipio, departamento,es_grupal,nombreGrupal);
            string zip_archivo = string.Format("{0}-{1}.zip", personas.FirstOrDefault().Creditos.FirstOrDefault().Codigo_Credito,personas.FirstOrDefault().Nombres);
            return new FileStreamResult(stream, "application/octet-stream")
            { FileDownloadName = zip_archivo };
        }
       
        private async Task<List<Municipio_Departamento>> Municipios_Comun(int? id_departamento)
        {
            string recuroyparametro = string.Format("Personas/Municipios?id_departamento={0}", id_departamento);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            List<Municipio_Departamento> municipios = JsonConvert.DeserializeObject<List<Municipio_Departamento>>(contenido);
            return municipios;
        }
        private async Task<Departamento_Pais> Departamentos(int? id_departamento)
        {
            string recuroyparametro = string.Format("Personas/DepartamentosPorID?id_departamento={0}", id_departamento);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            Departamento_Pais departamentos = JsonConvert.DeserializeObject<Departamento_Pais>(contenido);
            return departamentos;
        }
        public async Task<IActionResult> Mutuo_Copia(int id_persona)
        {
            string recuroyparametro = string.Format("Creditos/Mutuo?id_persona={0}", id_persona);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            Persona persona = JsonConvert.DeserializeObject<Persona>(contenido);

            Generar_Documento documento = new Generar_Documento();
            //documento.Guardar_Copia_Archivo();
            MemoryStream stream = documento.Generar_Mutuo_Copia(persona);
            return new FileStreamResult(stream, "application/octet-stream")
                {FileDownloadName = "mutuo_copia.docx"};
        }

        public async Task<IActionResult> Consultar_Record(int id_credito)
        {
            string recuroyparametro = string.Format("Creditos/id_credito?id_credito={0}", id_credito);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            Persona persona = JsonConvert.DeserializeObject<Persona>(contenido);

            Generar_Documento documento = new Generar_Documento();
            //documento.Guardar_Copia_Archivo();
            MemoryStream stream = documento.Consultar_Record(persona);
            return new FileStreamResult(stream, "application/msword")
                {FileDownloadName = "Consulta_Record.docx"};
        }

        public async Task<IActionResult> Compartir_Datos(int id_credito)
        {
            Municipio_Departamento municipio = new Municipio_Departamento();
            Departamento_Pais departamento = new Departamento_Pais();
            string recuroyparametro = string.Format("Creditos/Pagare?id_credito={0}", id_credito);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            Persona persona = JsonConvert.DeserializeObject<Persona>(contenido);

            if (persona.Id_Municipio_Departamento_Domicilio != null)
            {
                List<Municipio_Departamento> municipio_persona = await Municipios_Comun(persona.Id_Departamento_Pais_Domicilio);
                Departamento_Pais departamento_persona = await Departamentos(persona.Id_Departamento_Pais_Domicilio);
                foreach (var m_persona in municipio_persona)
                {
                    if (persona.Id_Municipio_Departamento_Domicilio == m_persona.Id_Municipio_Departamento)
                    {
                        municipio = municipio_persona.Single(c => c.Id_Municipio_Departamento == m_persona.Id_Municipio_Departamento);
                        departamento = departamento_persona;
                    }
                }
            }
            Generar_Documento documento = new Generar_Documento();
            //documento.Guardar_Copia_Archivo();
            MemoryStream stream = documento.Compartir_Datos(persona, municipio, departamento);
            return new FileStreamResult(stream, "application/msword")
                {FileDownloadName = "Compartir_Datos.docx"};
        }

        public async Task<IActionResult> Generar_Pagare(int id_credito)
        {
            Municipio_Departamento municipio = new Municipio_Departamento();
            Departamento_Pais departamento = new Departamento_Pais();
            string recuroyparametro = string.Format("Creditos/Pagare?id_credito={0}", id_credito);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            Persona persona = JsonConvert.DeserializeObject<Persona>(contenido);
            if (persona.Id_Municipio_Departamento_Domicilio != null)
            {
                List<Municipio_Departamento> municipio_persona = await Municipios_Comun(persona.Id_Departamento_Pais_Domicilio);
                Departamento_Pais departamento_persona = await Departamentos(persona.Id_Departamento_Pais_Domicilio);
                foreach (var m_persona in municipio_persona)
                {
                    if (persona.Id_Municipio_Departamento_Domicilio == m_persona.Id_Municipio_Departamento)
                    {
                        municipio = municipio_persona.Single(c => c.Id_Municipio_Departamento == m_persona.Id_Municipio_Departamento);
                        departamento = departamento_persona;
                    }
                }
            }
            Generar_Documento documento = new Generar_Documento();
            //documento.Guardar_Copia_Archivo();
            MemoryStream stream = documento.Generar_Pagare(persona, municipio, departamento);
            return new FileStreamResult(stream, "application/msword")
                {FileDownloadName = "pagare.docx"};
        }
        public async Task<IActionResult> PagareCodeudor(int id_credito)
        {
            PagarCodeudor DatosCodeudor = new PagarCodeudor();
            string recuroyparametro = string.Format("Creditos/PagareCodeudor?id_credito={0}", id_credito);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            DatosCodeudor = JsonConvert.DeserializeObject<PagarCodeudor>(contenido);

            Generar_Documento documento = new Generar_Documento();
            MemoryStream stream = documento.GenerarPagareCodeudor(DatosCodeudor);
            return new FileStreamResult(stream, "application/msword")
            { FileDownloadName = "pagarecodeudor.docx" };
        }
        public async Task<IActionResult> CalendarioPagos(int id_credito)
        {
            CalendarioPago dataCalendario = new CalendarioPago();

            string recuroyparametro = string.Format("Creditos/calendarioPagos?id_credito={0}", id_credito);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            dataCalendario = JsonConvert.DeserializeObject<CalendarioPago>(contenido);

            Generar_Documento documento = new Generar_Documento();
            MemoryStream stream = documento.DescargarCalendarioPagos(dataCalendario);
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "calendario_de_pagos.xlsx");

        }
        public async Task<IActionResult> OrdenDescuentoDeudor(int id_credito)
        {
            ordenDescuento ordenDescuento = new ordenDescuento();
            string recuroyparametro = string.Format("Creditos/ordenDescuentoDeudor?id_credito={0}", id_credito);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            ordenDescuento = JsonConvert.DeserializeObject<ordenDescuento>(contenido);

            Generar_Documento documento = new Generar_Documento();
            MemoryStream stream = documento.GenerarOrdenDescuentoDeudor(ordenDescuento);
            return new FileStreamResult(stream, "application/msword")
            { FileDownloadName = "OrdenDescuentoDeudor.docx" };
        } 
        public async Task<IActionResult> OrdenDescuentoFiador(int id_credito)
        {
            ordenDescuento ordenDescuento = new ordenDescuento();
            string recuroyparametro = string.Format("Creditos/ordenDescuentoFiador?id_credito={0}", id_credito);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            ordenDescuento = JsonConvert.DeserializeObject<ordenDescuento>(contenido);

            Generar_Documento documento = new Generar_Documento();
            MemoryStream stream = documento.GenerarOrdenDescuentoFiador(ordenDescuento);
            return new FileStreamResult(stream, "application/msword")
            { FileDownloadName = "OrdenDescuentoFiador.docx" };
        }
        public async Task<IActionResult> Compartir_DatosFiador(int id_credito)
        {
            ordenDescuento ordenDescuento = new ordenDescuento();
            string recuroyparametro = string.Format("Creditos/ordenDescuentoFiador?id_credito={0}", id_credito);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            ordenDescuento = JsonConvert.DeserializeObject<ordenDescuento>(contenido);

            Generar_Documento documento = new Generar_Documento();
            MemoryStream stream = documento.GenerarCompartirDatosFiador(ordenDescuento);
            return new FileStreamResult(stream, "application/msword")
            { FileDownloadName = "CompartirDatosFiador.docx" };
        }
        public async Task<IActionResult> Generar_Condiciones_Generales(int id_credito)
        {
            string recuroyparametro = string.Format("Creditos/Pagare?id_credito={0}", id_credito);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            Persona persona = JsonConvert.DeserializeObject<Persona>(contenido);

            Generar_Documento documento = new Generar_Documento();
            //documento.Guardar_Copia_Archivo();
            MemoryStream stream = documento.Generar_Condiciones_Generales(persona);
            return new FileStreamResult(stream, "application/msword")
                {FileDownloadName = "Condiciones_Generales.docx"};
        }

        public async Task<IActionResult> Generar_Carpeta_Azul(
            int id_persona, 
            int id_linea, 
            decimal monto,
            int id_periodo, 
            int plazo, 
            int id_credito = 0)
        {

            string recuroyparametro =
                string.Format(
                    "Creditos/Carpeta_Azul?id_persona={0}&id_linea={1}&monto={2}&id_periodo={3}&plazo={4}&id_credito={5}",
                    id_persona, id_linea, monto, id_periodo, plazo, id_credito);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;

            Carpeta_Azul persona = JsonConvert.DeserializeObject<Carpeta_Azul>(contenido);
            Generar_Documento documento = new Generar_Documento();
            //documento.Guardar_Copia_Archivo();
            MemoryStream stream = documento.Generar_Carpeta_Azul(persona, id_credito);
            return new FileStreamResult(stream, "application/msword")
                {FileDownloadName = "Carpeta_Azul.docx"};
        }

        public async Task<IActionResult> Generar_PlanAyc(int id_credito)
        {
            Municipio_Departamento municipio = new Municipio_Departamento();
            Departamento_Pais departamento = new Departamento_Pais();
            string recuroyparametro =
                string.Format("Creditos/Plan_AyC?id_credito={0}",  id_credito);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            Persona persona = JsonConvert.DeserializeObject<Persona>(contenido);
            if (persona.Id_Municipio_Departamento_Domicilio != null)
            {
                List<Municipio_Departamento> municipio_persona = await Municipios_Comun(persona.Id_Departamento_Pais_Domicilio);
                Departamento_Pais departamento_persona = await Departamentos(persona.Id_Departamento_Pais_Domicilio);
                foreach (var m_persona in municipio_persona)
                {
                    if (persona.Id_Municipio_Departamento_Domicilio == m_persona.Id_Municipio_Departamento)
                    {
                        municipio = municipio_persona.Single(c => c.Id_Municipio_Departamento == m_persona.Id_Municipio_Departamento);
                        departamento = departamento_persona;
                    }
                }
            }
            Generar_Documento documento = new Generar_Documento();
            //documento.Guardar_Copia_Archivo();
            MemoryStream stream = documento.Generar_PlanAyC(persona, municipio, departamento);
            return new FileStreamResult(stream, "application/msword")
                {FileDownloadName = "PlanAyC.docx"};
        }

        /*para la conección nueva de manera manual*/
        public async Task<IActionResult> Creditos_Manual(int id_persona)
        {
            CreditoFiltro filtro = new CreditoFiltro
            {
                id_persona = id_persona,
                porCliente = true
            };
            ResponseApi response = await Creditos_Principal(filtro);
            var result = response.ContenidofromJson;
            List<Credito> creditos = JsonConvert.DeserializeObject<List<Credito>>(result);
            return PartialView(creditos);
        }


        public IActionResult Nuevo_Credito_Form()
        {
            return PartialView();
        }

        public async Task<IActionResult> Nuevo_Credito_Manual(Credito credito)
        {
            ResponseApi response = await Nuevo_Credito_Principal(credito);
            return Ok();
        }

        public async Task<IActionResult> Editar_Credito_Manual(Credito credito)
        {
            ResponseApi response = await Editar_Credito_Principal(credito);
            return Ok();
        }

        /*para realizar los traslados a abonos que deben hacerse*/
        [HttpGet]
        public IActionResult Traslado_Abono()
        {
            return PartialView();
        }

        [HttpGet]
        public async Task<IActionResult> Pago_Dia()
        {
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, "Transacciones_Credito/Pagos_Requerido", null);
            string contenido = responseApi.ContenidofromJson;
            List<Traslado_Abono> pagos = JsonConvert.DeserializeObject<List<Traslado_Abono>>(contenido);
            return View(pagos);
        }

        [HttpGet]
        public async Task<IActionResult> Traslados(
            int id_agencia, 
            bool especificar_fecha,
            DateTime? fecha_especifica = null)
        {
            string fecha = especificar_fecha
                ? ((DateTime) fecha_especifica).ToString("MM/dd/yyyy")
                : DateTime.Now.ToString("MM/dd/yyyy");
            string recuroyparametro = string.Format(
                "Transacciones_Credito/Traslados_Requeridos_Agencia?id_agencia={0}&especificar_fecha={1}&fecha_especifica={2}",
                id_agencia,
                especificar_fecha,
                fecha);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            List<Traslado_Abono> pagos = JsonConvert.DeserializeObject<List<Traslado_Abono>>(contenido);
            var lis_json = JsonSerializer.Serialize(pagos);
            ViewBag.pagos = lis_json;
          
            return PartialView(pagos);
        }

        [HttpPost]
        public async Task<IActionResult> Ejecutar_Traslados(string lista_pagos)
        {
            List<Traslado_Abono> pagos = await Convertir_String(lista_pagos);
            string jsonstring = JsonConvert.SerializeObject(pagos).Replace("\"", "'");
            string recuroyparametro = string.Format(
                "Transacciones_Credito/Ejecutar_Pagos_Requeridos_Agencia");
            ResponseApi responseApi = await OperarApi(HttpMethod.Post, recuroyparametro, jsonstring);
            Mensaje_Respuesta mr = new Mensaje_Respuesta(responseApi.Satisfactorio, 0, responseApi.Error, responseApi.ContenidofromJson);
            return PartialView(MensajeAsync, mr);
        }
        [HttpPost]
        public async Task<IActionResult> Ejecutar_factura(string lista_pagos, bool isDte = false)
        {
            List<Traslado_Abono> pagos = await Convertir_String(lista_pagos);
            string jsonstring = JsonConvert.SerializeObject(pagos).Replace("\"", "'");
            string direccion_credito =
                 string.Format("Transacciones_Credito/GenerarDtetraslados?isDte={0}", isDte);
            ResponseApi respuesta_factura = await OperarApi(HttpMethod.Post, direccion_credito, jsonstring);
            if (respuesta_factura.Satisfactorio)
            {
                if (isDte)
                {
                    return Content(respuesta_factura.ContenidofromJson, "text/plain");
                }
                else
                {
                    Impresion factura = JsonConvert.DeserializeObject<Impresion>(respuesta_factura.ContenidofromJson);
                    factura.Factura.Fecha = DateTime.Now;
                    return new ViewAsPdf("Mostrar_Factura_Abono", factura)
                    {
                        PageSize = Size.Letter,
                        PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                        PageMargins = { Left = 80, Right = 10 }
                    };
                }
            }
            else
            {
                Mensaje_Respuesta mensaje = new Mensaje_Respuesta(respuesta_factura.Satisfactorio, 0,
                    respuesta_factura.Error, respuesta_factura.ContenidofromJson);
                return BadRequest(mensaje);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Ejecutar_facturaRefinanciamientos(decimal totalGravados, decimal totalExcentos, int  IdAgencia)
        {

            string recuroyparametro =
                string.Format(
                    "Transacciones_Credito/GenerarDteRefinanciamientos?totalGravados={0}&totalExcentos={1}&IdAgencia={2}",
                    totalGravados, totalExcentos, IdAgencia);
            ResponseApi responseApi = await OperarApi(HttpMethod.Post, recuroyparametro, null);
            if (responseApi.Satisfactorio)
                return Ok(responseApi.ContenidofromJson);
            else
                return BadRequest(responseApi.ContenidofromJson);
        }
        
        private async Task<List<Traslado_Abono>> Convertir_String(string lista)
        {
            List<Traslado_Abono> result = JsonConvert.DeserializeObject<List<Traslado_Abono>>(lista);
            return result;
        }

        private async Task<int> Id_Linea_Credito_Historial(int id_credito)
        {
            string recuroyparametro = string.Format("Creditos/Linea_Credito_Historial?id_credito={0}", id_credito);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            if (responseApi.Satisfactorio)
            {
                string contenido = responseApi.ContenidofromJson;
                Linea_Credito_Historial linea = JsonConvert.DeserializeObject<Linea_Credito_Historial>(contenido);
                return linea.Id_Linea_Credito_Historial;
            }
            else
                return 0;
        }

        #region Cobros_periodicos historial
        [HttpGet]
        public async Task<IActionResult> Cobros_Periodicos_Historial(int id_credito)
        {
            string recuroyparametro = string.Format("Cobro_Periodico/Cobros_Peridodicos_Credito?id_credito={0}", id_credito);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            List<Cobro_Periodico_Historial> cobros = JsonConvert.DeserializeObject<List<Cobro_Periodico_Historial>>(contenido);
            return PartialView(cobros);
        }

        [HttpGet]
        public async Task<IActionResult> Detalle_Cobro_Periodicos_Historial(bool esNuevo, int id_credito, int id_cobro_periodico_historial = 0)
        {
            ViewBag.esNuevo = esNuevo;
            ViewBag.Id_Linea_Credito_Historial = await Id_Linea_Credito_Historial(id_credito);
            if (esNuevo)
            {
                return PartialView();
            }

            string recuroyparametro = string.Format("Cobro_Periodico/Cobro_Periodico_Historial?id_cobro_periodico_historial={0}", id_cobro_periodico_historial);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            Cobro_Periodico_Historial cobro = JsonConvert.DeserializeObject<Cobro_Periodico_Historial>(contenido);
            return PartialView(cobro);
        }

        [HttpPost]
        public async Task<IActionResult> Nuevo_Cobro_Periodico_Historial(Cobro_Periodico_Historial cobro)
        {
            /*conectarse al api*/
            string jsonstring = JsonConvert.SerializeObject(cobro).Replace("\"", "'");
            ResponseApi resultado = await OperarApi(HttpMethod.Post, "Cobro_Periodico/Nuevo_Cobro_Periodico_Historial", jsonstring);
            if (resultado.Satisfactorio)
                return Ok(resultado.ContenidofromJson);
            else
                return BadRequest(resultado.ContenidofromJson);
        }

        [HttpPost]
        public async Task<IActionResult> Editar_Cobro_Periodico_Historial(Cobro_Periodico_Historial cobro)
        {
            /*conectarse al api*/
            string jsonstring = JsonConvert.SerializeObject(cobro).Replace("\"", "'");
            ResponseApi resultado = await OperarApi(HttpMethod.Put, "Cobro_Periodico/Editar_Cobro_Periodico_Historial", jsonstring);
            if (resultado.Satisfactorio)
                return Ok(resultado.ContenidofromJson);
            else
                return BadRequest(resultado.ContenidofromJson);
        }

        #endregion

        #region Manejo de deducciones en desembolso historial
        [HttpGet]
        public async Task<IActionResult> Deducciones_Desembolso_Historial(int id_credito)
        {
            string recuroyparametro = string.Format("Deduccion_Desembolso/Deducciones_Desembolso_Historial?id_credito={0}", id_credito);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            List<Deduccion_Historial> deducciones = JsonConvert.DeserializeObject<List<Deduccion_Historial>>(contenido);
            return PartialView(deducciones);
        }

        [HttpGet]
        public async Task<IActionResult> Detalle_Deduccion_Desembolso_Historial(bool esNuevo, int id_credito = 0, int id_deduccion_desembolso_historial = 0)
        {
            var tipovalor = from Tipo_Valor d in Enum.GetValues(typeof(Tipo_Valor))
                select new SelectListItem { Value = ((int)d).ToString(), Text = d.ToString() };
            ViewBag.Tipo_Valor = new SelectList(tipovalor, "Value", "Text");
            ViewBag.esNuevo = esNuevo;
            ViewBag.Id_Linea_Credito_Historial = await Id_Linea_Credito_Historial(id_credito);
            if (esNuevo)
            {
                return PartialView();
            }

            string recuroyparametro = string.Format("Deduccion_Desembolso/Deduccion_Desembolso_Historial?id_deduccion_desembolso_historial={0}", id_deduccion_desembolso_historial);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            Deduccion_Historial deduccion = JsonConvert.DeserializeObject<Deduccion_Historial>(contenido);
            return PartialView(deduccion);
        }

        [HttpPost]
        public async Task<IActionResult> Nuevo_Deduccion_Desembolso_Historial(Deduccion_Historial deduccion)
        {
            /*conectarse al api*/
            string jsonstring = JsonConvert.SerializeObject(deduccion).Replace("\"", "'");
            ResponseApi resultado = await OperarApi(HttpMethod.Post, "Deduccion_Desembolso/Nueva_Deduccion_Desembolso_Historial", jsonstring);
            if (resultado.Satisfactorio)
                return Ok(resultado.ContenidofromJson);
            else
                return BadRequest(resultado.ContenidofromJson);
        }

        [HttpPost]
        public async Task<IActionResult> Editar_Deduccion_Desembolso_Historial(Deduccion_Historial deduccion)
        {
            /*conectarse al api*/
            string jsonstring = JsonConvert.SerializeObject(deduccion).Replace("\"", "'");
            ResponseApi resultado = await OperarApi(HttpMethod.Put, "Deduccion_Desembolso/Editar_Deduccion_Desembolso_Historial", jsonstring);
            if (resultado.Satisfactorio)
                return Ok(resultado.ContenidofromJson);
            else
                return BadRequest(resultado.ContenidofromJson);
        }

        [HttpGet]
        public async Task<IActionResult> Deducciones_Historial(int id_persona)
        {
            string recuroyparametro = string.Format("Deduccion_Desembolso/Deducciones_Historiales?id_persona={0}", id_persona);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            List<Deduccion_Historial> deduccion = JsonConvert.DeserializeObject<List<Deduccion_Historial>>(contenido);
            return PartialView(deduccion);
        }

        #endregion


        [HttpGet]
        public IActionResult Filtrar_Rendimiento()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Resultado_Rendimiento(FechasFiltro filtro, int id_agencia)
        {
            string inicio = filtro.inicio.Value.ToString("MM/dd/yyyy");
            string final = filtro.final.Value.ToString("MM/dd/yyyy");
            string recuroyparametro = string.Format("Creditos/Rendimiento?fecha_inicio={0}&fecha_final={1}&id_agencia={2}", inicio, final, id_agencia);

            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            List<Rendimiento_Vista> rendimientos = JsonConvert.DeserializeObject<List<Rendimiento_Vista>>(contenido);
            return PartialView(rendimientos);
        }

        [HttpGet]
        public async Task<IActionResult> Reajuste_Saldo(int id_credito)
        {
            /*conectarse al api*/
            string recuroyparametro = string.Format("Transacciones_Credito/Reajuste_Saldo?id_credito={0}", id_credito);
            ResponseApi resultado = await OperarApi(HttpMethod.Put, recuroyparametro, null);
            if (resultado.Satisfactorio)
                return Ok(resultado.ContenidofromJson);
            else
                return BadRequest(resultado.ContenidofromJson);
        }

        [HttpGet]
        public async Task<IActionResult> Credito_Amortizacion(int id_linea, decimal monto, int id_periodo, int plazo, int id_credito = 0, int numero_cuotas = 0)
        {
            List<Amortizacion> amortizacion = await Credito_Amortizacion_Principal(id_linea, monto, id_periodo, plazo, id_credito, numero_cuotas);
            string valor = HttpContext.Session.GetString("empresa");
            ViewBag.nombreEmpresa = valor.Replace('"', ' ');
            return PartialView(amortizacion);
        }

        [HttpGet]
        public async Task<IActionResult> Ultima_Fecha_Amortizacion(int id_linea, decimal monto, int id_periodo, int plazo, int id_credito = 0, int numero_cuotas = 0)
        {
            List<Amortizacion> amortizacion = await Credito_Amortizacion_Principal(id_linea, monto, id_periodo, plazo, id_credito, numero_cuotas);

            return Ok(amortizacion.Last().FechaPago.ToString("yyyy-MM-dd"));
        }

        private async Task<List<Amortizacion>> Credito_Amortizacion_Principal(int id_linea, decimal monto, int id_periodo, int plazo, int id_credito = 0, int numero_cuotas = 0)
        {
            string recuroyparametro = string.Format("Creditos/CuotaPrimaria?id_linea={0}&monto={1}&id_periodo={2}&plazo={3}&id_credito={4}&numero_cuotas={5}", id_linea, monto, id_periodo, plazo, id_credito, numero_cuotas);
            ResponseApi responseApi = await OperarApi(HttpMethod.Post, recuroyparametro, null);
            /*convertir el json por ahora*/
            string contenido = responseApi.ContenidofromJson;
            ResultadoCalculoCreditio resultado = JsonConvert.DeserializeObject<ResultadoCalculoCreditio>(contenido);
            return resultado.Amortizacion;
        }

        [HttpGet]
        public async Task<IActionResult> Reporte_Estado_Cuenta(int id_credito)
        {
            string recuroyparametro = string.Format("Creditos/Estado_Cuenta_Creditos?id_credito={0}", id_credito);

            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            Estado_Cuenta_Prestamos estado = JsonConvert.DeserializeObject<Estado_Cuenta_Prestamos>(contenido);
            string valor = HttpContext.Session.GetString("empresa");
            ViewBag.nombreEmpresa = valor.Replace('"', ' ');
            return new ViewAsPdf("Reporte_Estado_Cuenta", estado, ViewData)
            {
                PageSize = Size.Letter,
                PageOrientation = Orientation.Portrait,
                PageMargins = { Left = 10, Right = 10 }
            };

        }


        public async Task<IActionResult> Filtro_Credito_Colocado()
        {
            FechasFiltro filtro = new FechasFiltro();
            var agencias = await CargarAgencias();
            var comportamientos = await Comportamientos_Credito_Reportes();

            ViewBag.Id_Agencia = new SelectList(agencias, "Value", "Text");
            ViewBag.Id_Comportamiento_Credito = new SelectList(comportamientos, "Value", "Text");
            return View(filtro);
        }

        [HttpGet]
        public async Task<IActionResult> Creditos_Colocados(
            FechasFiltro filtro,
            int Id_Agencia,
            int Id_Comportamiento_Credito)
        {
            string inicio = filtro.inicio.Value.ToString("MM/dd/yyyy");
            string final = filtro.final.Value.ToString("MM/dd/yyyy");
            string recuroyparametro = string.Format(
                "Creditos/Creditos_Colocados?inicio={0}&fin={1}&id_agencia={2}&id_comportamiento={3}",
                inicio,
                final,
                Id_Agencia,
                Id_Comportamiento_Credito);

            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            List<Creditos_Colocados> creditos = JsonConvert.DeserializeObject<List<Creditos_Colocados>>(contenido);
            string valor = HttpContext.Session.GetString("empresa");
            ViewBag.nombreEmpresa = valor.Replace('"', ' ');
            ViewBag.fecha = inicio;
            DateTime fecha_actual = DateTime.Now;
            ViewBag.fechaactual = fecha_actual;
            return new ViewAsPdf("Reporte_Creditos_Colocados", creditos, ViewData)
            {
                PageSize = Size.Letter,
                PageOrientation = Orientation.Portrait,
                PageMargins = { Left = 10, Right = 10 }
            };
        }
        [HttpGet]
        public async Task<IActionResult> MostrarRefinanciamientoFacturar(FechasFiltro filtro, int Id_Agencia, int Id_Comportamiento_Credito)
        {
            string inicio = filtro.inicio.Value.ToString("MM/dd/yyyy");
            string final = filtro.final.Value.ToString("MM/dd/yyyy");
            string recuroyparametro = string.Format(
                "Creditos/Creditos_Colocados?inicio={0}&fin={1}&id_agencia={2}&id_comportamiento={3}",
                inicio,
                final,
                Id_Agencia,
                Id_Comportamiento_Credito);

            // Llamada a la API para obtener los créditos
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            List<Creditos_Colocados> creditos = JsonConvert.DeserializeObject<List<Creditos_Colocados>>(contenido);

            var totalGravados = creditos.Sum(c => c.totalGravados ?? 0);
            var totalExcentos = creditos.Sum(c => c.totalExcentos ?? 0);

            ViewBag.TotalGravados = totalGravados;
            ViewBag.TotalExcentos = totalExcentos;
            return PartialView();
        }

        [HttpGet]
        public IActionResult Cambio_Fecha()
        {
            return PartialView();
        }
        [HttpPost]
        public async Task<IActionResult> Cambiar_Fecha_Credito(int id_credito, DateTime fecha_nueva)
        {
            string fecha = fecha_nueva.ToString("MM/dd/yyyy");
            string recuroyparametro = string.Format(
                "Creditos/Cambio_Fecha_Credito?id_credito={0}&fecha_nueva={1}",
                id_credito,
                fecha);
            ResponseApi responseApi = await OperarApi(HttpMethod.Put, recuroyparametro, null);
            if (responseApi.Satisfactorio)
                return Ok(responseApi.ContenidofromJson);
            else
                return BadRequest(responseApi.ContenidofromJson);
        }

        [HttpGet]
        public async Task<IActionResult> Creditos_Pendientes_Liquidar()
        {
            ViewBag.Rol = await Rol_Usuario();
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Creditos_Pendientes(DateTime fecha_nueva, int id_agencia = 0)
        {
            string fecha = fecha_nueva.ToString("MM/dd/yyyy");
            string recuroyparametro = string.Format("Creditos/Creditos_Pendientes_Liquidar?fecha={0}&id_agencia={1}", fecha, id_agencia);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);

            string contenido = responseApi.ContenidofromJson;
            List<Credito> creditos_pendientes = JsonConvert.DeserializeObject<List<Credito>>(contenido);
            return PartialView(creditos_pendientes);
        }

        private async Task<string> Rol_Usuario()
        {
            string recurso = string.Format("Account/RolPorUsuario");
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recurso, null);
            string contenido = responseApi.ContenidofromJson;
            string rol = JsonConvert.DeserializeObject<string>(contenido);
            return (rol);
        }
        [HttpGet]
        public IActionResult Buscar_Integrantes_Grupo()
        {
            return PartialView();
        }
        [HttpGet]
        public async Task<IActionResult> Consulta_Grupo(string nombre_grupo)
        {
            string recuroyparametro = string.Format(
                "Creditos/Consulta_Grupo?nombre_grupo={0}",
                nombre_grupo);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            Credito_Grupal grupo = JsonConvert.DeserializeObject<Credito_Grupal>(contenido);
            ViewBag.id_grupo = grupo.Id_Credito_Grupal;
            return Ok(grupo.Id_Credito_Grupal);
        }
        [HttpGet]
        public async Task<IActionResult> Integrantes_Grupo(string nombre_grupo)
        {
            string recuroyparametro = string.Format("Creditos/Consultar_Grupo?nombre_grupo={0}", nombre_grupo);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);

            string contenido = responseApi.ContenidofromJson;
            List<Detalle_Credito_Grupal> integrantes = JsonConvert.DeserializeObject<List<Detalle_Credito_Grupal>>(contenido);
            return PartialView(integrantes);

        }
        [HttpPost]
        public async Task<IActionResult> Guardar_Integrante_Grupo(int id_credito, int id_grupo, bool es_lider = false)
        {
            string recuroyparametro = string.Format("Creditos/Agregar_Personas_Grupo?id_credito={0}&id_grupo={1}&es_lider={2}", id_credito,
                id_grupo, es_lider);
            ResponseApi responseApi = await OperarApi(HttpMethod.Put, recuroyparametro, null);
            if (responseApi.Satisfactorio)
                return Ok(responseApi.ContenidofromJson);
            else
                return BadRequest(responseApi.ContenidofromJson);
        }
        [HttpPost]
        public async Task<IActionResult> Cambiar_Lider(int id_credito, int id_grupo, bool es_lider = false)
        {
            string recuroyparametro = string.Format("Creditos/Cambiar_Lider?id_credito={0}&id_grupo={1}&es_lider={2}", id_credito,
                id_grupo, es_lider);
            ResponseApi responseApi = await OperarApi(HttpMethod.Put, recuroyparametro, null);
            if (responseApi.Satisfactorio)
                return Ok(responseApi.ContenidofromJson);
            else
                return BadRequest(responseApi.ContenidofromJson);
        }
        [HttpPost]
        public async Task<IActionResult> Eliminar_Detalle_Grupal(int id_detalle_grupo)
        {
            string recuroyparametro = string.Format("Creditos/Borrar_Integrante_Grupo?id_detalle_grupo={0}", id_detalle_grupo);
            ResponseApi responseApi = await OperarApi(HttpMethod.Delete, recuroyparametro, null);
            if (responseApi.Satisfactorio)
                return Ok(responseApi.ContenidofromJson);
            else
                return BadRequest(responseApi.ContenidofromJson);
        }
        [HttpGet]
        public IActionResult Agregar_Grupo_Solidario()
        {
            return PartialView();
        }
        [HttpGet]
        public async Task<IActionResult> Guardar_Grupo(string nombre_grupo)
        {
            string recuroyparametro = string.Format("Creditos/Agregar_Grupo?nombre_grupo={0}", nombre_grupo);
            ResponseApi responseApi = await OperarApi(HttpMethod.Put, recuroyparametro, null);
            if (responseApi.Satisfactorio)
                return Ok(responseApi.ContenidofromJson);
            else
                return BadRequest(responseApi.ContenidofromJson);
        }

        [HttpGet]
        public async Task<IActionResult> Obtener_Fecha_Diferente(int id_credito)
        {
            string recuroyparametro = string.Format(
                "Creditos/Obtener_Fecha_Diferente?id_credito={0}",
                id_credito);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string fecha = responseApi.ContenidofromJson;
                return Ok(fecha.Replace("\"", ""));
        }

        /*para hacer un cóngel*/
        [HttpGet]
        public IActionResult FechaCongelar()
        {
            return PartialView();
        }

        [HttpGet]
        public async Task<IActionResult> Procesar_Congelar(int id_credito,DateTime fecha) 
        {
            string fecha_convertida = fecha.ToString("MM/dd/yyyy");
            string recuroyparametro = string.Format(
                "Creditos/Procesar_Congelar?id_credito={0}&fecha={1}",
                id_credito,
                fecha_convertida);
            ResponseApi responseApi = await OperarApi(HttpMethod.Put, recuroyparametro, null);
            if (responseApi.Satisfactorio)
                return Ok(responseApi.ContenidofromJson);
            else
                return BadRequest(responseApi.ContenidofromJson);
        }
        [HttpGet]
        public IActionResult Congelar_Credito()
        {
            return View();
        }
        /*Para Disminuir Saldo a un crédito*/
        [HttpGet]
        public IActionResult Disminuir_Capital()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Procesar_Disminuir_Capital(int id_credito,decimal monto)
        {
            string recuroyparametro = string.Format(
                "Creditos/Cambiar_Capital?id_credito={0}&monto={1}", 
                id_credito,
                monto);
            ResponseApi responseApi = await OperarApi(HttpMethod.Put, recuroyparametro, null);
            if (responseApi.Satisfactorio)
                return Ok(responseApi.ContenidofromJson);
            else
                return BadRequest(responseApi.ContenidofromJson);
        }

        [HttpGet]
        public IActionResult Calcular_Pago_Anticipado()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Detalle_Pago_Anticipado(bool penalizacion,int id_credito = 0 )
        {

            string recuroyparametro = string.Format("Transacciones_Credito/Calcular_Pago_Anticipado?" +
                "id_credito={0}&penalizacion={1}", id_credito,penalizacion);
            ResponseApi responseApi = await OperarApi(HttpMethod.Post, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            Pago_Anticipado pago_Anticipado = JsonConvert.DeserializeObject<Pago_Anticipado>(contenido);
            return PartialView(pago_Anticipado);
        }
        [HttpPost]
        public async Task<IActionResult> Operacion_Pago_Anticipado(decimal porcentaje_descuento, 
            decimal pago_anticipado, decimal descuento)
        {
            decimal total = 0.00M;
            if (porcentaje_descuento == 0) {
                total = pago_anticipado;
            }
            else
            {
                total = Math.Round((porcentaje_descuento * descuento) + pago_anticipado, 2);
            }
            
            
            return Ok(total);
        }

        [HttpGet]
        public async Task<IActionResult> Vista_Detalle_Credito_Ahorro(int id_credito)
        {

            string recuroyparametro = string.Format("Creditos/Detalle_Credito_Con_Ahorro?" +
                "id_credito={0}", id_credito);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            Detalle_Credito_Ahorro detalle = JsonConvert.DeserializeObject<Detalle_Credito_Ahorro>(contenido);
            return PartialView(detalle);
        }
    }
}
