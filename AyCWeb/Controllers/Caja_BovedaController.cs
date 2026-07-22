using System;
using System.Collections.Generic;
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
using ModelNucleo.Model;
using ModelNucleo.Model.ModelView;
using Newtonsoft.Json;
using Rotativa.AspNetCore;
using Rotativa.AspNetCore.Options;
using Orientation = Rotativa.AspNetCore.Options.Orientation;

namespace AyCWeb.Controllers
{
    public class Caja_BovedaController : BaseController
    {
        public async Task<IActionResult> Cajas(string id_usuario = null, int id_agencia = 0, bool boveda = false, string mensaje = null)
        {
            List<Caja> cajas = await JCajas(id_usuario, id_agencia, boveda);
            ViewBag.Id_Usuario = id_usuario;
            if (!string.IsNullOrEmpty(mensaje))
            {
                ViewBag.ConRespuesta = true;
                ViewBag.Respuesta = JsonConvert.DeserializeObject<Mensaje_Respuesta>(mensaje);
            }
            else {
                ViewBag.ConRespuesta = false;
                ViewBag.Respuesta = null;
            }
            return View(cajas);
        }

        public async Task<IActionResult> Cajas_Combo(int id_agencia)
        {
            return PartialView(await JCajas(null, id_agencia));
        }

        private async Task<List<Caja>> JCajas(string id_usuario = null, int id_agencia = 0, bool boveda = false)
        {
            string recuroyparametro = string.Format("Caja/Cajas?usuario={0}&id_agencia={1}&boveda={2}", id_usuario, id_agencia, boveda);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            return JsonConvert.DeserializeObject<List<Caja>>(contenido);
        }

