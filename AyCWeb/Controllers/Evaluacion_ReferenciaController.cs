using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AyCWeb.Models;
using Microsoft.AspNetCore.Mvc;
using ModelNucleo.Model;
using ModelNucleo.Model.Evaluacion_Referencia;
using ModelNucleo.Model.ModelView;
using Newtonsoft.Json;

namespace AyCWeb.Controllers
{
    public class Evaluacion_ReferenciaController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> Desplegar_Bateria(Referencia_Encabezado referencia)
        {
            string recuroyparametro = string.Format("Evaluacion_Referencia/Desplegar_Evaluacion?id_referencia={0}",referencia.id_referencia);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            List<Catalogo_Evaluacion_Referencia_Guardada> bateria = JsonConvert.DeserializeObject<List<Catalogo_Evaluacion_Referencia_Guardada>>(contenido);
            ViewBag.Encabezado = referencia;
            return View(bateria);
        }

        [HttpGet]
        public async Task<IActionResult> Personas_Referencias(string buscar)
        {
            List<Persona> personas = new List<Persona>();
            if (!string.IsNullOrEmpty(buscar))
            {
                string recuroyparametro = string.Format("Evaluacion_Referencia/Personas_Referencias?buscar={0}", buscar);
                ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
                string contenido = responseApi.ContenidofromJson;
                personas = JsonConvert.DeserializeObject<List<Persona>>(contenido);
            }
            return View(personas);
        }

        [HttpGet]
        public async Task<IActionResult> Notificacion_Falla(int id_referencia,bool no_equivocado,bool no_contesta)
        {
            string recuroyparametro = string.Format(
                    "Evaluacion_Referencia/Notificar_Fallo_Referencia?id_referencia={0}&no_equivocado={1}&no_contesta={2}",
                    id_referencia,
                    no_equivocado,
                    no_contesta);

            ResponseApi responseApi = await OperarApi(HttpMethod.Put, recuroyparametro, null);
            if (responseApi.Satisfactorio)
                return Ok(responseApi.ContenidofromJson);
            else
                return BadRequest(responseApi.Error);
        }

        [HttpGet]
        public async Task<IActionResult> Crear_Respuesta(int id_referencia, int id_pregunta, int id_respuesta)
        {
            string recuroyparametro = string
                .Format(
                    "Evaluacion_Referencia/Crear_Bateria?id_referencia={0}&id_pregunta={1}&id_respuesta={2}",
                    id_referencia,
                    id_pregunta,
                    id_respuesta);

            ResponseApi responseApi = await OperarApi(HttpMethod.Post, recuroyparametro, null);
            if (responseApi.Satisfactorio)
                return Ok("Pregunta respondida");
            else
                return BadRequest("La pregunta no se pudo agregar");
        }

        [HttpGet]
        public IActionResult Log_Llamada(int id_referencia)
        {
            ViewBag.Id_Referencia = id_referencia;
            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> Log_Llamada(Llamada_Telefonica_Referencia llamada)
        {
            string jsonstring = JsonConvert.SerializeObject(llamada).Replace("\"", "'");
            string recuroyparametro = string.Format("Evaluacion_Referencia/Log_Llamada_Referencia");
            ResponseApi responseApi = await OperarApi(HttpMethod.Post, recuroyparametro, jsonstring);
            if (responseApi.Satisfactorio)
                return Ok(responseApi.ContenidofromJson);
            else
                return BadRequest(responseApi.Error);
        }

        [HttpGet]
        public async Task<IActionResult> Rastrear_Llamada(int id_llamada)
        {
            string recuroyparametro = string.Format("Evaluacion_Referencia/Rastrear_Llamada?id_llamada={0}",id_llamada);
            ResponseApi responseApi = await OperarApi(HttpMethod.Put, recuroyparametro, null);
            if (responseApi.Satisfactorio)
                return Ok(responseApi.ContenidofromJson);
            else
                return BadRequest(responseApi.Error);
        }

        [HttpGet]
        public async Task<IActionResult> Logger_Llamada(int id_referencia)
        {
            string recuroyparametro = string.Format("Evaluacion_Referencia/Logger_Llamada_Referencia?id_referencia={0}", id_referencia);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            List<Llamada_Telefonica_Referencia> llamadas = JsonConvert.DeserializeObject<List<Llamada_Telefonica_Referencia>>(contenido);
            return PartialView(llamadas);
        }
    }
}