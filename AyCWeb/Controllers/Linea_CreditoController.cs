using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AyCWeb.Models;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ModelNucleo.Model;
using Newtonsoft.Json;

namespace AyCWeb.Controllers
{
    public class Linea_CreditoController : BaseController
    {

        // GET: Linea_Credito
        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> Lineas_Creditos(DataSourceLoadOptions loadOptions)
        {
            string recuroyparametro = string.Format("Linea_Credito/LineasCredito");

            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            /*convertir el json por ahora*/
            string contenido = responseApi.ContenidofromJson;
            List<Linea_Credito> lienascredito = JsonConvert.DeserializeObject<List<Linea_Credito>>(contenido);

            var result = DataSourceLoader.Load(lienascredito, loadOptions);
            var resultJson = JsonConvert.SerializeObject(result);
            return Content(resultJson, "application/json");
        }
   

        [HttpGet]
        public async Task<IActionResult> LineaDetalleParcial(int id_linea=0,bool esNuevo=false)
        {
            if (esNuevo)
                return PartialView();
            Linea_Credito linea= await Detalle(id_linea);
            return PartialView(linea);
        }

        [HttpGet]
        public async Task<IActionResult> LineaDetalle(int id_linea)
        {
            Linea_Credito linea = await Detalle(id_linea);
            return Ok(linea);
        }

        private async Task<Linea_Credito> Detalle(int id_linea)
        {
            string recuroyparametro = string.Format("Linea_Credito/Linea_CreditoDetalle?id_linea={0}",id_linea);

            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            /*convertir el json por ahora*/
            string contenido = responseApi.ContenidofromJson;
            return JsonConvert.DeserializeObject<Linea_Credito>(contenido);
        }

        #region Manejo principal de Linea de Crédito

        [HttpPost]
        public async Task<IActionResult> Nueva_Linea_Credito(Linea_Credito linea)
        {
            /*conectarse al api*/
            string jsonstring = JsonConvert.SerializeObject(linea).Replace("\"", "'");
            ResponseApi resultado = await OperarApi(HttpMethod.Post, "Linea_Credito/NuevaLinea_Credito", jsonstring);
            if (resultado.Satisfactorio)
                return Ok(resultado.ContenidofromJson);
            else
                return BadRequest(resultado.ContenidofromJson);
        }

        [HttpPost]
        public async Task<IActionResult> Editar_Linea_Credito(Linea_Credito linea)
        {
            /*conectarse al api*/
            string recuroyparametro = string.Format("Linea_Credito/Modificar");
            string jsonstring = JsonConvert.SerializeObject(linea).Replace("\"", "'");
            ResponseApi resultado = await OperarApi(HttpMethod.Post, recuroyparametro, jsonstring);
            if (resultado.Satisfactorio)
                return Ok(resultado.ContenidofromJson);
            else
                return BadRequest(resultado.ContenidofromJson);
        }
        

        #region deducciones principales
        [HttpGet]
        public async Task<IActionResult> Deducciones_Desembolso(int id_linea_credito)
        {
            string recuroyparametro = string.Format("Deduccion_Desembolso/Deducciones_Desembolso?id_linea_credito={0}", id_linea_credito);

            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            /*convertir el json por ahora*/
            string contenido = responseApi.ContenidofromJson;
            List<Deduccion_Desembolso> deducciones = JsonConvert.DeserializeObject<List<Deduccion_Desembolso>>(contenido);
            return PartialView(deducciones);
        }

        [HttpGet]
        public async Task<IActionResult> Detalle_Deduccion_Desembolso(bool esNuevo, int id_linea_credito = 0, int id_deduccion_desembolso = 0)
        {
            var tipovalor = from Tipo_Valor d in Enum.GetValues(typeof(Tipo_Valor))
                            select new SelectListItem { Value = ((int)d).ToString(), Text = d.ToString() };
            if (esNuevo)
            {
                ViewBag.Id_Linea_Credito = id_linea_credito;
                ViewBag.Tipo_Valor = new SelectList(tipovalor, "Value", "Text");
                Deduccion_Desembolso deduccion_nuevo = new Deduccion_Desembolso { Id_Linea_Credito = id_linea_credito };
                return PartialView(deduccion_nuevo);
            }

            string recuroyparametro = string.Format("Deduccion_Desembolso/Deduccion_Desembolso?id_deduccion_desembolso={0}", id_deduccion_desembolso);

            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            /*convertir el json por ahora*/
            string contenido = responseApi.ContenidofromJson;
            Deduccion_Desembolso deduccion = JsonConvert.DeserializeObject<Deduccion_Desembolso>(contenido);
            ViewBag.Tipo_Valor = new SelectList(tipovalor, "Value", "Text", (int)deduccion.Tipo_Valor);
            return PartialView(deduccion);
        }

        [HttpPost]
        public async Task<IActionResult> Nuevo_Deduccion_Desembolso(Deduccion_Desembolso deduccion)
        {
            string recuroyparametro = string.Format("Deduccion_Desembolso/Nueva_Deduccion_Desembolso");

            string jsonstring = JsonConvert.SerializeObject(deduccion).Replace("\"", "'");
            ResponseApi resultado = await OperarApi(HttpMethod.Post, recuroyparametro, jsonstring);
            if (resultado.Satisfactorio)
                return Ok(resultado.ContenidofromJson);
            else
                return BadRequest(resultado.ContenidofromJson);
        }
        [HttpPost]
        public async Task<IActionResult> Editar_Deduccion_Desembolso(Deduccion_Desembolso deduccion)
        {
            string recuroyparametro = string.Format("Deduccion_Desembolso/Editar_Deduccion_Desembolso");
            string jsonstring = JsonConvert.SerializeObject(deduccion).Replace("\"", "'");
            ResponseApi responseApi = await OperarApi(HttpMethod.Put, recuroyparametro, jsonstring);
            if (responseApi.Satisfactorio)
                return Ok(responseApi.ContenidofromJson);
            else
                return BadRequest(responseApi.ContenidofromJson);
        }

