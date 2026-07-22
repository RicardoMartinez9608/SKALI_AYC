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
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Rotativa.AspNetCore;
using Rotativa.AspNetCore.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AyCWeb.Controllers
{
    public class Depositos_PlazoController : BaseController
    {
        public IActionResult Index(AhorroFiltro filtro, string nombre, string dui)
        {
            if (filtro.id_persona != 0)
                filtro.porCliente = true;
            ViewBag.Id_Persona = filtro.id_persona;
            ViewBag.nombre = nombre;
            ViewBag.dui = dui;
            return View(filtro);
        }
        public async Task<IActionResult> Depositos(DataSourceLoadOptions loadOptions, AhorroFiltro filtro)
        {
            /*convertir el json por ahora*/
            List<Deposito_Plazo> ahorrros = await Depositos_Principal(filtro);
            var result = DataSourceLoader.Load(ahorrros, loadOptions);
            var resultJson = JsonConvert.SerializeObject(result);
            return Content(resultJson, "application/json");
        }

        /*conección directa*/
        private async Task<List<Deposito_Plazo>> Depositos_Principal(AhorroFiltro filtro)
        {
            string recuroyparametro = null;
            if (filtro.porCliente)
                recuroyparametro = string.Format("Deposito_Plazo/Deposito_Plazo_Por_Persona?id_persona={0}", filtro.id_persona);
            else
            {
                recuroyparametro = string.Format("Ahorros/Ahorros");
            }
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            List<Deposito_Plazo> ahorrros = JsonConvert.DeserializeObject<List<Deposito_Plazo>>(contenido);
            return ahorrros;
        }
        [HttpGet]
        public async Task<IActionResult> Manejo_Depositos(bool esNuevo, int idDeposito, int id_persona)
        {
            ViewBag.esNuevo = esNuevo;
       
            if (!esNuevo) {
                Deposito_Plazo ahorro = await Detalle(idDeposito);
                Debug.WriteLine($"Deposito: {ahorro.Id_Deposito_Plazo}");
                Debug.WriteLine($"Ahorro: {ahorro.Id_Ahorro}");
                return PartialView(ahorro);
               
            }
            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult>Crear_Cuenta_Ahorro(Deposito_Plazo ahorro)
        {
            string recuroyparametro = string.Format("Deposito_Plazo/Nuevo_Deposito_Plazo");
            string jsonstring = JsonConvert.SerializeObject(ahorro).Replace("\"", "'");
            ResponseApi responseApi = await OperarApi(HttpMethod.Post, recuroyparametro, jsonstring);
            if (responseApi.Satisfactorio)
                return Ok(responseApi.ContenidofromJson);
            else
                return BadRequest(responseApi.ContenidofromJson);
        }
        [HttpGet]
        public async Task<Deposito_Plazo> Detalle(int id_deposito)
        {
            string recuroyparametro = string.Format("Deposito_Plazo/GetDepositoDetalle?id_deposito={0}", id_deposito);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            /*convertir el json por ahora*/
            string contenido = responseApi.ContenidofromJson;
            return JsonConvert.DeserializeObject<Deposito_Plazo>(contenido);
        }
        public async Task<IActionResult> Editar_Ahorro_Manual(Deposito_Plazo ahorro)
        {
            ResponseApi response = await Editar_Deposito(ahorro);
            return Ok();
        }
        private async Task<ResponseApi> Editar_Deposito(Deposito_Plazo ahorro)
        {
            string url = string.Format("Deposito_Plazo/Modificar");
            string jsonstring = JsonConvert.SerializeObject(ahorro).Replace("\"", "'");
            ResponseApi resultado = await OperarApi(HttpMethod.Put, url, jsonstring);
            return resultado;
        }
        [HttpGet]
        public async Task<IEnumerable<SelectListItem>> AhorrosComboDeposito(int id_persona)
        {
            string recuroyparametro = string.Format("Ahorros/AhorrosPorPersona?id_persona={0}", id_persona);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            /*convertir el json por ahora*/
            string contenido = responseApi.ContenidofromJson;
            List<Ahorro> ahorrros = JsonConvert.DeserializeObject<List<Ahorro>>(contenido);
            return ahorrros.Select(s => new SelectListItem { Value = s.Id_Ahorro.ToString(), Text = s.No_Ahorro }); ;
        }
        //----------------------------------------Manejo Beneficiarios-------------------------------------------------------------
        [HttpGet]
        public async Task<IActionResult> Beneficiario_Autorizado()
        {
            return PartialView();
        }
        [HttpGet]
        public async Task<IActionResult> Detalle_Beneficiario(int id_deposito)
        {
            string recuroyparametro = string.Format("Deposito_Plazo/Beneficiarios_Deposito?id_deposito={0}", id_deposito);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            /*convertir el json por ahora*/
            string contenido = responseApi.ContenidofromJson;
            List<Beneficiarios_Deposito_Plazo> integrantes = JsonConvert.DeserializeObject<List<Beneficiarios_Deposito_Plazo>>(contenido);
            return PartialView(integrantes);
        }

        [HttpGet]
        public async Task<IActionResult> Nuevo_Beneficiario()
        {
            return PartialView();
        }
        [HttpPost]
        public async Task<IActionResult> Crear_Beneficiario(Beneficiarios_Deposito_Plazo beneficiario)
        {
            string recuroyparametro = string.Format("Deposito_Plazo/Agregar_Beneficiario_Deposito");
            string jsonstring = JsonConvert.SerializeObject(beneficiario).Replace("\"", "'");
            ResponseApi responseApi = await OperarApi(HttpMethod.Post, recuroyparametro, jsonstring);
            if (responseApi.Satisfactorio)
                return Ok(responseApi.ContenidofromJson);
            else
                return BadRequest(responseApi.ContenidofromJson);
        }
        [HttpPost]
        public async Task<IActionResult> Eliminar_Beneficiario(int id_beneficiario)
        {
            string recuroyparametro = string.Format("Deposito_Plazo/BorrarBeneficiario?id_beneficiario={0}", id_beneficiario);
            ResponseApi responseApi = await OperarApi(HttpMethod.Delete, recuroyparametro, null);
            if (responseApi.Satisfactorio)
                return Ok(responseApi.ContenidofromJson);
            else
                return BadRequest(responseApi.ContenidofromJson);
        }
        [HttpGet]
        public async Task<IActionResult> Detalle_Editar_Beneficiario(int id_deposito)
        {
            string recuroyparametro = string.Format("Deposito_Plazo/GetBeneficiarioDetalle?id_deposito={0}", id_deposito);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            /*convertir el json por ahora*/
            string contenido = responseApi.ContenidofromJson;
            Beneficiarios_Deposito_Plazo integrantes = JsonConvert.DeserializeObject<Beneficiarios_Deposito_Plazo>(contenido);
            return PartialView(integrantes);
        }
        [HttpPost]
        public async Task<IActionResult> Editar_Beneficiario(Beneficiarios_Deposito_Plazo beneficiario)
        {
            string recuroyparametro = string.Format("Deposito_Plazo/Modificar_Beneficiario");
            string jsonstring = JsonConvert.SerializeObject(beneficiario).Replace("\"", "'");
            ResponseApi responseApi = await OperarApi(HttpMethod.Put, recuroyparametro, jsonstring);
            if (responseApi.Satisfactorio)
                return Ok(responseApi.ContenidofromJson);
            else
                return BadRequest(responseApi.ContenidofromJson);
        }
        //----------------------------------------Manejo Autorizados-------------------------------------------------------------
        [HttpGet]
        public async Task<IActionResult> Detalle_Autorizado(int id_deposito)
        {
            string recuroyparametro = string.Format("Deposito_Plazo/Autorizados_Deposito?id_deposito={0}", id_deposito);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            /*convertir el json por ahora*/
            string contenido = responseApi.ContenidofromJson;
            List<Autorizado> integrantes = JsonConvert.DeserializeObject<List<Autorizado>>(contenido);
            return PartialView(integrantes);
        }
        [HttpGet]
        public async Task<IActionResult> Nuevo_Autorizado()
        {
            return PartialView();
        }
        [HttpPost]
        public async Task<IActionResult> Crear_Autorizado(Autorizado autorizado)
        {
            string recuroyparametro = string.Format("Deposito_Plazo/Agregar_Autorizado");
            string jsonstring = JsonConvert.SerializeObject(autorizado).Replace("\"", "'");
            ResponseApi responseApi = await OperarApi(HttpMethod.Post, recuroyparametro, jsonstring);
            if (responseApi.Satisfactorio)
                return Ok(responseApi.ContenidofromJson);
            else
                return BadRequest(responseApi.ContenidofromJson);
        }
        [HttpPost]
        public async Task<IActionResult> Eliminar_Autorizado(int id_autorizado)
        {
            string recuroyparametro = string.Format("Deposito_Plazo/BorrarAutorizado?id_autorizado={0}", id_autorizado);
            ResponseApi responseApi = await OperarApi(HttpMethod.Delete, recuroyparametro, null);
            if (responseApi.Satisfactorio)
                return Ok(responseApi.ContenidofromJson);
            else
                return BadRequest(responseApi.ContenidofromJson);
        }
        [HttpGet]
        public async Task<IActionResult> Detalle_Editar_Autorizado(int id_autorizado)
        {
            string recuroyparametro = string.Format("Deposito_Plazo/GetAutorizadoDetalle?id_autorizado={0}", id_autorizado);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            /*convertir el json por ahora*/
            string contenido = responseApi.ContenidofromJson;
            Autorizado integrantes = JsonConvert.DeserializeObject<Autorizado>(contenido);
            return PartialView(integrantes);
        }
        [HttpPost]
        public async Task<IActionResult> Editar_Autorizado(Autorizado autorizado)
        {
            string recuroyparametro = string.Format("Deposito_Plazo/Modificar_Autorizado");
            string jsonstring = JsonConvert.SerializeObject(autorizado).Replace("\"", "'");
            ResponseApi responseApi = await OperarApi(HttpMethod.Put, recuroyparametro, jsonstring);
            if (responseApi.Satisfactorio)
                return Ok(responseApi.ContenidofromJson);
            else
                return BadRequest(responseApi.ContenidofromJson);
        }
        public async Task<PartialViewResult> Transacciones(int id_deposito)
        {
            ResponseApi api = await Transacciones_Deposito(id_deposito);
            IEnumerable<Transaccion_Ahorro_Vista> transacciones = JsonConvert.DeserializeObject<List<Transaccion_Ahorro_Vista>>(api.ContenidofromJson);
            ViewBag.Respuesta = new Mensaje_Respuesta(api.Satisfactorio, transacciones.Count(), api.Error);
            return PartialView(transacciones);
        }
        private async Task<ResponseApi> Transacciones_Deposito(int id_deposito)
        {
            string recuroyparametro = string.Format("Transacciones_Deposito_Plazo/Transacciones?id_deposito={0}", id_deposito);
            return await OperarApi(HttpMethod.Get, recuroyparametro, null);
        }
        /*Operaciones básicas para una cuenta de ahorro*/
        [HttpPost]
        public async Task<IActionResult> RegistrarDeposito(
            int id_deposito,
            decimal valor,
            string comprobante = "",
            DateTime? fecha_cambiada = null,
            bool cambiar_fecha = false)
        {

            string fecha = cambiar_fecha ? ((DateTime)fecha_cambiada).ToString("MM/dd/yyyy")
                 : DateTime.Now.ToString("MM/dd/yyyy");

            string url = string.Format("Transacciones_Deposito_Plazo/GuardarDeposito?id_deposito={0}&valor={1}&comprobante={2}&fecha_cambiada={3}&cambiar_fecha={4}",
                id_deposito,
                valor,
                comprobante,
                fecha,
                cambiar_fecha);
            ResponseApi resultado = await OperarApi(HttpMethod.Put, url, null);
            Mensaje_Respuesta mensaje = new Mensaje_Respuesta(resultado.Satisfactorio, 0, resultado.Error, resultado.ContenidofromJson);
            return PartialView(MensajeAsync, mensaje);
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarRetiro(
        int id_deposito,
        decimal valor,
        string comprobante = "",
        DateTime? fecha_cambiada = null,
        bool cambiar_fecha = false)
        {

            string fecha = cambiar_fecha ? ((DateTime)fecha_cambiada).ToString("MM/dd/yyyy")
                 : DateTime.Now.ToString("MM/dd/yyyy");

            string url = string.Format(
                "Transacciones_Deposito_Plazo/GuardarRetiro?id_deposito={0}&valor={1}&comprobante={2}&fecha_cambiada={3}&cambiar_fecha={4}",
                id_deposito,
                valor,
                comprobante,
                fecha,
                cambiar_fecha);
            ResponseApi resultado = await OperarApi(HttpMethod.Put, url, null);
            Mensaje_Respuesta mensaje = new Mensaje_Respuesta(resultado.Satisfactorio, 0, resultado.Error, resultado.ContenidofromJson);
            return PartialView(MensajeAsync, mensaje);
        }
        /*postear transacción*/
        [HttpGet]
        public async Task<IActionResult> Mostrar_Recibo(int id_transaccion_deposito)
        {
            string direccion_credito = string.Format("Transacciones_Deposito_Plazo/Mostrar_Factura_Transaccion?id_transaccion_deposito={0}", id_transaccion_deposito);
            ResponseApi respuesta_factura = await OperarApi(HttpMethod.Get, direccion_credito, null);
            if (respuesta_factura.Satisfactorio)
            {
                Impresion factura = JsonConvert.DeserializeObject<Impresion>(respuesta_factura.ContenidofromJson);
                return new ViewAsPdf("Mostrar_Factura_Deposito", factura)
                {
                    PageSize = Size.A4,
                    PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                    PageMargins = { Left = 80, Right = 10 }
                };
            }
            else
            {
                Mensaje_Respuesta mensaje = new Mensaje_Respuesta(respuesta_factura.Satisfactorio, 0, respuesta_factura.Error, respuesta_factura.ContenidofromJson);
                return BadRequest(mensaje);
            }
        }
    }


}
