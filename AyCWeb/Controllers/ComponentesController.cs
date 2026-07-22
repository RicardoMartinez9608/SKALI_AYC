using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AyCWeb.Models;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Rendering;
using ModelNucleo.Model;
using ModelNucleo.Model.ModelView;
using System.Linq;

namespace AyCWeb.Controllers
{
    public class ComponentesController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
        private async Task<List<Cartera>> Carteras_Por_Agencia(int id_agencia)
        {
            string recurso = string.Format("Carteras/CarterasPorAgencia?id_agencia={0}", id_agencia);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recurso, null);
            string contenido = responseApi.ContenidofromJson;
            return JsonConvert.DeserializeObject<List<Cartera>>(contenido);
        }
        private async Task<List<Cartera>> Carteras_Por_Usuario_Logueado()
        {
            string recurso = string.Format("Carteras/CarterasPorAgenciaEnUsuario");
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recurso, null);
            string contenido = responseApi.ContenidofromJson;
            return JsonConvert.DeserializeObject<List<Cartera>>(contenido);
        }
        private async Task<List<TipoGarantia>> TipoGarantias()
        {
            string recuroyparametro = string.Format("Linea_Credito/getTipoGarantias");
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            return JsonConvert.DeserializeObject<List<TipoGarantia>>(contenido);
        }
        private async Task<List<TipoDestinoCredito>> TipoDestinoCredito()
        {
            string recuroyparametro = string.Format("Linea_Credito/getTipoDestinoCredito");
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            return JsonConvert.DeserializeObject<List<TipoDestinoCredito>>(contenido);
        }
        private async Task<List<Agencia>> Agencias()
        {
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, "Account/Agencias", null);
            /*convertir el json por ahora*/
            string contenido = responseApi.ContenidofromJson;
            return JsonConvert.DeserializeObject<List<Agencia>>(contenido);
        }
        [HttpGet]
        public async Task<IActionResult> Carteras(int id_agencia)
        {
            if (id_agencia > 0)
            {
                return PartialView(await Carteras_Por_Agencia(id_agencia));
            }
            else
            {
                return PartialView(await Carteras_Por_Usuario_Logueado());
            }
           
        }

        [HttpGet]
        public async Task<IActionResult> Agencias_Combo()
        {
            return PartialView(await Agencias());
        }
        [HttpGet]
        public async Task<IActionResult> getTipoGarantias()
        {
            return PartialView(await TipoGarantias());
        }
        [HttpGet]
        public async Task<IActionResult> getTipoDestinoCredito()
        {
            return PartialView(await TipoDestinoCredito());
        }
        [HttpGet]
        public async Task<IActionResult> Agencias_Combo_Metas()
        {
            return PartialView(await Agencias());
        }
        [HttpGet]
        public async Task<IActionResult> Agencias_Radio()
        {
            return PartialView(await Agencias());
        }

        [HttpGet]
        public async Task<IActionResult> Lineas_Ahorro()
        {
            string recuroyparametro = string.Format("Linea_Ahorros/LineasAhorro");

            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            /*convertir el json por ahora*/
            string contenido = responseApi.ContenidofromJson;
            List<Linea_Ahorro> lienasahorro = JsonConvert.DeserializeObject<List<Linea_Ahorro>>(contenido);

            return PartialView(lienasahorro);
        }

        [HttpGet]
        public async Task<IActionResult> Lineas_Credito()
        {
            string recuroyparametro = string.Format("Linea_Credito/LineasCredito");

            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            /*convertir el json por ahora*/
            string contenido = responseApi.ContenidofromJson;
            List<Linea_Credito> lienascredito = JsonConvert.DeserializeObject<List<Linea_Credito>>(contenido);

            return PartialView(lienascredito);
        }

        [HttpGet]
        public async Task<IActionResult> CreditosCombos(int id_persona)
        {
            string recuroyparametro = string.Format("Creditos/ComponentesCreditosPorPersona?id_persona={0}", id_persona);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            /*convertir el json por ahora*/
            string contenido = responseApi.ContenidofromJson;
            List<Credito_Componentes> creditos = JsonConvert.DeserializeObject<List<Credito_Componentes>>(contenido);
            return PartialView(creditos);
        }

        [HttpGet]
        public async Task<PartialViewResult> AhorrosCombo(int id_persona)
        {
            string recuroyparametro = string.Format("Ahorros/AhorrosPorPersona?id_persona={0}", id_persona);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            /*convertir el json por ahora*/
            string contenido = responseApi.ContenidofromJson;
            List<Ahorro> ahorrros = JsonConvert.DeserializeObject<List<Ahorro>>(contenido);
            return PartialView(ahorrros);
        }
        [HttpGet]
        public async Task<PartialViewResult> AhorrosComboDeposito(int id_persona)
        {
            string recuroyparametro = string.Format("Ahorros/AhorrosPorPersona?id_persona={0}", id_persona);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            /*convertir el json por ahora*/
            string contenido = responseApi.ContenidofromJson;
            List<Ahorro> ahorrros = JsonConvert.DeserializeObject<List<Ahorro>>(contenido);
            return PartialView(ahorrros);
        }
        [HttpGet]
        public async Task<PartialViewResult> ComboDeposito(int id_persona)
        {
            string recuroyparametro = string.Format("Deposito_Plazo/DepositosPorPersona?id_persona={0}", id_persona);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            /*convertir el json por ahora*/
            string contenido = responseApi.ContenidofromJson;
            List<Deposito_Plazo> ahorrros = JsonConvert.DeserializeObject<List<Deposito_Plazo>>(contenido);
            return PartialView(ahorrros);
        }

        [HttpGet]
        public async Task<PartialViewResult> Operaciones(int id_operacion = 0)
        {
            string recuroyparametro = string.Format("Operacion/Operaciones");
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            /*convertir el json por ahora*/
            string contenido = responseApi.ContenidofromJson;
            List<Operacion> operaciones = JsonConvert.DeserializeObject<List<Operacion>>(contenido);
            SelectList selected;
            if (id_operacion == 0)
                selected = new SelectList(operaciones, "Id_Operacion", "Descripcion");
            else
                selected = new SelectList(operaciones, "Id_Operacion", "Descripcion", id_operacion);
            ViewData["Id_Operacion"] = selected;
            return PartialView();
        }

        [HttpGet]
        public async Task<PartialViewResult> Definiciones_Transaccion(int id_definicion_transaccion = 0)
        {
            string recuroyparametro = string.Format("Configuracion_Contabilidad/Definiciones_Transaccion");
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            /*convertir el json por ahora*/
            string contenido = responseApi.ContenidofromJson;
            List<Definicion_Transaccion> definiciones = JsonConvert.DeserializeObject<List<Definicion_Transaccion>>(contenido);
            SelectList selected;
            if (id_definicion_transaccion == 0)
                selected = new SelectList(definiciones, "Id_Definicion_Transaccion", "Descripcion");
            else
                selected = new SelectList(definiciones, "Id_Definicion_Transaccion", "Descripcion", id_definicion_transaccion);
            ViewData["Id_Definicion_Transaccion"] = selected;
            return PartialView();
        }

        [HttpGet]
        public async Task<PartialViewResult> Usuarios_Combo()
        {
            string recuroyparametro = string.Format("Account/Usuarios");
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            /*convertir el json por ahora*/
            string contenido = responseApi.ContenidofromJson;
            List<Usuario> usuarios = JsonConvert.DeserializeObject<List<Usuario>>(contenido);
            return PartialView(usuarios);
        }

        [HttpGet]
        public async Task<PartialViewResult> Descripciones_Documento(int id_descripcion_documento_seleccionado)
        {
            string recuroyparametro = "Documentos/Descripciones_Documento";
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            List<Descripcion_Documento> descripciones = JsonConvert.DeserializeObject<List<Descripcion_Documento>>(contenido);
            ViewBag.Seleccionado = id_descripcion_documento_seleccionado;
            return PartialView(descripciones);
        }

        [HttpGet]
        public async Task<PartialViewResult> CarteraPorAgencia(int id_cartera_seleccionada)
        {
            string recuroyparametro = "Carteras/CarterasPorAgenciaEnUsuario";
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            List<Cartera> carteras = JsonConvert.DeserializeObject<List<Cartera>>(contenido);
            ViewBag.Seleccionado = id_cartera_seleccionada;
            return PartialView(carteras);
        }

        [HttpGet]
        public async Task<PartialViewResult> Comportamientos_Credito()
        {
            string recuroyparametro = "Creditos/Comportamientos_Credito";
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            List<Comportamiento_Credito> comportamientos = JsonConvert.DeserializeObject<List<Comportamiento_Credito>>(contenido);
            return PartialView(comportamientos);
        }
        [HttpGet]
        public async Task<PartialViewResult> Porcentaje_Ahorro_Credito()
        {
            string recuroyparametro = "Creditos/Porcentaje_Ahorro";
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            List<Porcentaje_Ahorro_Credito> comportamientos = JsonConvert.DeserializeObject<List<Porcentaje_Ahorro_Credito>>(contenido);
            
            return PartialView(comportamientos);
        }
        [HttpGet]
        public async Task<PartialViewResult> Denominaciones()
        {
            string recuroyparametro = string.Format("ReporteFlujoEfectivo/Vista_Flujo_Efectivo");
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            List<Detalle_Efectivo_Vista> flujo = JsonConvert.DeserializeObject<List<Detalle_Efectivo_Vista>>(contenido);
            return PartialView(flujo);
        }

        [HttpGet]
        public async Task<PartialViewResult> Concepto_Factura_Servicio()
        {
            string recuroyparametro = string.Format("Transaccion_Simple/Conceptos_Facturas_Servicios");
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            List<Factura_Servicio> conceptos = JsonConvert.DeserializeObject<List<Factura_Servicio>>(contenido);
            return PartialView(conceptos);
        }
        [HttpGet]
        public async Task<PartialViewResult> Concepto_Factura_Servicios_Basicos()
        {
            string recuroyparametro = string.Format("Transaccion_Simple/Conceptos_Facturas_Servicios_Basicos");
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            List<Factura_Servicio> conceptos = JsonConvert.DeserializeObject<List<Factura_Servicio>>(contenido);
            return PartialView(conceptos);
        }
        [HttpGet]
        public async Task<PartialViewResult> Otras_Operaciones()
        {
            string recuroyparametro = string.Format("Transaccion_Simple/Otras_Operaciones");
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            List<Otras_Operaciones> conceptos = JsonConvert.DeserializeObject<List<Otras_Operaciones>>(contenido);
            return PartialView(conceptos);
        }
        [HttpGet]
        public async Task<IActionResult> Parentesco()
        {
            string recuroyparametro = string.Format("Deposito_Plazo/Parestesco");

            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            /*convertir el json por ahora*/
            string contenido = responseApi.ContenidofromJson;
            List<Parentesco_Persona> parentesco = JsonConvert.DeserializeObject<List<Parentesco_Persona>>(contenido);

            return PartialView(parentesco);
        }
        [HttpGet]
        public async Task<IActionResult> Forma_Pago()
        {
            string recuroyparametro = string.Format("Deposito_Plazo/Forma_Pago");

            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            /*convertir el json por ahora*/
            string contenido = responseApi.ContenidofromJson;
            List<Forma_Pago_DP> parentesco = JsonConvert.DeserializeObject<List<Forma_Pago_DP>>(contenido);

            return PartialView(parentesco);
        }
    }
}