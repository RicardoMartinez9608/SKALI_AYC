using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AyCWeb.Models;
using Microsoft.AspNetCore.Mvc;
using ModelNucleo.Model;
using Newtonsoft.Json;

namespace AyCWeb.Controllers
{
    public class Configuracion_CalificacionesController : BaseController
    {
        public async Task< IActionResult> Index()
        {
            return View();
        }
        private async Task<List<Configuracion_Calificacion>> Calificaciones()
        {
            string recuroyparametro = string.Format("Configuracion_Calificacion/Obtener_Calificaciones");
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            return JsonConvert.DeserializeObject<List<Configuracion_Calificacion>>(contenido);
        }

        //[HttpPost]
        //public async Task<IActionResult> Crear_Calificaciones(Configuracion_Calificacion calificacion)
        //{
        //    string jsonstring = JsonConvert.SerializeObject(calificacion).Replace("\"", "'");
        //    string paraCrear = string.Format("Configuracion_Calificacion/Crear_Configuracion");
        //    ResponseApi responseApi = await OperarApi(HttpMethod.Post, paraCrear, jsonstring);
        //    if (responseApi.Satisfactorio)
        //        return Ok(responseApi.ContenidofromJson);
        //    else
        //        return BadRequest(responseApi.ContenidofromJson);
        //}
        [HttpPost]
        public async Task<IActionResult> Guardar(Configuracion_Calificacion configuracion)
        {
            ResponseApi resultado = await Nuevo_Calificacion(configuracion);
            if (resultado.Satisfactorio)
            {
                return Ok(resultado.ContenidofromJson);
            }
            else
                return BadRequest(resultado.ContenidofromJson);
        }
        private async Task<ResponseApi> Nuevo_Calificacion(Configuracion_Calificacion configuracion)
        {
            string jsonstring = JsonConvert.SerializeObject(configuracion).Replace("\"", "'");
            ResponseApi resultado = await OperarApi(HttpMethod.Post, "Configuracion_Calificacion/Crear_Configuracion", jsonstring);
            return resultado;
        }

        [HttpGet]
        public async Task<IActionResult> Lista_Calificacion()
        {
            return PartialView(await Calificaciones());
        }

        [HttpGet]
        public IActionResult Agregar_Calificacion()
        {
            return PartialView();
        }

        [HttpGet]
        public async Task<IActionResult> Buros_Combo()
        {
            return PartialView(await Buros());
        }

        private async Task<List<Buro>> Buros()
        {
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, "Configuracion_Calificacion/Buros", null);
            /*convertir el json por ahora*/
            string contenido = responseApi.ContenidofromJson;
            return JsonConvert.DeserializeObject<List<Buro>>(contenido);
        }

        [HttpGet]
        public async Task<IActionResult> Obtener_Calificacion(int id_calificacion)
        {
            string recuroyparametro = string.Format("Configuracion_Calificacion/Obtener_Calificaciones_por_ID?id_calificacion={0}", id_calificacion);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);

            string contenido = responseApi.ContenidofromJson;
            Configuracion_Calificacion configuracion = JsonConvert.DeserializeObject<Configuracion_Calificacion>(contenido);
            return PartialView(configuracion);
        }

        [HttpPost]
        public async Task<IActionResult> Editar_Calificacion(Configuracion_Calificacion calificacion_actualizar)
        {
            string recuroyparametro = string.Format("Configuracion_Calificacion/Editar_Calificacion");
            string jsonstring = JsonConvert.SerializeObject(calificacion_actualizar).Replace("\"", "'");
            ResponseApi responseApi = await OperarApi(HttpMethod.Put, recuroyparametro, jsonstring);
            if (responseApi.Satisfactorio)
                return Ok(responseApi.ContenidofromJson);
            else
                return BadRequest(responseApi.ContenidofromJson);
        }
    }
}
