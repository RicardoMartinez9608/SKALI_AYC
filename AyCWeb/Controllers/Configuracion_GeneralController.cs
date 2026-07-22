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
    public class Configuracion_GeneralController : BaseController
    {
        public async Task<IActionResult> Index()
        {
            string recuroyparametro = string.Format("Configuracion_General/Configuraciones");
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            /*convertir el json por ahora*/
            string contenido = responseApi.ContenidofromJson;
            List<Configuracion_General> configuraciones = JsonConvert.DeserializeObject<List<Configuracion_General>>(contenido);
            return View(configuraciones);
        }

        [HttpGet]
        public async Task<IActionResult> Detalle_Configuracion(int id_configuracion)
        {
            string recuroyparametro = string.Format("Configuracion_General/Detalle?id_configuracion={0}",id_configuracion);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            /*convertir el json por ahora*/
            string contenido = responseApi.ContenidofromJson;
            Configuracion_General configuracion = JsonConvert.DeserializeObject<Configuracion_General>(contenido);
            return View(configuracion);
        }

        public async Task<IActionResult> Editar_Configuracion(Configuracion_General configuracion)
        {
            string recuroyparametro = string.Format("Configuracion_General/Editar");
            string jsonstring = JsonConvert.SerializeObject(configuracion).Replace("\"", "'");
            ResponseApi responseApi = await OperarApi(HttpMethod.Put, recuroyparametro, jsonstring);
            return RedirectToAction("Index");
        }
    }
}