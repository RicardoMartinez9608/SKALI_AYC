using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AyCWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ModelNucleo.Model.Contabilidad;
using ModelNucleo.Model.ModelView;
using Newtonsoft.Json;

namespace AyCWeb.Controllers
{
    public class Configuracion_ContabilidadController : BaseController
    {
        // GET: Configuracion_Contabilidad
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Configuraciones(
            int id_agencia,
            int id_linea_credito = 0,
            int id_linea_ahorro = 0,
            int id_factura_servicio=0,
            int id_factura_servicio_basico=0,
            int id_otras_operaciones = 0)
        {
            /*arreglar los que son servicios básicos*/
            if (id_factura_servicio == 3 && id_factura_servicio_basico > 0)
                id_factura_servicio = id_factura_servicio_basico;

            string recuroyparametro = string.Format(
                "Configuracion_Contabilidad/Configuraciones?id_agencia={0}&id_linea_credito={1}&id_linea_ahorro={2}&id_factura_servicio={3}" +
                "&id_otras_operaciones={4}",
                id_agencia,
                id_linea_credito,
                id_linea_ahorro,
                id_factura_servicio,
                id_otras_operaciones);

            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            List<Conf_Partida_Contable> configuraciones = JsonConvert.DeserializeObject<List<Conf_Partida_Contable>>(contenido);
            return PartialView(configuraciones);
        }

        [HttpGet]
        public async Task<Detalle_Partida_Contable_Configuracion_Vista> Detalle_Configuracion(int id_configuracion)
        {
            string recuroyparametro = string.Format(
                "Configuracion_Contabilidad/Detalle_Configuracion?id_configuracion={0}", id_configuracion);

            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            Detalle_Partida_Contable_Configuracion_Vista configuracion = JsonConvert
                .DeserializeObject<Detalle_Partida_Contable_Configuracion_Vista>(contenido);
            return configuracion;
        }