        #endregion

        #endregion

        #region Manejo de Linea Credito Historial

        [HttpGet]
        public async Task<IActionResult> Detalle_Linea_Credito_Historial(int id_credito)
        {
            string recuroyparametro = string.Format("Linea_Credito/Detalle_Linea_Credito_Historial?id_credito={0}", id_credito);

            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            /*convertir el json por ahora*/
            string contenido = responseApi.ContenidofromJson;
            Linea_Credito_Historial lienascredito = JsonConvert.DeserializeObject<Linea_Credito_Historial>(contenido);
            return PartialView(lienascredito);
        }

        [HttpPost]
        public async Task<IActionResult> Editar_Linea_Credito_Historial(Linea_Credito_Historial linea)
        {
            /*conectarse al api*/
            string recuroyparametro = string.Format("Linea_Credito/Editar_Linea_Credito_Historial");
            string jsonstring = JsonConvert.SerializeObject(linea).Replace("\"", "'");
            ResponseApi resultado = await OperarApi(HttpMethod.Put, recuroyparametro, jsonstring);
            if (resultado.Satisfactorio)
                return Ok(resultado.ContenidofromJson);
            else
                return BadRequest(resultado.ContenidofromJson);
        }

        #endregion

        public async Task<IActionResult> Delete(int id)
        {
            return Ok();
        }

        //-------------------------------------Interes Linea de Credito---------------------------------------

        [HttpGet]
        public async Task<ActionResult> ListarInteresPeriodo(int id_linea_credito)
        {
            string recuroyparametro = string.Format("InteresLineaCreditoPeriodo/Intereses_Periodos?id_linea_credito={0}", id_linea_credito);
            string jsonstring = JsonConvert.SerializeObject(id_linea_credito).Replace("\"", "'");
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro,jsonstring );
            /*convertir el json por ahora*/
            string contenido = responseApi.ContenidofromJson;
            List<Interes_Periodo_Linea_Credito> lineaInteres = JsonConvert.DeserializeObject<List<Interes_Periodo_Linea_Credito>>(contenido);
            return PartialView(lineaInteres);
        }

        [HttpGet]
        public async Task<ActionResult> EditarInteresPeriodo(int id_linea_credito = 0)
        {
            
            var periodos = await CargarPeriodos();
            ViewBag.Id_Periodo = new SelectList(periodos, "Value", "Text");
            ViewBag.Id_Linea_Credito = id_linea_credito;
            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> Nuevo_Interes_Periodo(Interes_Periodo_Linea_Credito interesPeriodo)
        {
            /*conectarse al api*/
            string jsonstring = JsonConvert.SerializeObject(interesPeriodo).Replace("\"", "'");
            ResponseApi resultado = await OperarApi(HttpMethod.Post, "InteresLineaCreditoPeriodo/Crear_Editar_Interes", jsonstring);
            if (resultado.Satisfactorio)
                return Ok(resultado.ContenidofromJson);
            else
                return BadRequest(resultado.ContenidofromJson);
        }

        [HttpGet]
        public async Task<IActionResult> Editar_Interes(int idinteres)
        {
            string recuroyparametro = string.Format("InteresLineaCreditoPeriodo/DetalleInteres?idinteres={0}", idinteres);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);

            string contenido = responseApi.ContenidofromJson;
            Interes_Periodo_Linea_Credito interesPeriodo = JsonConvert.DeserializeObject<Interes_Periodo_Linea_Credito>(contenido);
            return PartialView(interesPeriodo);
        }

        [HttpPost]
        public async Task<IActionResult> Editar_Interes_Exe(Interes_Periodo_Linea_Credito interesPeriodo)
        {
            string recuroyparametro = string.Format("InteresLineaCreditoPeriodo/Editar_Interes_Periodos");
            string jsonstring = JsonConvert.SerializeObject(interesPeriodo).Replace("\"", "'");
            ResponseApi responseApi = await OperarApi(HttpMethod.Put, recuroyparametro, jsonstring);
            if (responseApi.Satisfactorio)
                return Ok(responseApi.ContenidofromJson);
            else
                return BadRequest(responseApi.ContenidofromJson);
        }

        /*manejo de los mutuos por línea*/
        [HttpGet]
        public async Task<IActionResult> Mutuos_Linea(int id_linea_credito)
        {
            string recuroyparametro = string.Format("Documentos_Legales/Mutuos?id_linea_credito={0}", id_linea_credito);
            string jsonstring = JsonConvert.SerializeObject(id_linea_credito).Replace("\"", "'");
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, jsonstring);
            /*convertir el json por ahora*/
            string contenido = responseApi.ContenidofromJson;
            List<Linea_Credito> linea_mutuo = JsonConvert.DeserializeObject<List<Linea_Credito>>(contenido);
            return View(linea_mutuo);
        }

    }
}