        public IActionResult Nueva_Caja(string id_usuario = null)
        {
            ViewBag.Id_Usuario = id_usuario;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Nueva_Caja(Caja caja)
        {
            /*conectarse al api*/
            string jsonstring = JsonConvert.SerializeObject(caja).Replace("\"", "'");
            ResponseApi resultado = await OperarApi(HttpMethod.Post, "Caja/Crear_Caja", jsonstring);
            Mensaje_Respuesta mensaje = new Mensaje_Respuesta(resultado.Satisfactorio, 0, resultado.Error, resultado.ContenidofromJson);
            string msj_str = JsonConvert.SerializeObject(mensaje).Replace("\"", "'");
            return RedirectToAction("Cajas", new { id_usuario = caja.Id_Usuario, mensaje = msj_str });
        }

        public IActionResult F_nueva_caja(string id_usuario)
        {
            ViewBag.Id_Usuario = id_usuario;
            return View();
        }

        public async Task<IActionResult> Editar_Caja(int id_caja)
        {
            Caja caja = await Detalle_Caja_Unica(id_caja);
            return View(caja);
        }

        public async Task<IActionResult> Detalle_Caja(int id_caja)
        {
            Caja caja = await Detalle_Caja_Unica(id_caja);
            return View(caja);
        }

        private async Task<Caja> Detalle_Caja_Unica(int id_caja)
        {
            string recuroyparametro = string.Format("Caja/Detalle_Caja?id_caja={0}", id_caja);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            return JsonConvert.DeserializeObject<Caja>(contenido);
        }

        [HttpPost]
        public async Task<IActionResult> Editar_Caja(Caja caja)
        {
            /*conectarse al api*/
            string jsonstring = JsonConvert.SerializeObject(caja).Replace("\"", "'");
            ResponseApi resultado = await OperarApi(HttpMethod.Put, "Caja/Editar_Caja", jsonstring);
            Mensaje_Respuesta mensaje = new Mensaje_Respuesta(resultado.Satisfactorio, 0, resultado.Error, resultado.ContenidofromJson);
            string msj_str = JsonConvert.SerializeObject(mensaje).Replace("\"", "'");
            return RedirectToAction("Cajas", new { id_usuario = caja.Id_Usuario, mensaje = msj_str });
        }

        [HttpGet]
        public async Task<IActionResult> Detalle_Boveda()
        {
            string recuroyparametro = string.Format("Caja/Boveda_Asignada");
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            Boveda_Cajas boveda = JsonConvert.DeserializeObject<Boveda_Cajas>(contenido);
            return View(boveda);
        }

        public async Task<IActionResult> Usuarios(DataSourceLoadOptions loadOptions)
        {
            string busqueda = "_";
            if (loadOptions.Filter != null)
                busqueda = FilterBusquedaDX(loadOptions.Filter[0].ToString());


            string recuroyparametro = string.Format("Account/ListaUsuarios?busqueda={0}", busqueda);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            /*convertir el json por ahora*/
            string contenido = responseApi.ContenidofromJson;
            List<Usuario> usuarios = JsonConvert.DeserializeObject<List<Usuario>>(contenido);

            var result = DataSourceLoader.Load(usuarios, loadOptions);
            var resultJson = JsonConvert.SerializeObject(result);
            return Content(resultJson, "application/json");
        }

        //--------------------------------------------Asignacion de Fondos a Caja------------------------------------

        public async Task<IActionResult> Asignacion_Fondos_Caja()
        {
            string recuroyparametro = string.Format("ReporteFlujoEfectivo/Vista_Flujo_Efectivo");
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            List<Detalle_Efectivo_Vista> flujo = JsonConvert.DeserializeObject<List<Detalle_Efectivo_Vista>>(contenido);
            return View(flujo);
        }
        public async Task<IActionResult> Cajas_Combo_Agencia()
        {
            return PartialView(await Cajas_Agencia());
        }

        private async Task<List<Caja>> Cajas_Agencia()
        {
            string recuroyparametro = string.Format("Caja/CajasporAgencia");
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            return JsonConvert.DeserializeObject<List<Caja>>(contenido);
        }
        [HttpPost]
        public async Task<IActionResult> ReporteAsignacionFondos(List<String> ids)
        {
            List<String> valores = ids;
            string recuroyparametro = string.Format("ReporteFlujoEfectivo/Reporte_Denominacion");
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            Reporte_Denominacion flujo = JsonConvert.DeserializeObject<Reporte_Denominacion>(contenido);
            string valor = HttpContext.Session.GetString("empresa");
            ViewBag.nombreEmpresa = valor.Replace('"', ' ');

            return new ViewAsPdf("ReporteAsignacionFondos", flujo, ViewData)
            {
                PageSize = Size.Letter,
                PageOrientation = Orientation.Portrait,
                PageMargins = { Left = 10, Right = 10 }
            };
        }

        //comienza para nuevo orden de menu
        public IActionResult Remesa_Bancaria()
        {
            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> Transaccion_Remesa_Banco(int Id_Banco, decimal Monto,string Referencia, string razon_movimiento)
        {
            string recuroyparametro = string.Format("Transaccion_Simple/Remesar?id_banco={0}&monto={1}&referencia={2}&razon_movimiento={3}", Id_Banco, Monto, Referencia, razon_movimiento);
            ResponseApi responseApi = await OperarApi(HttpMethod.Put, recuroyparametro, null);
            if (responseApi.Satisfactorio)
                return Ok(responseApi.ContenidofromJson);
            return BadRequest(responseApi.ContenidofromJson);
        }


        public IActionResult Nueva_Factura_Servicio()
        {
            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> Procesar_Factura_Servicio(
            int id_factura_servicio,
            string no_comprobante,
            decimal monto,
            decimal monto_telefonia,
            bool es_credito_fiscal,
            DateTime fecha,
            string detalle_factura_servicio,
            int id_factura_servicio_basico = 0)
        {
            if (id_factura_servicio_basico > 0)
                id_factura_servicio = id_factura_servicio_basico;

            string fecha_concepto = fecha.ToString("MM/dd/yyyy");
            string recuroyparametro = string
                .Format("Transaccion_Simple/Guardar_Factura_Servicio?id_factura_servicio={0}&no_comprobante={1}&monto={2}&monto_telefonia={3}&es_credito_fiscal={4}&fecha={5}&detalle_factura_servicio={6}", 
                id_factura_servicio,
                no_comprobante,
                monto,
                monto_telefonia,
                es_credito_fiscal,
                fecha_concepto, 
                detalle_factura_servicio);
            ResponseApi responseApi = await OperarApi(HttpMethod.Post, recuroyparametro, null);
            if (responseApi.Satisfactorio)
                return Ok(responseApi.ContenidofromJson);
            return BadRequest(responseApi.ContenidofromJson);
        }

        public IActionResult Fondos_Agencia()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Remesar_Fondos_Agencia(decimal monto, int id_agencia)
        {
            string recuroyparametro = string.Format("Transaccion_Simple/Remesar_Fondos?monto={0}&id_agencia={1}", monto, id_agencia);
            ResponseApi responseApi = await OperarApi(HttpMethod.Put, recuroyparametro, null);
            if (responseApi.Satisfactorio)
                return Ok(responseApi.ContenidofromJson);
            return BadRequest(responseApi.ContenidofromJson);
        }


        /*Cuidado asignación de Fondo Inicial (por implementación)*/
        public IActionResult Fondo_Inicial()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Remesar_Fondo_Inicial(decimal monto, int id_agencia)
        {
            string recuroyparametro = string.Format("Transaccion_Simple/Fondo_Inicial?monto={0}&id_agencia={1}", monto, id_agencia);
            ResponseApi responseApi = await OperarApi(HttpMethod.Put, recuroyparametro, null);
            if (responseApi.Satisfactorio)
                return Ok(responseApi.ContenidofromJson);
            return BadRequest(responseApi.ContenidofromJson);
        }

        /*manejo de disponibilidad*/
        #region Disponibilidad
        [HttpGet]
        public IActionResult Manejar_Disponibilidad()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Nuevo_Cheque()
        {
            var bancos =await CargarBancos();
            var agencias =await CargarAgencias();
            ViewBag.Id_Banco= new SelectList(bancos, "Value", "Text");
            ViewBag.Id_Agencia= new SelectList(agencias, "Value", "Text");

            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> Nuevo_Cheque(Ingreso_Cheque cheque) 
        {
            string jsonstring = JsonConvert.SerializeObject(cheque).Replace("\"", "'");
            ResponseApi resultado = await OperarApi(HttpMethod.Post, "Cheque/NuevoCheque", jsonstring);
            if (resultado.Satisfactorio)
                return Ok(resultado.ContenidofromJson);
            return BadRequest(resultado.ContenidofromJson);
        }

        private async Task<IEnumerable<SelectListItem>> CargarBancos()
        {
            string recuroyparametro = "Banco/Bancos";
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            List<Banco> bancos = JsonConvert.DeserializeObject<List<Banco>>(contenido);
            return bancos.Select(s => new SelectListItem { Value = s.Id_Banco.ToString(), Text = s.No_Cuenta+" ("+s.Nombre+")" });
        }

        private async Task<IEnumerable<SelectListItem>> CargarAgencias()
        {
            string recuroyparametro = "Account/Agencias";
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            List<Agencia> agencias = JsonConvert.DeserializeObject<List<Agencia>>(contenido);
            return agencias.Select(s => new SelectListItem { Value = s.Id_Agencia.ToString(), Text = s.Nombre });
        }

        [HttpGet]
        public IActionResult Bolsa_Tercero()
        {
            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> Guardar_Bolsa_Tercero(decimal monto_tercero)
        {
            string recuroyparametro = string.Format("Transaccion_Simple/Colectar_Bolsa_Tercero?monto={0}", monto_tercero);
            ResponseApi responseApi = await OperarApi(HttpMethod.Post, recuroyparametro, null);
            if (responseApi.Satisfactorio)
                return Ok(responseApi.ContenidofromJson);
            return BadRequest(responseApi.ContenidofromJson);
        }

        #endregion
        [HttpGet]
        public async Task<IActionResult> Revertir_Traslado_Vista()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Datos_Reversion_Traslado(string codigo_credito, DateTime fecha_reporte)
        {
            string fecha = fecha_reporte.ToString("MM/dd/yyyy");
            string recuroyparametro = string.Format("Transacciones_Credito/Mostrar_Revertir_Traslado?codigo_credito={0}" +
                "&fecha={1}", codigo_credito,fecha);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            List<Transacciones_Reversion_Traslado> integrantes = JsonConvert.DeserializeObject<List<Transacciones_Reversion_Traslado>>(contenido);
            return PartialView(integrantes);

        }
        [HttpPost]
        public async Task<IActionResult> Revertir_Traslado(int id_transaccion_credito, int id_transaccion_ahorro)
        {
            string recuroyparametro = string.Format("Transacciones_Credito/Revertir_Traslado?id_transaccion_credito={0}" +
                "&id_transaccion_ahorro={1}", id_transaccion_credito, id_transaccion_ahorro);
            ResponseApi responseApi = await OperarApi(HttpMethod.Post, recuroyparametro, null);
            if (responseApi.Satisfactorio)
                return Ok(responseApi.ContenidofromJson);
            return BadRequest(responseApi.ContenidofromJson);
        }
    }
}