        [HttpGet]
        public async Task<IActionResult> Nueva_Configuracion()
        {
            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> Nueva_Configuracion(
            Conf_Partida_Contable configuracion, 
            int id_factura_servicio_basico)
        {
            /*resolver los servicios básicos*/
            if (configuracion.Id_Factura_Servicio == 3)
                configuracion.Id_Factura_Servicio = id_factura_servicio_basico;

            string jsonstring = JsonConvert.SerializeObject(configuracion).Replace("\"", "'");
            string recuroyparametro = string.Format("Configuracion_Contabilidad/Nuevo_Configuracion");
            ResponseApi responseApi = await OperarApi(HttpMethod.Post, recuroyparametro, jsonstring);
            if (responseApi.Satisfactorio)
                return Ok(responseApi.ContenidofromJson);
            else
                return BadRequest(responseApi.ContenidofromJson);
        }

        [HttpGet]
        public async Task<IActionResult> Editar_Configuracion(int id_configuracion)
        {
            Detalle_Partida_Contable_Configuracion_Vista vista = await Detalle_Configuracion(id_configuracion);
            Conf_Partida_Contable configuracion = vista.Conf_Partida_Contable;

            var lineaCredito = await CargarLineas();
            var agencias = await CargarAgencias();
            var lineaahorros = await CargarLineaAhorro();

            ViewBag.Id_Linea_Credito = new SelectList(lineaCredito, "Value", "Text",configuracion.Id_Linea_Credito);
            ViewBag.Id_Agencia = new SelectList(agencias, "Value", "Text",configuracion.Id_Agencia);
            ViewBag.Id_Linea_Ahorro = new SelectList(lineaahorros, "Value", "Text",configuracion.Id_Linea_Ahorro);
            return PartialView(configuracion);
        }

        [HttpPost]
        public async Task<IActionResult> Editar_Configuracion(Conf_Partida_Contable configuracion)
        {
            string jsonstring = JsonConvert.SerializeObject(configuracion).Replace("\"", "'");
            string recuroyparametro = string.Format("Configuracion_Contabilidad/Editar_Configuracion");
            ResponseApi responseApi = await OperarApi(HttpMethod.Put, recuroyparametro, jsonstring);
            if (responseApi.Satisfactorio)
                return Ok(responseApi.ContenidofromJson);
            else
                return BadRequest(responseApi.ContenidofromJson);
        }

        //detalle de la partida contable configuraciones
        [HttpGet]
        public async Task<IActionResult> Detalles_Conf_Partida_Contable(int id_configuracion)
        {
            Detalle_Partida_Contable_Configuracion_Vista vista = await Detalle_Configuracion(id_configuracion);
            return PartialView(vista);
        }

        [HttpGet]
        public IActionResult Nuevo_Detalle_Conf_Partida_Contable(int id_conf_partida_contable)
        {
            ViewBag.Id_Conf_Partida_Contable = id_conf_partida_contable;
            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> Nuevo_Detalle_Conf_Partida_Contable(Conf_Detalle_Partida_Contable nuevo_detalle)
        {
            string jsonstring = JsonConvert.SerializeObject(nuevo_detalle).Replace("\"", "'");
            string recuroyparametro = string.Format("Configuracion_Contabilidad/Nuevo_Detalle_Conf");
            ResponseApi responseApi = await OperarApi(HttpMethod.Post, recuroyparametro, jsonstring);
            if (responseApi.Satisfactorio)
                return Ok(responseApi.ContenidofromJson);
            else
                return BadRequest(responseApi.ContenidofromJson);
        }

        /*Edición de los detalles*/
        [HttpGet]
        public async Task<IActionResult> Editar_Detalle_Conf_Partida_Contable(int id_conf_detalle_partida) {

            string recuroyparametro = string.Format(
                "Configuracion_Contabilidad/Detalle_Conf_Partida?id_conf_detalle_partida={0}", id_conf_detalle_partida);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            Conf_Detalle_Partida_Contable detalle = JsonConvert.DeserializeObject<Conf_Detalle_Partida_Contable>(contenido);
            return PartialView(detalle);
        }

        [HttpPost]
        public async Task<IActionResult> Editar_Detalle_Conf_Partida_Contable(Conf_Detalle_Partida_Contable nuevo_detalle)
        {
            string jsonstring = JsonConvert.SerializeObject(nuevo_detalle).Replace("\"", "'");
            string recuroyparametro = string.Format("Configuracion_Contabilidad/Editar_Detalle_Conf_Partida");
            ResponseApi responseApi = await OperarApi(HttpMethod.Put, recuroyparametro, jsonstring);
            if (responseApi.Satisfactorio)
                return Ok(responseApi.ContenidofromJson);
            else
                return BadRequest(responseApi.ContenidofromJson);
        }

        [HttpPost]
        public async Task<IActionResult> Borrar_Detalle_Conf_Partida_Contable(int id_conf_detalle_partida)
        {
            string recuroyparametro = string.Format("Configuracion_Contabilidad/Borrar_Detalle_Conf_Partida?id_conf_detalle_partida={0}", id_conf_detalle_partida);
            ResponseApi responseApi = await OperarApi(HttpMethod.Delete, recuroyparametro, null);
            if (responseApi.Satisfactorio)
                return Ok(responseApi.ContenidofromJson);
            else
                return BadRequest(responseApi.ContenidofromJson);
        }

        [HttpPost]
        public async Task<IActionResult> Borrar_Conf_Partida_Contable(int id_configuracion)
        {
            string recuroyparametro = string.Format("Configuracion_Contabilidad/Borrar_Configuracion?id_configuracion={0}", id_configuracion);
            ResponseApi responseApi = await OperarApi(HttpMethod.Delete, recuroyparametro, null);
            if (responseApi.Satisfactorio)
                return Ok(responseApi.ContenidofromJson);
            else
                return BadRequest(responseApi.ContenidofromJson);
        }
    }
}
